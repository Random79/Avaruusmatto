using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	Waypoint deltaVector;
	public Quaternion deltaRotation;

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

		//TODO: if at waypoint, SetDestination
		if(isAtWaypoint(Waypoints[currentWaypoint]))
		{
			currentWaypoint++;
			Stop ();
			
			if(currentWaypoint<Waypoints.Count)
			{
				IsTurning=true;
				var currentWp=Waypoints[currentWaypoint];
				deltaVector = Waypoints[currentWaypoint] - new Waypoint(myX,myY,myZ);
				var magnitude = System.Math.Sqrt (System.Math.Pow (deltaVector.X,2) + System.Math.Pow (deltaVector.Y,2) + System.Math.Pow (deltaVector.Z,2));
				deltaVector = deltaVector/magnitude;

				deltaRotation = Quaternion.FromToRotation(new Vector3 ((float)deltaVector.X,(float)deltaVector.Y,(float)deltaVector.Z), transform.rotation);
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

		Vector3 localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

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

	void SetDestination(Waypoint w)
	{
	
		if( Mathf.Abs( deltaRotation.eulerAngles.x - transform.forward.x ) < 0.001 && Mathf.Abs( deltaRotation.eulerAngles.y - transform.forward.y) < 0.001)
		{
			IsTurning=false;
			return;
		}
		if (deltaRotation.eulerAngles.x < 0){
			if (transform.forward.x < deltaRotation.eulerAngles.x/2){
				rigidbody.AddRelativeTorque (100000, 0, 0);
			}
			if (transform.forward.x > deltaRotation.eulerAngles.x/2){
				rigidbody.AddRelativeTorque (-100000, 0, 0);
			}
			
		} 
		
		if (deltaRotation.eulerAngles.x > 0){
			if (transform.forward.x > deltaRotation.eulerAngles.x/2){
				rigidbody.AddRelativeTorque (-100000, 0, 0);
			}
			if (transform.forward.x > deltaRotation.eulerAngles.x/2){
				rigidbody.AddRelativeTorque (100000, 0, 0);
			}
			
		} 



	}

	void Stop()
	{
		myVelX =  myVelY = myVelZ = 0;
	}
}
