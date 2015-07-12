using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class helmPanelScript : MonoBehaviour {
	public GameObject sourceObject;
	private SpaceShip sourceShip;

	private Text yawText;
	private Text pitchText;
	private Text rollText;

	private Text myXText;
	private Text myYText;
	private Text myZText;

	private Text spdText;

	private InputField setBearingXText, setBearingYText, setBearingZText;




	// Use this for initialization
	void Start () {
		sourceShip = (SpaceShip) sourceObject.GetComponent( typeof(SpaceShip));
		var spdMeter = GameObject.Find("pilotInfoPanel/velocityValue");
		if(spdMeter!= null)
		{
			spdText= (Text) spdMeter.GetComponent(typeof(Text));
		}

		var temp = GameObject.Find("attitudevalues/yawValue");
		if(temp!= null)
		{
			yawText = (Text) temp.GetComponent(typeof(Text));
		}

		temp = GameObject.Find("attitudevalues/pitchValue");
		if(temp!= null)
		{
			pitchText = (Text) temp.GetComponent(typeof(Text));
		}

		temp = GameObject.Find("attitudevalues/rollValue");
		if(temp!= null)
		{
			rollText = (Text) temp.GetComponent(typeof(Text));
		}

		temp = GameObject.Find("positionValues/xValue");
		if(temp!= null)
		{
			myXText = (Text) temp.GetComponent(typeof(Text));
		}

		temp = GameObject.Find("positionValues/yValue");
		if(temp!= null)
		{
			myYText = (Text) temp.GetComponent(typeof(Text));
		}

		temp = GameObject.Find("positionValues/zValue");
		if(temp!= null)
		{
			myZText = (Text) temp.GetComponent(typeof(Text));
		}
		
		var srotxt = GameObject.Find("helmPanel/setRotationXText");
		if(srotxt!= null)
		{
			setBearingXText = (InputField) srotxt.GetComponent(typeof(InputField));
		}
		var srotyt = GameObject.Find("helmPanel/setRotationYText");
		if(srotyt!= null)
		{
			setBearingYText = (InputField) srotyt.GetComponent(typeof(InputField));
		}
		var srotzt = GameObject.Find("helmPanel/setRotationZText");
		if(srotzt!= null)
		{
			setBearingZText = (InputField) srotzt.GetComponent(typeof(InputField));
		}
	}

	public void setBearing()
	{
		float BearingXInput, BearingYInput, BearingZInput;
		/*
		if(float.TryParse(setRotationXText.text, out rotationXInput)) return;
		if(float.TryParse(setRotationYText.text, out rotationYInput)) return;
		if(float.TryParse(setRotationZText.text, out rotationZInput)) return;
		*/
		if(string.IsNullOrEmpty(setBearingXText.text) 
		   || string.IsNullOrEmpty(setBearingYText.text) 
		   || string.IsNullOrEmpty(setBearingZText.text))
			return;

		BearingXInput = float.Parse(setBearingXText.text);
		BearingYInput = float.Parse(setBearingYText.text);
		BearingZInput = float.Parse(setBearingZText.text);
		sourceShip.SetBearing(BearingXInput,BearingYInput,BearingZInput);
		//Debug.Log("seRotation:"+rotationXInput.ToString()+" "+rotationYInput.ToString()+" "+rotationZInput.ToString());


	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if(spdText!=null)
		{
			spdText.text = sourceShip.myVelocity.ToString("F2");
		}

		if(yawText!=null)
		{
			yawText.text =  sourceShip.transform.rotation.eulerAngles.y.ToString("00.000");
		}

		if(pitchText!=null)
		{
			pitchText.text =  sourceShip.transform.rotation.eulerAngles.x.ToString("00.000");
		}

		if(rollText!=null)
		{
			rollText.text =  sourceShip.transform.rotation.eulerAngles.z.ToString("00.000");
		}

		if(myXText!=null)
		{
			myXText.text = sourceShip.myX.ToString("#,##0");
		}

		if(myYText!=null)
		{
			myYText.text = sourceShip.myY.ToString("#,##0");
		}

		if(myZText!=null)
		{
			myZText.text = sourceShip.myZ.ToString("#,##0");
		}

	}
}
