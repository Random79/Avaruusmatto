using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class helmPanelScript : MonoBehaviour {
	public GameObject sourceObject;
	private SpaceShip sourceShip;

	private Text headingText;
	private Text spdText;
	private Text positionText;


	// Use this for initialization
	void Start () {
		sourceShip = (SpaceShip) sourceObject.GetComponent( typeof(SpaceShip));
		var spdMeter = GameObject.Find("helmPanel/spdMeter");
		if(spdMeter!= null)
		{
			spdText= (Text) spdMeter.GetComponent(typeof(Text));
		}

		var helmMeter = GameObject.Find("helmPanel/headingMeter");
		if(helmMeter!= null)
		{
			headingText = (Text) helmMeter.GetComponent(typeof(Text));
		}

		var posMeter = GameObject.Find("helmPanel/positionMeter");
		if(posMeter!= null)
		{
			positionText = (Text) posMeter.GetComponent(typeof(Text));
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if(spdText!=null)
		{
			spdText.text = sourceShip.myVelocity.ToString("F2");
		}

		if(headingText!=null)
		{
			headingText.text = "\n" + sourceShip.transform.rotation.eulerAngles.x.ToString("00.000") + "\n"
				+ sourceShip.transform.rotation.eulerAngles.y.ToString("00.000") + "\n"
					+ sourceShip.transform.rotation.eulerAngles.z.ToString("00.000");
		}

		if(positionText!=null)
		{
			positionText.text = "\n" + sourceShip.myX.ToString("#,##0") + "\n"
				+ sourceShip.myY.ToString("#,##0") + "\n"
					+ sourceShip.myZ.ToString("#,##0");
		}


	}
}
