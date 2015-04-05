using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	public Camera movingCam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey("up"))
		{
			this.transform.Rotate(new Vector3(-1,0,0));
		}
		if(Input.GetKey("down"))
		{
			this.transform.Rotate(new Vector3(1,0,0));
		}
		if(Input.GetKey("left"))
		{
			this.transform.Rotate(new Vector3(0,-1,0));
		}
		if(Input.GetKey("right"))
		{
			this.transform.Rotate(new Vector3(0,1,0));
		}
		if(Input.GetKey(KeyCode.Home))
		{
			this.transform.Rotate(new Vector3(0,0,1));
		}
		if(Input.GetKey(KeyCode.End))
		{
			this.transform.Rotate(new Vector3(0,0,-1));
		}
		if(Input.GetKey(KeyCode.PageUp))
		{
			this.transform.Translate(Vector3.forward*Time.deltaTime*100);
		}
		if(Input.GetKey(KeyCode.PageDown))
		{
			this.transform.Translate(new Vector3(0,0,-1)*Time.deltaTime*100);
		}

	}
}
