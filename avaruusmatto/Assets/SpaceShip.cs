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

	public Vector3 localAngularVelocity;

	// Use this for initialization
	void Start () {
		myX = 0;
		myY = 0;
		myZ = 5;
		Waypoints.Add(new Waypoint(0,0,myZ));	// Veny menee tänne
		Waypoints.Add(new Waypoint(6,7,8));		// Tämä pitäisi kai olla suunta mihin veny katsoo?
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

				// quaternioni euleriksi
 				
				// tarkista onko deltarotation.x negatiivinen vai positiivinen
				// jos negatiivinen
				//   
				//transform.forward < deltaRotation/2;


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
			// tähän jäin. Ei pitäis olla negatiivinen... johtuu 360 ylityksestä

			/*
			 * Tällanen pitää laittaa c sharpille. h on transform.forward ja d on toDir.X
			Ddir<-d-h
			Ddir

			if((Ddir<=180 && Ddir>=0) || (Ddir>=-180 && Ddir<=0)){
				
				kiihdytys<-Ddir/2
				stop<-Ddir
			}
			if(Ddir>180){ 
				Ddir<-Ddir-360
				kiihdytys<-Ddir/2
				stop<-Ddir
			} 

			if(Ddir<(-180)){
				Ddir<-Ddir+360
				kiihdytys<-Ddir/2
				stop<-Ddir
			}

			*/

			DeltadirX = transform.forward.x - toDir.x;
			accX = DeltadirX*0.45f;
			decX = DeltadirX*0.65f;

			updateDirX = 0;

			// tarkistetaan, kumpaan suuntaan pitää kääntyä x ja y akselien suhteen
			if (deltaRotation.x > 0) turnXToPos = false; 
			if (deltaRotation.y > 0) turnYToPos = false; 

			turningState=2;
		}
		// tila 2 tekee käännökset
		if(turningState==2)
		{
			updateDirX = updateDirX + (transform.forward.x - updateDirX);
			if (turnXToPos)
			{
				if (updateDirX<=accX) {rigidbody.AddRelativeTorque (100, 0, 0);}
				if (updateDirX>=decX) {rigidbody.AddRelativeTorque (-100, 0, 0);}
			}
			if (turnXToPos==false)
			{
				if (updateDirX>=accX) {rigidbody.AddRelativeTorque (-100, 0, 0);}
				if (updateDirX<=decX) {rigidbody.AddRelativeTorque (100, 0, 0);}
			}

		}


		/*
		{
			if (turnXToPos){
				if (transform.rotation.x < deltaRotation.x*0.6){
					rigidbody.AddRelativeTorque (100, 0, 0);
				}
				if (transform.rotation.x > deltaRotation.x*0.4){
					rigidbody.AddRelativeTorque (-100, 0, 0);
				}
				
			} 
			
			if (turnXToPos==false){
				if (transform.rotation.x > deltaRotation.x*0.4){
					rigidbody.AddRelativeTorque (-100, 0, 0);
				}
				if (transform.rotation.x < deltaRotation.x*0.6){
					rigidbody.AddRelativeTorque (100, 0, 0);
				}
				
			} 

			if (turnYToPos){
				if (transform.eulerAngles.y > deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, 100, 0);
				}
				if (transform.eulerAngles.y < deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, -100, 0);
				}
				
			} 
			
			if (turnYToPos==false){
				if (transform.eulerAngles.y > deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, -100, 0);
				}
				if (transform.eulerAngles.y < deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, 100, 0);
				}
				
			}
			*/
			// sit, kun ollaan päästy oikeaan suuntaan, vaihdetaan tila
			if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.eulerAngles.x ) < 0.01)// && Mathf.Abs( deltaRotation.eulerAngles.y - transform.forward.y) < 0.001)
			{
				turningState=3;
			}
		//}
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
			if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.forward.x ) > 0.01
			   /*&& Mathf.Abs( deltaRotation.eulerAngles.y - transform.forward.y) > 0.001*/)
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
			rigidbody.AddRelativeTorque (1000, 0, 0); 
		}
		if (localAngularVelocity.x > 0) {
			rigidbody.AddRelativeTorque (-1000, 0, 0); 
		}
		if (localAngularVelocity.y < 0) {
			rigidbody.AddRelativeTorque (0, 1000, 0); 
		}
		if (localAngularVelocity.y > 0) {
			rigidbody.AddRelativeTorque (0, -1000, 0); 
		}
		if (localAngularVelocity.z < 0) {
			rigidbody.AddRelativeTorque (0, 0, 1000); 
		}
		if (localAngularVelocity.z > 0) {
			rigidbody.AddRelativeTorque (0, 0, -1000); 
		}
	}

}
