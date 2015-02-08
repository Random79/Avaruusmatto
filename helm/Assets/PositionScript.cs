using UnityEngine;
using System.Collections;

public class PositionScript : MonoBehaviour {
	
	// Private cacheRigidBody
	private Rigidbody _cacheRigidbody;
	private Vector3 meterDistance = new Vector3 (0,0,0); 
	private Vector3 kilometerDistance = new Vector3 (0,0,0); 
	private Vector3 astronomicalUnitDistance = new Vector3 (0,0,0); 
	private Vector3 lightyearDistance = new Vector3 (0,0,0); 
	
	// Update is called once per frame
	void Update () {
		// Update Position
		// AE Unit = 149 597 870 700 meter
		meterDistance = meterDistance + _cacheRigidbody.velocity;
		
		// Update the kilometerDistance vector after meterDistance vector reaches +-1000 size
		// Update the astronomicalDistance vector after kilometerDistance vector reaches +-149597871 size
		// Update the lightyearDistance  vector after astronomicalDistance vector reaches +-63241.077 size
	}
}
