using UnityEngine;
using System.Collections;

public class Gamelogic : MonoBehaviour {

	Vector3 mainv = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 GetMyCoordinates(Vector3 v)
	{
		return new Vector3 (v.x - mainv.x, v.y, v.z);
	}

	public void SetMainCoordinates(Vector3 nv)
	{
		mainv=nv;
	}
}
