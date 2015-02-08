using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quest : MonoBehaviour {
	bool questActive =  false;
	int state = 1;
	public class data {
		public enum  dtype{
			text = 0,
			button
		};
		public int state;
		public dtype type;
		public string text;
		public int toState;
	}

	List<data> questdata = new List<data>();

	// Use this for initialization
	void Start () {
		questdata.Add (new data{state = 1, type=data.dtype.text,text = "This is first dialog, please click next"});
		questdata.Add (new data{state = 1, type=data.dtype.button,text = "next",toState=2});
		questdata.Add (new data{state = 2, type=data.dtype.text,text = "Would you click the Jenson Button?"});
		questdata.Add (new data{state = 2, type=data.dtype.button,text = "yes",toState=3});
		questdata.Add (new data{state = 2, type=data.dtype.button,text = "no",toState=1});
		questdata.Add (new data{state = 4, type=data.dtype.text,text = "Thank you"});
		questdata.Add (new data{state = 4, type=data.dtype.button,text = "exit",toState=5});

	}
	
	// Update is called once per frame
	void Update () {
		if (state == 5)
						questActive = false;
	}

	void OnGUI(){

		if (questActive) {
			string questText ="";
			foreach(var d in questdata) {
				if(d.state == state && d.type == data.dtype.text)
				questText += d.text + "\n";
				}
			GUI.TextArea(new Rect(200,10,600,100),questText);

			GUI.BeginGroup(new Rect(200, 150, 200, 200));
			int numbuttons = 0;
			foreach(var d in questdata) {
				if(d.state == state && d.type == data.dtype.button)
				{
					if (GUI.Button (new Rect (0,numbuttons*50,150,30), d.text))
					{
						state = d.toState;
					}
					numbuttons++;
				}
						
			}
			GUI.EndGroup();

			if (GUI.Button (new Rect (0,250,150,30), "Reset"))
			{
				state = 1;
			}
		}
			
	}

	void ActivateQuest()
	{
		if (questActive)
						questActive = false;
				else {
						questActive = true;
				}

	}

	void QuestMessage(string param)
	{
		if(param == "JensonButtonPressed" && state == 3) state++;
	}
}
