﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

	private bool activateCam = false;
	public static bool firstTime = true;
	public int camIndex = 0;
    public static bool isPretraining = false; //covers the slider and multiple choice training
	public static bool isTraining= false; //for camera training
	public bool isFocus = false;

	private bool hasSneaked = false;
	private bool alreadyShown = false;

	private int pressCount = 0;
	public static bool showingWarning=false;

    private GameObject activeCamObj;

	public GameObject binoculars;
	public GameObject securityCam;
	public GameObject magnifyingGlass;
	public GameObject wirelessCam;

	// Use this for initialization
	void OnEnable () {

        ToggleCamObjects(false);

		hasSneaked = false;
		pressCount = 0;
	}

    public void SetCameraObject()
    {
        if (firstTime)
        {
            if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
            {
                securityCam.SetActive(true);
                activeCamObj = securityCam;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.WesternTown)
            {
                binoculars.SetActive(true);
                activeCamObj = binoculars;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.VikingVillage)
            {
                magnifyingGlass.SetActive(true);
                activeCamObj = magnifyingGlass;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.Office)
            {
                securityCam.SetActive(true);
                activeCamObj = securityCam;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.Apartment)
            {
                securityCam.SetActive(true);
                activeCamObj = securityCam;
                securityCam.transform.localEulerAngles = new Vector3(securityCam.transform.localEulerAngles.x, securityCam.transform.localEulerAngles.y + 180f, securityCam.transform.localEulerAngles.z);
            }
        }
    }


    public void ToggleCamObjects(bool shouldEnable)
    {
        if (activeCamObj != null)
        {
            Debug.Log("active cam obj is " + activeCamObj.ToString());
            activeCamObj.SetActive(false);
        }
        else
        {
            securityCam.SetActive(shouldEnable);
            binoculars.SetActive(shouldEnable);
            magnifyingGlass.SetActive(shouldEnable);
            wirelessCam.SetActive(shouldEnable);
        }
    }
    
	
	// Update is called once per frame
	void LateUpdate () {

//		Debug.Log ("activate cam: " + activateCam.ToString ());
		if (Input.GetButtonDown("Action Button") && isFocus && !showingWarning && !hasSneaked && !isPretraining) {
			Debug.Log ("showing warning is: " + showingWarning.ToString ());
			Debug.Log ("press count is: " + pressCount.ToString ());
            ShoplifterScript.haltPlayer = false;
            Experiment.Instance.shopLiftLog.LogButtonPress();
            if (pressCount <= 1) {
				Debug.Log ("activate cam: " + activateCam.ToString () + " isFocus : " + isFocus.ToString ());
				if (activateCam && isFocus) {
					Experiment.Instance.shopLift.infoGroup.alpha = 0f;
                    TCPServer.Instance.SetState(TCP_Config.DefineStates.CAM_CORRECT_PRESS, true);
                    Debug.Log ("SHOWING POSITIVE FEEDBACK");
					Sneak ();
					hasSneaked = true;
					StartCoroutine (Experiment.Instance.shopLift.ShowPositiveFeedback ());
				} else if (isFocus && !activateCam && !hasSneaked) {
					pressCount++;
					Debug.Log ("PRESSED ONCE");
					Experiment.Instance.shopLift.infoGroup.alpha = 0f;
                    TCPServer.Instance.SetState(TCP_Config.DefineStates.CAM_INCORRECT_PRESS, true);
                    Debug.Log ("SHOWING NEGATIVE FEEDBACK in update");
					StartCoroutine (Experiment.Instance.shopLift.ShowNegativeFeedback ());
					alreadyShown = true;
				}
			} else {
				showingWarning = true;
				StartCoroutine (Experiment.Instance.shopLift.ShowWarning ());
		}
//		if (activateCam &&  !firstTime && Input.GetButtonDown("Sneak Button")) {
//			Sneak ();
//			activateCam = false;
//		}

	}

		
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
		if (col.gameObject.tag == "Player" && !isPretraining) {
			hasSneaked = false;
			activateCam = true;
			pressCount = 0;
			activateCam = true;
			if (isTraining) {
                ShoplifterScript.haltPlayer = true;
                StartCoroutine(ShoplifterScript.Instance.HaltPlayerMovement());
                Experiment.Instance.shopLift.infoText.text = "Press (X) to deactivate the camera";
				activateCam = true;
				Experiment.Instance.shopLift.infoGroup.alpha = 1f;
			}
		}
		
		
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player" && !isPretraining) {
			//firstTime = false;
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

	void OnDisable()
	{
		alreadyShown = false;
		hasSneaked = false;
		if (firstTime) {
			if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation) {
				securityCam.SetActive (true);
				binoculars.SetActive (false);

			} else if (ExperimentSettings.env == ExperimentSettings.Environment.WesternTown) {
				securityCam.SetActive (false);
				binoculars.SetActive (true);
			}else if (ExperimentSettings.env == ExperimentSettings.Environment.VikingVillage) {
				securityCam.SetActive (false);
				binoculars.SetActive (false);
				magnifyingGlass.SetActive (true);
			}
		}
	}
}
