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

	public void ChangeCamera(int c)
	{
		foreach(var cam in cameras)
		{
			cam.enabled = false;
		}
		cameras[c].enabled = true;
	}
}
