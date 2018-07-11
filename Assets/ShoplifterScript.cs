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

	//PHASE 1
    public GameObject phase1Start;
    public GameObject phase1End;
	public GameObject phase1LeftDoor;
	public GameObject phase1RightDoor;


	//PHASE 2 LEFT
	public GameObject phase2Start_L;
	public GameObject phase2End_L;
	public GameObject phase2RightRegister_L;
	public GameObject phase2LeftRegister_L;


	//PHASE 2 RIGHT
	public GameObject phase2Start_R;
	public GameObject phase2End_R;
	public GameObject phase2RightRegister_R;
	public GameObject phase2LeftRegister_R;


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

	private List<int> registerLeft;
	private int stageIndex  = 0;

	//camera zone
	public GameObject[] phase1CamZones;
	public Transform[] phase1CamLimits;
	public GameObject[] phase2CamZones;
	public Transform[] phase2CamLimits;


    public GameObject dummyObj;

	public GameObject testFloor;

    // Use this for initialization
    void Start() {
        infoGroup.alpha = 0f;
		positiveFeedbackGroup.alpha = 0f;
		negativeFeedbackGroup.alpha = 0f;
        intertrialGroup.alpha = 0f;
        registerVals = new int[4];
//        cartAnim.enabled = true;
        camVehicle.SetActive(true);
//        animBody.SetActive(false);

        dummyObj.transform.eulerAngles = new Vector3(0f, 90f, 0f);
//        cartAnim.Play("Phase1Start");
        //camVehicle.transform.position = phase1Start.transform.position;
//        mouseLook = camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook;
        camTrans = camVehicle.GetComponent<RigidbodyFirstPersonController>().cam.transform;

		RandomizeCameraZones ();
        
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

	void RandomizeCameraZones()
	{
		phase1CamZones [0].transform.localPosition = new Vector3 (Random.Range(-40f,-28f), phase1CamZones [0].transform.localPosition.y, phase1CamZones [0].transform.localPosition.z);
		phase1CamZones [1].transform.localPosition = new Vector3 (Random.Range(-26f,-14f), phase1CamZones [1].transform.localPosition.y, phase1CamZones [1].transform.localPosition.z);


		phase2CamZones [0].transform.localPosition = new Vector3 (Random.Range(46f,56f), phase2CamZones [0].transform.localPosition.y, phase2CamZones [0].transform.localPosition.z);
		phase2CamZones [1].transform.localPosition = new Vector3 (Random.Range(58f,68f), phase2CamZones [1].transform.localPosition.y, phase2CamZones [1].transform.localPosition.z);

	}

	public void ChangeCamZoneFocus(int camIndex)
	{
		Debug.Log ("cam index is: " + camIndex.ToString ());
		if (camIndex <= 1) {
			phase1CamZones [camIndex].GetComponent<CameraZone> ().isFocus = true;
			Debug.Log (phase1CamZones [camIndex].gameObject.name + " is the new focus");
		} else {
			if (camIndex == 4) {
				phase1CamZones[0].GetComponent<CameraZone>().isFocus = true;

				Debug.Log (phase1CamZones [0].gameObject.name + " is the new focus");
			}
			else
				phase2CamZones [camIndex - 2].GetComponent<CameraZone> ().isFocus = true;

			Debug.Log (phase2CamZones [camIndex - 2].gameObject.name + " is the new focus");
		}
	}

    //for initial random assignment
    void AssignRooms()
    {
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

	void ChangeCameraZoneVisibility(bool isVisible)
	{
		for (int i = 0; i < 2; i++) {
			phase1CamZones [i].GetComponent<Renderer>().enabled = isVisible;
			phase2CamZones [i].GetComponent<Renderer> ().enabled = isVisible;
		}
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
		CameraZone.isTraining = true;
		int numTraining = 0;
		while (numTraining < 3) {
			Debug.Log ("about to run phase 1");
			yield return StartCoroutine (RunPhaseOne ());

			Debug.Log ("about to run phase 2");
			yield return StartCoroutine (RunPhaseTwo (false,false));
			TurnOffRooms ();
			if (numTrials < maxTrials - 1 || registerLeft.Count > 0)
				yield return StartCoroutine (ShowEndTrialScreen ());
			numTraining++;
			yield return 0;
		}
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

	IEnumerator RunPhaseOne()
	{
		Debug.Log ("running phase one");
		baseAudio.Play ();
//		animBody.GetComponent<Rigidbody> ().isKinematic = false;
		if (numTrials >= 1)
			ChangeCameraZoneVisibility (false);

//		cartAnim.enabled = true;
		ToggleMouseLook(false);

		Experiment.Instance.shopLiftLog.LogMoveEvent (1,true);
//		cartAnim.Play("Phase1Move");
//		camVehicle.transform.position = phase1End.transform.position;
		camVehicle.SetActive(true);
		yield return StartCoroutine(MovePlayerTo (phase1Start.transform.position, phase1End.transform.position, phase1Factor));
//		camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook.m_CharacterTargetRot = Quaternion.Euler(dummyObj.transform.eulerAngles);
//		yield return new WaitForSeconds(5f);

		Experiment.Instance.shopLiftLog.LogMoveEvent (1,false);

		// yield return StartCoroutine (MovePlayerTo (phase1Start.transform.position,phase1End.transform.position,phase1Factor));
//		ToggleMouseLook(true);
		camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=true;
		Experiment.Instance.shopLiftLog.LogDecisionEvent (true);
		fakeRoadblockP1.SetActive (true);
		yield return StartCoroutine(WaitForPlayerDecision(phase1LeftDoor.transform.position,phase1RightDoor.transform.position));
		fakeRoadblockP1.SetActive (false);
//		ToggleMouseLook(false);


		Experiment.Instance.shopLiftLog.LogDecisionEvent (false);
		if (playerChoice == 1)
		{

			Experiment.Instance.shopLiftLog.LogDecision (1,1);
			rightRoom.SetActive(true);
			leftRoom.SetActive(false);
			choiceAudio = rightAudio;
			ChangeColors (rightRoomColor);

			phase2Start = envManager.phase2Start_R;
			phase2End = envManager.phase2End_R;
			phase2LeftRegister = envManager.phase2LeftRegister_R;
			phase2RightRegister = envManager.phase2RightRegister_R;



			camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;

			Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
			yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position,phase1RightDoor.transform.position,2f));
			yield return StartCoroutine(MovePlayerTo(phase1RightDoor.transform.position,phase2Start.transform.position,2f));
			baseAudio.Stop ();
			choiceAudio.Play ();
//			yield return StartCoroutine (MovePlayerTo (phase2RightDoorStart.transform.position, phase2Start.transform.position, 2f));
//			cartAnim.Play("RightDoorMove");


		} else if (playerChoice == 0)
		{

			Experiment.Instance.shopLiftLog.LogDecision (0,1);
			leftRoom.SetActive(true);
			rightRoom.SetActive(false);
			choiceAudio = leftAudio;
			ChangeColors (leftRoomColor);

			phase2Start = envManager.phase2Start_L;
			phase2End = envManager.phase2End_L;
			phase2LeftRegister = envManager.phase2LeftRegister_L;
			phase2RightRegister = envManager.phase2RightRegister_L;

			camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
			yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position,phase1LeftDoor.transform.position,2f));
			yield return StartCoroutine(MovePlayerTo(phase1LeftDoor.transform.position,phase2Start.transform.position,2f));
			baseAudio.Stop ();
			choiceAudio.Play ();
//			yield return StartCoroutine (MovePlayerTo (phase2LeftDoorStart.transform.position, phase2Start.transform.position, 2f));
//			cartAnim.Play("LeftDoorMove");
		}
		yield return null;
	}

	IEnumerator RunPhaseTwo(bool isDirect, bool hasRewards)
	{

		Debug.Log ("running phase two");
		if (!isDirect) {
//			camVehicle.transform.position = phase2End.transform.position;
//			camVehicle.SetActive (true);
//			camVehicle.GetComponent<RigidbodyFirstPersonController> ().mouseLook.m_CharacterTargetRot = Quaternion.Euler (dummyObj.transform.eulerAngles);
			phase1Choice = playerChoice; //store player choice for this phase to calculate the register reward

			yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position,phase2End.transform.position,5f));


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
//			camVehicle.GetComponent<RigidbodyFirstPersonController> ().mouseLook.m_CharacterTargetRot = Quaternion.Euler (dummyObj.transform.eulerAngles);
//			Debug.Log ("moving cartanim");
			//			cartAnim.Play ("Phase2Move");

			yield return StartCoroutine(MovePlayerTo(phase2Start.transform.position,phase2End.transform.position,5f));
