using UnityEngine;
using System.Collections;

public class KeyboardHander : MonoBehaviour {

	Gamelogic game;
	//GameObject active;
	// Use this for initialization
	void Start () {
		var gameObject = GameObject.Find("Core");
		game = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
	//	active = game.activeKeyHandler;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.F1))
		{
			//game.SwitchToHelm(false);
			Application.LoadLevel ("helm");
		}
		if(Input.GetKey(KeyCode.F2))
		{
			game.SwitchToHelm(true);
		}
		if(Input.GetKey(KeyCode.F3))
		{
			//game.SwitchToScience();
			Application.LoadLevel ("testscene");
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

		if(Input.GetKey(KeyCode.Keypad2))
		{
			p.axis = KeyAxis.vertical;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.Keypad8))
		{
			p.axis = KeyAxis.vertical;
			p.amount =-1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.Keypad6))
		{
			p.axis = KeyAxis.horizontal;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.Keypad4))
		{
			p.axis = KeyAxis.horizontal;
			p.amount =-1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.Keypad9))
		{
			p.axis = KeyAxis.roll;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.Keypad7))
		{
			p.axis = KeyAxis.roll;
			p.amount =-1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.Keypad5))
		{
			p.axis = KeyAxis.stopRot;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}


		if(Input.GetKey(KeyCode.W))
		{
			p.axis = KeyAxis.translateFw;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.S))
		{
			p.axis = KeyAxis.translateFw;
			p.amount =-1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.A))
		{
			p.axis = KeyAxis.translateSide;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.D))
		{
			p.axis = KeyAxis.translateSide;
			p.amount =-1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.R))
		{
			p.axis = KeyAxis.translateUp;
			p.amount = 1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.F))
		{
			p.axis = KeyAxis.translateUp;
			p.amount =-1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}

		if(Input.GetKey(KeyCode.LeftShift))
		{
			p.axis = KeyAxis.lightSpeed;
			p.amount =1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		if(Input.GetKey(KeyCode.LeftControl))
		{
			p.axis = KeyAxis.stop;
			p.amount =1;
			game.activeKeyHandler.SendMessage("HandleAxis",p);
		}
		
		
	}
}
