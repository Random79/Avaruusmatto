using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarpPanelScript : MonoBehaviour {

	public GameObject SourceObject;
	public GameObject CountDownObject;

	private Text warpAmountText;
	private Text warpTimeText;
	private SpaceShip ship;
	// Use this for initialization
	void Start () {
		var warpAmount = GameObject.Find("warpPanel/setWarpSpeedText/Text");
		if(warpAmount!= null)
		{
			warpAmountText= (Text) warpAmount.GetComponent(typeof(Text));
		}
		var warpTime = GameObject.Find("warpPanel/setWarpTimeText/Text");
		if(warpTime!= null)
		{
			warpTimeText= (Text) warpTime.GetComponent(typeof(Text));
		}
		if(SourceObject !=null)
		{
			ship = (SpaceShip)SourceObject.GetComponent(typeof(SpaceShip));
		}
	}
	
	public void EngageWarpPressed()
	{
		var co = GameObject.Find("centerPosition");
		if(co==null || string.IsNullOrEmpty(warpAmountText.text) || string.IsNullOrEmpty(warpTimeText.text))
		{
			return;
		}
		float warp;
		if(!float.TryParse(warpAmountText.text,out warp)) return;
		float time;
		if(!float.TryParse(warpTimeText.text,out time)) return;

		StartCoroutine(WarpEngageProcedure(co,warp,time));
	}

	IEnumerator WarpEngageProcedure(GameObject co,float warp, float time)
	{

		var cd = Instantiate(CountDownObject) as GameObject;
		if(cd!=null)
		{ 
			
			var c = cd.gameObject.GetComponentInChildren(typeof(Countdown)) as Countdown;
			if(c!=null )
			{
				c.CountFrom = 5;
				c.originalPosition = co.transform;
				
			}
		}
		
		yield return new WaitForSeconds(5);

		var cd2 = Instantiate(CountDownObject) as GameObject;
		if(cd2!=null)
		{ 
			
			var c2 = cd2.gameObject.GetComponentInChildren(typeof(Countdown)) as Countdown;
			if(c2!=null )
			{
				c2.CountFrom = time;
				c2.originalPosition = co.transform;
				c2.shake = true;
				c2.moveAway = false;
				c2.prefix ="WARP:\n";
				c2.numberFormat = "#.#";
				
			}
			
		}
		ship.SetWarpFactor(warp);
		yield return new WaitForSeconds(time);
		ship.SetWarpFactor(0);
	}
}
