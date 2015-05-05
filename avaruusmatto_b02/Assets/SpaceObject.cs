using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SpaceObject : MonoBehaviour {

	Vector3 lastCoordinates=new Vector3(0,0,0);
	public double myX = 0;
	public double myY = 0;
	public double myZ = 0;
	public float radius = 1;
	
	// Use this for initialization
	void Start () {
		RegisterToGame(false);
		DontDestroyOnLoad(this.gameObject);
	}

	void OnDestroy()
	{
		RegisterToGame(true);
		Destroy(this.gameObject);
	}

	public void RegisterToGame(bool remove)
	{
		var gameObject = GameObject.Find("_Game");
		if(gameObject== null) return;

		Gamelogic gl = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		if(remove) gl.DeregisterSpaceObject(this);
		else gl.RegisterSpaceObject(this);
	}
	// Update is called once per frame
	void FixedUpdate () {
		// haetaan pelin koordinaatiston paikkatieto pelilogiikalta.
		UpdatePosition();
	}

	public void UpdatePosition()
	{
		var gameObject = GameObject.Find("_Game");
		Gamelogic gl = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		var newcoordinates = gl.GetMyCoordinates(myX,myY,myZ);
		if(newcoordinates != lastCoordinates) 
			CheckCollision();
		transform.position=newcoordinates;
	}

	public void SetPosition(double x, double y,double z)
	{
		myX = x;
		myZ = z;
		myY = y;
	}

	public void OnCollision(SpaceObject collisionObject)
	{
		// TODO:  fiksumpi käsittely sille mitä collisiossa tapahtuu..
		if(this is SSAurora) Destroy(collisionObject.gameObject); // auroran ei pitäisi liikkua, eli tähän ei pitäisi tulla
		else if(collisionObject is SSAurora) 
		{
			Destroy(this.gameObject);
			Debug.Log("Deleted: " + this.gameObject.name);
		}
		else 
		{
			Destroy(collisionObject.gameObject);
			Debug.Log("Deleted: " + collisionObject.gameObject.name);
		}
	}

	void CheckCollision()
	{
		var gameObject = GameObject.Find("_Game");
		Gamelogic gl = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		var myPos = gl.GetMyCoordinates(myX,myY,myZ); 
		//var myPos = new Vector3(0,0,0);
		// space objecet listaa vois pitää vaikka gamelogic niin ei tarttis aina hakee
		foreach (SpaceObject o in gl.SpaceObjects) {
			if(o==this) continue;
			// haetaan pelikoordinaatit, että voidaan käyttää unityn ominaisuuksia
			var oPos = gl.GetMyCoordinates(o.myX,o.myY,o.myZ);
   
			// TODO: tähän väliin tsekata kunnolla collision
			var distance = Vector3.Distance(oPos,myPos);
			//Debug.Log(distance);
			if(distance<radius)
			{
				//Debug.Log("Collision ");
				o.OnCollision(this);
			}

		}

	}

}

