using UnityEngine;
using System.Collections;

public class SSAurora : MonoBehaviour {
	long myX=0;
	long myY = 0;
	long myZ=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Keypad8)) {
			myX--;	
		}

		GameObject.Find ("_Game").SendMessage ("SetMainCoordinates", new Vector3 (myX, myY, myZ));
	}
}
