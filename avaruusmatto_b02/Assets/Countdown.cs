using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {
	public float CountFrom;
	public float CounterValue;
	public Transform originalPosition;
	public string prefix;
	public string numberFormat;
	public bool moveAway = true;
	public float moveAwaySpeed = 5;
	public bool shake = false;

	private TextMesh text;
	private string oldValue="";
	// Use this for initializaion
	void Start () {
		text= gameObject.GetComponent("TextMesh") as TextMesh;
		CounterValue = CountFrom;
		transform.position = originalPosition.position;
		if(string.IsNullOrEmpty(prefix)) prefix = "";
		if(string.IsNullOrEmpty(numberFormat)) numberFormat = "0";
	}
	
	// Update is called once per frame
	void Update () {
		float oldCounterValue = CounterValue;
		CounterValue -= Time.deltaTime;


		if(moveAway)
			gameObject.transform.position += gameObject.transform.forward *moveAwaySpeed*Time.deltaTime;
		if(shake)
			gameObject.transform.position += new Vector3(Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10))*Time.deltaTime;

		var showValue= CounterValue.ToString(numberFormat) ;

		// onko sekunttiluku vaihtunut, palataan alkupisteeseen
		if(!oldValue.Equals(showValue))
		{
			gameObject.transform.position = originalPosition.position;
		}
		oldValue=showValue;
		text.text = prefix + showValue;
		if(CounterValue<0)
		{
			this.enabled = false;
			Destroy(gameObject);
		}
	}
}
