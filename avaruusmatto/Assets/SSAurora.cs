using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
public class SSAurora : MonoBehaviour {

	// aluksen paikka koordinaatistossa. 
	public double myX = 0;
	public double myY = 0;
	public double myZ = 0;
  
	// aluksen massa (kg) 
	// työntövoima (N) tämä on konventionaalinen työntövoima esim. dokkausta, väistöliikkeitä ja aluksen valtausta varten. 
	public float myThrust = 1000000;
	// warppi ei toimi Newtonimaisesti. 

	// nopeus maailman kolmen akselin suhteen ja totaalinopeus 
	public float myVelX = 0f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;

	public float maxAngularVel = 10f;

	//v = myThrust/myMass *Delta.time 

	// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
	// Vector3 myAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

	// Use this for initialization
	void Start () {
	//Time.fixedTime
	}

	// Update is called once per frame //pitäiskö tähän laittaa void FixedUpdate?  sit vois käyttää Time.deltaTime -käskyä.
	void FixedUpdate () {

		// My velocity lasketaan joka updatella. kalle 16.2.2015.
		myVelocity = Mathf.Sqrt (Mathf.Pow (myVelX,2) + Mathf.Pow (myVelY,2) + Mathf.Pow (myVelZ,2));

		var rotX = transform.localRotation.x;
		var rotY = transform.localRotation.y;
		var rotZ = transform.localRotation.z;

		// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
		Vector3 localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

		if (Input.GetKey (KeyCode.R)) {

			// koordinaatistotransformaatio lokaalista maailmakoordinaatistoon. aluksen Z-akseli edustaa nokan suuntaa. R on eteenpäin-nappi.
			myVelX = myVelX + myThrust/rigidbody.mass*Time.fixedDeltaTime*Mathf.Cos(rotX);
			myVelY = myVelY + myThrust/rigidbody.mass*Time.fixedDeltaTime*Mathf.Cos(rotY);
			myVelZ = myVelZ + myThrust/rigidbody.mass*Time.fixedDeltaTime*Mathf.Cos(rotY);
			//myVelX = myVelX + myThrust/rigidbody.mass*Time.fixedDeltaTime;	

			myX += myVelX * Time.fixedDeltaTime;
			myY += myVelY * Time.fixedDeltaTime;
			myZ += myVelZ * Time.fixedDeltaTime;
			
		}
		if (Input.GetKey (KeyCode.Q)){
			myVelX = 0;
		}


		var param = new double[3];  //TODO tän vois nimetä myPositionVectoriksi. lisäks tähän vois laittaa loppuun rotaatiot. 
		param [0] = myX;
		param [1] = myY;
		param [2] = myZ;

		if (Input.GetKey (KeyCode.Keypad8)) {
			rigidbody.AddRelativeTorque (0, 0, -100000);
		}
		if (Input.GetKey (KeyCode.Keypad2)) {
			rigidbody.AddRelativeTorque (0, 0, 100000); 
		}
		if (Input.GetKey (KeyCode.Keypad4)) {
			rigidbody.AddRelativeTorque (0, -100000, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad6)) {
			rigidbody.AddRelativeTorque (0, 100000, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad7)) {
			rigidbody.AddRelativeTorque (100000, 0, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad9)) {
			rigidbody.AddRelativeTorque (-100000, 0, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad5)) {

			if (localAngularVelocity.x < 0) {
				rigidbody.AddRelativeTorque (100000, 0, 0); 
			}
			if (localAngularVelocity.x > 0) {
				rigidbody.AddRelativeTorque (-100000, 0, 0); 
			}
			if (localAngularVelocity.y < 0) {
				rigidbody.AddRelativeTorque (0, 100000, 0); 
			}
			if (localAngularVelocity.y > 0) {
				rigidbody.AddRelativeTorque (0, -100000, 0); 
			}
			if (localAngularVelocity.z < 0) {
				rigidbody.AddRelativeTorque (0, 0, 100000); 
			}
			if (localAngularVelocity.z > 0) {
				rigidbody.AddRelativeTorque (0, 0, -100000); 
			}
		}


		GameObject.Find ("_Game").SendMessage ("SetMainCoordinates", param);
	}
}
