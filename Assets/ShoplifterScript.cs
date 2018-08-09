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

	public float minSpeed = 25f;
	public float maxSpeed  = 60f;

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

	//stage 1 learning variables
	private int numTrials_Learning = 0;
	public int maxTrials_Learning = 24;

	//stage 2 reevaulation variables
	public int maxTrials_Reeval = 4;
	public int maxBlocks_Reeval = 3;

    public GameObject leftDoorPos;
    public GameObject rightDoorPos;
    public float phase1Factor = 5f;
    public Animator cartAnim;
    private int playerChoice = -1; //0 for left and 1 for right
	public List<int> registerVals; // 0-1 is L-R for toy , 2-3 is L-R for hardware

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
	public CanvasGroup prefSolo;
	public CanvasGroup prefGroup;


	private GameObject roomOne;
	private GameObject roomTwo;

	//instr strings
	private string doorText = "Press (X) to open the door";
	private string registerText = "Press (X) to open the suitcase!";


	//audio
	private AudioSource one_L_Audio;
	private AudioSource two_L_Audio;
	private AudioSource three_L_Audio;
	private AudioSource one_R_Audio;
	private AudioSource two_R_Audio;
	private AudioSource three_R_Audio;

	private AudioSource currentAudio;

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


	private Texture leftTexture;
	private Texture rightTexture;
	public List<Texture> suitcaseTextures;


	public GameObject suitcasePrefab;

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
		prefSolo.alpha = 0f;
		prefGroup.alpha = 0f;
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
		Debug.Log(ExperimentSettings.env.ToString());
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

	void RandomizeSuitcaseTextures()
	{
		if (Random.value > 0.5f) {
			leftTexture = suitcaseTextures [0];
			rightTexture = suitcaseTextures [1];
		} else {
			leftTexture = suitcaseTextures [1];
			rightTexture = suitcaseTextures [0];
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

		phase1CamZone_L.transform.position = new Vector3(envManager.phase1Start_L.transform.position.x,-1.7f,Mathf.Lerp (envManager.phase1Start_L.transform.position.z, envManager.phase1End_L.transform.position.z, 0.5f));
		phase1CamZone_R.transform.position =new Vector3(envManager.phase1Start_R.transform.position.x,-1.7f, Mathf.Lerp (envManager.phase1Start_R.transform.position.z, envManager.phase1End_R.transform.position.z, 0.5f));


		phase2CamZone_L.transform.position = new Vector3(envManager.phase2Start_L.transform.position.x,-1.7f,Mathf.Lerp (envManager.phase2Start_L.transform.position.z, envManager.phase2End_L.transform.position.z, 0.5f));
		phase2CamZone_R.transform.position = new Vector3(envManager.phase2Start_R.transform.position.x,-1.7f,Mathf.Lerp (envManager.phase2Start_R.transform.position.z, envManager.phase2End_R.transform.position.z, 0.5f));

		phase3CamZone_L.transform.position = new Vector3(envManager.phase3Start_L.transform.position.x,-1.7f,Mathf.Lerp (envManager.phase3Start_L.transform.position.z, envManager.phase3End_L.transform.position.z, 0.5f));
		phase3CamZone_R.transform.position = new Vector3(envManager.phase3Start_R.transform.position.x,-1.7f,Mathf.Lerp (envManager.phase3Start_R.transform.position.z, envManager.phase3End_R.transform.position.z, 0.5f));
	}

	public void ChangeCamZoneFocus(int camIndex)
	{
		Debug.Log ("changing cam zone focus to " + camIndex.ToString ());
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
		activeCamZone.GetComponent<CameraZone> ().isFocus = true;

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
			three_L_Audio = envManager.three_L_Audio;
			leftRoomColor = roomOneColor;
			rightRoom = roomTwo;
			three_R_Audio = envManager.three_R_Audio;
			rightRoomColor = roomTwoColor;


			Experiment.Instance.shopLiftLog.LogRooms ("TOY","HARDWARE");
        }
        else
        {
            leftRoom = roomTwo;
			three_L_Audio = envManager.three_R_Audio;
			leftRoomColor = roomTwoColor;

            rightRoom = roomOne;
			three_R_Audio = envManager.three_L_Audio;
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
			leftRoomColor = roomOneColor;
			three_L_Audio = envManager.three_L_Audio;


			rightRoom = roomTwo;
			rightRoomColor = roomTwoColor;
			three_R_Audio = envManager.three_R_Audio;

			Experiment.Instance.shopLiftLog.LogRooms ("HARDWARE","TOY");

        }
        else
        {
			leftRoom = roomTwo;
			leftRoomColor = roomTwoColor;
			three_L_Audio = envManager.three_R_Audio;


			rightRoom = roomOne;
			rightRoomColor = roomOneColor;
			three_R_Audio = envManager.three_L_Audio;

			Experiment.Instance.shopLiftLog.LogRooms ("TOY","HARDWARE");
        }

		Texture tempTexture=null;
		leftTexture = tempTexture;
		leftTexture = rightTexture;
		rightTexture = tempTexture;
		leftRoom.transform.position = envManager.leftRoomTransform.position;
		rightRoom.transform.position = envManager.rightRoomTransform.position;
    }

	void ShuffleRegisterRewards()
	{
		List<int> registerValList = new List<int> ();
		for(int i=0;i<registerVals.Count;i++)
		{
			registerValList.Add (registerVals [i]);
		}
		registerVals.Clear ();
		for (int i = 0; i < registerVals.Count; i++) {
			int randomIndex = Random.Range (0, registerValList.Count);
			registerVals.Add(registerValList [randomIndex]);
			registerValList.RemoveAt (randomIndex);
		}
	}

	IEnumerator RunCamTrainingPhase()
	{

		RandomizeSpeedChangeZones ();
		Debug.Log ("starting cam training phase");
		CameraZone.isTraining = true;

		trainingInstructionsGroup.alpha = 1f;
		yield return StartCoroutine(WaitForButtonPress (10000f));
		trainingInstructionsGroup.alpha = 0f;
		trainingPeriodGroup.alpha = 1f;
		bool isLeft = false;
		int numTraining = 0;
		while (numTraining < 4) {
			Debug.Log ("about to run phase 1");
			isLeft = !isLeft;
			yield return StartCoroutine (RunPhaseOne ((isLeft) ? 0:1,false,-1,false));

			Debug.Log ("about to run phase 2");
			yield return StartCoroutine (RunPhaseTwo ((isLeft) ? 0:1,false,false,-1,false));
//			TurnOffRooms ();
			Debug.Log("about to run phase 3");
			yield return StartCoroutine(RunPhaseThree((isLeft) ? 0:1,false,false));
			if (numTrials < maxTrials - 1)
				yield return StartCoroutine (ShowEndTrialScreen ());
			numTraining++;
			yield return 0;
		}
//		ResetCamZone ();
		CameraZone.isTraining = false;

		trainingPeriodGroup.alpha = 0f;
		yield return null;
	}


    IEnumerator PickRegisterValues()
    {
		registerLeft = new List<int> ();
		registerVals = new List<int> ();
		for(int i=0;i<2;i++)
        {
			registerVals.Add(Random.Range(5, 95));
        }

		Debug.Log ("register val at 0 is: " + registerVals [0].ToString ());
		Debug.Log ("register val at 1 is: " + registerVals [1].ToString ());
        yield return null;
    }

	IEnumerator RunPhaseOne(int pathIndex, bool isGuided, int guidedChoice, bool terminateWithChoice)
	{
		ChangeCamZoneFocus ((pathIndex == 0) ? 0 : 3);
		GameObject targetDoor = (pathIndex==0) ? phase1Door_L : phase1Door_R;
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		clearCameraZoneFlags = false;
		Debug.Log ("running phase one");
		AudioSource baseAudio = (pathIndex == 0) ? one_L_Audio : one_R_Audio;
		float delayOne = Random.Range (0f, baseAudio.clip.length);

		baseAudio.time = delayOne;
		baseAudio.Play ();
		if (numTrials >= 1)
			ChangeCameraZoneVisibility (false);

		ChangeCamZoneFocus((pathIndex==0) ? 0:3);
		ToggleMouseLook(false);
		Debug.Log ("path index is: " + pathIndex.ToString ());
		Vector3 startPos = (pathIndex == 0) ? phase1Start_L.transform.position : phase1Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase1End_L.transform.position : phase1End_R.transform.position;
		Debug.Log ("player pos: " + camVehicle.transform.position.ToString());
		Debug.Log ("start pos is: " + startPos.ToString());
		camVehicle.transform.position = startPos;
		camVehicle.SetActive (true);
		Debug.Log ("player pos: " + camVehicle.transform.position.ToString());
		Experiment.Instance.shopLiftLog.LogMoveEvent (1,true);
		Debug.Log ("about to velo move player");
		yield return StartCoroutine(VelocityPlayerTo (startPos,endPos, phase1Factor));

		Debug.Log ("finished velo move player");
		Experiment.Instance.shopLiftLog.LogMoveEvent (1,false);

		clearCameraZoneFlags = true;
		yield return StartCoroutine(WaitForDoorOpenPress (doorText));
		float delayTwo = 0f;
		if (!terminateWithChoice) {
			Doors.canOpen = true;
			yield return StartCoroutine (targetDoor.GetComponent<Doors> ().Open ());
		
//		ToggleMouseLook(false);


			if (pathIndex == 0) {

				ChangeCamZoneFocus (1);
				currentAudio = two_L_Audio;
				ChangeColors (rightRoomColor);

				phase2Start_L = envManager.phase2Start_L;
				phase2End_L = envManager.phase2End_L;

				Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
				if (!terminateWithChoice) {
					yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase1Door_L.transform.GetChild (0).position, 0.5f));
					baseAudio.Stop ();
					delayTwo = Random.Range (0f, currentAudio.clip.length);
					currentAudio.time = delayTwo;
					currentAudio.Play ();
					yield return StartCoroutine (MovePlayerTo (phase1Door_L.transform.GetChild (0).position, phase2Start_L.transform.position, 0.5f));
				}

			} else if (pathIndex == 1) {

				ChangeCamZoneFocus (4);
				currentAudio = two_R_Audio;
				ChangeColors (leftRoomColor);

				phase2Start_R = envManager.phase2Start_R;
				phase2End_R = envManager.phase2End_R;
				Experiment.Instance.shopLiftLog.LogMoveEvent (2, true);
				if (!terminateWithChoice) {
					yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase1Door_R.transform.GetChild (0).position, 0.5f));
									
					baseAudio.Stop ();
					delayTwo = Random.Range (0f, currentAudio.clip.length);
					currentAudio.time = delayTwo;
					currentAudio.Play ();
					yield return StartCoroutine (MovePlayerTo (phase1Door_R.transform.GetChild (0).position, phase2Start_R.transform.position, 0.5f));
				}
			}
			Doors.canOpen = false;
		} else {
			baseAudio.Stop ();
		}

		Debug.Log ("closing the first door now");
		targetDoor.GetComponent<Doors> ().Close ();
		yield return null;
	}

	IEnumerator RunPhaseTwo(int pathIndex,bool isDirect, bool isGuided, int guidedChoice, bool terminateWithChoice)
	{
		ChangeCamZoneFocus ((pathIndex == 0) ? 1 : 4);
		GameObject targetDoor = (pathIndex==0) ? phase2Door_L : phase2Door_R;
		Vector3 startPos = (pathIndex == 0) ? phase2Start_L.transform.position : phase2Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase2End_L.transform.position : phase2End_R.transform.position;
		if (isDirect) {
			currentAudio = (pathIndex == 0) ? two_L_Audio : two_R_Audio;
			float delay = Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delay;
			currentAudio.Play ();
		}
		clearCameraZoneFlags = false;
		Debug.Log ("running phase two");
			camVehicle.transform.position = startPos;
			Debug.Log("velo player in phase 2");
		yield return StartCoroutine(VelocityPlayerTo(startPos,endPos,phase1Factor));
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);

		clearCameraZoneFlags = true;
		float delayThree = 0f;
		yield return StartCoroutine (WaitForDoorOpenPress (doorText));

			yield return StartCoroutine(targetDoor.GetComponent<Doors> ().Open ());
			if (pathIndex == 0) {
			yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase2Door_L.transform.GetChild(0).position, 0.5f));
			currentAudio.Stop ();
			currentAudio = three_L_Audio;
			delayThree = Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delayThree;
			currentAudio.Play ();
				yield return StartCoroutine (MovePlayerTo (phase2Door_L.transform.GetChild(0).position, phase3Start_L.transform.position, 0.5f));
		
		} else if (pathIndex == 1) {			
			currentAudio.Stop ();
			currentAudio = three_R_Audio;
			delayThree = Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delayThree;
			currentAudio.Play();
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, phase2Door_R.transform.GetChild(0).position, 0.5f));
				yield return StartCoroutine (MovePlayerTo (phase2Door_R.transform.GetChild(0).position, phase3Start_R.transform.position, 0.5f));
			}

		Debug.Log ("closing the second door now");
		targetDoor.GetComponent<Doors>().Close();
		yield return null;
	}

	IEnumerator RunPhaseThree(int pathIndex,bool isDirect, bool hasRewards)
	{
		ChangeCamZoneFocus ((pathIndex == 0) ? 2 : 5);
		Vector3 startPos = (pathIndex == 0) ? phase3Start_L.transform.position : phase3Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase3End_L.transform.position : phase3End_R.transform.position;
		if (isDirect) {
			currentAudio = (pathIndex == 0) ? three_L_Audio : three_R_Audio;
			float delay = Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delay;
			currentAudio.Play ();
		}
		clearCameraZoneFlags = false;
		Debug.Log ("running phase three");
			camVehicle.transform.position = startPos;
			Debug.Log("velo player in phase 3");
			Experiment.Instance.shopLiftLog.LogMoveEvent (3, true);
		yield return StartCoroutine(VelocityPlayerTo(startPos,endPos,phase1Factor));
			Experiment.Instance.shopLiftLog.LogMoveEvent (3, false);
		if (hasRewards) {
			clearCameraZoneFlags = true;

			if (pathIndex == 0) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, register_L.transform.position, 0.5f));

			} else if (pathIndex == 1) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, register_R.transform.position, 0.5f));
			}

			yield return StartCoroutine (WaitForDoorOpenPress (registerText));
			yield return StartCoroutine (ShowRegisterReward (pathIndex));
			Debug.Log ("closing the third door now");
		}
		currentAudio.Stop ();
		yield return null;
	}


	IEnumerator RunLearningPhase()
	{
		Debug.Log("running task");
		int sliderCount = 0;
		instructionGroup.alpha = 1f;
		yield return StartCoroutine (WaitForButtonPress (10000f));
		instructionGroup.alpha = 0f;

		ChangeCameraZoneVisibility (false); // no need to show cam zones as they were already shown during training

		bool isLeft = (Random.value < 0.5f) ? true: false;
		bool showOneTwo = false;
		//stage 1
		Experiment.Instance.shopLiftLog.LogStageEvent(1,true);


		//		while(numTrials < 1)
		while(numTrials_Learning < maxTrials_Learning)
		{ 
			Debug.Log ("about to run phase 1");
			isLeft = !isLeft;
			yield return StartCoroutine (RunPhaseOne ((isLeft) ? 0 : 1, false, -1,false));

			Debug.Log ("about to run phase 2");

				yield return StartCoroutine (RunPhaseTwo((isLeft) ? 0 : 1,false,false,-1,true));

			Debug.Log("about to run phase 3");
			yield return StartCoroutine(RunPhaseThree((isLeft) ? 0:1,false,true));
//			TurnOffRooms ();
			if (numTrials_Learning % 3 == 0 && numTrials_Learning > 0) {
				showOneTwo = !showOneTwo;
				if (sliderCount <= 5) {
					if (showOneTwo) {
						yield return StartCoroutine (AskPreference (0));
					} else
						yield return StartCoroutine (AskPreference (1));
				}
				sliderCount++;
			}
			if (numTrials_Learning < maxTrials_Learning - 1)
				yield return StartCoroutine (ShowEndTrialScreen ());
			else
				yield return StartCoroutine (ShowNextStageScreen ());
			numTrials_Learning++;
			yield return 0;
		}

		camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;
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
		if (Random.value<0.5f)
			SetupTransitionReeval ();
		else
			SetupRewardReeval ();
		
		Experiment.Instance.shopLiftLog.LogStageEvent(2,true);
		while (numBlocks_Reeval < maxBlocks_Reeval) {
			while (numTrials_Reeval < maxTrials_Reeval) {
				leftChoice = !leftChoice; //flip it

				Debug.Log ("about to run phase 2");
				yield return StartCoroutine (RunPhaseTwo((leftChoice) ? 0 : 1,true,false,-1,true));

				Debug.Log("about to run phase 3");
				yield return StartCoroutine(RunPhaseThree((leftChoice) ? 0:1,false,true));
//				TurnOffRooms ();
				if(numTrials_Reeval < maxTrials_Reeval-1)
					yield return StartCoroutine (ShowEndTrialScreen ());
				numTrials_Reeval++;
				yield return 0;
			}
			yield return StartCoroutine (AskPreference (1));
//			yield return StartCoroutine (RunRestPeriod());
			numTrials_Reeval = 0;
			numBlocks_Reeval++;
			yield return 0;
		}
		yield return StartCoroutine (ShowNextStageScreen ());
		Experiment.Instance.shopLiftLog.LogStageEvent(2,false);
		yield return null;
	}
	IEnumerator RunRestPeriod()
	{
		restGroup.alpha = 1f;
		yield return new WaitForSeconds (20f);
		restGroup.alpha = 0f;
		yield return null;
	}


	IEnumerator RunTestingPhase()
	{
		bool leftChoice = false;
		for (int i = 3; i < 4; i++) {
			leftChoice = !leftChoice;
			yield return StartCoroutine(RunPhaseOne ((leftChoice) ? 0 : 1, false, -1, true));
			if (i == 1 || i == 3) {
				yield return StartCoroutine (RunRestPeriod ());
			}
			if (i==3) {
				yield return StartCoroutine (AskSoloPreference (0));
				yield return StartCoroutine (AskSoloPreference (1));
				yield return StartCoroutine (AskPreference (0));

			}
		}
		yield return StartCoroutine (ShowEndTrialScreen ());
		yield return null;
	}

	void SetupTransitionReeval()
	{
		ReassignRooms ();
	}

	void SetupRewardReeval()
	{
		int temp = 0;
		int temp2 = 0;
		Debug.Log ("count: " + registerVals.Count.ToString ());
		registerVals.Reverse ();
		Debug.Log ("count: " + registerVals.Count.ToString ());
		Debug.Log ("register vals at 0 : " + registerVals [0].ToString ());
		Debug.Log ("register vals at 1 : " + registerVals [1].ToString ());
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

	IEnumerator AskSoloPreference(int prefIndex)
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		prefSolo.GetComponent<PrefSoloSetup> ().SetupPrefs (prefIndex);

		prefSolo.alpha = 1f;
		yield return StartCoroutine (WaitForButtonPress (10f));
		prefSolo.alpha = 0f;
		Cursor.visible = false;

		yield return null;
	}

	IEnumerator AskPreference(int prefType)
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		switch (prefType) {
		//between 1 and 2
		case 0:
			prefGroup.GetComponent<PrefGroupSetup> ().SetupPrefs (0);
			break;
		//between 3 and 4
		case 1:
			prefGroup.GetComponent<PrefGroupSetup> ().SetupPrefs (1);
			break;
			
		}

		prefGroup.alpha = 1f;
		yield return StartCoroutine (WaitForButtonPress (10f));
		prefGroup.alpha = 0f;

		Cursor.visible = false;
		yield return null;
	}

	IEnumerator WaitForDoorOpenPress(string text)
	{
		infoText.text = text;
		infoGroup.alpha = 1f;
		yield return StartCoroutine (WaitForButtonPress (5f));
		infoGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator WaitForButtonPress(float maxWaitTime)
	{
		float timer = 0f;
		while (!Input.GetButtonDown ("Action Button") && timer < maxWaitTime) {
			timer += Time.deltaTime;
			yield return 0;
		}
		yield return null;
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

		phase2Start_L =envManager.phase2Start_L;
		phase2Start_R =envManager.phase2Start_R;
		phase2End_L =envManager.phase2End_L;
		phase2End_R =envManager.phase2End_R;

		phase3Start_L =envManager.phase3Start_L;
		phase3Start_R =envManager.phase3Start_R;
		phase3End_L =envManager.phase3End_L;
		phase3End_R =envManager.phase3End_R;

		one_L_Audio = envManager.one_L_Audio;
		two_L_Audio = envManager.two_L_Audio;
		three_L_Audio = envManager.three_L_Audio;

		one_R_Audio = envManager.one_R_Audio;
		two_R_Audio = envManager.two_R_Audio;
		three_R_Audio = envManager.three_R_Audio;

		roomOne = envManager.roomOne;
		roomTwo = envManager.roomTwo;

		phase1CamZone_L = envManager.phase1CamZone_L;
		phase1CamZone_R = envManager.phase1CamZone_R;

		phase2CamZone_L = envManager.phase2CamZone_L;
		phase2CamZone_R = envManager.phase2CamZone_R;

		phase3CamZone_L = envManager.phase3CamZone_L;
		phase3CamZone_R = envManager.phase3CamZone_R;

		register_L = envManager.register_L;
		register_R = envManager.register_R;

		leftRegisterObj = envManager.leftRegisterObj;
		rightRegisterObj = envManager.rightRegisterObj;

		//after env has been selected and all necessary object references set, assign rooms and randomize cam zones
		AssignRooms ();
		RandomizeSpeedChangeZones ();
		RandomizeCameraZones ();
		yield return null;
	}

    IEnumerator RunTask()
    {
		stageIndex = 1;

		yield return StartCoroutine(PickRegisterValues());

		for (int i = 0; i < environments.Count; i++) {
			yield return StartCoroutine (PickEnvironment (i));

			if (ExperimentSettings.isTraining)
				yield return StartCoroutine (RunCamTrainingPhase ());

			//randomize rooms and cam zones again
			AssignRooms ();
			RandomizeCameraZones ();
			RandomizeSuitcaseTextures ();

			//learning phase
			if (ExperimentSettings.isLearning)
				yield return StartCoroutine (RunLearningPhase ());

			//shuffle rewards
//		ReassignRooms ();
//			ShuffleRegisterRewards ();

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

	IEnumerator ShowRegisterReward(int pathIndex)
    {
		GameObject chosenRegister = null;
		Texture chosenTexture = null;
		choiceOutput = pathIndex;
		switch (choiceOutput) {
		case 0:
			chosenRegister = leftRegisterObj;
			chosenTexture = leftTexture;
			break;
		case 1:
			chosenRegister = rightRegisterObj;
			chosenTexture = rightTexture;
			break;
		
		}

		Debug.Log("chosen register is: " + chosenRegister.name);
		GameObject suitcaseObj = Instantiate (suitcasePrefab, camVehicle.transform.position + (new Vector3(0f,0.2f,-1f) * 2f), Quaternion.Euler (new Vector3 (-90f, 180f, 0f))) as GameObject;
//		suitcaseObj.GetComponent<Suitcase> ().ChooseTexture (chosenTexture);
		//first wait for 1 second to show face/scene
//		yield return new WaitForSeconds (2f);

		//then open the suitcase
		suitcaseObj.GetComponent<Animator> ().SetTrigger ("Open");
//		suitcaseObj.GetComponent<Suitcase>().TurnImageOff();

		GameObject coinShowerObj = Instantiate(coinShower,chosenRegister.transform.position,Quaternion.identity) as GameObject;		
		if (activeEnvLabel == "Cybercity") {
			coinShowerObj.transform.GetChild (0).transform.localPosition = Vector3.zero;
		}
		//wait until suitcase is fully open
		yield return new WaitForSeconds (0.5f);
		rewardScore.enabled = true;
		rewardScore.text = "$" + registerVals [choiceOutput].ToString ();

		chosenRegister.GetComponent<AudioSource> ().Play (); //play the cash register audio

		Experiment.Instance.shopLiftLog.LogRegisterReward(phase1Choice,phase2Choice,registerVals[choiceOutput]);
//        infoText.text = "You got $" + registerVals[choiceOutput].ToString() + " from the register";
		Debug.Log("waiting for 2 seconds");
		yield return StartCoroutine(rewardScore.gameObject.GetComponent<FontChanger> ().GrowText (2f));
		rewardScore.enabled = false;
//        infoGroup.alpha = 0f;
		Destroy(coinShowerObj);
		Destroy (suitcaseObj);
//		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
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
//		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}


	//uses velocity and distance check to move player
	IEnumerator VelocityPlayerTo(Vector3 startPos, Vector3 endPos, float factor)
	{
		Vector3 moveDir = new Vector3 (0f, 0f, -1f);
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		float timer = 0f;
		Debug.Log ("velocity player movement");
		while (Vector3.Distance(camVehicle.transform.position,endPos) > 1.5f) {
			timer += Time.deltaTime;
//			Debug.Log ("timer " + timer.ToString ());
//			Debug.Log("move dir: " + moveDir.ToString() + " current speed: " + currentSpeed.ToString());
			camVehicle.GetComponent<Rigidbody>().velocity = moveDir * currentSpeed;
//			Debug.Log ("distance to end: " + Vector3.Distance (camVehicle.transform.position, endPos).ToString ());
//			camVehicle.transform.position = Vector3.Lerp (startPos, endPos, timer / factor);
			yield return 0;
		}
//		Debug.Log ("stopping the player");
		camVehicle.GetComponent<Rigidbody> ().velocity = Vector3.zero;
//		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}

		
}
