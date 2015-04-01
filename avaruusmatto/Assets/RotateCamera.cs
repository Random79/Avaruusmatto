using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	public Camera movingCam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = Vector3.zero;
		var amount = 0;
		if(Input.GetKey("up"))
		{
			dir += transform.right;
			amount = 1;
		}
		if(Input.GetKey("down"))
		{
			dir += transform.forward;
			amount = -1;
		}
		if(Input.GetKey("left"))
		{
			dir += transform.up;
			amount = 1;
		}
		if(Input.GetKey("right"))
		{
			dir += transform.up;
			amount = -1;
		}

		if(dir != Vector3.zero)
			transform.RotateAround(Vector3.zero,dir,amount);

		if(Input.GetKey(KeyCode.H))
		{
			var newcoordinates = transform.position;
			newcoordinates.y += 1;
			transform.position = newcoordinates;

		}
	}
}
