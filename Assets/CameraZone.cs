using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

	private bool activateCam = false;
	private bool firstTime = true;
	public int camIndex = 0;
	public static bool isTraining= false;
	public bool isFocus = false;

	private bool hasSneaked = false;
	private bool justOnce = true;
	private bool alreadyShown = false;

	private int pressCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (firstTime)
			GetComponent<MeshRenderer> ().enabled = true;
		else
			GetComponent<MeshRenderer> ().enabled = false;

//		Debug.Log ("activate cam: " + activateCam.ToString ());
		if (Input.GetButtonDown("Action Button") && justOnce && isFocus) {
			
			if (pressCount <= 2) {
				Debug.Log ("activate cam: " + activateCam.ToString () + " isFocus : " + isFocus.ToString ());
				if (activateCam && isFocus) {
					Experiment.Instance.shopLift.infoGroup.alpha = 0f;
					Debug.Log ("SHOWING POSITIVE FEEDBACK");
					Sneak ();
					justOnce = false;
					hasSneaked = true;
					StartCoroutine (Experiment.Instance.shopLift.ShowPositiveFeedback ());
				} else if (isFocus && !activateCam && !hasSneaked) {
					pressCount++;
					Experiment.Instance.shopLift.infoGroup.alpha = 0f;
					Debug.Log ("SHOWING NEGATIVE FEEDBACK");
					StartCoroutine (Experiment.Instance.shopLift.ShowNegativeFeedback ());
					alreadyShown = true;
				}
			} else {
				StartCoroutine (Experiment.Instance.shopLift.ShowWarning ());
			}
		}
//		if (activateCam &&  !firstTime && Input.GetButtonDown("Sneak Button")) {
//			Sneak ();
//			activateCam = false;
//		}

	}

	public void Reset()
	{
		firstTime = true;
	}

	void Sneak()
	{
		Experiment.Instance.shopLiftLog.LogSneaking (Experiment.Instance.shopLift.camVehicle.transform.position, camIndex);
		Debug.Log ("SNEAKING NOW");
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			justOnce = true;
			hasSneaked = false;
			activateCam = true;
//			Debug.Log ("activate cam is true");
//			Debug.Log ("CAM ACTIVATED for " + gameObject.name);
			activateCam = true;
			if (isTraining) {
//				Debug.Log ("showing sneak text now");
				Experiment.Instance.shopLift.infoText.text = "Press (X) to Sneak now!";
				activateCam = true;
				Experiment.Instance.shopLift.infoGroup.alpha = 1f;
			}
		}
		
		
	}

	void OnTriggerExit(Collider col)
	{
		firstTime = false;
		if (col.gameObject.tag == "Player") {
			activateCam = false;
//			Debug.Log ("activate cam is false");
			if (!hasSneaked && !alreadyShown) {
				Debug.Log ("showing negative feedback");
				StartCoroutine (Experiment.Instance.shopLift.ShowNegativeFeedback ());
				isFocus = false;
				alreadyShown = false;
			}
				//make the next cam the focus
//				Experiment.Instance.shopLift.ChangeCamZoneFocus (camIndex + 1);

			Experiment.Instance.shopLift.infoGroup.alpha = 0f;
			pressCount = 0;
		}

	}
}
