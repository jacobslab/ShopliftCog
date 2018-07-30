using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
public class ShoplifterScript : MonoBehaviour {

    public GameObject camVehicle;
    public GameObject animBody;
    private MouseLook mouseLook;

	private float currentSpeed;

	public float minSpeed = 10f;
	public float maxSpeed  = 30f;

	//PHASE 1
	private GameObject phase1Start_L;
	private GameObject phase1End_L;
	private GameObject phase1Start_R;
	private GameObject phase1End_R;


	//PHASE 2
	private GameObject phase2Start_L;
	private GameObject phase2End_L;
	private GameObject phase2Start_R;
	private GameObject phase2End_R;

	//PHASE 3
	private GameObject phase3Start_L;
	private GameObject phase3End_L;
	private GameObject phase3Start_R;
	private GameObject phase3End_R;

	//registers
	private GameObject register_L;
	private GameObject register_R;

	//registerobj
	private GameObject leftRegisterObj;
	private GameObject rightRegisterObj;

	//doors
	private GameObject phase1Door_L;
	private GameObject phase1Door_R;
	private GameObject phase2Door_L;
	private GameObject phase2Door_R;
	private GameObject phase3Door_L;
	private GameObject phase3Door_R;


	//environments
	public GameObject spaceStationEnv;
	public GameObject cybercityEnv;

	//speed change zones
	public List<GameObject> phase1SpeedChangeZones_L;
	public List<GameObject> phase1SpeedChangeZones_R;
	public List<GameObject> phase2SpeedChangeZones_L;
	public List<GameObject> phase2SpeedChangeZones_R;
	public List<GameObject> phase3SpeedChangeZones_L;
	public List<GameObject> phase3SpeedChangeZones_R;


	//stage 2 reevaulation variables
	public int maxTrials_Reeval = 8;
	public int maxBlocks_Reeval = 4;

    public GameObject leftDoorPos;
    public GameObject rightDoorPos;
    public float phase1Factor = 5f;
    public Animator cartAnim;
    private int playerChoice = -1; //0 for left and 1 for right
    public int[] registerVals; // 0-1 is L-R for toy , 2-3 is L-R for hardware

	private string activeEnvLabel = "";

	public List<GameObject> environments;
	private EnvironmentManager envManager;

    private GameObject leftRoom;
    private GameObject rightRoom;

    private int phase1Choice = 0;
    private int phase2Choice = 0;
    private int choiceOutput = 0;

    public Transform phase1Target;
    public Transform phase2Target;

	private bool clearCameraZoneFlags = false;

    private Transform camTrans;

    private int numTrials = 0;
    private int maxTrials = 4;

    //ui
	public CanvasGroup instructionGroup;
	public CanvasGroup infoGroup;
    public Text infoText;
    public CanvasGroup intertrialGroup;
	public Text intertrialText;
	public CanvasGroup positiveFeedbackGroup;
	public CanvasGroup negativeFeedbackGroup;
	public CanvasGroup trainingInstructionsGroup;
	public CanvasGroup trainingPeriodGroup;
	public CanvasGroup restGroup;
	public Text rewardScore;
	public CanvasGroup warningFeedbackGroup;


	private GameObject roomOne;
	private GameObject roomTwo;

	private AudioSource baseAudio;
	private AudioSource roomOneAudio;
	private AudioSource roomTwoAudio;
	private AudioSource choiceAudio;
	private AudioSource leftAudio;
	private AudioSource rightAudio;

	private Color leftRoomColor;
	private Color rightRoomColor;
	private Color roomOneColor;
	private Color roomTwoColor;

	public GameObject coinShower;

	private List<int> registerLeft;
	private int stageIndex  = 0;

	//camera zone
	private GameObject phase1CamZone_L;
	private GameObject phase1CamZone_R;
	private GameObject phase2CamZone_L;
	private GameObject phase2CamZone_R;
	private GameObject phase3CamZone_L;
	private GameObject phase3CamZone_R;

	private GameObject activeCamZone;

	public Text rigidStatusText;

    public GameObject dummyObj;

	public GameObject testFloor;


    // Use this for initialization
    void Start() {
		UpdateFirstEnvironments ();
        infoGroup.alpha = 0f;
		positiveFeedbackGroup.alpha = 0f;
		negativeFeedbackGroup.alpha = 0f;
        intertrialGroup.alpha = 0f;
		trainingInstructionsGroup.alpha = 0f;
		trainingPeriodGroup.alpha = 0f;
        registerVals = new int[4];
//        cartAnim.enabled = true;
        camVehicle.SetActive(true);
//        animBody.SetActive(false);

        dummyObj.transform.eulerAngles = new Vector3(0f, 90f, 0f);
//        cartAnim.Play("Phase1Start");
        //camVehicle.transform.position = phase1Start.transform.position;
//        mouseLook = camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook;
        camTrans = camVehicle.GetComponent<RigidbodyFirstPersonController>().cam.transform;
		RandomizeSpeed ();
//		RandomizeCameraZones ();
		rewardScore.enabled=false;
//		mouseLook.XSensitivity = 0f;

		Cursor.visible = false;
		//then start the task
		StartCoroutine("RunTask");
    }

