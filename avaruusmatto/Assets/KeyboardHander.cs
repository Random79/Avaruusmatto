using UnityEngine;
using System.Collections;

public class KeyboardHander : MonoBehaviour {

	Gamelogic game;
	GameObject active;
	// Use this for initialization
	void Start () {
		var gameObject = GameObject.Find("_Game");
		game = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
		active = game.activeKeyHandler;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.F1))
		{
			game.ChangeCamera(0);
		}
		if(Input.GetKey(KeyCode.F2))
		{
			game.ChangeCamera(1);
		}
		if(Input.GetKey(KeyCode.F3))
		{
			game.ChangeCamera(2);
		}

		// forward axis info to active element.
		var v = Input.GetAxis("Vertical");
		if(v !=0)
		{
			AxisEventParam p = new AxisEventParam();
			p.amount = v;
			p.axis=KeyAxis.vertical;

			active.SendMessage("HandleAxis",p);
		}
	}
}
