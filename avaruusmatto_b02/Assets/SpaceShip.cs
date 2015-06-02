using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpaceObject))]
/**
 * Waypoint Class
 */
public class Waypoint {
	public Waypoint(double x, double y, double z)
	{
		X=x;
		Y=y;
		Z=z;
	}
	public static Waypoint operator +(Waypoint a, Waypoint b)
	{
		return new Waypoint(a.X+b.X,a.Y+b.Y,a.Z+b.Z);
	}
	public static Waypoint operator -(Waypoint a, Waypoint b)
	{
		return new Waypoint(a.X-b.X,a.Y-b.Y,a.Z-b.Z);
	}
	public static Waypoint operator /(Waypoint a, double b)
	{
		return new Waypoint(a.X/b,a.Y/b,a.Z/b);
	}

	public double X;
	public double Y;
	public double Z;
}

/**
 * Spaceship class
 */
public class SpaceShip : SpaceObject {

	public List<Waypoint> Waypoints = new List<Waypoint>();

	int currentWaypoint = 0;
	double originalDistanceToDestination = 0;
	Vector3 distanceToDestination;
	Vector3 retroStartDist;
	public float myThrust = 50;
	public float myVelX = 0f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;
	
	public float maxAngularVel = 10f;
	
	public Vector3 myLocalVel;
	
	public enum autoPilotStates
	{
		idle = 0,
		setDirection,
		prograde,
		retrograde,
		bearingTurn
	}

	public autoPilotStates autopilotState = autoPilotStates.idle;
	public byte turningState = 0;

	public bool turnXToPos = true;
	public bool turnYToPos = true;
	public bool turnZToPos = true;

	Waypoint deltaVector;
	public Quaternion deltaRotation;
	public Quaternion toDirQ;
	public Vector3 toDir;



	Vector3 stopRotAngDist;
	// rotaation thrustereiden maximivoima
	private float torq = 100;


	public float DeltadirX;
	public float accX;
	public float decX;
	public float updateDirX;
	SDegree updateDirStepX= new SDegree(0);
	SDegree updateDirLagStepX= new SDegree(0);
	public byte subState2X = 0;

	public float DeltadirY;
	public float accY;
	public float decY;
	public float updateDirY;
	SDegree updateDirStepY = new SDegree(0);
	SDegree updateDirLagStepY = new SDegree(0);
	public byte subState2Y = 0;

	public float DeltadirZ;

	public Vector3 localAngularVelocity;
	public float InertiaMass;
	public float angAcc;

	protected float WarpFactor =0;


	// Use this for initialization
	void Start () {

	/*	myX = 0;
		myY = 0;
		myZ = 10;
*/		
		RegisterToGame(false);
		DontDestroyOnLoad(this.gameObject);

		Waypoints.Add(new Waypoint(myX,myY,myZ));	
		/*
		Waypoints.Add(new Waypoint(-100,-100,-500));
		Waypoints.Add(new Waypoint(0,100,1000));
		Waypoints.Add(new Waypoint(0,0,10));
		*/
		if(Waypoints.Count>0)
			SetDirection(Waypoints[0]);
		//StopX(); StopY(); StopZ();



	}

	public void SetBearing(float x, float y, float z)
	{
		//Debug.Log("SetRotation: "+ x.ToString() + "," + y.ToString() + "," +z.ToString());

		//TODO: alusta bearing parametrit.
		// fixedupdaten autopilotstate.bearingTurn caseen suoritetaan kääntörutiin.

		autopilotState = autoPilotStates.bearingTurn;

		Vector3 to = new Vector3(x, y, z);

		deltaRotation = Quaternion.FromToRotation(transform.forward, to);
		toDirQ =  Quaternion.FromToRotation(Vector3.forward, to);
		toDir = toDirQ.eulerAngles;
		DeltadirX = toDir.x - transform.rotation.eulerAngles.x;
		DeltadirY = toDir.y - transform.rotation.eulerAngles.y;
		DeltadirZ = toDir.z - transform.rotation.eulerAngles.z;

		localAngularVelocity = GetComponent<Rigidbody>().transform.InverseTransformDirection(GetComponent<Rigidbody>().angularVelocity);

		float vax = localAngularVelocity.x;
		float vay = localAngularVelocity.y;
		float vaz = localAngularVelocity.z;
		
		/*stopRotAngDist = new Vector3 ((GetComponent<Rigidbody>().mass * Mathf.Pow(vax, 2) ) / (2 * torq),
		                              (GetComponent<Rigidbody>().mass * Mathf.Pow(vay, 2) ) / (2 * torq),
		                              (GetComponent<Rigidbody>().mass * Mathf.Pow(vaz, 2) ) / (2 * torq));
*/


		stopRotAngDist = new Vector3 ((Mathf.Pow(localAngularVelocity.x,2) * InertiaMass) / (2 * torq),
		                              (Mathf.Pow(localAngularVelocity.y,2) * InertiaMass) / (2 * torq),
		                              (Mathf.Pow(localAngularVelocity.z,2) * InertiaMass) / (2 * torq));

		if (DeltadirX < 0) turnXToPos = false; else turnXToPos = true;
		if (DeltadirY < 0) turnYToPos = false; else turnYToPos = true;
		if (DeltadirZ < 0) turnZToPos = false; else turnZToPos = true;

		// pseudoa
		// sitten, kun stopRotAngDist.x == bearing.x. 
		//		aloitetaan jarrutus.x
		// sama kaikille akseleille.


	}


