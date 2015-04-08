using UnityEngine;
using System.Collections;

public class KeyboardHander : MonoBehaviour {

	Gamelogic game;
	//GameObject active;
	// Use this for initialization
	void Start () {
		var gameObject = GameObject.Find("_Game");
		game = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
	//	active = game.activeKeyHandler;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.F1))
		{
			game.SwitchToHelm(false);
		}
		if(Input.GetKey(KeyCode.F2))
		{
			game.SwitchToHelm(true);
		}
		if(Input.GetKey(KeyCode.F3))
		{
			game.SwitchToScience();
		}

		// forward axis info to active element.
		/*var v = Input.GetAxis("Vertical");
		Debug.Log(v);
		if(v !=0)
		{
			AxisEventParam p = new AxisEventParam();
			p.amount = v;
			p.axis=KeyAxis.vertical;

			active.SendMessage("HandleAxis",p);
		}*/
		AxisEventParam p = new AxisEventParam();
		p.amount = 1;
		if(Input.GetKey(KeyCode.Keypad8))
		{
			p.axis = KeyAxis.vertical;
		}
		if(Input.GetKey(KeyCode.Keypad2))
		{
			p.axis = KeyAxis.vertical;
			p.amount =-1;
		}
		if(Input.GetKey(KeyCode.Keypad6))
		{
			p.axis = KeyAxis.horizontal;
		}
		if(Input.GetKey(KeyCode.Keypad4))
		{
			p.axis = KeyAxis.horizontal;
			p.amount =-1;
		}
		if(Input.GetKey(KeyCode.Keypad9))
		{
			p.axis = KeyAxis.roll;
		}
		if(Input.GetKey(KeyCode.Keypad7))
		{
			p.axis = KeyAxis.roll;
			p.amount =-1;
		}
		if(Input.GetKey(KeyCode.Keypad5))
		{
			p.axis = KeyAxis.stopRot;
		}
		game.activeKeyHandler.SendMessage("HandleAxis",p);
	}
}
