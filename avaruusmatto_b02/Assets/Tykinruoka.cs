using UnityEngine;
using System.Collections;

public class Tykinruoka : MonoBehaviour {

	public double myX = 0;
	public double myY = 0;
	public double myZ = 0;

	// Use this for initialization
	void Start () {
		myZ = 10;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// haetaan pelin koordinaatiston paikkatieto pelilogiikalta.
		var gameObject = GameObject.Find("Core");
		Gamelogic gl = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		var newcoordinates = gl.GetMyCoordinates(myX,myY,myZ);
		transform.position=newcoordinates;
	
	}

}
