using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChangeZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
//			Debug.Log ("about to RANDOMIZE SPEED");
			Experiment.Instance.shopLift.RandomizeSpeed ();
		}


	}
}
