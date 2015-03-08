using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]

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
	public float myThrust = 1000000;
	public float myVelX = 0f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;
	
	public float maxAngularVel = 10f;
	bool IsTurning = false;
	public byte turningState = 0; 
	public bool turnXToPos = true;
	public bool turnYToPos = true;

	Waypoint deltaVector;
	public Quaternion deltaRotation;
	public Quaternion toDirQ;
	public Vector3 toDir;

	public float DeltadirX;
	public float accX;
	public float decX;
	public float updateDirX;
	public float updateDirStepX;
	public float updateDirLagStepX;
	public byte subState2X = 0;

	public float DeltadirY;
	public float accY;
	public float decY;
	public float updateDirY;
	public float updateDirStepY;
	public float updateDirLagStepY;
	public byte subState2Y = 0;

	public Vector3 localAngularVelocity;

	// Use this for initialization
	void Start () {
		myX = 0;
		myY = 0;
		myZ = 5;
		Waypoints.Add(new Waypoint(0,0,myZ));	// Veny menee tänne
		Waypoints.Add(new Waypoint(-8,-10,6));		// Tämä pitäisi kai olla suunta mihin veny katsoo?
		if(Waypoints.Count>0)
			SetDestination(Waypoints[0]);
		Stop ();
	}
	
	void FixedUpdate() {
	
		// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
		localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

		// Jos ollaan jo nykyisessä waypointissa, niin suunnataan nokka seuraavaan
		if(isAtWaypoint(Waypoints[currentWaypoint]))
		{
			if(currentWaypoint<Waypoints.Count)
				currentWaypoint++;
			Stop ();
			
			if(currentWaypoint<Waypoints.Count)
			{
				IsTurning=true;
				var currentWp=Waypoints[currentWaypoint];
			}
		}

		if(IsTurning) 
			SetDestination(Waypoints[currentWaypoint]);


		myVelocity = Mathf.Sqrt (Mathf.Pow (myVelX,2) + Mathf.Pow (myVelY,2) + Mathf.Pow (myVelZ,2));
		

		myX += myVelX*Time.fixedDeltaTime;
		myY += myVelY*Time.fixedDeltaTime;
		myZ += myVelZ*Time.fixedDeltaTime;

		UpdatePosition();
	}

	bool isAtWaypoint(Waypoint w)
	{
		double distance = Mathf.Sqrt (Mathf.Pow ((float)(w.X-myX),2) + Mathf.Pow ((float)(w.Y-myY),2) + Mathf.Pow ((float)(w.Z-myZ),2));
		if(distance<0) distance = 0-distance;
		if(distance < 2) return true;
		return false;
	}


	//TODO pitääkö tällanen voidi olla fixedupdatessa, vai toimiiko ilman?
	void SetDestination(Waypoint w)
	{
		//pysäytetään rotaatio
		if(IsTurning==true && turningState==0 && localAngularVelocity.magnitude > 0.01) 
		{
			stopRotation();
		}
		//kun rotaatio on pysähtynyt, niin mennään seuraavaan tilaan
		if(IsTurning==true && turningState==0 && localAngularVelocity.magnitude == 0) 
		{
			turningState=1;
		}
		// tilassa 1 alustetaan parametrit kääntymistä varten
		if(turningState==1)
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

			if((DeltadirX<=180 && DeltadirX>=0) || (DeltadirX>=-180 && DeltadirX<=0))
			{
				
				accX = DeltadirX *0.3f;
				decX = DeltadirX *0.7f;
			}
			if(DeltadirX>180)
			{
				DeltadirX = DeltadirX-360f;
				accX = DeltadirX *0.3f;
				decX = DeltadirX *0.7f;
			}
			if(DeltadirX<(-180))
			{
				DeltadirX = DeltadirX+360f;
				accX = DeltadirX *0.3f;
				decX = DeltadirX *0.7f;
			}
			updateDirX = 0f;
			updateDirStepX = transform.rotation.eulerAngles.x;
			updateDirLagStepX = transform.rotation.eulerAngles.x;

			// y-akselille sama. alustuksen voisi yhdistää yhteen metodiin, olle vain annettaisiin tarvittava koordinaattiakseli.
			
			if((DeltadirY<=180 && DeltadirY>=0) || (DeltadirY>=-180 && DeltadirY<=0))
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
			if(DeltadirY<(-180))
			{
				DeltadirY = DeltadirY+360f;
				accY = DeltadirY *0.3f;
				decY = DeltadirY *0.7f;
			}
			updateDirY = 0f;
			updateDirStepY = transform.rotation.eulerAngles.y;
			updateDirLagStepY = transform.rotation.eulerAngles.y;

			// tarkistetaan, kumpaan suuntaan pitää kääntyä x ja y akselien suhteen
			if (deltaRotation.x < 0) turnXToPos = false; 
			if (deltaRotation.y < 0) turnYToPos = false; 

			turningState=2;
			subState2X = 1;
			subState2Y = 1;
		}
		// tila 2 tekee käännökset
		if(turningState==2)
		{
			if (subState2X==1)
			{
				updateDirStepX = transform.rotation.eulerAngles.x;
				updateDirX = updateDirX - (updateDirLagStepX - updateDirStepX);
				if (turnXToPos)
				{
					if (updateDirX<=accX) {rigidbody.AddRelativeTorque (100, 0, 0);}
					if (updateDirX>=decX) {subState2X=2;}
				}
				if (turnXToPos==false)
				{
					if (updateDirX>=accX) {rigidbody.AddRelativeTorque (-100, 0, 0);}
					if (updateDirX<=decX) {subState2X=2;}
				}
				if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.eulerAngles.x ) < 0.1)
				{
					subState2X=2;
				}

				if(updateDirStepX>359 && updateDirLagStepX<1) 
				{
					updateDirLagStepX=+360 + transform.rotation.eulerAngles.x;
				}
				else
				{
					updateDirLagStepX = transform.rotation.eulerAngles.x;
				}
			}
			// y-akselille sama
			if (subState2Y==1)
			{
				updateDirStepY = transform.rotation.eulerAngles.y;
				updateDirY = updateDirY - (updateDirLagStepY - updateDirStepY);
				if (turnYToPos)
				{
					if (updateDirY<=accY) {rigidbody.AddRelativeTorque (0, 100, 0);}
					if (updateDirY>=decY) {subState2Y=2;}
				}
				if (turnYToPos==false)
				{
					if (updateDirY>=accY) {rigidbody.AddRelativeTorque (0, -100, 0);}
					if (updateDirY<=decY) {subState2Y=2;}
				}
				if( Mathf.Abs( deltaRotation.eulerAngles.y - transform.eulerAngles.y ) < 0.1)
				{
					subState2Y=2;
				}
				
				if(updateDirStepY>359 && updateDirLagStepY<1) 
				{
					updateDirLagStepY=+360 + transform.rotation.eulerAngles.y;
				}
				else
				{
					updateDirLagStepY = transform.rotation.eulerAngles.y;
				}
			}

			if (subState2X == 2)
			{
				stopRotationX();
			}

			if (subState2Y == 2)
			{
				stopRotationY();
			}

			if (subState2X==2 && subState2Y==2)
			{
				turningState=3;
				subState2X=0;
				subState2Y=0;
			}
		}

		// pysäytetään rotaatio vielä varmuuden vuoksi
		if (turningState == 3 && localAngularVelocity.magnitude >= 0.001)
		{
			stopRotation();
		}
		if (turningState == 3 && localAngularVelocity.magnitude < 0.001)
		{
			turningState=4;

		}
		// tarkistetaan vielä, onko suunta oikea
		if (turningState == 4)
		{
			// jos ei ole oikea, niin mennään uudestaan tilaan 0 ja aloitetaan alusta
			if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.rotation.eulerAngles.x) > 0.1 && Mathf.Abs( deltaRotation.eulerAngles.y - transform.forward.y) > 0.1)
			{
				turningState=0;
			}
			// muuten lopetetaan kääntyminen
			else 
			{
				IsTurning=false;
				turningState=0;
			}
		}
		return;
	}
	
	void Stop()
	{
		myVelX =  myVelY = myVelZ = 0;
	}

	void stopRotation()
	{
		if (localAngularVelocity.x < 0) {
			rigidbody.AddRelativeTorque (100, 0, 0); 
		}
		if (localAngularVelocity.x > 0) {
			rigidbody.AddRelativeTorque (-100, 0, 0); 
		}
		if (localAngularVelocity.y < 0) {
			rigidbody.AddRelativeTorque (0, 100, 0); 
		}
		if (localAngularVelocity.y > 0) {
			rigidbody.AddRelativeTorque (0, -100, 0); 
		}
		if (localAngularVelocity.z < 0) {
			rigidbody.AddRelativeTorque (0, 0, 100); 
		}
		if (localAngularVelocity.z > 0) {
			rigidbody.AddRelativeTorque (0, 0, -100); 
		}
	}

	void stopRotationX()
	{
		if (localAngularVelocity.x < 0) {
			rigidbody.AddRelativeTorque (100, 0, 0); 
		}
		if (localAngularVelocity.x > 0) {
			rigidbody.AddRelativeTorque (-100, 0, 0); 
		}
	}

	void stopRotationY()
	{
		if (localAngularVelocity.y < 0) {
			rigidbody.AddRelativeTorque (0, 100, 0); 
		}
		if (localAngularVelocity.y > 0) {
			rigidbody.AddRelativeTorque (0, -100, 0); 
		}
	}

}