//			yield return new WaitForSeconds (5f);
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);
		}
//		ToggleMouseLook (true);

		camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=true;
		fakeRoadblockP2.SetActive (true);
		yield return StartCoroutine(WaitForPlayerDecision(phase2LeftRegister.transform.position,phase2RightRegister.transform.position));
		fakeRoadblockP2.SetActive (false);
//		ToggleMouseLook(false);
		Debug.Log("PLAYER CHOICE: " + playerChoice.ToString());
		Experiment.Instance.shopLiftLog.LogDecisionEvent (false);

		camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;
		if (hasRewards) {
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
//		animBody.GetComponent<Rigidbody> ().isKinematic = true;
//		yield return new WaitForSeconds(3f);
//		cartAnim.enabled = false;
//		camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook.m_CharacterTargetRot = Quaternion.Euler(dummyObj.transform.eulerAngles);
			yield return StartCoroutine (ShowRegisterReward ());
		}

		yield return null;
	}

	IEnumerator PickEnvironment()
	{
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
	
		AssignRooms ();
		yield return null;
	}

    IEnumerator RunTask()
    {
		stageIndex = 1;
		//Experiment.Instance.trialLog.LogTrialNavigation (true);
        Debug.Log("running task");
		instructionGroup.alpha = 1f;
		while(!Input.GetButtonDown("Action Button"))
		{
			yield return 0;
		}
		instructionGroup.alpha = 0f;

		yield return StartCoroutine (PickEnvironment ());

//		yield return StartCoroutine (RunCamTrainingPhase ());

		ChangeCameraZoneVisibility (true);
        yield return StartCoroutine(PickFourRegisterValues());

		//stage 1
		Experiment.Instance.shopLiftLog.LogStageEvent(1,true);
//		while(numTrials < 1)
		while(registerLeft.Count > 0 || numTrials < maxTrials)
        { 
			Debug.Log ("about to run phase 1");
			yield return StartCoroutine (RunPhaseOne());

			Debug.Log ("about to run phase 2");
			yield return StartCoroutine (RunPhaseTwo(false,true));
			TurnOffRooms ();
			if (numTrials < maxTrials - 1 || registerLeft.Count > 0)
				yield return StartCoroutine (ShowEndTrialScreen ());
			else
				yield return StartCoroutine (ShowNextStageScreen ());
        	numTrials++;
        	yield return 0;
		}
		Experiment.Instance.shopLiftLog.LogStageEvent (1, false);
//		ReassignRooms ();
		ShuffleRegisterRewards ();
		numTrials = 0; //reset num trials
		Debug.Log("about to start stage 2");
		//stage 2
		stageIndex = 2;
		bool leftChoice = false;
		Experiment.Instance.shopLiftLog.LogStageEvent(2,true);
		while (numTrials < maxTrials) {
			leftChoice = !leftChoice; //flip it
			if (leftChoice) {
				leftRoom.SetActive (true);
				leftAudio.Play ();
				ChangeColors (leftRoomColor);
				phase1Choice = 0;
			} else {
				rightRoom.SetActive (true);
				rightAudio.Play ();
				ChangeColors (rightRoomColor);
				phase1Choice = 1;
			}
			yield return StartCoroutine (RunPhaseTwo (true,true));
			TurnOffRooms ();
			yield return StartCoroutine(ShowEndTrialScreen());
			yield return 0;
		}

		Experiment.Instance.shopLiftLog.LogStageEvent(2,false);
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
        choiceOutput = (phase1Choice * 2) + phase2Choice;
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
		infoGroup.alpha = 1f;
		Experiment.Instance.shopLiftLog.LogRegisterReward(phase1Choice,phase2Choice,registerVals[choiceOutput]);
        infoText.text = "You got $" + registerVals[choiceOutput].ToString() + " from the register";
        yield return new WaitForSeconds(2f);
        infoGroup.alpha = 0f;

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

	IEnumerator WaitForPlayerDecision(Vector3 leftPoint, Vector3 rightPoint)
	{
		while (!Input.GetButtonDown("Action Button") || (Vector3.Distance(camVehicle.transform.position,rightPoint)  == Vector3.Distance(camVehicle.transform.position,leftPoint))) {
//			Debug.Log (Mathf.Abs(camVehicle.transform.localPosition.z).ToString());
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



	IEnumerator MovePlayerTo(Vector3 startPos, Vector3 endPos, float factor)
	{
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		float timer = 0f;
		Debug.Log ("about to move player");
		while (timer / factor < 1f) {
			timer += Time.deltaTime;
//			Debug.Log ("timer " + timer.ToString ());
			camVehicle.transform.position = Vector3.Lerp (startPos, endPos, timer / factor);
			yield return 0;
		}

		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}

		
}
