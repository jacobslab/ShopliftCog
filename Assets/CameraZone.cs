using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

	private bool activateCam = false;
	private bool firstTime = true;
	public int camIndex = 0;
	public static bool isTraining= false;
	public bool isFocus = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (isTraining && Input.GetButtonDown("Sneak Button")) {
			if (activateCam && isFocus) {
				Experiment.Instance.shopLift.infoGroup.alpha = 0f;
				Debug.Log ("SHOWING POSITIVE FEEDBACK");
				Sneak ();
				StartCoroutine (Experiment.Instance.shopLift.ShowPositiveFeedback ());
			} else if(isFocus) {
				Experiment.Instance.shopLift.infoGroup.alpha = 0f;

				Debug.Log ("SHOWING NEGATIVE FEEDBACK");
				StartCoroutine (Experiment.Instance.shopLift.ShowNegativeFeedback ());
			}
		}
		if (activateCam &&  !firstTime && Input.GetButtonDown("Sneak Button")) {
			Sneak ();
			activateCam = false;
		}

	}

	void Sneak()
	{
		Experiment.Instance.shopLiftLog.LogSneaking (Experiment.Instance.shopLift.camVehicle.transform.position, camIndex);
		Debug.Log ("SNEAKING NOW");
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			activateCam = true;
			Debug.Log ("CAM ACTIVATED for " + gameObject.name);
			if (isTraining) {
				Experiment.Instance.shopLift.infoText.text = "Press (A) to Sneak now!";
				Experiment.Instance.shopLift.infoGroup.alpha = 1f;
			}
		}
		
		
	}

	void OnTriggerExit(Collider col)
	{
		firstTime = false;
		if (col.gameObject.tag == "Player") {
			activateCam = false;
			if (isTraining) {
				Experiment.Instance.shopLift.infoGroup.alpha = 0f;
				//make the next cam the focus
				Experiment.Instance.shopLift.ChangeCamZoneFocus (camIndex + 1);
			}
		}
	}
}
