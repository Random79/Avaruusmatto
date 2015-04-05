using UnityEngine;
using System.Collections;

public class KeyboardHander : MonoBehaviour {

	Gamelogic game;
	// Use this for initialization
	void Start () {
		var gameObject = GameObject.Find("_Game");
		game = (Gamelogic) gameObject.GetComponent(typeof(Gamelogic));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.F1))
		{
			game.ChangeCamera(1);
		}
		if(Input.GetKey(KeyCode.F2))
		{
			game.ChangeCamera(2);
		}
		if(Input.GetKey(KeyCode.F3))
		{
			game.ChangeCamera(3);
		}
	}
}
