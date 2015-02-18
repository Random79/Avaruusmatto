using UnityEngine;
using System.Collections;

public class SSAurora : MonoBehaviour {

	// aluksen paikka koordinaatistossa. 
	public double myX = 0;
	public double myY = 0;
	public double myZ = 0;

	// aluksen massa (kg) 
	public float myMass = 15000000; //TODO: linkkaa rigidbody mass:iin
	// työntövoima (N) tämä on konventionaalinen työntövoima esim. dokkausta, väistöliikkeitä ja aluksen valtausta varten. 
	public float myThrust = 1000000;
	// warppi ei toimi Newtonimaisesti. 

	// nopeus maailman kolmen akselin suhteen ja totaalinopeus 
	public float myVelX = 0f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;


	//v = myThrust/myMass *Delta.time 

	// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
	//Vector3 myAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

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

		if (Input.GetKey (KeyCode.Keypad8)) {
			myVelX = myVelX + myThrust/myMass*Time.fixedDeltaTime;	
		}
		else if (Input.GetKey (KeyCode.Keypad0)){
			myVelX = 0;
		}
		myX += myVelX * Time.fixedDeltaTime;

		var param = new double[3];
		param [0] = myX;
		param [1] = myY;
		param [2] = myZ;

		if (Input.GetKey (KeyCode.Keypad2)) {
			rigidbody.AddRelativeTorque (0, 0, 10000); 
		}


		GameObject.Find ("_Game").SendMessage ("SetMainCoordinates", param);
	}
}
