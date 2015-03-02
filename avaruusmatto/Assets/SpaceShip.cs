using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
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

	public Vector3 localAngularVelocity;

	// Use this for initialization
	void Start () {
		myX=-5;
		Waypoints.Add(new Waypoint(myX,myY,myZ));
		Waypoints.Add(new Waypoint(6,7,8));
		if(Waypoints.Count>0)
			SetDestination(Waypoints[0]);
		Stop ();
	}
	
	void FixedUpdate() {

		// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
		localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

		//TODO: if at waypoint, SetDestination
		if(isAtWaypoint(Waypoints[currentWaypoint]))
		{
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
		if(IsTurning==true && turningState==0 && localAngularVelocity.magnitude >= 0.01) 
		{
			turningState=1;
		}
		// tilassa 1 alustetaan parametrit kääntymmistä varten
		if(turningState==1)
		{
			//vektori omasta pisteestä waypointille
			deltaVector = Waypoints[currentWaypoint] - new Waypoint(myX,myY,myZ);
			// yksikkövektori. eli komponentien neliösumman neliöjuuri on 1. 
			var magnitude = System.Math.Sqrt (System.Math.Pow (deltaVector.X,2) + 
							System.Math.Pow (deltaVector.Y,2) + System.Math.Pow (deltaVector.Z,2));
			// tämä on yhden pituinen vektori, joka osoittaa waypointin suuntaan.
			deltaVector = deltaVector/magnitude;
			// tämä on quaternioni muodossa oleva pyörähdysjuttu waypointin suuntaan
			deltaRotation = Quaternion.FromToRotation(new Vector3 ((float)deltaVector.X,(float)deltaVector.Y,(float)deltaVector.Z), 
			                                          new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z));

			// tarkistetaan, kumpaan suuntaan pitää kääntyä x ja y akselien suhteen
			if (deltaRotation.eulerAngles.x > 0) turnXToPos = false; 
			if (deltaRotation.eulerAngles.y > 0) turnYToPos = false; 
			turningState=2;
		}
		// tila 2 tekee käännökset
		if(turningState==2)
		{
			if (turnXToPos){
				if (transform.forward.x < deltaRotation.eulerAngles.x/2){
					rigidbody.AddRelativeTorque (100000, 0, 0);
				}
				if (transform.forward.x > deltaRotation.eulerAngles.x/2){
					rigidbody.AddRelativeTorque (-100000, 0, 0);
				}
				
			} 
			
			if (turnXToPos==false){
				if (transform.forward.x > deltaRotation.eulerAngles.x/2){
					rigidbody.AddRelativeTorque (-100000, 0, 0);
				}
				if (transform.forward.x > deltaRotation.eulerAngles.x/2){
					rigidbody.AddRelativeTorque (100000, 0, 0);
				}
				
			} 
			if (turnYToPos){
				if (transform.forward.y < deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, 100000, 0);
				}
				if (transform.forward.y > deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, -100000, 0);
				}
				
			} 
			
			if (turnYToPos==false){
				if (transform.forward.y > deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, -100000, 0);
				}
				if (transform.forward.y > deltaRotation.eulerAngles.y/2){
					rigidbody.AddRelativeTorque (0, 100000, 0);
				}
				
			}
			// sit, kun ollaan päästy oikeaan suuntaan, vaihdetaan tila
			if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.forward.x ) < 0.001 && Mathf.Abs( deltaRotation.eulerAngles.y - transform.forward.y) < 0.001)
			{
				turningState=3;
			}
		}
		// pysäytetään rotaatio vielä varmuuden vuoksi
		if (turningState == 3 && localAngularVelocity.magnitude >= 0.01)
		{
			stopRotation();
		}
		if (turningState == 3 && localAngularVelocity.magnitude < 0.01)
		{
			turningState=4;

		}
		// tarkistetaan vielä, onko suunta oikea
		if (turningState == 4)
		{
			// jos ei ole oikea, niin mennään uudestaan tilaan 0 ja aloitetaan alusta
			if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.forward.x ) > 0.001 && Mathf.Abs( deltaRotation.eulerAngles.y - transform.forward.y) > 0.001)
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
			rigidbody.AddRelativeTorque (100000, 0, 0); 
		}
		if (localAngularVelocity.x > 0) {
			rigidbody.AddRelativeTorque (-100000, 0, 0); 
		}
		if (localAngularVelocity.y < 0) {
			rigidbody.AddRelativeTorque (0, 100000, 0); 
		}
		if (localAngularVelocity.y > 0) {
			rigidbody.AddRelativeTorque (0, -100000, 0); 
		}
		if (localAngularVelocity.z < 0) {
			rigidbody.AddRelativeTorque (0, 0, 100000); 
		}
		if (localAngularVelocity.z > 0) {
			rigidbody.AddRelativeTorque (0, 0, -100000); 
		}
	}

}
