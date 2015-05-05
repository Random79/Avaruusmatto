using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

	public Rigidbody droneShip;
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

/*		var dynObject = GameObject.Find("DynamicObjects");
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
*/		
		var dynObject =  new GameObject("Drones");
		DontDestroyOnLoad(dynObject);
		var rb = Instantiate(droneShip) as Rigidbody;
		if(rb!=null)
		{ 
			rb.transform.parent = dynObject.transform;
			var ss = rb.gameObject.GetComponentInChildren(typeof(SpaceShip)) as SpaceShip;

			if(ss!=null)
			{
				ss.SetPosition(0,0,15);
				ss.Waypoints.Add(new Waypoint(ss.myX,ss.myY,ss.myZ));
				ss.Waypoints.Add(new Waypoint(-100,-100,-500));
				ss.Waypoints.Add(new Waypoint(0,0,15));
				//ss.autopilotState = SpaceShip.autoPilotStates.setDirection;
			}
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
