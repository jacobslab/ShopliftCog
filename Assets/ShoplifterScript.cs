using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
public class ShoplifterScript : MonoBehaviour {

    public GameObject camVehicle;
    public GameObject animBody;
    private MouseLook mouseLook;

	private float currentSpeed;

	public float minSpeed = 10f;
	public float maxSpeed  = 30f;

	//PHASE 1
	private GameObject phase1Start;
	private GameObject phase1End;
	private GameObject phase1LeftDoor;
	private GameObject phase1RightDoor;


	//PHASE 2 LEFT
	private GameObject phase2Start_L;
	private GameObject phase2End_L;
	private GameObject phase2RightRegister_L;
	private GameObject phase2LeftRegister_L;

	public GameObject[] phase1SpeedChangeZones;
	public GameObject[] phase2SpeedChangeZones_L;
	public GameObject[] phase2SpeedChangeZones_R;

	//PHASE 2 RIGHT
	private GameObject phase2Start_R;
	private GameObject phase2End_R;
	private GameObject phase2RightRegister_R;
	private GameObject phase2LeftRegister_R;

	//registerobj

	private GameObject leftRegisterObj_L;
	private GameObject rightRegisterObj_L;
	private GameObject leftRegisterObj_R;
	private GameObject rightRegisterObj_R;

	//stage 2 reevaulation variables
	public int maxTrials_Reeval = 8;
	public int maxBlocks_Reeval = 4;

	private GameObject phase2Start;
	private GameObject phase2End;
	private GameObject phase2RightRegister;
	private GameObject phase2LeftRegister;

    public GameObject leftDoorPos;
    public GameObject rightDoorPos;
    public float phase1Factor = 5f;
    public Animator cartAnim;
    private int playerChoice = -1; //0 for left and 1 for right
    public int[] registerVals; // 0-1 is L-R for toy , 2-3 is L-R for hardware


	public List<GameObject> environments;
	private EnvironmentManager envManager;

    private GameObject leftRoom;
    private GameObject rightRoom;

    private int phase1Choice = 0;
    private int phase2Choice = 0;
    private int choiceOutput = 0;

    public Transform phase1Target;
    public Transform phase2Target;


	public GameObject fakeRoadblockP1;
	public GameObject fakeRoadblockP2;

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


	private GameObject roomOne;
	private GameObject roomTwo;
	private GameObject leftDoorObj;
	private GameObject rightDoorObj;

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
	public GameObject rewardPopupText;

	private List<int> registerLeft;
	private int stageIndex  = 0;

	//camera zone
	private GameObject phase1CamZone;
	private GameObject phase2CamZone_L;
	private GameObject phase2CamZone_R;


    public GameObject dummyObj;

	public GameObject testFloor;

    // Use this for initialization
    void Start() {
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
		phase1SpeedChangeZones [0].transform.position = new Vector3(phase1Start.transform.position.x,phase1SpeedChangeZones[0].transform.position.y,Random.Range(phase1Start.transform.position.z,Vector3.Lerp (phase1Start.transform.position, phase1End.transform.position, 0.5f).z));
		phase1SpeedChangeZones [1].transform.position = new Vector3(phase1Start.transform.position.x,phase1SpeedChangeZones[1].transform.position.y,Random.Range(Vector3.Lerp (phase1Start.transform.position, phase1End.transform.position, 0.5f).z,phase1End.transform.position.z));

		phase2SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x,phase2SpeedChangeZones_L[0].transform.position.y,Random.Range(envManager.phase2Start_L.transform.position.z,Vector3.Lerp (envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z));
		phase2SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x,phase2SpeedChangeZones_L[0].transform.position.y,Random.Range(Vector3.Lerp (envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z,envManager.phase2End_L.transform.position.z));

		phase2SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x,phase2SpeedChangeZones_R[0].transform.position.y,Random.Range(envManager.phase2Start_R.transform.position.z,Vector3.Lerp (envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z));
		phase2SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x,phase2SpeedChangeZones_R[0].transform.position.y,Random.Range(Vector3.Lerp (envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z,envManager.phase2End_R.transform.position.z));

	}

