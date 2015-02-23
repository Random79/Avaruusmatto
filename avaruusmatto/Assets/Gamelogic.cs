using UnityEngine;
using System.Collections;

public class Gamelogic : MonoBehaviour {

	double myX=0;
	double myY=0;
	double myZ=0;

	// Use this for initialization
	void Start () {

		var kap = Resources.Load ("Hatakapsuli") as GameObject;
		var kapsuli = Instantiate (kap) as GameObject;
		var kapso = kapsuli.GetComponent<SpaceObject>();
		kapso.SetPosition(2,1,12);
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
