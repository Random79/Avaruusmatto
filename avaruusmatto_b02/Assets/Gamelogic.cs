using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


public class Gamelogic : MonoBehaviour {

	List <GameObject> kapsulit = new List<GameObject>();
	public List <SpaceObject> SpaceObjects = new List<SpaceObject>(); 
	double myX=0;
	double myY=0;
	double myZ=0;

	public List<Camera> cameras;
	public GameObject activeKeyHandler;
	public GameObject Helm;
	public GameObject Science;


	private static bool created=false;
	void Awake()
	{
		if(!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created=true;
		}
		else Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start () {

		
		
	}

	void FixedUpdate ()
	{
		foreach (var obj in SpaceObjects)
		{
			if(obj is SSAurora) continue;
			var newcoordinates = GetMyCoordinates(obj.myX,obj.myY,obj.myZ);
			// todo check collision
			//if(newcoordinates != lastCoordinates) 
			//	CheckCollision();
			obj.transform.position=newcoordinates;
		}
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

	public void ChangeCamera(int c)
	{
		foreach(var cam in cameras)
		{
			cam.enabled = false;
		}
		cameras[c].enabled = true;
	}

	public void SwitchToHelm(bool thirdPerson)
	{
		if(thirdPerson) ChangeCamera(1);
		else ChangeCamera(0);
		activeKeyHandler = Helm;
	}
	public void SwitchToScience()
	{
		ChangeCamera(2);
		activeKeyHandler = Science;
	}
}
