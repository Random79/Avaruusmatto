using UnityEngine;
using System.Collections;

		/*
		 * 	tässä on R -kielellä matriisipyöritys.  tämä pitäis kääntää tarpvitavilta osin ceelle.
		 * pointtina on, että x,y ja z ovat rotaatioita akselien suhteen. Rx,Ry ja Rz ovat rotaatiomatriiseja
		 * velvec on aluksen lokaalin koordinaatiston liikesuunta. Esim. (0,0,10) on 10 liikettä z-akselin suuntaan, eli eteenpäin.
		 * muista oikea matriisikertolaskun järjestys!!!
		 * 
		 * deg2Rad ja rad2Deg ovat jo Cllä olevia komentoja. r käyttää radiaaneja ja ilmeisesti unity aika mielellään asteita...
		deg2Rad<-pi/180
		rad2Deg<-180/pi

		* tässä ovat rotaatiot 
		x<-90*deg2Rad
		y<-0
		z<-0

		* pyöräytysmatriisit
		Rx<-matrix(c(1,0,0, 0,cos(x),-sin(x), 0,sin(x),cos(x)),nrow=3,byrow=TRUE)
		Ry<-matrix(c(cos(y),0,sin(y), 0,1,0, -sin(y),0,cos(y)),nrow=3,byrow=TRUE)
		Rz<-matrix(c(cos(z),-sin(z),0, sin(z),cos(z),0, 0,0,1),nrow=3,byrow=TRUE)

		* alus kulkee 10 eteenpäin (z-akselin suuntaan)
		velVec<-c(0,0,10)

		*päivitetty sijainti maailman koordinaatistossa
		worldpos<-velVec%*%Rz%*%Ry%*%Rx
		worldpos
		*/
		

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

	GameObject ghost;

	//v = myThrust/myMass *Delta.time 

	// vector3:n tallennettu pyörimisnopeus (localAngularVelocity.x, localAngularVelocity.y, localAngularVelocity.z)
	// Vector3 myAngularVelocity = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity);

	// Use this for initialization
	void Start () {
		ghost = new GameObject();
		
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

		ghost.transform.rotation = gameObject.transform.rotation;

		if (Input.GetKey (KeyCode.R)) {


			ghost.transform.Translate(Vector3.forward*myThrust/rigidbody.mass*Mathf.Pow(Time.fixedDeltaTime,2));
			//myLocalVelZ = myVelX + myThrust/rigidbody.mass*Time.fixedDeltaTime;	
			//rigidbody.AddRelativeForce (0, 0.1f, 0);
			myVelX += ghost.transform.rotation.x;
			myVelY += ghost.transform.rotation.y;
			myVelZ += ghost.transform.rotation.z;
		}
		if (Input.GetKey (KeyCode.Q)){
			myVelX = 0;
		}



		myX += myVelX;
		myY += myVelY;
		myZ += myVelZ;

		var param = new double[3];  //TODO tän vois nimetä myPositionVectoriksi. lisäks tähän vois laittaa loppuun rotaatiot. 
		param [0] = myX;
		param [1] = myY;
		param [2] = myZ;

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


		GameObject.Find ("_Game").SendMessage ("SetMainCoordinates", param);
	}
}