	void FixedUpdate() {

		// warp physics, ignore others
		if(WarpFactor >0)
		{
			myVelocity = WarpFactor * 300000000;
			var warpVector = new Vector3(myVelX,myVelY,myVelZ) ;
			warpVector.Normalize();
			warpVector *= myVelocity;
			myX += warpVector.x *Time.fixedDeltaTime;
			myY += warpVector.y*Time.fixedDeltaTime;
			myZ += warpVector.z*Time.fixedDeltaTime;
			
			UpdatePosition();
			return;
		}

		myVelocity = Mathf.Sqrt (Mathf.Pow (myVelX,2) + Mathf.Pow (myVelY,2) + Mathf.Pow (myVelZ,2));

		// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
		localAngularVelocity = GetComponent<Rigidbody>().transform.InverseTransformDirection(GetComponent<Rigidbody>().angularVelocity);

		InertiaMass = (2 * GetComponent<Rigidbody>().mass * Mathf.Pow(1, 2)) / 5;
		angAcc = torq / InertiaMass;

		stopRotAngDist = new Vector3 (Mathf.Pow(localAngularVelocity.x,2) / (2 * angAcc),
		                              Mathf.Pow(localAngularVelocity.x,2) / (2 * angAcc),
		                              Mathf.Pow(localAngularVelocity.x,2) / (2 * angAcc));

		// Jos ollaan jo nykyisessä waypointissa, niin suunnataan nokka seuraavaan
		if(isAtWaypoint(Waypoints[currentWaypoint]) && myVelocity < 0.1) 
		{
			
			if(currentWaypoint<Waypoints.Count)
				currentWaypoint++;
			//Stop ();
			autopilotState=autoPilotStates.idle;
			
			if(currentWaypoint<Waypoints.Count)
			{
				autopilotState=autoPilotStates.setDirection;
				var currentWp=Waypoints[currentWaypoint];
				originalDistanceToDestination =  Mathf.Sqrt (Mathf.Pow ((float)(currentWp.X-myX),2) + Mathf.Pow ((float)(currentWp.Y-myY),2) + Mathf.Pow ((float)(currentWp.Z-myZ),2));
				distanceToDestination = new Vector3 ((float)(currentWp.X-myX), (float)(currentWp.Y-myY), (float)(currentWp.Z-myZ));
			}
			else 
				currentWaypoint =0;
		}

		switch(autopilotState)
		{
		case autoPilotStates.setDirection:
			if(SetDirection(Waypoints[currentWaypoint])==true)
				autopilotState = autoPilotStates.prograde;
			break;
		
		case autoPilotStates.prograde:
			var currentWp=Waypoints[currentWaypoint];
			distanceToDestination = new Vector3 ((float)(currentWp.X-myX), (float)(currentWp.Y-myY), (float)(currentWp.Z-myZ));
			// retroStartDist on maksimietäisyys, jolla pystytään pysäyttämään alus.
			retroStartDist = new Vector3 ((GetComponent<Rigidbody>().mass * Mathf.Pow(myVelX, 2) ) / (2 * myThrust),
			                          (GetComponent<Rigidbody>().mass * Mathf.Pow(myVelY, 2) ) / (2 * myThrust),
			                          (GetComponent<Rigidbody>().mass * Mathf.Pow(myVelZ, 2) ) / (2 * myThrust));

			if(distanceToDestination.magnitude>(originalDistanceToDestination*0.65))
				addThrust(Vector3.forward);

			if(distanceToDestination.magnitude<=retroStartDist.magnitude)
				autopilotState = autoPilotStates.retrograde;

			break;

		case autoPilotStates.retrograde:
			// retroStartDist on maksimietäisyys, jolla pystytään pysäyttämään alus.
			//retroStartDist = (GetComponent<Rigidbody>().mass * Mathf.Pow(myVelocity, 2) ) / (2 * myThrust);

			if(distanceToDestination.x <= retroStartDist.x && Mathf.Abs(myVelX) > 0.1)
				StopX();
			if(distanceToDestination.y <= retroStartDist.y && Mathf.Abs(myVelY) > 0.1)
				StopY();
			   if(distanceToDestination.z <= retroStartDist.z && Mathf.Abs(myVelZ) > 0.1)
				StopZ();

			else if(isAtWaypoint(Waypoints[currentWaypoint])==false && myVelocity < 0.2)
			{
				autopilotState = autoPilotStates.setDirection;
			}
			if (isAtWaypoint(Waypoints[currentWaypoint])==true && myVelocity < 0.2) 
			{
				autopilotState = autoPilotStates.idle;
			}
			break;

		case autoPilotStates.bearingTurn:


			break;

		case autoPilotStates.idle:
		default:
			break;
		}

		/*
		Vector3 refVelVec = new Vector3 (myVelX, myVelY, myVelZ);
		Quaternion rotations = GetComponent<Rigidbody>().rotation;
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
		myLocalVel = m.MultiplyPoint3x4(refVelVec);
		*/
		Vector3 refVelVec = new Vector3 (myVelX, myVelY, myVelZ);
		myLocalVel = GetComponent<Rigidbody>().transform.InverseTransformDirection(refVelVec);

		myX += myVelX*Time.fixedDeltaTime;
		myY += myVelY*Time.fixedDeltaTime;
		myZ += myVelZ*Time.fixedDeltaTime;

		UpdatePosition();

	}

	
	public void SetWarpFactor(float warp)
	{
		WarpFactor = warp;
	}

