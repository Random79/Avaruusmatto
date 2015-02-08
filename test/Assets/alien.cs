using UnityEngine;
using System.Collections;

public class alien : MonoBehaviour {

	bool guiActive = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() {
		GameObject.Find ("Game").SendMessage("ActivateQuest");
				if (guiActive)
						guiActive = false;
				else
						guiActive = true;
		}

	void OnGUI(){

			if (GUI.Button (new Rect (300,250,150,30), "Jenson"))
			{
				GameObject.Find ("Game").SendMessage("QuestMessage", "JensonButtonPressed");
			}
	}


}
