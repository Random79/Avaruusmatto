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

	//GameObject ghost;

	public Camera mainCam;
	public Camera SecondaryCam;

	//v = myThrust/myMass *Delta.time 

	// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
	// Vector3 myAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

	// Use this for initialization
	void Start () {
	/*	ghost = new GameObject();
		ghost.name = "Ghost";
		ghost.transform.parent = gameObject.transform;
	*/	SecondaryCam.enabled = false;
		mainCam.enabled = true;
	//Time.fixedTime
	}

	// Update is called once per frame //pitäiskö tähän laittaa void FixedUpdate?  sit vois käyttää Time.deltaTime -käskyä.
	void FixedUpdate () {
		//System.Windows.Media.Matrix


		// My velocity lasketaan joka updatella. kalle 16.2.2015.
		myVelocity = Mathf.Sqrt (Mathf.Pow (myVelX,2) + Mathf.Pow (myVelY,2) + Mathf.Pow (myVelZ,2));
		
		//  (0,0,10) transform -> (4,4,4) 

		// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
		Vector3 localAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

		//ghost.transform.rotation = gameObject.transform.rotation;

		if (Input.GetKey (KeyCode.R)) {

			Vector3 deltaVel = new Vector3(0, 0, myThrust/rigidbody.mass*Time.fixedDeltaTime);
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

		if (Input.GetKey (KeyCode.F)) {
			
			Vector3 deltaVel = new Vector3(0, 0, myThrust/rigidbody.mass*Time.fixedDeltaTime*(-1));
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

		if (Input.GetKey (KeyCode.W)) {
			
			Vector3 deltaVel = new Vector3(0, myThrust/rigidbody.mass*Time.fixedDeltaTime, 0);
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

		if (Input.GetKey (KeyCode.S)) {

			Vector3 deltaVel = new Vector3(0, myThrust/rigidbody.mass*Time.fixedDeltaTime*(-1), 0);
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

		if (Input.GetKey (KeyCode.A)) {
			
			Vector3 deltaVel = new Vector3(myThrust/rigidbody.mass*Time.fixedDeltaTime*(-1), 0, 0);
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}
		
		if (Input.GetKey (KeyCode.D)) {
			
			Vector3 deltaVel = new Vector3(myThrust/rigidbody.mass*Time.fixedDeltaTime, 0, 0);
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
		
			Vector3 deltaVel = new Vector3(0, 0, 300000000);
			Quaternion rotations = rigidbody.rotation;
			Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
			Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
			myVelX += deltaVelRotated.x;
			myVelY += deltaVelRotated.y;
			myVelZ += deltaVelRotated.z;
		}

		myX += myVelX*Time.fixedDeltaTime;
		myY += myVelY*Time.fixedDeltaTime;
		myZ += myVelZ*Time.fixedDeltaTime;


		if (Input.GetKey (KeyCode.Keypad9)) {
			rigidbody.AddRelativeTorque (0, 0, -100000);
		}
		if (Input.GetKey (KeyCode.Keypad7)) {
			rigidbody.AddRelativeTorque (0, 0, 100000); 
		}
		if (Input.GetKey (KeyCode.Keypad4)) {
			rigidbody.AddRelativeTorque (0, -100000, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad6)) {
			rigidbody.AddRelativeTorque (0, 100000, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad8)) {
			rigidbody.AddRelativeTorque (100000, 0, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad2)) {
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

		var param = new double[3];  //TODO tän vois nimetä myPositionVectoriksi. lisäks tähän vois laittaa loppuun rotaatiot. 
		param [0] = myX;
		param [1] = myY;
		param [2] = myZ;

		GameObject.Find ("_Game").SendMessage ("SetMainCoordinates", param);
	
	}

	void Update()
	{
		// camera control
		if(Input.GetKeyUp(KeyCode.F1))
		{	
			mainCam.enabled = mainCam.enabled==true ? false : true;
			SecondaryCam.enabled = SecondaryCam.enabled == true ?  false : true;
		}
	}
}
