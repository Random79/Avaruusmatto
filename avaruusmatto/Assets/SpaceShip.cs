﻿using UnityEngine;
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
		double distance = Mathf.Sqrt (Mathf.Pow (w.X-myX,2) + Mathf.Pow (w.Y-myY,2) + Mathf.Pow (w.Z-myZ,2));
		if(distance<0) distance = 0-distance;
		if(distance < 2) return true;
		return false;
	}

	void SetDestination(Waypoint w)
	{
		myVelX = 1;
		// TODO; tee jotain järkevää
	}

	void Stop()
	{
		myVelX =  myVelY = myVelZ = 0;
	}
}
