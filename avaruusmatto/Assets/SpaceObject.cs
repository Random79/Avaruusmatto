using UnityEngine;
using System.Collections;

public class SpaceObject : MonoBehaviour {

	public double myX = 0;
	public double myY = 0;
	public double myZ = 0;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// haetaan pelin koordinaatiston paikkatieto pelilogiikalta.
		var gameObject = GameObject.Find("_Game");
		Gamelogic gl = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		var newcoordinates = gl.GetMyCoordinates(myX,myY,myZ);
		transform.position=newcoordinates;
	}

	public void SetPosition(double x, double y,double z)
	{
		myX = x;
		myZ = z;
		myY = y;
	}

}