    // Update is called once per frame
    void Update() {
//        if (Input.GetKeyDown(KeyCode.E)) {
//            StartCoroutine("RunTask");
//        }
//		rigidStatusText.text = camVehicle.GetComponent<Rigidbody>().velocity.ToString();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
			Application.Quit ();
//			phase2Walls [0] = toyColor;
            //camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook.m_CharacterTargetRot = Quaternion.Euler(dummyObj.transform.eulerAngles);
    //        camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook.m_CameraTargetRot = Quaternion.identity;
     //       camVehicle.GetComponent<RigidbodyFirstPersonController>().cam.transform.rotation = Quaternion.Euler(dummyObj.transform.eulerAngles);
        //     camVehicle.GetComponent<RigidbodyFirstPersonController>().RotateView(dummyObj.transform);
        }


    }

	void RandomizeSpeedChangeZones()
	{
		Debug.Log ("randomized speed change zones");
		phase1SpeedChangeZones_L [0].transform.position = new Vector3(phase1Start_L.transform.position.x,phase1Start_L.transform.position.y,Random.Range(phase1Start_L.transform.position.z,Vector3.Lerp (phase1Start_L.transform.position, phase1End_L.transform.position, 0.5f).z));
		phase1SpeedChangeZones_L [1].transform.position = new Vector3(phase1Start_L.transform.position.x,phase1Start_L.transform.position.y,Random.Range(Vector3.Lerp (phase1Start_L.transform.position, phase1End_L.transform.position, 0.5f).z,phase1End_L.transform.position.z));

		phase1SpeedChangeZones_R [0].transform.position = new Vector3(phase1Start_R.transform.position.x,phase1Start_R.transform.position.y,Random.Range(phase1Start_R.transform.position.z,Vector3.Lerp (phase1Start_R.transform.position, phase1End_R.transform.position, 0.5f).z));
		phase1SpeedChangeZones_R [1].transform.position = new Vector3(phase1Start_R.transform.position.x,phase1Start_R.transform.position.y,Random.Range(Vector3.Lerp (phase1Start_R.transform.position, phase1End_R.transform.position, 0.5f).z,phase1End_R.transform.position.z));

		phase2SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x,phase2Start_L.transform.position.y,Random.Range(envManager.phase2Start_L.transform.position.z,Vector3.Lerp (envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z));
		phase2SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x,phase2Start_L.transform.position.y,Random.Range(Vector3.Lerp (envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z,envManager.phase2End_L.transform.position.z));

		phase2SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x,phase2Start_R.transform.position.y,Random.Range(envManager.phase2Start_R.transform.position.z,Vector3.Lerp (envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z));
		phase2SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x,phase2Start_R.transform.position.y,Random.Range(Vector3.Lerp (envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z,envManager.phase2End_R.transform.position.z));

		phase3SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase3Start_L.transform.position.x,phase3Start_L.transform.position.y,Random.Range(envManager.phase3Start_L.transform.position.z,Vector3.Lerp (envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f).z));
		phase3SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase3Start_L.transform.position.x,phase3Start_L.transform.position.y,Random.Range(Vector3.Lerp (envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f).z,envManager.phase3End_L.transform.position.z));

		phase3SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase3Start_R.transform.position.x,phase3Start_R.transform.position.y,Random.Range(envManager.phase3Start_R.transform.position.z,Vector3.Lerp (envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f).z));
		phase3SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase3Start_R.transform.position.x,phase3Start_R.transform.position.y,Random.Range(Vector3.Lerp (envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f).z,envManager.phase3End_R.transform.position.z));

	}

	void RandomizeCameraZones()
	{
		Debug.Log ("randomized cam zones");

		phase1CamZone_L.transform.position = Vector3.Lerp (envManager.phase1Start_L.transform.position, envManager.phase1End_L.transform.position, 0.5f);
		phase1CamZone_R.transform.position = Vector3.Lerp (envManager.phase1Start_R.transform.position, envManager.phase1End_R.transform.position, 0.5f);


		phase2CamZone_L.transform.position = Vector3.Lerp (envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f);
		phase2CamZone_R.transform.position = Vector3.Lerp (envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f);

		phase3CamZone_L.transform.position = Vector3.Lerp (envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f);
		phase3CamZone_R.transform.position = Vector3.Lerp (envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f);
	}

	public void ChangeCamZoneFocus(int camIndex)
	{
		if (activeCamZone != null) {
			activeCamZone.GetComponent<CameraZone> ().isFocus = false;
			activeCamZone.SetActive (false);
		}
		switch (camIndex) {
		case 0:
			phase1CamZone_L.SetActive (true);
			activeCamZone = phase1CamZone_L;
			break;
		case 1:
			phase2CamZone_L.SetActive (true);
			activeCamZone = phase2CamZone_L;
			break;
		case 2:
			phase3CamZone_L.SetActive (true);
			activeCamZone = phase3CamZone_L;
			break;
		case 3:
			phase1CamZone_R.SetActive (true);
			activeCamZone = phase1CamZone_R;
			break;
		case 4:
			phase2CamZone_R.SetActive (true);
			activeCamZone = phase2CamZone_R;
			break;
		case 5:
			phase3CamZone_R.SetActive (true);
			activeCamZone = phase3CamZone_R;
			break;
		default:
			phase1CamZone_L.SetActive (true);
			activeCamZone = phase1CamZone_L;
			break;
		}
//		Debug.Log ("cam index is: " + camIndex.ToString ());
//		if (camIndex <= 1) {
//			phase1CamZones [camIndex].GetComponent<CameraZone> ().isFocus = true;
//			Debug.Log (phase1CamZone.gameObject.name + " is the new focus");
//		} else {
//			if (camIndex == 4) {
//				phase1CamZone.GetComponent<CameraZone>().isFocus = true;
//
//				Debug.Log (phase1CamZone.gameObject.name + " is the new focus");
//			}
//			else
//				phase2CamZones [camIndex - 2].GetComponent<CameraZone> ().isFocus = true;
//
//			Debug.Log (phase2CamZones [camIndex - 2].gameObject.name + " is the new focus");
//		}
	}

    //for initial random assignment
    void AssignRooms()
    {

		leftRegisterObj = envManager.leftRegisterObj;
		rightRegisterObj = envManager.rightRegisterObj;
        if (Random.value < 0.5f)
        {
            leftRoom = roomOne;
			leftAudio = roomOneAudio;
			leftRoomColor = roomOneColor;
			rightRoom = roomTwo;
			rightAudio = roomTwoAudio;
			rightRoomColor = roomTwoColor;

			Experiment.Instance.shopLiftLog.LogRooms ("TOY","HARDWARE");
        }
        else
        {
            leftRoom = roomTwo;
			leftAudio = roomTwoAudio;
			leftRoomColor = roomTwoColor;

            rightRoom = roomOne;
			rightAudio = roomOneAudio;
			rightRoomColor = roomOneColor;


			Experiment.Instance.shopLiftLog.LogRooms ("HARDWARE","TOY");
        }

		leftRoom.transform.localPosition = envManager.leftRoomTransform.localPosition;
		rightRoom.transform.localPosition = envManager.rightRoomTransform.localPosition;
		Debug.Log ("set " + leftRoom.gameObject.name + " as left and " + rightRoom.gameObject.name + " as right");
    }

	void ResetCamZone()
	{

		phase1CamZone_L.GetComponent<CameraZone> ().Reset ();
		phase2CamZone_L.GetComponent<CameraZone> ().Reset ();
		phase3CamZone_L.GetComponent<CameraZone> ().Reset ();
		phase1CamZone_R.GetComponent<CameraZone> ().Reset ();
		phase2CamZone_R.GetComponent<CameraZone> ().Reset ();
		phase3CamZone_R.GetComponent<CameraZone> ().Reset ();
	}

	void ChangeCameraZoneVisibility(bool isVisible)
	{
		phase1CamZone_L.GetComponent<Renderer>().enabled = isVisible;
		phase1CamZone_R.GetComponent<Renderer>().enabled = isVisible;
		phase2CamZone_L.GetComponent<Renderer> ().enabled = isVisible;
		phase2CamZone_R.GetComponent<Renderer> ().enabled = isVisible;
		phase3CamZone_L.GetComponent<Renderer> ().enabled = isVisible;
		phase3CamZone_R.GetComponent<Renderer> ().enabled = isVisible;
	}

	void ChangeColors(Color newColor)
	{
//		for (int i = 0; i < phase2Walls.Length; i++) {
//			Debug.Log ("new color is:  " + newColor.ToString ());
//			phase2Walls [i].GetComponent<Renderer> ().material.color = newColor;
//		}
	}

    //for remapping
    void ReassignRooms()
    {
		Experiment.Instance.shopLiftLog.LogReassignEvent ();
		if(leftRoom==roomTwo)
        {
			leftRoom = roomOne;
			leftAudio = roomOneAudio;
			leftRoomColor = roomOneColor;


			rightRoom = roomTwo;
			rightAudio = roomTwoAudio;
			rightRoomColor = roomTwoColor;

			Experiment.Instance.shopLiftLog.LogRooms ("HARDWARE","TOY");

        }
        else
        {
			leftRoom = roomTwo;
			leftAudio = roomTwoAudio;
			leftRoomColor = roomTwoColor;

			rightRoom = roomOne;
			rightAudio = roomOneAudio;
			rightRoomColor = roomOneColor;

			Experiment.Instance.shopLiftLog.LogRooms ("TOY","HARDWARE");
        }
    }

	void ShuffleRegisterRewards()
	{
		List<int> registerValList = new List<int> ();
		for(int i=0;i<registerVals.Length;i++)
		{
			registerValList.Add (registerVals [i]);
		}

		for (int i = 0; i < registerVals.Length; i++) {
			int randomIndex = Random.Range (0, registerValList.Count);
			registerVals [i] = registerValList [randomIndex];
			registerValList.RemoveAt (randomIndex);
		}
	}

	IEnumerator RunCamTrainingPhase()
	{
		Debug.Log ("starting cam trainign phase");
		CameraZone.isTraining = true;

		trainingInstructionsGroup.alpha = 1f;
		yield return new WaitForSeconds (3f);
		trainingInstructionsGroup.alpha = 0f;
		trainingPeriodGroup.alpha = 1f;
		bool isLeft = false;
		int numTraining = 0;
		while (numTraining < 4) {
			Debug.Log ("about to run phase 1");
			isLeft = !isLeft;
			yield return StartCoroutine (RunPhaseOne ((isLeft) ? 0:1,false,-1,false));

			Debug.Log ("about to run phase 2");
			yield return StartCoroutine (RunPhaseTwo ((isLeft) ? 0:1,false,false,false,-1,false));
//			TurnOffRooms ();
			if (numTrials < maxTrials - 1)
				yield return StartCoroutine (ShowEndTrialScreen ());
			numTraining++;
			yield return 0;
		}
		ResetCamZone ();
		CameraZone.isTraining = false;

		trainingPeriodGroup.alpha = 0f;
		yield return null;
	}


    IEnumerator PickFourRegisterValues()
    {
		registerLeft = new List<int> ();
        for(int i=0;i<4;i++)
        {
			registerLeft.Add(i);
            registerVals[i] = Random.Range(5, 95);
        }
        yield return null;
    }

	IEnumerator RunPhaseOne(int pathIndex, bool isGuided, int guidedChoice, bool terminateWithChoice)
	{

		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		clearCameraZoneFlags = false;
		Debug.Log ("running phase one");
		baseAudio.Play ();
		if (numTrials >= 1)
			ChangeCameraZoneVisibility (false);

		ChangeCamZoneFocus(0);
		ToggleMouseLook(false);
		Debug.Log ("path index is: " + pathIndex.ToString ());
		Vector3 startPos = (pathIndex == 0) ? phase1Start_L.transform.position : phase1Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase1End_L.transform.position : phase1End_R.transform.position;
		Debug.Log ("player pos: " + camVehicle.transform.position.ToString());
		Debug.Log ("start pos is: " + startPos.ToString());
		camVehicle.SetActive (true);
		camVehicle.transform.position = startPos;
		Debug.Log ("player pos: " + camVehicle.transform.position.ToString());
		Experiment.Instance.shopLiftLog.LogMoveEvent (1,true);
		Debug.Log ("about to velo move player");
		yield return StartCoroutine(VelocityPlayerTo (startPos,endPos, phase1Factor));

		Debug.Log ("finished velo move player");
		Experiment.Instance.shopLiftLog.LogMoveEvent (1,false);

		if (!terminateWithChoice) {
			Doors.canOpen = true;
		}
//		ToggleMouseLook(false);


		if (pathIndex == 0) {

				ChangeCamZoneFocus (0);
				choiceAudio = rightAudio;
				ChangeColors (rightRoomColor);

				phase2Start_L = envManager.phase2Start_L;
				phase2End_L = envManager.phase2End_L;

				Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
			if (!terminateWithChoice) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase1Door_L.transform.position, 2f));
				baseAudio.Stop ();
				choiceAudio.Play ();
				yield return StartCoroutine (MovePlayerTo (phase1Door_L.transform.position, phase2Start_L.transform.position, 2f));
			}

		} else if (pathIndex == 1) {

				ChangeCamZoneFocus (3);
				choiceAudio = leftAudio;
				ChangeColors (leftRoomColor);

				phase2Start_R = envManager.phase2Start_R;
				phase2End_R = envManager.phase2End_R;
				Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
			if (!terminateWithChoice) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase1Door_R.transform.position, 2f));
				baseAudio.Stop ();
				choiceAudio.Play ();
				yield return StartCoroutine (MovePlayerTo (phase1Door_R.transform.position, phase2Start_R.transform.position, 2f));
			}
		}
		Doors.canOpen = false;
		clearCameraZoneFlags = true;
		yield return null;
	}

	IEnumerator RunPhaseTwo(int pathIndex,bool isDirect, bool hasRewards, bool isGuided, int guidedChoice, bool terminateWithChoice)
	{
		Vector3 startPos = (pathIndex == 0) ? phase2Start_L.transform.position : phase2Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase2End_L.transform.position : phase2End_R.transform.position;
		clearCameraZoneFlags = false;
		Debug.Log ("running phase two");
		if (isDirect) {
			camVehicle.transform.position = startPos;
			Debug.Log("about to move player in phase 2");
			yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position,endPos,10f));

			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);
		} else {
			camVehicle.transform.position = startPos;
			Debug.Log("velo player in phase 2");
			yield return StartCoroutine(VelocityPlayerTo(startPos,endPos,10f));
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);
		}
		if (hasRewards) {
			if (pathIndex == 0) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase2Door_L.transform.position, 2f));
				yield return StartCoroutine (MovePlayerTo (phase2Door_L.transform.position, phase3Start_L.transform.position, 2f));
		
			} else if (pathIndex == 1) {
				choiceAudio = rightAudio;
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase2Door_R.transform.position, 2f));
				yield return StartCoroutine (MovePlayerTo (phase2Door_R.transform.position, phase3Start_R.transform.position, 2f));
			}
		}

		clearCameraZoneFlags = true;
		yield return null;
	}

	IEnumerator RunPhaseThree(int pathIndex,bool isDirect, bool hasRewards, bool terminateWithChoice)
	{
		Vector3 startPos = (pathIndex == 0) ? phase3Start_L.transform.position : phase3Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase3End_L.transform.position : phase3End_R.transform.position;
		clearCameraZoneFlags = false;
		Debug.Log ("running phase three");
		if (isDirect) {
			camVehicle.transform.position = startPos;
			Debug.Log("about to move player in phase 3");
			Experiment.Instance.shopLiftLog.LogMoveEvent (3, true);
			yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position,endPos,10f));

			Experiment.Instance.shopLiftLog.LogMoveEvent (3, false);
		} else {
			camVehicle.transform.position = startPos;
			Debug.Log("velo player in phase 2");
			yield return StartCoroutine(VelocityPlayerTo(startPos,endPos,10f));
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);
		}
		if (hasRewards) {
			if (pathIndex == 0) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase3Door_L.transform.position, 2f));
				yield return StartCoroutine (MovePlayerTo (phase3Door_L.transform.position, phase3Start_L.transform.position, 2f));

			} else if (pathIndex == 1) {
				choiceAudio = rightAudio;
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase3Door_R.transform.position, 2f));
				yield return StartCoroutine (MovePlayerTo (phase2Door_R.transform.position, phase3Start_R.transform.position, 2f));
			}
		}

		clearCameraZoneFlags = true;
		yield return null;
	}


	IEnumerator RunLearningPhase()
	{
		Debug.Log("running task");
		instructionGroup.alpha = 1f;
		while(!Input.GetButtonDown("Action Button"))
		{
			yield return 0;
		}
		instructionGroup.alpha = 0f;

		ChangeCameraZoneVisibility (true);
		yield return StartCoroutine(PickFourRegisterValues());

		bool isLeft = (Random.value < 0.5f) ? true: false;
		//stage 1
		Experiment.Instance.shopLiftLog.LogStageEvent(1,true);


		//		while(numTrials < 1)
		while(registerLeft.Count > 0 || numTrials < maxTrials)
		{ 
			Debug.Log ("about to run phase 1");
			isLeft = !isLeft;
			yield return StartCoroutine (RunPhaseOne ((isLeft) ? 0 : 1, false, -1,false));

			Debug.Log ("about to run phase 2");

				yield return StartCoroutine (RunPhaseTwo((isLeft) ? 0 : 1,false,true,true,-1,false));

//			TurnOffRooms ();
			if (numTrials < maxTrials - 1 || registerLeft.Count > 0)
				yield return StartCoroutine (ShowEndTrialScreen ());
			else
				yield return StartCoroutine (ShowNextStageScreen ());
			numTrials++;
			yield return 0;
		}

		camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;
		Experiment.Instance.shopLiftLog.LogStageEvent (1, false);
		yield return null;
	}

	IEnumerator RunReevaluationPhase()
	{
//		int numTrials_Reeval = 0;
//		int numBlocks_Reeval = 0;
//		Debug.Log("about to start Re-Evaluation Phase");
//		stageIndex = 2;
//		bool leftChoice = false;
//		Experiment.Instance.shopLiftLog.LogStageEvent(2,true);
//		while (numBlocks_Reeval < maxBlocks_Reeval) {
//			while (numTrials_Reeval < maxTrials_Reeval) {
//				leftChoice = !leftChoice; //flip it
//				if (leftChoice) {
//					leftRoom.SetActive (true);
//					leftAudio.Play ();
//					ChangeColors (leftRoomColor);
//					phase1Choice = 0;
//
//					phase2CamZone_L.SetActive (true);
//					phase2CamZone_R.SetActive (false);
//
//					phase2Start = envManager.phase2Start_L;
//					phase2End = envManager.phase2End_L;
//					phase2LeftRegister = envManager.phase2LeftRegister_L;
//					phase2RightRegister = envManager.phase2RightRegister_L;
//
//				} else {
//					rightRoom.SetActive (true);
//					rightAudio.Play ();
//					ChangeColors (rightRoomColor);
//					phase1Choice = 1;
//
//					phase2CamZone_R.SetActive (true);
//					phase2CamZone_L.SetActive (false);
//
//					phase2Start = envManager.phase2Start_R;
//					phase2End = envManager.phase2End_R;
//					phase2LeftRegister = envManager.phase2LeftRegister_R;
//					phase2RightRegister = envManager.phase2RightRegister_R;
//				}
//				yield return StartCoroutine (RunPhaseTwo (true, true, false, -1,false));
//				TurnOffRooms ();
//				if(numTrials_Reeval < maxTrials_Reeval-1)
//					yield return StartCoroutine (ShowEndTrialScreen ());
//				numTrials_Reeval++;
//				yield return 0;
//			}
//
//			yield return StartCoroutine (RunRestPeriod());
//			numTrials_Reeval = 0;
//			numBlocks_Reeval++;
//			yield return 0;
//		}
//
//		Experiment.Instance.shopLiftLog.LogStageEvent(2,false);
		yield return null;
	}
	IEnumerator RunRestPeriod()
	{
		restGroup.alpha = 1f;
		yield return new WaitForSeconds (30f);
		restGroup.alpha = 0f;
		yield return null;
	}


	IEnumerator RunTestingPhase()
	{
//		for (int i = 0; i < 4; i++) {
//			yield return StartCoroutine (RunPhaseOne (false,-1,true));
//		}
//		bool leftChoice = false;
//		TurnOffRooms ();
//		for (int j = 0; j < 16; j++) {
//			leftChoice = !leftChoice; //flip it
//			if (leftChoice) {
//				leftRoom.SetActive (true);
//				leftAudio.Play ();
//				ChangeColors (leftRoomColor);
//				phase1Choice = 0;
//
//				phase2CamZone_L.SetActive (true);
//				phase2CamZone_R.SetActive (false);
//
//				phase2Start = envManager.phase2Start_L;
//				phase2End = envManager.phase2End_L;
//				phase2LeftRegister = envManager.phase2LeftRegister_L;
//				phase2RightRegister = envManager.phase2RightRegister_L;
//
//			} else {
//				rightRoom.SetActive (true);
//				rightAudio.Play ();
//				ChangeColors (rightRoomColor);
//				phase1Choice = 1;
//
//				phase2CamZone_R.SetActive (true);
//				phase2CamZone_L.SetActive (false);
//
//				phase2Start = envManager.phase2Start_R;
//				phase2End = envManager.phase2End_R;
//				phase2LeftRegister = envManager.phase2LeftRegister_R;
//				phase2RightRegister = envManager.phase2RightRegister_R;
//
//			}
//			yield return StartCoroutine (RunPhaseTwo (true, true, false, -1,true));
//			TurnOffRooms ();
//			if(j<15)
//				yield return StartCoroutine (ShowEndTrialScreen ());
//		}
		yield return null;
	}

	void UpdateFirstEnvironments ()
	{
		if (ExperimentSettings.env == ExperimentSettings.Environment.Cybercity) {
			environments [0] = cybercityEnv;
			environments [1] = spaceStationEnv;
		} else {
			environments [0] = spaceStationEnv;
			environments [1] = cybercityEnv;
		}
	}

	IEnumerator PickEnvironment(int envIndex)
	{
		//first turn off all environments
		for (int i = 0; i<environments.Count; i++) {
			environments [i].SetActive (false);
		}
		Debug.Log ("picking environment");
		switch (envIndex) {
		case 0:
			environments [0].SetActive (true);
			envManager = environments [0].GetComponent<EnvironmentManager> ();
			activeEnvLabel = environments [0].name;
			break;
		case 1:
			environments [1].SetActive (true);
			envManager = environments [1].GetComponent<EnvironmentManager> ();
			activeEnvLabel = environments [1].name;
			break;
		}
		phase1Start_L =envManager.phase1Start_L;
		phase1Start_R =envManager.phase1Start_R;
		phase1End_L =envManager.phase1End_L;
		phase1End_R =envManager.phase1End_R;

		phase1Door_L = envManager.phase1Door_L;
		phase1Door_R = envManager.phase1Door_R;
		phase2Door_L = envManager.phase2Door_L;
		phase2Door_R = envManager.phase2Door_R;
		phase3Door_L = envManager.phase3Door_L;
		phase3Door_R = envManager.phase3Door_R;

		phase2Start_L =envManager.phase2Start_L;
		phase2Start_R =envManager.phase2Start_R;
		phase2End_L =envManager.phase2End_L;
		phase2End_R =envManager.phase2End_R;

		phase3Start_L =envManager.phase3Start_L;
		phase3Start_R =envManager.phase3Start_R;
		phase3End_L =envManager.phase3End_L;
		phase3End_R =envManager.phase3End_R;

		roomOne = envManager.roomOne;
		roomOneAudio = envManager.roomOneAudio;
		roomTwo = envManager.roomTwo;
		roomTwoAudio = envManager.roomTwoAudio;
		baseAudio = envManager.baseAudio;

		phase1CamZone_L = envManager.phase1CamZone_L;
		phase1CamZone_R = envManager.phase1CamZone_R;

		phase2CamZone_L = envManager.phase2CamZone_L;
		phase2CamZone_R = envManager.phase2CamZone_R;

		phase3CamZone_L = envManager.phase3CamZone_L;
		phase3CamZone_R = envManager.phase3CamZone_R;

		//after env has been selected and all necessary object references set, assign rooms and randomize cam zones
		AssignRooms ();
		RandomizeSpeedChangeZones ();
		RandomizeCameraZones ();
		yield return null;
	}

    IEnumerator RunTask()
    {
		stageIndex = 1;
		for (int i = 0; i < environments.Count; i++) {
			yield return StartCoroutine (PickEnvironment (i));

			if (ExperimentSettings.isTraining)
				yield return StartCoroutine (RunCamTrainingPhase ());

			//randomize rooms and cam zones again
			AssignRooms ();
			RandomizeCameraZones ();

			//learning phase
			if (ExperimentSettings.isLearning)
				yield return StartCoroutine (RunLearningPhase ());

			//shuffle rewards
//		ReassignRooms ();
			ShuffleRegisterRewards ();

			//re-evaluation phase
			if (ExperimentSettings.isReeval)
				yield return StartCoroutine (RunReevaluationPhase ());

			//testing phase
			if (ExperimentSettings.isTesting)
				yield return StartCoroutine (RunTestingPhase ());

			//show end session screen
			yield return StartCoroutine (ShowEndEnvironmentStageScreen ());
//			SceneManager.LoadScene (0); //load main menu
//			SceneManager.UnloadSceneAsync (1); //then destroy all objects of the current scene
			yield return null;
		}
	}

	void TurnOffRooms()
	{
		roomTwo.SetActive (false);
		roomOne.SetActive (false);
		roomOneAudio.Stop ();
		roomTwoAudio.Stop ();

	}

	void ShowRegisterText()
	{
		
	}

	public IEnumerator ShowPositiveFeedback()
	{
		positiveFeedbackGroup.alpha = 1f;
//		Debug.Log ("about to wait for 1 second");
		yield return new WaitForSeconds (1f);
//		Debug.Log ("turning it off");
		positiveFeedbackGroup.alpha = 0f;
		yield return null;
	}
	public IEnumerator ShowNegativeFeedback()
	{
		negativeFeedbackGroup.alpha = 1f;
//		Debug.Log ("about to wait for 1 second");
		yield return new WaitForSeconds (1f);
//		Debug.Log ("turning it off");
		negativeFeedbackGroup.alpha = 0f;
		yield return null;
	}

	public IEnumerator ShowWarning()
	{
		warningFeedbackGroup.alpha = 1f;
		while (!clearCameraZoneFlags) {
			yield return 0;
		}
		warningFeedbackGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator ShowRegisterReward()
    {
		GameObject chosenRegister = null;
        choiceOutput = (phase1Choice * 2) + phase2Choice;
		Debug.Log ("phase1 choice: " + phase1Choice.ToString () + " phase 2 choice " + phase2Choice.ToString ());
		Debug.Log ("choice output is:" + choiceOutput.ToString ());
		switch (choiceOutput) {
		case 0:
			chosenRegister = leftRegisterObj;
			break;
		case 1:
			chosenRegister = rightRegisterObj;
			break;
		
		}
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		if (stageIndex == 1) {
			for (int i = 0; i < registerLeft.Count; i++) {
				if (choiceOutput == registerLeft [i]) {
					//remove the index and go out of the for loop
					Debug.Log("REMOVING: "  + registerLeft[i].ToString());
					registerLeft.RemoveAt (i);
					i = registerLeft.Count; 
				}
			}
		}
//		infoGroup.alpha = 1f;
		Debug.Log("chosen register is: " + chosenRegister.name);
		GameObject coinShowerObj = Instantiate(coinShower,chosenRegister.transform.position,Quaternion.identity) as GameObject;
		if (activeEnvLabel == "Cybercity") {
			coinShowerObj.transform.GetChild (0).transform.localPosition = Vector3.zero;
		}
		rewardScore.enabled = true;
		rewardScore.text = "$" + registerVals [choiceOutput].ToString ();

		chosenRegister.GetComponent<AudioSource> ().Play (); //play the cash register audio

		Experiment.Instance.shopLiftLog.LogRegisterReward(phase1Choice,phase2Choice,registerVals[choiceOutput]);
//        infoText.text = "You got $" + registerVals[choiceOutput].ToString() + " from the register";
		Debug.Log("waiting for 2 seconds");
		yield return new WaitForSeconds(2f);

		rewardScore.enabled = false;
//        infoGroup.alpha = 0f;
		Destroy(coinShowerObj);
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
        yield return null;
    }

	IEnumerator ShowNextStageScreen()
	{
		intertrialGroup.alpha = 1f;
		intertrialText.text = "On the next day...";
		Experiment.Instance.shopLiftLog.LogEndTrial ();
		yield return new WaitForSeconds(2f);
		intertrialGroup.alpha = 0f;
		yield return null;
	}

    IEnumerator ShowEndTrialScreen()
    {
        intertrialGroup.alpha = 1f;
		intertrialText.text = "Starting next trial...";
		Experiment.Instance.shopLiftLog.LogEndTrial ();
        yield return new WaitForSeconds(2f);
        intertrialGroup.alpha = 0f;
        yield return null;
    }
	IEnumerator ShowEndEnvironmentStageScreen()
	{ 	intertrialGroup.alpha = 1f;
		intertrialText.text = "Congratulations! You finished one environment \n Now, take a short rest before teleporting to the next dimension!";
		Experiment.Instance.shopLiftLog.LogEndEnvironmentStage();
		yield return new WaitForSeconds(60f);
		intertrialGroup.alpha = 0f;
		yield return null;
	}
	IEnumerator ShowEndSessionScreen()
	{ 
		intertrialGroup.alpha = 1f;
		intertrialText.text = "Congratulations! You have completed your session!";
		Experiment.Instance.shopLiftLog.LogEndSession();
		yield return new WaitForSeconds(2f);
		intertrialGroup.alpha = 0f;
		yield return null;
		
	}

	public void RandomizeSpeed()
	{
		currentSpeed = Random.Range (minSpeed, maxSpeed);
//		Debug.Log ("randomized speed to: " + currentSpeed.ToString ());
	}

    void ToggleMouseLook(bool shouldActivate)
    {
        
        if(shouldActivate)
        {

            camVehicle.SetActive(true);
//            animBody.SetActive(false);
//            mouseLook.XSensitivity = 2f;
        }
        else
        {
            camVehicle.SetActive(false);
//            animBody.SetActive(true);
//            mouseLook.XSensitivity = 0f;
        }
//        cartAnim.enabled = !shouldActivate;
    }

	IEnumerator ForcePlayerDecision(Vector3 chosenPoint, Vector3 otherPoint)
	{
		while (!Input.GetButtonDown("Action Button") || (Vector3.Distance(camVehicle.transform.position,chosenPoint)  >= Vector3.Distance(camVehicle.transform.position,otherPoint))) {
			yield return 0;
		}
		yield return null;
	}

	IEnumerator WaitForPlayerDecision(Vector3 leftPoint, Vector3 rightPoint)
	{
		while (!Input.GetButtonDown("Action Button") || (Vector3.Distance(camVehicle.transform.position,rightPoint)  == Vector3.Distance(camVehicle.transform.position,leftPoint))) {
			yield return 0;
		}
		//right
		if (Vector3.Distance(camVehicle.transform.position,rightPoint) < Vector3.Distance(camVehicle.transform.position,leftPoint)) {
			playerChoice = 1;
			
		} 
		//left
		else {
			playerChoice = 0;
			
		}

		Debug.Log ("the player choice is: " + playerChoice.ToString ());

        yield return null;

	}

	//uses timer based lerping to move player
	IEnumerator MovePlayerTo(Vector3 startPos, Vector3 endPos, float factor)
	{
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		float timer = 0f;
//		Debug.Log ("about to move player normally");
		while (timer/factor < 1f) {
			timer += Time.deltaTime;
			camVehicle.transform.position = Vector3.Lerp (startPos, endPos, timer / factor);
			yield return 0;
		}
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}


	//uses velocity and distance check to move player
	IEnumerator VelocityPlayerTo(Vector3 startPos, Vector3 endPos, float factor)
	{
		Vector3 moveDir = new Vector3 (0f, 0f, -1f);
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		float timer = 0f;
//		Debug.Log ("velocity player movement");
		while (Vector3.Distance(camVehicle.transform.position,endPos) > 1.5f) {
			timer += Time.deltaTime;
//			Debug.Log ("timer " + timer.ToString ());
			camVehicle.GetComponent<Rigidbody>().velocity = moveDir * currentSpeed;
//			Debug.Log ("distance to end: " + Vector3.Distance (camVehicle.transform.position, endPos).ToString ());
//			camVehicle.transform.position = Vector3.Lerp (startPos, endPos, timer / factor);
			yield return 0;
		}
//		Debug.Log ("stopping the player");
		camVehicle.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}

		
}
