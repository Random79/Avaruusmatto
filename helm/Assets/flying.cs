using UnityEngine;
using System.Collections;

public class flying : MonoBehaviour {


	public float maxWarpSpeed = 900.0f;
	public float maxAngularVel = 0.0001f;
	public float warpTime = 4f;
	public float timer = 0f;
	public float warpSequence = 0f;
	public float setSpeed = 50f;
	//public bool warpGenerator = true;
	public float velX = 0f;
	public float velY = 0f;
	public float velZ = 0f;
	public float velocity = 0f;
	public bool warp = false;

	// Use this for initialization
	//void Start () {
	
	//}

	// näppiskomennot
	void FixedUpdate () {

		Vector3 localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);
		velX = rigidbody.velocity.x ;
		velY = rigidbody.velocity.y;
		velZ = rigidbody.velocity.z;
		velocity = Mathf.Sqrt (Mathf.Pow (velX,2) + Mathf.Pow (velY,2) + Mathf.Pow (velZ,2));

		// Warp Drive
		if (Input.GetKeyDown (KeyCode.KeypadPlus) && Mathf.Abs (localAngularVelocity.x) < maxAngularVel && Mathf.Abs (localAngularVelocity.y) < maxAngularVel && Mathf.Abs (localAngularVelocity.z) < maxAngularVel  && velX < maxWarpSpeed ) {
			warp = true;
		}			

		if (warp == true && warpSequence == 0) {
			warpSequence = 1;	
		}

		if(warpSequence == 1) {
			timer += Time.deltaTime;
		}
		if (timer >= 3f && warpSequence == 1){
			warpSequence = 2;
			timer = 0f;
		}
		if (warpSequence == 2){
			timer += Time.deltaTime;
		}
		if (timer >= warpTime && warpSequence == 2) {
			warpSequence = 3;
			timer = 0f;
		}
		if (warpSequence == 2 && velocity < maxWarpSpeed){
			rigidbody.AddRelativeForce (maxWarpSpeed,0,0, ForceMode.VelocityChange);
		}
		if (warpSequence == 3) {
			rigidbody.AddRelativeForce (-(velocity-setSpeed),0,0, ForceMode.VelocityChange);
			warpSequence = 0;
			warp = false;
		}

		// Roll
		if (warpSequence == 0) {
						if (Input.GetKey (KeyCode.Keypad8)) {
								rigidbody.AddRelativeTorque (0, 0, -10000);
						}
						if (Input.GetKey (KeyCode.Keypad2)) {
								rigidbody.AddRelativeTorque (0, 0, 10000); 
						}
						if (Input.GetKey (KeyCode.Keypad4)) {
								rigidbody.AddRelativeTorque (0, -10000, 0); 
						}
						if (Input.GetKey (KeyCode.Keypad6)) {
								rigidbody.AddRelativeTorque (0, 10000, 0); 
						}
						if (Input.GetKey (KeyCode.Keypad7)) {
								rigidbody.AddRelativeTorque (10000, 0, 0); 
						}
						if (Input.GetKey (KeyCode.Keypad9)) {
								rigidbody.AddRelativeTorque (-10000, 0, 0); 
						}
						if (Input.GetKey (KeyCode.Keypad5)) {

					
								if (localAngularVelocity.x < 0) {
										rigidbody.AddRelativeTorque (10000, 0, 0); 
								}
								if (localAngularVelocity.x > 0) {
										rigidbody.AddRelativeTorque (-10000, 0, 0); 
								}
								if (localAngularVelocity.y < 0) {
										rigidbody.AddRelativeTorque (0, 10000, 0); 
								}
								if (localAngularVelocity.y > 0) {
										rigidbody.AddRelativeTorque (0, -10000, 0); 
								}
								if (localAngularVelocity.z < 0) {
										rigidbody.AddRelativeTorque (0, 0, 10000); 
								}
								if (localAngularVelocity.z > 0) {
										rigidbody.AddRelativeTorque (0, 0, -10000); 
								}
						}
				}
		// Translate
		if (Input.GetKey (KeyCode.W)) {
			rigidbody.AddRelativeForce (0.1f, 0, 0); 
		}
		if (Input.GetKey (KeyCode.S)) {
			rigidbody.AddRelativeForce (-0.1f, 0, 0); 
		}
		if (Input.GetKey (KeyCode.R)) {
			rigidbody.AddRelativeForce (0, 0.1f, 0); 
		}
		if (Input.GetKey (KeyCode.F)) {
			rigidbody.AddRelativeForce (0, -0.1f, 0); 
		}
		if (Input.GetKey (KeyCode.A)) {
			rigidbody.AddRelativeForce (0, 0, 0.1f); 
		}
		if (Input.GetKey (KeyCode.D)) {
			rigidbody.AddRelativeForce (0, 0, -0.1f); 
		}
	}

}