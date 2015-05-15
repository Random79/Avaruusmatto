using UnityEngine;
using System.Collections;

public class AIControl : MonoBehaviour {

	static bool created = false;
	public Rigidbody droneShip;
	public GameObject droneText;

	void Awake()
	
	{
		if(!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created=true;
		}
		else Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		var dynObject =  new GameObject("Drones");
		DontDestroyOnLoad(dynObject);
		var textsObject =  new GameObject("3DTexts");
		DontDestroyOnLoad(textsObject);
		var rb = Instantiate(droneShip) as Rigidbody;
		if(rb!=null)
		{ 
			rb.transform.parent = dynObject.transform;
			var ss = rb.gameObject.GetComponentInChildren(typeof(SpaceShip)) as SpaceShip;
			
			if(ss!=null)
			{
				ss.SetPosition(0,0,15);
				ss.Waypoints.Add(new Waypoint(ss.myX,ss.myY,ss.myZ));
				ss.Waypoints.Add(new Waypoint(-100,-100,-500));
				ss.Waypoints.Add(new Waypoint(0,0,15));
				//ss.autopilotState = SpaceShip.autoPilotStates.setDirection;
				var dt = Instantiate(droneText) as GameObject;
				//var dt = GameObject.Find("targetText");
				if(dt!=null)
				{
					var ol = dt.GetComponentInChildren(typeof(ObjectLabel)) as ObjectLabel;
					dt.transform.parent = textsObject.transform;
					if(ol!=null)
					{
						ol.target = ss.transform;
						//ol.transform.parent= ss.transform;
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