	bool isAtWaypoint(Waypoint w)
	{
		double distance = Mathf.Sqrt (Mathf.Pow ((float)(w.X-myX),2) + Mathf.Pow ((float)(w.Y-myY),2) + Mathf.Pow ((float)(w.Z-myZ),2));
		//Debug.Log("Distance: " + distance);
		if(distance < 300) 
			return true;
		return false;
	}

	void addThrust(Vector3 dir)
	{
		Vector3 deltaVel;

		// diagonaalimatriisi. diagonaalilla on työntövoima
		var thrustMatrix = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.Diagonal(3, 3, (myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime));
		var dirVec = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(new[] {(double)dir.x, (double)dir.y, (double)dir.z});
		var thrustDirection = dirVec * thrustMatrix;

		// tässä oli kans äsken viä bugi... noi
		deltaVel = new Vector3 ((float)thrustDirection.At(0), (float)thrustDirection.At(1), (float)thrustDirection.At(2));
		Quaternion rotations = GetComponent<Rigidbody>().rotation;
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
		Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
		myVelX += deltaVelRotated.x;
		myVelY += deltaVelRotated.y;
		myVelZ += deltaVelRotated.z;
	}

	//TODO pitääkö tällanen voidi olla fixedupdatessa, vai toimiiko ilman?
	bool SetDirection(Waypoint w)
	{
		//pysäytetään rotaatio
		if( turningState==0 && localAngularVelocity.magnitude > 0.01) 
		{
			stopRotation();
		}
		//kun rotaatio on pysähtynyt, niin mennään seuraavaan tilaan
		else if( turningState==0 && localAngularVelocity.magnitude <= 0.01) 
		{
			// angularvelocity taitaa olla vaan luku,. sitä ei voi asettaa tällä tavalla... ehkä...
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			turningState=1;
		}
		// tilassa 1 alustetaan parametrit kääntymistä varten
		else if(turningState==1)
		{
			//vektori omasta pisteestä waypointille
			deltaVector = w - new Waypoint(myX,myY,myZ);
			// yksikkövektori. eli komponentien neliösumman neliöjuuri on 1. 
			var magnitude = System.Math.Sqrt (System.Math.Pow (deltaVector.X,2) + 
							System.Math.Pow (deltaVector.Y,2) + System.Math.Pow (deltaVector.Z,2));
			// tämä on yhden pituinen vektori, joka osoittaa waypointin suuntaan.
			deltaVector = deltaVector/magnitude;
			// tämä on quaternioni muodossa oleva pyörähdysjuttu waypointin suuntaan
	
			// Vector3 from = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
			Vector3 to = new Vector3((float)deltaVector.X, (float)deltaVector.Y, (float)deltaVector.Z);

			deltaRotation = Quaternion.FromToRotation(transform.forward, to);
			toDirQ = Quaternion.FromToRotation(Vector3.forward, to);
			toDir = toDirQ.eulerAngles;
			DeltadirX = toDir.x - transform.rotation.eulerAngles.x;
			DeltadirY = toDir.y - transform.rotation.eulerAngles.y;

			if(DeltadirX<=180 && DeltadirX>=-180)
			{

				accX = DeltadirX *0.3f;
				decX = DeltadirX *0.7f;
			}
			if(DeltadirX>180)
			{
				DeltadirX =DeltadirX - 360f;
				accX = DeltadirX *0.3f;
				decX = DeltadirX *0.7f;
			}
			if(DeltadirX<-180)
			{
				DeltadirX =DeltadirX + 360f;
				accX = DeltadirX *0.3f;
				decX = DeltadirX *0.7f;
			}

			updateDirX = 0f;
			updateDirStepX.Angle = transform.rotation.eulerAngles.x;
			updateDirLagStepX.Angle = transform.rotation.eulerAngles.x;

			// y-akselille sama. alustuksen voisi yhdistää yhteen metodiin, olle vain annettaisiin tarvittava koordinaattiakseli.
			
			if(DeltadirY<=180 && DeltadirY>=-180)
			{
				
				accY = DeltadirY *0.3f;
				decY = DeltadirY *0.7f;
			}
			if(DeltadirY>180)
			{
				DeltadirY = DeltadirY-360f;
				accY = DeltadirY *0.3f;
				decY = DeltadirY *0.7f;
			}
			if(DeltadirY<-180)
			{
				DeltadirY = DeltadirY+360f;
				accY = DeltadirY *0.3f;
				decY = DeltadirY *0.7f;
			}
			updateDirY = 0f;
			updateDirStepY.Angle = transform.rotation.eulerAngles.y;
			updateDirLagStepY.Angle = transform.rotation.eulerAngles.y;

			// tarkistetaan, kumpaan suuntaan pitää kääntyä x ja y akselien suhteen
			// tähän lisättiin elset, ja sit alkoi toimia :)
			if (DeltadirX < 0) turnXToPos = false; else turnXToPos = true;
			if (DeltadirY < 0) turnYToPos = false; else turnYToPos = true;

			turningState=2;
			subState2X = 1;
			subState2Y = 1;
		}
		// tila 2 tekee käännökset
		else if(turningState==2)
		{
			if (subState2X==1)
			{
				updateDirStepX.Angle = transform.rotation.eulerAngles.x;
				var stepX =  updateDirStepX - updateDirLagStepX;
				updateDirX = updateDirX + stepX.Angle;

				if (Mathf.Abs(DeltadirX)>=1)
				{
					if (turnXToPos)
					{
						if (updateDirX<=accX) {GetComponent<Rigidbody>().AddRelativeTorque (100, 0, 0);}
						if (updateDirX>=decX) {subState2X=2;}
					}
					if (turnXToPos==false && DeltadirX < 1)
					{
						if (updateDirX>=accX) {GetComponent<Rigidbody>().AddRelativeTorque (-100, 0, 0);}
						if (updateDirX<=decX) {subState2X=2;}
					}
				}

				if (Mathf.Abs(DeltadirX)<1)
				{
					if (turnXToPos)
					{
						if (updateDirX<=accX) {GetComponent<Rigidbody>().AddRelativeTorque (10, 0, 0);}
						if (updateDirX>=decX) {subState2X=2;}
					}
					if (turnXToPos==false && DeltadirX < 1)
					{
						if (updateDirX>=accX) {GetComponent<Rigidbody>().AddRelativeTorque (-10, 0, 0);}
						if (updateDirX<=decX) {subState2X=2;}
					}
				}



				updateDirLagStepX.Angle = transform.rotation.eulerAngles.x;

			}
			// y-akselille sama
			if (subState2Y==1)
			{
				updateDirStepY.Angle = transform.rotation.eulerAngles.y;
				var stepY = updateDirStepY - updateDirLagStepY;
				//TODO stepYhyn pitäis laittaa stepY.GetRotation. tämä ei toimi
				updateDirY = updateDirY + stepY.Angle;

				if(Mathf.Abs(DeltadirY)>=1)
				{
					if (turnYToPos)
					{
						if (updateDirY<=accY) {GetComponent<Rigidbody>().AddRelativeTorque (0, 100, 0);}
						if (updateDirY>=decY) {subState2Y=2;}
					}
					if (turnYToPos==false)
					{
						if (updateDirY>=accY) {GetComponent<Rigidbody>().AddRelativeTorque (0, -100, 0);}
						if (updateDirY<=decY) {subState2Y=2;}
					}
				}
				if(Mathf.Abs(DeltadirY)<1)
				{
					if (turnYToPos)
					{
						if (updateDirY<=accY) {GetComponent<Rigidbody>().AddRelativeTorque (0, 10, 0);}
						if (updateDirY>=decY) {subState2Y=2;}
					}
					if (turnYToPos==false)
					{
						if (updateDirY>=accY) {GetComponent<Rigidbody>().AddRelativeTorque (0, -10, 0);}
						if (updateDirY<=decY) {subState2Y=2;}
					}
				}


				updateDirLagStepY.Angle = transform.rotation.eulerAngles.y;

			}

			if (subState2X == 2)
			{
				stopRotationX();
				if (GetComponent<Rigidbody>().angularVelocity.x < 0.001)
				{
					localAngularVelocity.x = 0;
				}
			}

			if (subState2Y == 2)
			{
				stopRotationY();
				if (GetComponent<Rigidbody>().angularVelocity.y < 0.001)
				{
					localAngularVelocity.y = 0;
				}
			}

			if (subState2X==2 && subState2Y==2)
			{
				turningState=3;
				subState2X=0;
				subState2Y=0;
			}
		}

		// pysäytetään rotaatio vielä varmuuden vuoksi
		else if (turningState == 3 && localAngularVelocity.magnitude >= 0.01)
		{
			stopRotation();
		}
		else if (turningState == 3 && localAngularVelocity.magnitude < 0.01)
		{
			turningState=4;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		// tarkistetaan vielä, onko suunta oikea

		else if (turningState == 4)
		{
			var diffX = toDir.x - transform.rotation.eulerAngles.x;
			var diffY = toDir.y - transform.rotation.eulerAngles.y;
			// jos ei ole oikea, niin mennään uudestaan tilaan 1 ja aloitetaan alusta
			if( Mathf.Abs(diffX) > 0.001 || Mathf.Abs(diffY) > 0.001)
			{
				turningState=1;

			}
			// muuten lopetetaan kääntyminen
			else 
			{
				turningState=0;
				return true;
			}
		}
		return false;
	}

	void StopX()
	{
		Vector3 deltaVel;

		if (myLocalVel.x > 0.1f)
		{
			deltaVel = new Vector3(myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1), 0, 0);
			Quaternion rotations = GetComponent<Rigidbody>().rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;

		}
		if (myLocalVel.x < -0.1f)
		{
			deltaVel = new Vector3(myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime, 0, 0);
			Quaternion rotations = GetComponent<Rigidbody>().rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}
	}

