using UnityEngine;
using System.Collections;
using System.Xml;

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
		// todo: mihin kuuluu?
		string[] args = System.Environment.GetCommandLineArgs();
		foreach(var a in args)
		{
			Debug.Log (a);
		}


		var apppath = UnityEngine.Application.dataPath + "/missions/default";
		ParseObjects(apppath,dynObject, textsObject);
	}

	void ParseObjects(string dir, GameObject parent, GameObject textParent)
	{
		var filename = dir +"/objects.xml";
		XmlDocument doc = new XmlDocument ();
		doc.Load("file://"+filename);
		
		var ships = doc.SelectNodes("//spaceships/spaceship");
		foreach (XmlNode ship in ships)
		{
			var rb = Instantiate(droneShip) as Rigidbody;
			if(rb!=null)
			{ 
				rb.transform.parent = parent.transform;
				var ss = rb.gameObject.GetComponentInChildren(typeof(SpaceShip)) as SpaceShip;
				rb.name = ship.Attributes["name"].Value;
				if(ss!=null)
				{
					var x = double.Parse(ship.Attributes["x"].Value);
					var y = double.Parse(ship.Attributes["y"].Value);
					var z = double.Parse (ship.Attributes["z"].Value);
					ss.SetPosition(x,y,z);
					var waypoints = ship.SelectNodes("waypoints/waypoint");
					foreach (XmlNode wp in waypoints)
					{
						var wpx = double.Parse(wp.Attributes["x"].Value);
						var wpy = double.Parse(wp.Attributes["y"].Value);
						var wpz = double.Parse(wp.Attributes["z"].Value);
						ss.Waypoints.Add(new Waypoint(wpx,wpy,wpz));
					}
				/*	ss.Waypoints.Add(new Waypoint(ss.myX,ss.myY,ss.myZ));
					ss.Waypoints.Add(new Waypoint(-100,-100,-500));
					ss.Waypoints.Add(new Waypoint(0,0,15));
					//ss.autopilotState = SpaceShip.autoPilotStates.setDirection;
					*/
					var dt = Instantiate(droneText) as GameObject;
					//var dt = GameObject.Find("targetText");

					if(dt!=null)
					{
						var ol = dt.GetComponentInChildren(typeof(ObjectLabel)) as ObjectLabel;
						dt.transform.parent = textParent.transform;
						if(ol!=null)
						{
							ol.target = ss.transform;
							//ol.transform.parent= ss.transform;
						}
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
