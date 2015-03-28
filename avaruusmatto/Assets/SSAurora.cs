using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpaceObject))]
public class SSAurora : SpaceObject {

	// aluksen massa (kg) 
	// työntövoima (N) tämä on konventionaalinen työntövoima esim. dokkausta, väistöliikkeitä ja aluksen valtausta varten. 
	public float myThrust = 1000000;
	// warppi ei toimi Newtonimaisesti. 

	// nopeus maailman kolmen akselin suhteen ja totaalinopeus 
	public float myVelX = 0f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;

	public float maxAngularVel = 0f;
	Vector3 localAngularVelocity;

	// xy-suunta kohteeseen;
	public Vector2 targetBearing;
	public double targetDistance;

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
		RegisterToGame(false);
	//Time.fixedTime
	}

	void OnDestroy()
	{
		RegisterToGame(true);
	}

	// Update is called once per frame //pitäiskö tähän laittaa void FixedUpdate?  sit vois käyttää Time.deltaTime -käskyä.
	void FixedUpdate () {
		//System.Windows.Media.Matrix


		// My velocity lasketaan joka updatella. kalle 16.2.2015.
		myVelocity = Mathf.Sqrt (Mathf.Pow (myVelX,2) + Mathf.Pow (myVelY,2) + Mathf.Pow (myVelZ,2));

		// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
		localAngularVelocity = GetComponent<Rigidbody>().transform.InverseTransformDirection(GetComponent<Rigidbody>().angularVelocity);

		//ghost.transform.rotation = gameObject.transform.rotation;

		Vector3 deltaVel = GetDirection();

		Quaternion rotations = GetComponent<Rigidbody>().rotation;
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
		Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
		myVelX += deltaVelRotated.x;
		myVelY += deltaVelRotated.y;
		myVelZ += deltaVelRotated.z;

		myX += myVelX*Time.fixedDeltaTime;
		myY += myVelY*Time.fixedDeltaTime;
		myZ += myVelZ*Time.fixedDeltaTime;

		var dir = GetRotation();
		GetComponent<Rigidbody>().AddRelativeTorque(dir);

		var param = new double[3];  //TODO tän vois nimetä myPositionVectoriksi. lisäks tähän vois laittaa loppuun rotaatiot. 
		param [0] = myX;
		param [1] = myY;
		param [2] = myZ;

		/*
		var heading = target.position - player.position;
		targetDistance = heading.magnitude; magnitude ei välttämättä toimi doublella. 
		//funktio on Mathf.Sqrt(Mathf.pow(heading.x,2)+Mathf.pow(heading.y,2)+Mathf.pow(heading.z,2))
		var direction = heading / distance; 
		targetBearing = direction.eulerangles.x, direction.eulerangles.y;
		*/

		GameObject.Find ("_Game").SendMessage ("SetMainCoordinates", param);
	
	}

	public Vector3 GetDirection()
	{
		var retValue = new Vector3(0,0,0);
		if (Input.GetKey (KeyCode.W)) {
			retValue += new Vector3(0, 0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			retValue += new Vector3(0, 0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1));
		}
		if (Input.GetKey (KeyCode.R)) {			
			retValue += new Vector3(0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime, 0);
		}
		if (Input.GetKey (KeyCode.F)) {
			retValue += new Vector3(0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1), 0);
		}
		if (Input.GetKey (KeyCode.A)) {	
			retValue += new Vector3(myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1), 0, 0);
		}
		if (Input.GetKey (KeyCode.D)) {	
			retValue += new Vector3(myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime, 0, 0);
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {	
			retValue += new Vector3(0, 0, 300000000);
		}

		return retValue;
	}

	public Vector3 GetRotation()
	{
		var retValue = new Vector3(0,0,0);
		if (Input.GetKey (KeyCode.Keypad9)) {
			retValue += new Vector3 (0, 0, -100000);
		}
		if (Input.GetKey (KeyCode.Keypad7)) {
			retValue += new Vector3 (0, 0, 100000); 
		}
		if (Input.GetKey (KeyCode.Keypad4)) {
			retValue += new Vector3 (0, -100000, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad6)) {
			retValue += new Vector3 (0, 100000, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad8)) {
			retValue += new Vector3 (100000, 0, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad2)) {
			retValue += new Vector3 (-100000, 0, 0); 
		}
		if (Input.GetKey (KeyCode.Keypad5)) {
			
			if (localAngularVelocity.x < 0) {
				retValue += new Vector3 (100000, 0, 0); 
			}
			if (localAngularVelocity.x > 0) {
				retValue += new Vector3 (-100000, 0, 0); 
			}
			if (localAngularVelocity.y < 0) {
				retValue += new Vector3 (0, 100000, 0); 
			}
			if (localAngularVelocity.y > 0) {
				retValue += new Vector3 (0, -100000, 0); 
			}
			if (localAngularVelocity.z < 0) {
				retValue += new Vector3 (0, 0, 100000); 
			}
			if (localAngularVelocity.z > 0) {
				retValue += new Vector3 (0, 0, -100000); 
			}
		}
		return retValue;
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
