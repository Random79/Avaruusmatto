using UnityEngine;
using System.Collections;

public class keepme : MonoBehaviour {
	private bool created;
	void Awake()
	{
		if(!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created=true;
		}
		else Destroy(this.gameObject);
	}
}