	void StopY()
	{
		Vector3 deltaVel;

		if (myLocalVel.y > 0.1f)
		{
			deltaVel = new Vector3(0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime, 0);
			Quaternion rotations = GetComponent<Rigidbody>().rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
			
		}
		if (myLocalVel.y < -0.1f)
		{
			deltaVel = new Vector3(0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1), 0);
			Quaternion rotations = GetComponent<Rigidbody>().rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}
	}
		
	void StopZ()
	{
		Vector3 deltaVel;

		if (myLocalVel.z > 0.1f)
		{
			deltaVel = new Vector3(0, 0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1));
			Quaternion rotations = GetComponent<Rigidbody>().rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
			
		}
		if (myLocalVel.z < -0.1f)
		{
			deltaVel = new Vector3(0, 0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime);
			Quaternion rotations = GetComponent<Rigidbody>().rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

	}
	/*
	void Stop()
	{
		myVelX =  myVelY = myVelZ = 0;
	}
	*/
	void stopRotation()
	{

		if (localAngularVelocity.x < 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (100, 0, 0); 
		}
		if (localAngularVelocity.x > 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (-100, 0, 0); 
		}
		if (localAngularVelocity.y < 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (0, 100, 0); 
		}
		if (localAngularVelocity.y > 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (0, -100, 0); 
		}
		if (localAngularVelocity.z < 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (0, 0, 100); 
		}
		if (localAngularVelocity.z > 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (0, 0, -100); 
		}


	}

	void stopRotationX()
	{
		if (localAngularVelocity.x < 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (100, 0, 0); 
		}
		if (localAngularVelocity.x > 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (-100, 0, 0); 
		}

	}

	void stopRotationY()
	{

		if (localAngularVelocity.y < 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (0, 100, 0); 
		}
		if (localAngularVelocity.y > 0) {
			GetComponent<Rigidbody>().AddRelativeTorque (0, -100, 0); 
		}

	}

}
