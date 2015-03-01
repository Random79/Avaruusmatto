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
	public double X;
	public double Y;
	public double Z;
}

public class SpaceShip : SpaceObject {

	public List<Waypoint> Waypoints = new List<Waypoint>();

	int currentWaypoint = 0;
	public float myThrust = 1000000;
	public float myVelX = 10f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;
	
	public float maxAngularVel = 10f;


	// Use this for initialization
	void Start () {
		myX=-5;
		Waypoints.Add(new Waypoint(0,1,5));
		if(Waypoints.Count>0)
			SetDestination(Waypoints[0]);
	}
	
	void FixedUpdate() {

		//TODO: if at waypoint, SetDestination
		if(isAtWaypoint(Waypoints[currentWaypoint]))
		{
			currentWaypoint++;
			if(Waypoints.Count>currentWaypoint)
				SetDestination(Waypoints[currentWaypoint]);
			else Stop();
		}

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
	
		//var magnitude = System.Math.Sqrt (System.Math.Pow (w.X,2) + System.Math.Pow (w.Y,2) + System.Math.Pow (w.Z,2));

		//Vector3 waypointUnitDirection = new Vector3 (w.X/magnitude, w.Y/magnitude, w.Z/magnitude);

		// tehdään deltaRotation
		//float deltaRotation = Vector3.Angle(waypointUnitDirection, transform.forward);

	
		// while (transform.forward.x != deltaRotation.x +-toleranssi && transform.forward.y != deltaRotation.y +-toleranssi ) {

		// pyöräytetään X ja Y rotaatioo erikseen komennolla rigidbody.AddRelativeTorque (-100000, 0, 0); rotaation määrä*0.4 asti. 
		// annetaan olla rotaation määrä*0.6 asti vapaasti ja sitten hidastetaan vastakkaiseen suuntaan rotaation määrä*1 asti
	
	
	
		//}

	


	}

	void Stop()
	{
		myVelX =  myVelY = myVelZ = 0;
	}
}