	void RandomizeCameraZones()
	{
		Debug.Log ("randomized cam zones");
		phase1CamZone.transform.localPosition = new Vector3 (phase1Start.transform.localPosition.x, phase1CamZone.transform.localPosition.y, Random.Range(phase1Start.transform.localPosition.z,phase1End.transform.localPosition.z));

		phase2CamZone_L.transform.localPosition = new Vector3 (envManager.phase2Start_L.transform.localPosition.x, phase2CamZone_L.transform.localPosition.y, Random.Range (envManager.phase2Start_L.transform.localPosition.z, envManager.phase2End_L.transform.localPosition.z));
		phase2CamZone_R.transform.localPosition = new Vector3 (envManager.phase2Start_R.transform.localPosition.x, phase2CamZone_R.transform.localPosition.y, Random.Range (envManager.phase2Start_R.transform.localPosition.z, envManager.phase2End_R.transform.localPosition.z));

	}

	public void ChangeCamZoneFocus(int camIndex)
	{
		switch (camIndex) {
		case 0:
			phase1CamZone.SetActive (true);
			phase2CamZone_L.SetActive (false);
			phase2CamZone_R.SetActive (false);
			phase1CamZone.GetComponent<CameraZone> ().isFocus = true;
			phase2CamZone_L.GetComponent<CameraZone> ().isFocus = false;
			phase2CamZone_R.GetComponent<CameraZone> ().isFocus = false;
			break;
		case 1:

			phase2CamZone_L.SetActive (true);
			phase2CamZone_R.SetActive (false);
			phase1CamZone.SetActive (false);
			phase2CamZone_L.GetComponent<CameraZone> ().isFocus = true;
			phase2CamZone_R.GetComponent<CameraZone> ().isFocus = false;
			phase1CamZone.GetComponent<CameraZone> ().isFocus = false;
			break;
		case 2:
			phase2CamZone_R.SetActive (true);
			phase2CamZone_L.SetActive (false);
			phase1CamZone.SetActive (false);
			phase2CamZone_R.GetComponent<CameraZone> ().isFocus = true;
			phase2CamZone_L.GetComponent<CameraZone> ().isFocus = false;
			phase1CamZone.GetComponent<CameraZone> ().isFocus = false;
			break;
		default:
			phase1CamZone.GetComponent<CameraZone> ().isFocus = true;
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
        if (Random.value < 0.5f)
        {
            leftRoom = roomOne;
			leftAudio = roomOneAudio;
			leftRoomColor = roomOneColor;
			leftRegisterObj_L = envManager.oneLeftRegisterObj;
			rightRegisterObj_L = envManager.oneRightRegisterObj;

			rightRoom = roomTwo;
			rightAudio = roomTwoAudio;
			rightRoomColor = roomTwoColor;
			leftRegisterObj_R = envManager.twoLeftRegisterObj;
			rightRegisterObj_R = envManager.twoRightRegisterObj;

			Experiment.Instance.shopLiftLog.LogRooms ("TOY","HARDWARE");
        }
        else
        {
            leftRoom = roomTwo;
			leftAudio = roomTwoAudio;
			leftRoomColor = roomTwoColor;
			leftRegisterObj_L = envManager.twoLeftRegisterObj;
			rightRegisterObj_L = envManager.twoRightRegisterObj;

            rightRoom = roomOne;
			rightAudio = roomOneAudio;
			rightRoomColor = roomOneColor;
			leftRegisterObj_R = envManager.oneLeftRegisterObj;
			rightRegisterObj_R = envManager.oneRightRegisterObj;

			Experiment.Instance.shopLiftLog.LogRooms ("HARDWARE","TOY");
        }

		leftRoom.transform.localPosition = envManager.leftRoomTransform.localPosition;
		rightRoom.transform.localPosition = envManager.rightRoomTransform.localPosition;
		Debug.Log ("set " + leftRoom.gameObject.name + " as left and " + rightRoom.gameObject.name + " as right");
    }

	void ResetCamZone()
	{

		phase1CamZone.GetComponent<CameraZone> ().Reset ();
		phase2CamZone_L.GetComponent<CameraZone> ().Reset ();
		phase2CamZone_R.GetComponent<CameraZone> ().Reset ();
	}

	void ChangeCameraZoneVisibility(bool isVisible)
	{
			phase1CamZone.GetComponent<Renderer>().enabled = isVisible;
			phase2CamZone_L.GetComponent<Renderer> ().enabled = isVisible;
			phase2CamZone_R.GetComponent<Renderer> ().enabled = isVisible;
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
			leftRegisterObj_L = envManager.oneLeftRegisterObj;
			rightRegisterObj_L = envManager.oneRightRegisterObj;


			rightRoom = roomTwo;
			rightAudio = roomTwoAudio;
			rightRoomColor = roomTwoColor;
			leftRegisterObj_R = envManager.twoLeftRegisterObj;
			rightRegisterObj_R = envManager.twoRightRegisterObj;

			Experiment.Instance.shopLiftLog.LogRooms ("HARDWARE","TOY");

        }
        else
        {
			leftRoom = roomTwo;
			leftAudio = roomTwoAudio;
			leftRoomColor = roomTwoColor;
			leftRegisterObj_L = envManager.twoLeftRegisterObj;
			rightRegisterObj_L = envManager.twoRightRegisterObj;

			rightRoom = roomOne;
			rightAudio = roomOneAudio;
			rightRoomColor = roomOneColor;
			leftRegisterObj_R = envManager.oneLeftRegisterObj;
			rightRegisterObj_R = envManager.oneRightRegisterObj;

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
		int numTraining = 0;
		while (numTraining < 3) {
			Debug.Log ("about to run phase 1");
			yield return StartCoroutine (RunPhaseOne (false,-1,false));

			Debug.Log ("about to run phase 2");
			yield return StartCoroutine (RunPhaseTwo (false,false,false,-1,false));
			TurnOffRooms ();
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

	IEnumerator RunPhaseOne(bool isGuided, int guidedChoice, bool terminateWithChoice)
	{
		Debug.Log ("running phase one");
		baseAudio.Play ();
		if (numTrials >= 1)
			ChangeCameraZoneVisibility (false);

		ChangeCamZoneFocus(0);
		ToggleMouseLook(false);
		camVehicle.transform.position = phase1Start.transform.position;
		Experiment.Instance.shopLiftLog.LogMoveEvent (1,true);
		camVehicle.SetActive(true);
		yield return StartCoroutine(VelocityPlayerTo (phase1Start.transform.position, phase1End.transform.position, phase1Factor));

		Experiment.Instance.shopLiftLog.LogMoveEvent (1,false);
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		Experiment.Instance.shopLiftLog.LogDecisionEvent (true);
		fakeRoadblockP1.SetActive (true);
		if (isGuided) {
			infoGroup.alpha = 1f;
			if (guidedChoice == 0) { //left
				infoText.text = "Pick the Left Door";
				yield return StartCoroutine (ForcePlayerDecision (phase1LeftDoor.transform.position, phase1RightDoor.transform.position));
			} else //right
			{
				infoText.text = "Pick the Right Door";
				yield return StartCoroutine (ForcePlayerDecision (phase1RightDoor.transform.position, phase1LeftDoor.transform.position));
			}
				
			playerChoice = guidedChoice;
			infoGroup.alpha = 0f;
		} else {
			
			yield return StartCoroutine (WaitForPlayerDecision (phase1LeftDoor.transform.position, phase1RightDoor.transform.position));
		}
		if (!terminateWithChoice) {
			Doors.canOpen = true;
			fakeRoadblockP1.SetActive (false);
		}
//		ToggleMouseLook(false);


			Experiment.Instance.shopLiftLog.LogDecisionEvent (false);
			if (playerChoice == 1) {

				ChangeCamZoneFocus (2);
				Experiment.Instance.shopLiftLog.LogDecision (1, 1);
				rightRoom.SetActive (true);
				leftRoom.SetActive (false);
				choiceAudio = rightAudio;
				ChangeColors (rightRoomColor);

				phase2CamZone_R.SetActive (true);
				phase2CamZone_L.SetActive (false);

				phase2Start = envManager.phase2Start_R;
				phase2End = envManager.phase2End_R;
				phase2LeftRegister = envManager.phase2LeftRegister_R;
				phase2RightRegister = envManager.phase2RightRegister_R;



				camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;

				Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
			if (!terminateWithChoice) {
				yield return StartCoroutine (rightDoorObj.GetComponent<Doors> ().Open ());
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase1RightDoor.transform.position, 2f));
				baseAudio.Stop ();
				choiceAudio.Play ();
				yield return StartCoroutine (MovePlayerTo (phase1RightDoor.transform.position, phase2Start.transform.position, 2f));
			}

//			yield return StartCoroutine (MovePlayerTo (phase2RightDoorStart.transform.position, phase2Start.transform.position, 2f));
//			cartAnim.Play("RightDoorMove");


			} else if (playerChoice == 0) {

				ChangeCamZoneFocus (1);
				Experiment.Instance.shopLiftLog.LogDecision (0, 1);
				leftRoom.SetActive (true);
				rightRoom.SetActive (false);
				choiceAudio = leftAudio;
				ChangeColors (leftRoomColor);

				phase2CamZone_R.SetActive (false);
				phase2CamZone_L.SetActive (true);

				phase2Start = envManager.phase2Start_L;
				phase2End = envManager.phase2End_L;
				phase2LeftRegister = envManager.phase2LeftRegister_L;
				phase2RightRegister = envManager.phase2RightRegister_L;

				camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
				Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
			if (!terminateWithChoice) {
				yield return StartCoroutine (leftDoorObj.GetComponent<Doors> ().Open ());
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase1LeftDoor.transform.position, 2f));
				baseAudio.Stop ();
				choiceAudio.Play ();
				yield return StartCoroutine (MovePlayerTo (phase1LeftDoor.transform.position, phase2Start.transform.position, 2f));
			}

//			yield return StartCoroutine (MovePlayerTo (phase2LeftDoorStart.transform.position, phase2Start.transform.position, 2f));
//			cartAnim.Play("LeftDoorMove");
		}
		Doors.canOpen = false;
		yield return null;
	}

	IEnumerator RunPhaseTwo(bool isDirect, bool hasRewards, bool isGuided, int guidedChoice, bool terminateWithChoice)
	{

		Debug.Log ("running phase two");
		if (!isDirect) {
			camVehicle.transform.position = phase2Start.transform.position;
//			camVehicle.SetActive (true);
//			camVehicle.GetComponent<RigidbodyFirstPersonController> ().mouseLook.m_CharacterTargetRot = Quaternion.Euler (dummyObj.transform.eulerAngles);
			phase1Choice = playerChoice; //store player choice for this phase to calculate the register reward

			yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position,phase2End.transform.position,10f));


