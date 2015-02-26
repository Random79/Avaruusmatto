using UnityEngine;
using System.Collections;

public class SpaceShip : SpaceObject {

	public float myThrust = 1000000;
	public float myVelX = 10f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;
	
	public float maxAngularVel = 10f;


	// Use this for initialization
	void Start () {
		myX = -10;
		myVelX = 1;
	}
	
	void FixedUpdate() {
		myVelocity = Mathf.Sqrt (Mathf.Pow (myVelX,2) + Mathf.Pow (myVelY,2) + Mathf.Pow (myVelZ,2));

		Vector3 localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

		myX += myVelX*Time.fixedDeltaTime;
		myY += myVelY*Time.fixedDeltaTime;
		myZ += myVelZ*Time.fixedDeltaTime;

		UpdatePosition();
	}
}
