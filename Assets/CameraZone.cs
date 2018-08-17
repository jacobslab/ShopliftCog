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
	private bool alreadyShown = false;
	public MeshRenderer activeMeshRend;

	private int pressCount = 0;
	public static bool showingWarning=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (firstTime)
			activeMeshRend.enabled = true;
		else
			activeMeshRend.enabled = false;

//		Debug.Log ("activate cam: " + activateCam.ToString ());
		if (Input.GetButtonDown("Action Button") && isFocus && !showingWarning && !hasSneaked) {
			Debug.Log ("showing warning is: " + showingWarning.ToString ());
			Debug.Log ("press count is: " + pressCount.ToString ());
			if (pressCount <= 2) {
				Debug.Log ("activate cam: " + activateCam.ToString () + " isFocus : " + isFocus.ToString ());
				if (activateCam && isFocus) {
					Experiment.Instance.shopLift.infoGroup.alpha = 0f;
					Debug.Log ("SHOWING POSITIVE FEEDBACK");
					Sneak ();
					hasSneaked = true;
					StartCoroutine (Experiment.Instance.shopLift.ShowPositiveFeedback ());
				} else if (isFocus && !activateCam && !hasSneaked) {
					pressCount++;
					Debug.Log ("PRESSED ONCE");
					Experiment.Instance.shopLift.infoGroup.alpha = 0f;
					Debug.Log ("SHOWING NEGATIVE FEEDBACK in update");
					StartCoroutine (Experiment.Instance.shopLift.ShowNegativeFeedback ());
					alreadyShown = true;
				}
			} else {
				showingWarning = true;
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
			hasSneaked = false;
			activateCam = true;
			pressCount = 0;
//			Debug.Log ("activate cam is true");
//			Debug.Log ("CAM ACTIVATED for " + gameObject.name);
			activateCam = true;
			if (isTraining) {
//				Debug.Log ("showing sneak text now");
				Experiment.Instance.shopLift.infoText.text = "Press (X) to deactivate the camera!";
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
			if (!hasSneaked && !alreadyShown) {
				Debug.Log ("showing negative feedback on trigger exit");
				StartCoroutine (Experiment.Instance.shopLift.ShowNegativeFeedback ());
				isFocus = false;
				pressCount = 0;
			}
		}

		Experiment.Instance.shopLift.infoGroup.alpha = 0f;

	}

	void OnEnable()
	{
		hasSneaked = false;
		pressCount = 0;
	}

	void OnDisable()
	{
		alreadyShown = false;
		hasSneaked = false;
	}
}
