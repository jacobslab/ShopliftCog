using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

	private bool activateCam = false;
	public static bool firstTime = true;
	public int camIndex = 0;
    public static bool isPretraining = false; //covers the slider and multiple choice training
	public static bool isTraining= false; //for camera training
	public bool isFocus = false;

	public bool hasSneaked = false;
	private bool alreadyShown = false;

	private int pressCount = 0;
	public static bool showingWarning=false;

    public static bool enableCamZones = true;

    private GameObject activeCamObj;

	public GameObject binoculars;
	public GameObject securityCam;
	public GameObject magnifyingGlass;
	public GameObject wirelessCam;


    public CameraZoneManager camZoneManager;



	// Use this for initialization
	void OnEnable () {

        hasSneaked = false;
		pressCount = 0;
	}

    public void SetCameraObject()
    {
        Debug.Log("setting cam object");
        if (firstTime)
        {
            ToggleCamObjects(false);
            if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
            {
                //securityCam.SetActive(true);
                activeCamObj = securityCam;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.WesternTown)
            {
                //binoculars.SetActive(true);
                activeCamObj = binoculars;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.VikingVillage)
            {
                //magnifyingGlass.SetActive(true);
                activeCamObj = magnifyingGlass;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.Office)
            {
                //securityCam.SetActive(true);
                activeCamObj = securityCam;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.Apartment)
            {
                //securityCam.SetActive(true);
                activeCamObj = securityCam;
                securityCam.transform.localEulerAngles = new Vector3(securityCam.transform.localEulerAngles.x, securityCam.transform.localEulerAngles.y + 180f, securityCam.transform.localEulerAngles.z);
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.MedievalDungeon)
            {
                activeCamObj = securityCam;
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.LibraryDungeon)
            {
                activeCamObj = securityCam;
            }
        }
    }

    public void MakeCamInvisible(bool isInvisible)
    {
        if(activeCamObj!=null)
        {
            //activeCamObj.GetComponent<Renderer>().enabled = !isInvisible;
            activeCamObj.SetActive(!isInvisible);
        }

    }


    public void ToggleCamObjects(bool shouldEnable)
    {
        Debug.Log("toggling cam objects");
        if (activeCamObj != null)
        {
            Debug.Log("active cam obj is " + activeCamObj.ToString());
            Debug.Log("should enable " + shouldEnable.ToString());
            activeCamObj.SetActive(shouldEnable);
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
        if (enableCamZones)
        {
            Debug.Log("showing warning is: " + showingWarning.ToString());
            Debug.Log("press count is: " + pressCount.ToString());
            Debug.Log("isFocus is: " + isFocus.ToString());
            Debug.Log("isPreTraining is: " + isPretraining.ToString());
            Debug.Log("hasSneaked is: " + hasSneaked.ToString());
            if (Input.GetButtonDown("Action Button") && isFocus && !showingWarning &&
                !hasSneaked && !isPretraining && (Experiment.Instance.shopLift.isGamePaused == false))
            {

                ShoplifterScript.haltPlayer = false;
                Experiment.Instance.shopLiftLog.LogButtonPress();
                if (pressCount <= 1)
                {
                    Debug.Log("activate cam: " + activateCam.ToString() + " isFocus : " + isFocus.ToString());
                    if (activateCam && isFocus)
                    {
                        Experiment.Instance.shopLift.infoGroup.alpha = 0f;
                        TCPServer.Instance.SetState(TCP_Config.DefineStates.CAM_CORRECT_PRESS, true);
                        Debug.Log("SHOWING POSITIVE FEEDBACK");
                        Sneak();
                        StopCoroutine(Experiment.Instance.shopLift.ShowNegativeFeedback());
                        StartCoroutine(Experiment.Instance.shopLift.ShowPositiveFeedback());
                    }
                    else if (isFocus && !activateCam && !hasSneaked)
                    {
                        pressCount++;
                        Debug.Log("PRESSED ONCE");
                        Experiment.Instance.shopLift.infoGroup.alpha = 0f;
                        TCPServer.Instance.SetState(TCP_Config.DefineStates.CAM_INCORRECT_PRESS, true);
                        Debug.Log("SHOWING NEGATIVE FEEDBACK in update");
                        StopCoroutine(Experiment.Instance.shopLift.ShowPositiveFeedback());
                        StartCoroutine(Experiment.Instance.shopLift.ShowNegativeFeedback());
                        alreadyShown = true;
                    }
                }
                else if (!isTraining)
                {
                    //only show warning if it is not training
                    showingWarning = true;
                    StartCoroutine(Experiment.Instance.shopLift.ShowWarning());
                }
                //		if (activateCam &&  !firstTime && Input.GetButtonDown("Sneak Button")) {
                //			Sneak ();
                //			activateCam = false;
                //		}

            }
        }

		
	}

	public void Reset()
	{
		firstTime = true;
        ToggleCamObjects(false);
        SetCameraObject();
    }

	void Sneak()
    {
        hasSneaked = true;
        Experiment.Instance.shopLiftLog.LogSneaking (Experiment.Instance.shopLift.camVehicle.transform.position, camIndex);
		Debug.Log ("SNEAKING NOW");
	}

	void OnTriggerEnter(Collider col)
	{
        if (enableCamZones)
        {
            /*if (col.gameObject.tag == "Player" && !isPretraining)
            {
                hasSneaked = false;
                activateCam = true;
                pressCount = 0;
                activateCam = true;
                if (ExperimentSettings.Instance.stage == ExperimentSettings.Stage.Pretraining && firstTime)
                {
                    ShoplifterScript.haltPlayer = true;
                    StartCoroutine(ShoplifterScript.Instance.HaltPlayerMovement());
                    Experiment.Instance.shopLift.infoText.text = "Press (X) to deactivate the camera";
                    activateCam = true;
                    Experiment.Instance.shopLift.infoGroup.alpha = 1f;
                }
            }*/
        }
		
	}

	void OnTriggerExit(Collider col)
	{
        if (enableCamZones)
        {
            /*if (col.gameObject.tag == "Player")
            {
                if (ExperimentSettings.Instance.stage == ExperimentSettings.Stage.Pretraining && !hasSneaked)
                {

                    activateCam = false;
                    //make them repeat that room
                    StartCoroutine(ShoplifterScript.Instance.RepeatRoom());
                    
                }
                else
                {
                    //firstTime = false;
                    activateCam = false;
                    if (!hasSneaked && !alreadyShown)
                    {
                        Debug.Log("showing negative feedback on trigger exit");
                        StartCoroutine(Experiment.Instance.shopLift.ShowNegativeFeedback());
                        isFocus = false;
                        pressCount = 0;
                    }
                }
            }*/
        }
		Experiment.Instance.shopLift.infoGroup.alpha = 0f;

	}

	void OnDisable()
	{
		alreadyShown = false;
		hasSneaked = false;
		//if (firstTime) {
		//	if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation) {
		//		securityCam.SetActive (true);
		//		binoculars.SetActive (false);

		//	} else if (ExperimentSettings.env == ExperimentSettings.Environment.WesternTown) {
		//		securityCam.SetActive (false);
		//		binoculars.SetActive (true);
		//	}else if (ExperimentSettings.env == ExperimentSettings.Environment.VikingVillage) {
		//		securityCam.SetActive (false);
		//		binoculars.SetActive (false);
		//		magnifyingGlass.SetActive (true);
		//	}
		//}
	}
}
