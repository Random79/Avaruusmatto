using UnityEngine;
using System.Collections;

public class Tykinruoka : MonoBehaviour {

	Vector3 position = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	/*	if (Input.GetKey (KeyCode.Keypad8)) {

		}
*/

		// haetaan pelin koordinaatiston paikkatieto pelilogiikalta.
		var gameObject = GameObject.Find("_Game");
		Gamelogic gl = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		var newcordinates = gl.GetMyCoordinates(position);
		transform.position=(newcordinates);
	
	}

}
