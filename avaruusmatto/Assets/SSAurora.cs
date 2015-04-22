using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpaceShip))]
public class SSAurora : SpaceShip {

	// aluksen massa (kg) 
	// työntövoima (N) tämä on konventionaalinen työntövoima esim. dokkausta, väistöliikkeitä ja aluksen valtausta varten. 
/*	public float myThrust = 1000000;
	// warppi ei toimi Newtonimaisesti. 

	// nopeus maailman kolmen akselin suhteen ja totaalinopeus 
	public float myVelX = 0f;
	public float myVelY = 0f;
	public float myVelZ = 0f;
	public float myVelocity = 0f;
*/
	//public float maxAngularVel = 0f;
	//Vector3 localAngularVelocity;

	// xy-suunta kohteeseen;
	public Vector2 targetBearing;
	public double targetDistance;

	GameObject game;

	//GameObject ghost;



	//v = myThrust/myMass *Delta.time 

	// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
	// Vector3 myAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

	// Use this for initialization
	void Start () {
	/*	ghost = new GameObject();
		ghost.name = "Ghost";
		ghost.transform.parent = gameObject.transform;
	*/	
		game = GameObject.Find ("_Game");
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


		var param = new double[3];  //TODO tän vois nimetä myPositionVectoriksi. lisäks tähän vois laittaa loppuun rotaatiot. 
		param [0] = myX;
		param [1] = myY;
		param [2] = myZ;
		game.SendMessage("SetMainCoordinates",param);

		myX += myVelX*Time.fixedDeltaTime;
		myY += myVelY*Time.fixedDeltaTime;
		myZ += myVelZ*Time.fixedDeltaTime;
		
		/*
		var heading = target.position - player.position;
		targetDistance = heading.magnitude; magnitude ei välttämättä toimi doublella. 
		//funktio on Mathf.Sqrt(Mathf.pow(heading.x,2)+Mathf.pow(heading.y,2)+Mathf.pow(heading.z,2))
		var direction = heading / distance; 
		targetBearing = direction.eulerangles.x, direction.eulerangles.y;
		*/
		/*
		var gameObject = GameObject.Find("Cylinder");
		if(gameObject!= null)
		{
			SpaceShip ship = (SpaceShip) gameObject.GetComponent(typeof(SpaceShip));
			if(ship!=null)
			{
				var x = ship.myX;
			}
		}
*/
	}

	public void HandleAxis(AxisEventParam p)
	{

		Vector3 deltaVel = GetDirection(p);
		
		Quaternion rotations = GetComponent<Rigidbody>().rotation;
		Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rotations, Vector3.one);
		Vector3 deltaVelRotated = m.MultiplyPoint3x4(deltaVel);
		myVelX += deltaVelRotated.x;
		myVelY += deltaVelRotated.y;
		myVelZ += deltaVelRotated.z;
		

		
		var dir = GetRotation(p);
		GetComponent<Rigidbody>().AddRelativeTorque(dir);
	
	}

	public Vector3 GetDirection(AxisEventParam p)
	{
		var retValue = new Vector3(0,0,0);
		if (p.axis == KeyAxis.translateFw) {
			if (p.amount>0) {
				retValue += new Vector3(0, 0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime);
			}
			if(p.amount<0) {
				retValue += new Vector3(0, 0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1));
			}
		}
		if (p.axis == KeyAxis.translateUp) {
			if (p.amount>0) {
				retValue += new Vector3(0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime, 0);
			}
			if(p.amount<0) {
				retValue += new Vector3(0, myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1), 0);
			}
		}
		if (p.axis == KeyAxis.translateSide) {
			if (p.amount>0) {
				retValue += new Vector3(myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime*(-1), 0, 0);
			}
			if(p.amount<0) {
				retValue += new Vector3(myThrust/GetComponent<Rigidbody>().mass*Time.fixedDeltaTime, 0, 0);
			}
		}

		if (p.axis == KeyAxis.lightSpeed) {	
			retValue += new Vector3(0, 0, 300000000);
		}
		if (p.axis == KeyAxis.stop) {	
			myVelX = 0;
			myVelY = 0;
			myVelZ = 0;
		}

		return retValue;
	}

	public Vector3 GetRotation(AxisEventParam p)
	{
		var retValue = new Vector3(0,0,0);

		if(p.axis == KeyAxis.vertical)
		{
			if (p.amount>0) {
				retValue += new Vector3 (100000, 0, 0); 
			}
			if (p.amount<0) {
				retValue += new Vector3 (-100000, 0, 0); 
			}
		}

		if(p.axis == KeyAxis.horizontal)
		{
			if (p.amount>0) {
				retValue += new Vector3 (0, 100000, 0);  
			}
			if (p.amount<0) {
				retValue += new Vector3 (0, -100000, 0); 
			}
		}
		if(p.axis == KeyAxis.roll)
		{
			if (p.amount>0) {
				retValue += new Vector3 (0, 0, -100000);  
			}
			if (p.amount<0) {
				retValue += new Vector3 (0, 0, 100000); 
			}
		}
		if(p.axis == KeyAxis.stopRot)
		{
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
		/*
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
		}*/
		return retValue;
	}

	void Update()
	{

	}
}
