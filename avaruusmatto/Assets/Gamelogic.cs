using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gamelogic : MonoBehaviour {

	List <GameObject> kapsulit = new List<GameObject>();
	double myX=0;
	double myY=0;
	double myZ=0;

	// Use this for initialization
	void Start () {

		var dynObject = GameObject.Find("DynamicObjects");
		for (int x = 0; x<1000;x++)
		{
			var kap = Resources.Load ("PalavaKapsuli") as GameObject;
			var kapsuli = Instantiate (kap) as GameObject;
			kapsulit.Add(kapsuli);

			var kapso = kapsuli.GetComponent<SpaceObject>();
			kapso.transform.parent = dynObject.transform;
			kapso.SetPosition(MyRandom(500,2),MyRandom(500,2),MyRandom(500,2));
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
	
	// Update is called once per frame
	void Update () {
	
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
}
