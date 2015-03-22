﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * Degree struct
 */
public struct Degree {
	public int Angle;
	
	public Degree(int a)
	{
		this.Angle = a;
	}
	
	private void normalize()
	{
		while(this.Angle >= 360) this.Angle-=360;
		while(this.Angle<0) this.Angle+=360;
	}
	public static Degree operator +(Degree a, Degree b)
	{
		var c = new Degree();
		c.Angle = a.Angle + b.Angle;
		c.normalize();
		return c;
	}
	public static Degree operator -(Degree a, Degree b)
	{
		var c = new Degree();
		c.Angle = a.Angle-b.Angle;
		c.normalize();
		return c;
	}
	public static Degree operator /(Degree a, Degree b)
	{
		var c = new Degree();
		c.Angle = a.Angle/b.Angle;
		c.normalize();
		return c;
	}
	public static Degree operator *(Degree a, Degree b)
	{
		var c = new Degree();
		c.Angle = a.Angle*b.Angle;
		c.normalize();
		return c;
	}
	
}

public class Gamelogic : MonoBehaviour {

	List <GameObject> kapsulit = new List<GameObject>();
	public List <SpaceObject> SpaceObjects = new List<SpaceObject>(); 
	double myX=0;
	double myY=0;
	double myZ=0;

	// Use this for initialization
	void Start () {

		var dynObject = GameObject.Find("DynamicObjects");
		for (int x = 0; x<100;x++)
		{
			var kap = Resources.Load ("PalavaKapsuli") as GameObject;
			var kapsuli = Instantiate (kap) as GameObject;
			kapsulit.Add(kapsuli);

			var kapso = kapsuli.GetComponent<SpaceObject>();
			kapso.transform.parent = dynObject.transform;
			//kapso.SetPosition(4,2,8);
      		kapso.SetPosition(MyRandom(40,2),MyRandom(40,2),MyRandom(40,2));
		}

	}

	float MyRandom(float range, float ExcludeRadius)
	{
		float r =0;
		do{
			r = Random.Range(0-range,range);
		}while (r>(0-ExcludeRadius) && r<ExcludeRadius);
		return r;
	}


	public Vector3 GetMyCoordinates(double x, double y,double z)
	{
		return new Vector3 ((float)(x - myX), (float)(y-myY),(float)(z-myZ));
	}

	public void SetMainCoordinates(double[] coords)
	{
		myX = coords[0]; 
		myY = coords[1];
		myZ = coords[2];

	}

	public void RegisterSpaceObject(SpaceObject so)
	{
		SpaceObjects.Add(so);
	}

	public void DeregisterSpaceObject(SpaceObject so)
	{
		SpaceObjects.Remove(so);
	}
}