//			yield return new WaitForSeconds (2.5f);

//			yield return new WaitForSeconds (7.5f);

			//   camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook.m_CharacterTargetRot = Quaternion.Euler(dummyObj.transform.eulerAngles);

			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);
			Experiment.Instance.shopLiftLog.LogDecisionEvent (true);
		} else {
//			cartAnim.enabled = true;
//			animBody.GetComponent<Rigidbody> ().isKinematic = false;
//			camVehicle.transform.position = phase2End.transform.position;
			camVehicle.SetActive (true);
			camVehicle.transform.position = phase2Start.transform.position;
//			camVehicle.GetComponent<RigidbodyFirstPersonController> ().mouseLook.m_CharacterTargetRot = Quaternion.Euler (dummyObj.transform.eulerAngles);
//			Debug.Log ("moving cartanim");
			//			cartAnim.Play ("Phase2Move");

			yield return StartCoroutine(VelocityPlayerTo(phase2Start.transform.position,phase2End.transform.position,10f));
//			yield return new WaitForSeconds (5f);
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);
		}
//		ToggleMouseLook (true);


		if (hasRewards) {
			camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=true;
			fakeRoadblockP2.SetActive (true);
			if (isGuided) {
				Debug.Log ("forcing player choice");
				infoGroup.alpha = 1f;
				if (guidedChoice == 0) {
					infoText.text = "Pick the Left Register";
					yield return StartCoroutine (ForcePlayerDecision (phase2LeftRegister.transform.position, phase2RightRegister.transform.position));
				} else {
					infoText.text = "Pick the Right Register";
					yield return StartCoroutine (ForcePlayerDecision (phase2RightRegister.transform.position, phase2LeftRegister.transform.position));
				}
				infoGroup.alpha = 0f;
				playerChoice = guidedChoice;
			} else {
				Debug.Log ("waiting for player choice");
				yield return StartCoroutine (WaitForPlayerDecision (phase2LeftRegister.transform.position, phase2RightRegister.transform.position));
			}
			fakeRoadblockP2.SetActive (false);
			//		ToggleMouseLook(false);
			Debug.Log("PLAYER CHOICE: " + playerChoice.ToString());
			Experiment.Instance.shopLiftLog.LogDecisionEvent (false);

			camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;
			if (playerChoice == 1) {
				Experiment.Instance.shopLiftLog.LogDecision (1, 2);
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase2RightRegister.transform.position, 2f));
//			cartAnim.Play("RightRegisterMove");
			} else if (playerChoice == 0) {

				Experiment.Instance.shopLiftLog.LogDecision (0, 2);
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase2LeftRegister.transform.position, 2f));
//			cartAnim.Play("LeftRegisterMove");
			}

			phase2Choice = playerChoice;

			if(!terminateWithChoice)
				yield return StartCoroutine (ShowRegisterReward ());
		}

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

		//stage 1
		Experiment.Instance.shopLiftLog.LogStageEvent(1,true);


		//		while(numTrials < 1)
		while(registerLeft.Count > 0 || numTrials < maxTrials)
		{ 
			Debug.Log ("about to run phase 1");
			if (registerLeft.Count == 1)
				yield return StartCoroutine (RunPhaseOne (true,(registerLeft[0] < 2) ? 0 : 1,false)); //check if the leftover register val belongs to left or right choice for phase 1 
			else
				yield return StartCoroutine (RunPhaseOne (false, -1,false));

			Debug.Log ("about to run phase 2");

			if (registerLeft.Count == 1)
				yield return StartCoroutine (RunPhaseTwo(false,true,true,(registerLeft[0]%2==0)? 0 : 1,false)); // all left register indexes are even (0,2) and right registers are odd (1,3)
			else

				yield return StartCoroutine (RunPhaseTwo(false,true,false,-1,false));
			TurnOffRooms ();
			if (numTrials < maxTrials - 1 || registerLeft.Count > 0)
				yield return StartCoroutine (ShowEndTrialScreen ());
			else
				yield return StartCoroutine (ShowNextStageScreen ());
			numTrials++;
			yield return 0;
		}

		Experiment.Instance.shopLiftLog.LogStageEvent (1, false);
		yield return null;
	}

	IEnumerator RunReevaluationPhase()
	{
		int numTrials_Reeval = 0;
		int numBlocks_Reeval = 0;
		Debug.Log("about to start Re-Evaluation Phase");
		stageIndex = 2;
		bool leftChoice = false;
		Experiment.Instance.shopLiftLog.LogStageEvent(2,true);
		while (numBlocks_Reeval < maxBlocks_Reeval) {
			while (numTrials_Reeval < maxTrials_Reeval) {
				leftChoice = !leftChoice; //flip it
				if (leftChoice) {
					leftRoom.SetActive (true);
					leftAudio.Play ();
					ChangeColors (leftRoomColor);
					phase1Choice = 0;

					phase2CamZone_L.SetActive (true);
					phase2CamZone_R.SetActive (false);

					phase2Start = envManager.phase2Start_L;
					phase2End = envManager.phase2End_L;
					phase2LeftRegister = envManager.phase2LeftRegister_L;
					phase2RightRegister = envManager.phase2RightRegister_L;

				} else {
					rightRoom.SetActive (true);
					rightAudio.Play ();
					ChangeColors (rightRoomColor);
					phase1Choice = 1;

					phase2CamZone_R.SetActive (true);
					phase2CamZone_L.SetActive (false);

					phase2Start = envManager.phase2Start_R;
					phase2End = envManager.phase2End_R;
					phase2LeftRegister = envManager.phase2LeftRegister_R;
					phase2RightRegister = envManager.phase2RightRegister_R;

				}
				yield return StartCoroutine (RunPhaseTwo (true, true, false, -1,false));
				TurnOffRooms ();
				yield return StartCoroutine (ShowEndTrialScreen ());
				numTrials_Reeval++;
				yield return 0;
			}

			yield return StartCoroutine (RunRestPeriod());
			numBlocks_Reeval++;
			yield return 0;
		}

		Experiment.Instance.shopLiftLog.LogStageEvent(2,false);
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
		for (int i = 0; i < 4; i++) {
			yield return StartCoroutine (RunPhaseOne (false,-1,true));
		}
		bool leftChoice = false;
		TurnOffRooms ();
		for (int j = 0; j < 16; j++) {
			leftChoice = !leftChoice; //flip it
			if (leftChoice) {
				leftRoom.SetActive (true);
				leftAudio.Play ();
				ChangeColors (leftRoomColor);
				phase1Choice = 0;

				phase2CamZone_L.SetActive (true);
				phase2CamZone_R.SetActive (false);

				phase2Start = envManager.phase2Start_L;
				phase2End = envManager.phase2End_L;
				phase2LeftRegister = envManager.phase2LeftRegister_L;
				phase2RightRegister = envManager.phase2RightRegister_L;

			} else {
				rightRoom.SetActive (true);
				rightAudio.Play ();
				ChangeColors (rightRoomColor);
				phase1Choice = 1;

				phase2CamZone_R.SetActive (true);
				phase2CamZone_L.SetActive (false);

				phase2Start = envManager.phase2Start_R;
				phase2End = envManager.phase2End_R;
				phase2LeftRegister = envManager.phase2LeftRegister_R;
				phase2RightRegister = envManager.phase2RightRegister_R;

			}
			yield return StartCoroutine (RunPhaseTwo (true, true, false, -1,true));
			TurnOffRooms ();
			yield return StartCoroutine (ShowEndTrialScreen ());
		}
		yield return null;
	}

	IEnumerator PickEnvironment()
	{
		Debug.Log ("picking environment");
		environments [0].SetActive (true); //just turn space station on for now
		envManager =  environments[0].GetComponent<SpaceStationManager>();
		phase1Start =envManager.phase1Start;
		phase1End = envManager.phase1End;
		phase1LeftDoor = envManager.phase1LeftDoor;
		phase1RightDoor = envManager.phase1RightDoor;
		roomOne = envManager.roomOne;
		roomOneAudio = envManager.roomOneAudio;
		roomTwo = envManager.roomTwo;
		roomTwoAudio = envManager.roomTwoAudio;
		baseAudio = envManager.baseAudio;
		phase1CamZone = envManager.phase1CamZone;
		phase2CamZone_L = envManager.phase2CamZone_L;
		phase2CamZone_R = envManager.phase2CamZone_R;
		leftDoorObj = envManager.leftDoor;
		rightDoorObj = envManager.rightDoor;
	
		//after env has been selected and all necessary object references set, assign rooms and randomize cam zones
		AssignRooms ();
		RandomizeSpeedChangeZones ();
		RandomizeCameraZones ();
		yield return null;
	}

    IEnumerator RunTask()
    {
		stageIndex = 1;

		yield return StartCoroutine (PickEnvironment ());

		yield return StartCoroutine (RunCamTrainingPhase ());

		//learning phase
		yield return StartCoroutine(RunLearningPhase());

		//shuffle rewards
//		ReassignRooms ();
		ShuffleRegisterRewards ();

		//re-evaluation phase
		yield return StartCoroutine(RunReevaluationPhase());

		//testing phase
		yield return StartCoroutine(RunTestingPhase());
        yield return null;
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
		yield return new WaitForSeconds (1f);
		positiveFeedbackGroup.alpha = 0f;
		yield return null;
	}
	public IEnumerator ShowNegativeFeedback()
	{
		negativeFeedbackGroup.alpha = 1f;
		yield return new WaitForSeconds (1f);
		negativeFeedbackGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator ShowRegisterReward()
    {
		GameObject chosenRegister = null;
        choiceOutput = (phase1Choice * 2) + phase2Choice;
		switch (choiceOutput) {
		case 0:
			chosenRegister = leftRegisterObj_L;
			break;
		case 1:
			chosenRegister = rightRegisterObj_L;
			break;
		case 2:
			chosenRegister = leftRegisterObj_R;
			break;
		case 3:
			chosenRegister = rightRegisterObj_R;
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
		GameObject coinShowerObj = Instantiate(coinShower,chosenRegister.transform.position,Quaternion.identity) as GameObject;
		GameObject rewardPopup = Instantiate(rewardPopupText,chosenRegister.transform.position,Quaternion.identity) as GameObject;

		chosenRegister.GetComponent<AudioSource> ().Play (); //play the cash register audio

		rewardPopup.transform.GetChild(0).gameObject.GetComponent<TextMesh> ().text = "$" + registerVals [choiceOutput].ToString ();
		Experiment.Instance.shopLiftLog.LogRegisterReward(phase1Choice,phase2Choice,registerVals[choiceOutput]);
//        infoText.text = "You got $" + registerVals[choiceOutput].ToString() + " from the register";
		Debug.Log("waiting for 2 seconds");
		yield return new WaitForSeconds(2f);
//        infoGroup.alpha = 0f;
		Destroy(coinShowerObj);
		Destroy (rewardPopup);
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

	public void RandomizeSpeed()
	{
		currentSpeed = Random.Range (minSpeed, maxSpeed);
		Debug.Log ("randomized speed to: " + currentSpeed.ToString ());
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
		Debug.Log ("about to move player");
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
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		float timer = 0f;
		Debug.Log ("about to move player");
		while (Vector3.Distance(camVehicle.transform.position,endPos) > 1.5f) {
			timer += Time.deltaTime;
//			Debug.Log ("timer " + timer.ToString ());
			camVehicle.GetComponent<Rigidbody>().velocity = camVehicle.transform.forward * currentSpeed;
//			Debug.Log ("distance to end: " + Vector3.Distance (camVehicle.transform.position, endPos).ToString ());
//			camVehicle.transform.position = Vector3.Lerp (startPos, endPos, timer / factor);
			yield return 0;
		}
		Debug.Log ("stopping the player");
		camVehicle.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}

		
}
