using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour {

	public Transform tx;
	public int StarAmount = 100;
	public float StarSize = 1.0f;
	public float StarDistance = 10;
	public float StarClipDistance = 0.3f;
	private float StarDistanceSqr,StarClipDistanceSqr;
	private ParticleSystem.Particle[] points;
	private SSAurora player;
	// Use this for initialization
	void Start () {
		//tx = transform;
		StarDistanceSqr = StarDistance*StarDistance;
		StarClipDistanceSqr = StarClipDistance * StarClipDistance;

		points = new ParticleSystem.Particle[StarAmount];
		for (int i=0 ; i<StarAmount; i++)
		{
			points[i].position = Random.insideUnitSphere*StarDistance;
			points[i].color = Color.white;
			points[i].size = StarSize;
		}

		var p = GameObject.Find("Vessel");
		if(p!=null)
		{
			player = (SSAurora) p.GetComponent(typeof(SSAurora));
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = tx.transform.position + tx.transform.forward *5;
		for(int i =0 ; i<StarAmount;i++)
		{
			if(points[i].position.sqrMagnitude > StarDistanceSqr
			   || points[i].position.magnitude < StarClipDistance)
			{
				points[i].position = Random.insideUnitSphere*StarDistance;
			}

			points[i].position -=  new Vector3(player.myVelX *Time.deltaTime,player.myVelY*Time.deltaTime,player.myVelZ*Time.deltaTime);
		}

		var p = GetComponent<ParticleSystem>() as ParticleSystem;
		p.SetParticles(points,points.Length);
	}
}
