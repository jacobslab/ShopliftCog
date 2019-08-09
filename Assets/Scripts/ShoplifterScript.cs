using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Linq;
using System.IO;
public class ShoplifterScript : MonoBehaviour
{

    public GameObject camVehicle;
    public Camera mainCam;
    public GameObject animBody;
    private MouseLook mouseLook;

    private float currentSpeed;

    public float minSpeed = 25f;
    public float maxSpeed = 60f;

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

    //speed change zones
    public List<GameObject> phase1SpeedChangeZones_L;
    public List<GameObject> phase1SpeedChangeZones_R;
    public List<GameObject> phase2SpeedChangeZones_L;
    public List<GameObject> phase2SpeedChangeZones_R;
    public List<GameObject> phase3SpeedChangeZones_L;
    public List<GameObject> phase3SpeedChangeZones_R;

    //camera zone manager
    public CameraZoneManager cameraZoneManager;


    //learning bool
    private bool hasLearned = false;

    //deviation measure
    public Queue<float> deviationQueue;

    public AudioListener mainSceneListener;

    //halts player
    public static bool haltPlayer = false;

    private int correctResponses = 0;

    //SYSTEM2 Server
    public TCPServer tcpServer;


    public List<int> registerVal1;
    public List<int> registerVal2;

    bool isTransition = false;

    //stage 1 learning variables
    private int numTrials_Learning = 0;
    private int maxTrials_Learning = 24;


    //variables for additional learning phase
    private int numAdditionalTrials = 0;
    private int maxAdditionalTrials = 12;

    //stage 2 reevaulation variables
    private int maxTrials_Reeval = 2;
    private int maxBlocks_Reeval = 6;

    private int envIndex = 0;

    private List<float> camZoneFactors;

    //stage 4 post-test variables
    private int maxTrials_PostTest = 10;

    public GameObject leftDoorPos;
    public GameObject rightDoorPos;
    public float phase1Factor = 5f;
    public Animator cartAnim;
    private int playerChoice = -1; //0 for left and 1 for right
    public List<int> registerVals; // 0-1 is L-R for toy , 2-3 is L-R for hardware

    private string activeEnvLabel = "";

    public GameObject instructionVideo;

    public List<GameObject> environments;
    private EnvironmentManager envManager;

    private GameObject leftRoom;
    private GameObject rightRoom;

    float suggestedSpeed = 0f;

    float directionEnv = -1f; //-1 for space station, +1 for western town
    private int phase1Choice = 0;
    private int phase2Choice = 0;
    private int choiceOutput = 0;

    public Transform phase1Target;
    public Transform phase2Target;

    private bool clearCameraZoneFlags = false;

    private Transform camTrans;

    private int numTrials = 0;
    private int maxTrials = 4;

    bool firstTime = true;

    string currentPhaseName = "NONE";
#if !SPANISH
    private string pressToContinueInstruction = "Press (X) button to continue";
    private string musicBaselineInstruction = "In what follows you will hear music from the game. \n Please maintain your gaze at the fixation cross, relax, and pay attention to the music.";
    private string imageSlideshowInstruction = "In what follows you will see images from the game. \n Please maintain your gaze on the screen, relax, and pay attention to different images that appear on the screen.";
#else
    private string pressToContinueInstruction = "Presiona (X) para continuar";
    private string musicBaselineInstruction = "A continuar escucharas música del juego.\n Por favor mantenga su mirada en la cruz. \n Relájate y preste atención a la música.";
    private string imageSlideshowInstruction = "A continuar escucharas imagenes del juego.\n Por favor mantenga su mirada en la cruz. \n Relájate y preste atención a la imagenes.";
    
#endif

    //tip metrics
    private int consecutiveIncorrectCameraPresses = 0; //activated when >=4
    private bool didTimeout = false; //activated after a timeout during slider event
    private bool afterSlider = false; //activated immediately after a slider event

    //ui
    public CanvasGroup introInstructionGroup;
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
    public CanvasGroup dotGroup;
    public Text rewardScore;
    public CanvasGroup warningFeedbackGroup;
    public CanvasGroup prefSolo;
    public CanvasGroup prefGroup;
    public CanvasGroup multipleChoiceGroup;
    public CanvasGroup imagineGroup;
    public CanvasGroup imageryQualityGroup;
    public CanvasGroup tipsGroup;
    public Text tipsText;
    public CanvasGroup blackScreen;
    public CanvasGroup pauseUI;
    public CanvasGroup sys2ConnectionGroup;
    public Text sys2ConnectionText;



    //TRAINING environment
    public GameObject vikingEnv;
    



    private GameObject roomOne;
    private GameObject roomTwo;

    //instr strings
    private string doorText = "Presiona el botón (X) para abrir la puertar";
    private string registerText = "Presiona el botón (X) para abrir la maleta";


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

    private GameObject suitcaseObj;

    public GameObject coinShower;

    private List<int> registerLeft;
    private int stageIndex = 0;

    //camera zone
    private GameObject phase1CamZone_L;
    private GameObject phase1CamZone_R;
    private GameObject phase2CamZone_L;
    private GameObject phase2CamZone_R;
    private GameObject phase3CamZone_L;
    private GameObject phase3CamZone_R;

    private GameObject activeCamZone;

    public Text rigidStatusText;

    private GameObject leftSuitcase;
    private GameObject rightSuitcase;
    private List<GameObject> suitcases;

    public List<int> reevalConditions;

    public bool isPaused = false;


    //for baseline music sequence at the end
    private List<AudioClip> completeAudioList;
    public AudioSource musicBaselinePlayer;
    public float musicBaselinePlayTime = 15f;

    //for baseline image slideshow sequence at the end
    private List<Texture> completeImageList;
    public RawImage slideshowImage;
    public float imageSlideshowPlaytime = 4f;


    private GameObject suitcasePrefab;
    private Material skyboxMat;

    public GameObject testFloor;

    private int[] trainingReward = new int[2];

    enum EnvironmentIndex
    {
        FirstEnv,
        SecondEnv,
        TrainingEnv
    };



    public class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;
        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }
    //SINGLETON
    private static ShoplifterScript _instance;

    public static ShoplifterScript Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {

        if (_instance != null)
        {
            Debug.Log("Instance already exists!");
            Destroy(transform.gameObject);
            return;
        }
        _instance = this;

        blackScreen.alpha = 1f;
        pauseUI.alpha = 0f;
        instructionVideo.SetActive(false);
        EnablePlayerCam(false);
        Application.targetFrameRate = 60;
        deviationQueue = new Queue<float>();
    }

    void EnablePlayerCam(bool shouldEnable)
    {
        mainCam.enabled = shouldEnable;
    }

    // Use this for initialization
    void Start()
    {
        camZoneFactors = new List<float>();
        camZoneFactors = GetRandomNumbers(environments.Count); //get as many unique random numbers as there are environments
                                                               //		reevalConditions = new List<int>();
                                                               //		reevalConditions= ShuffleReevalConditions();
        introInstructionGroup.alpha = 0f;
        //		UpdateFirstEnvironments ();
        infoGroup.alpha = 0f;
        imagineGroup.alpha = 0f;
        positiveFeedbackGroup.alpha = 0f;
        negativeFeedbackGroup.alpha = 0f;
        intertrialGroup.alpha = 0f;
        trainingInstructionsGroup.alpha = 0f;
        trainingPeriodGroup.alpha = 0f;
        prefSolo.gameObject.SetActive(false);
        prefGroup.gameObject.SetActive(false);
        slideshowImage.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

        suitcaseObj = null;
        //        cartAnim.enabled = true;
        camVehicle.SetActive(true);
        //        animBody.SetActive(false);

        //        cartAnim.Play("Phase1Start");
        //camVehicle.transform.position = phase1Start.transform.position;
        //        mouseLook = camVehicle.GetComponent<RigidbodyFirstPersonController>().mouseLook;
        camTrans = camVehicle.GetComponent<RigidbodyFirstPersonController>().cam.transform;
        RandomizeSpeed();
        //		RandomizeCameraZones ();
        rewardScore.enabled = false;
        //		mouseLook.XSensitivity = 0f;

        Cursor.visible = false;
        //then start the task
        //		Debug.Log(ExperimentSettings.env.ToString());
        StartCoroutine("RunTask");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        GetPauseInput();
    }

    void RandomizeSuitcases()
    {
        if (Random.value > 0.5f)
        {
            leftSuitcase = suitcases[0];
            rightSuitcase = suitcases[1];
        }
        else
        {
            leftSuitcase = suitcases[1];
            rightSuitcase = suitcases[0];
        }
    }

    void CheckpointSession(int blockCount, bool isOngoing)
    {
        string separator = "\t";
        int currentBlockCount = blockCount;
        //environment + phase + reeval type + leftroom + leftreward + rightroom +  rightreward
        string line = ((isOngoing) ? "ONGOING" : "FINISHED") + separator + envIndex.ToString() + separator + currentPhaseName + separator + reevalConditions[currentBlockCount].ToString() + separator + leftRoom.name + separator + registerVals[0].ToString() + separator + rightRoom.name + separator + registerVals[1].ToString();
        Debug.Log("checkpointed line is: " + line);
        System.IO.File.WriteAllText(Experiment.Instance.sessionDirectory + "checkpoint.txt", line);
    }

    void RandomizeSpeedChangeZones()
    {
        Debug.Log("randomized speed change zones");
        phase1SpeedChangeZones_L[0].transform.position = new Vector3(phase1Start_L.transform.position.x, phase1Start_L.transform.position.y, Random.Range(phase1Start_L.transform.position.z, Vector3.Lerp(phase1Start_L.transform.position, phase1End_L.transform.position, 0.5f).z));
        phase1SpeedChangeZones_L[1].transform.position = new Vector3(phase1Start_L.transform.position.x, phase1Start_L.transform.position.y, Random.Range(Vector3.Lerp(phase1Start_L.transform.position, phase1End_L.transform.position, 0.5f).z, phase1End_L.transform.position.z));

        phase1SpeedChangeZones_R[0].transform.position = new Vector3(phase1Start_R.transform.position.x, phase1Start_R.transform.position.y, Random.Range(phase1Start_R.transform.position.z, Vector3.Lerp(phase1Start_R.transform.position, phase1End_R.transform.position, 0.5f).z));
        phase1SpeedChangeZones_R[1].transform.position = new Vector3(phase1Start_R.transform.position.x, phase1Start_R.transform.position.y, Random.Range(Vector3.Lerp(phase1Start_R.transform.position, phase1End_R.transform.position, 0.5f).z, phase1End_R.transform.position.z));

        phase2SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x, phase2Start_L.transform.position.y, Random.Range(envManager.phase2Start_L.transform.position.z, Vector3.Lerp(envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z));
        phase2SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x, phase2Start_L.transform.position.y, Random.Range(Vector3.Lerp(envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z, envManager.phase2End_L.transform.position.z));

        phase2SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x, phase2Start_R.transform.position.y, Random.Range(envManager.phase2Start_R.transform.position.z, Vector3.Lerp(envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z));
        phase2SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x, phase2Start_R.transform.position.y, Random.Range(Vector3.Lerp(envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z, envManager.phase2End_R.transform.position.z));

        phase3SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase3Start_L.transform.position.x, phase3Start_L.transform.position.y, Random.Range(envManager.phase3Start_L.transform.position.z, Vector3.Lerp(envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f).z));
        phase3SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase3Start_L.transform.position.x, phase3Start_L.transform.position.y, Random.Range(Vector3.Lerp(envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f).z, envManager.phase3End_L.transform.position.z));

        phase3SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase3Start_R.transform.position.x, phase3Start_R.transform.position.y, Random.Range(envManager.phase3Start_R.transform.position.z, Vector3.Lerp(envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f).z));
        phase3SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase3Start_R.transform.position.x, phase3Start_R.transform.position.y, Random.Range(Vector3.Lerp(envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f).z, envManager.phase3End_R.transform.position.z));

    }

    List<int> ShuffleReevalConditions()
    {
        List<int> tempList = new List<int>();
        for (int i = 0; i < environments.Count; i++)
        {
            tempList.Add(i);
        }
        tempList = ShuffleList(tempList);
        for (int i = 0; i < tempList.Count; i++)
        {
            Debug.Log("reeval condition: " + tempList[i].ToString());
        }
        return tempList;
    }

    List<float> GetRandomNumbers(int randomCount)
    {
        System.Random rand = new System.Random();
        List<float> randList = new List<float>();
        for (int i = 0; i < randomCount; i++)
        {
            int randInt = rand.Next(50, 80);
            Debug.Log("rand int is " + randInt.ToString());
            float nextDouble = (float)randInt / 100f;
            Debug.Log("next double is  " + nextDouble.ToString());
            //float nextDouble = (float)rand.NextDouble();
            nextDouble = Mathf.Clamp(nextDouble, 0.5f, 0.8f);
            randList.Add((float)(nextDouble));
            Debug.Log("cam zone factor: " + randList[i]);
        }
        return randList;
    }

    IEnumerator RandomizeCameraZones(int blockCount)
    {
        float randFactor = camZoneFactors[blockCount];

        Experiment.Instance.shopLiftLog.LogCameraLerpIndex(randFactor, blockCount);

        phase1CamZone_L.transform.position = new Vector3(envManager.phase1Start_L.transform.position.x, envManager.phase1Start_L.transform.position.y, Mathf.Lerp(envManager.phase1Start_L.transform.position.z, envManager.phase1End_L.transform.position.z, randFactor));
        phase1CamZone_R.transform.position = new Vector3(envManager.phase1Start_R.transform.position.x, envManager.phase1Start_R.transform.position.y, Mathf.Lerp(envManager.phase1Start_R.transform.position.z, envManager.phase1End_R.transform.position.z, randFactor));


        phase2CamZone_L.transform.position = new Vector3(envManager.phase2Start_L.transform.position.x, envManager.phase2Start_L.transform.position.y, Mathf.Lerp(envManager.phase2Start_L.transform.position.z, envManager.phase2End_L.transform.position.z, randFactor));
        phase2CamZone_R.transform.position = new Vector3(envManager.phase2Start_R.transform.position.x, envManager.phase2Start_R.transform.position.y, Mathf.Lerp(envManager.phase2Start_R.transform.position.z, envManager.phase2End_R.transform.position.z, randFactor));

        phase3CamZone_L.transform.position = new Vector3(envManager.phase3Start_L.transform.position.x, envManager.phase3Start_L.transform.position.y, Mathf.Lerp(envManager.phase3Start_L.transform.position.z, envManager.phase3End_L.transform.position.z, randFactor));
        phase3CamZone_R.transform.position = new Vector3(envManager.phase3Start_R.transform.position.x, envManager.phase3Start_R.transform.position.y, Mathf.Lerp(envManager.phase3Start_R.transform.position.z, envManager.phase3End_R.transform.position.z, randFactor));

        if (directionEnv == 1)
        { //is western town
          // Debug.Log("turned cam zones");
            phase1CamZone_L.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            phase1CamZone_R.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            phase2CamZone_L.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            phase2CamZone_R.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            phase3CamZone_L.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            phase3CamZone_R.transform.eulerAngles = new Vector3(0f, 180f, 0f);

        }
        ResetCamZone();
        phase1CamZone_L.SetActive(false);
        phase1CamZone_R.SetActive(false);
        phase2CamZone_L.SetActive(false);
        phase2CamZone_R.SetActive(false);
        phase3CamZone_L.SetActive(false);
        phase3CamZone_R.SetActive(false);
        yield return null;
    }

    public void ChangeCamZoneFocus(int camIndex)
    {
        Debug.Log("changing cam zone focus to " + camIndex.ToString());
        if (activeCamZone != null)
        {
            activeCamZone.GetComponent<CameraZone>().isFocus = false;
            activeCamZone.SetActive(false);
        }
        switch (camIndex)
        {
            case 0:
                phase1CamZone_L.SetActive(true);
                activeCamZone = phase1CamZone_L;
                break;
            case 1:
                phase2CamZone_L.SetActive(true);
                activeCamZone = phase2CamZone_L;
                break;
            case 2:
                phase3CamZone_L.SetActive(true);
                activeCamZone = phase3CamZone_L;
                break;
            case 3:
                phase1CamZone_R.SetActive(true);
                activeCamZone = phase1CamZone_R;
                break;
            case 4:
                phase2CamZone_R.SetActive(true);
                activeCamZone = phase2CamZone_R;
                break;
            case 5:
                phase3CamZone_R.SetActive(true);
                activeCamZone = phase3CamZone_R;
                break;
            default:
                phase1CamZone_L.SetActive(true);
                activeCamZone = phase1CamZone_L;
                break;
        }
        activeCamZone.GetComponent<CameraZone>().isFocus = true;

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
    void AssignRooms(bool focusLeft, bool isTraining)
    {
        leftRegisterObj = envManager.leftRegisterObj;
        rightRegisterObj = envManager.rightRegisterObj;
        if (!Config.isDayThree && !isTraining)
        {

            if (Random.value < 0.5f)
            {
                leftRoom = roomOne;
                three_L_Audio = envManager.three_L_Audio;
                leftRoomColor = roomOneColor;
                rightRoom = roomTwo;
                three_R_Audio = envManager.three_R_Audio;
                rightRoomColor = roomTwoColor;


                Experiment.Instance.shopLiftLog.LogRooms(roomOne.name, roomTwo.name);
            }
            else
            {
                leftRoom = roomTwo;
                three_L_Audio = envManager.three_R_Audio;
                leftRoomColor = roomTwoColor;

                rightRoom = roomOne;
                three_R_Audio = envManager.three_L_Audio;
                rightRoomColor = roomOneColor;


                Experiment.Instance.shopLiftLog.LogRooms(roomTwo.name, roomOne.name);
            }
        }
        else
        {
            if (focusLeft)
            {
                Debug.Log("FOCUSING LEFT");
                leftRoom = roomOne;
                three_L_Audio = envManager.three_L_Audio;
                leftRoomColor = roomOneColor;
                rightRoom = roomTwo;
                three_R_Audio = envManager.three_R_Audio;
                rightRoomColor = roomTwoColor;


                Experiment.Instance.shopLiftLog.LogRooms(roomOne.name, roomTwo.name);
            }
            else
            {
                leftRoom = roomTwo;
                three_L_Audio = envManager.three_R_Audio;
                leftRoomColor = roomTwoColor;

                rightRoom = roomOne;
                three_R_Audio = envManager.three_L_Audio;
                rightRoomColor = roomOneColor;


                Experiment.Instance.shopLiftLog.LogRooms(roomTwo.name, roomOne.name);
            }

        }


        leftRoom.transform.localPosition = envManager.leftRoomTransform.localPosition;
        rightRoom.transform.localPosition = envManager.rightRoomTransform.localPosition;
        Debug.Log("set " + leftRoom.gameObject.name + " as left and " + rightRoom.gameObject.name + " as right");
    }

    void ResetCamZone()
    {

        phase1CamZone_L.GetComponent<CameraZone>().Reset();
        phase2CamZone_L.GetComponent<CameraZone>().Reset();
        phase3CamZone_L.GetComponent<CameraZone>().Reset();
        phase1CamZone_R.GetComponent<CameraZone>().Reset();
        phase2CamZone_R.GetComponent<CameraZone>().Reset();
        phase3CamZone_R.GetComponent<CameraZone>().Reset();
    }

    void ChangeCameraZoneVisibility(bool isVisible)
    {
        Debug.Log("changing cam zone visibility to " + isVisible.ToString());
        phase1CamZone_L.GetComponent<Renderer>().enabled = isVisible;
        phase1CamZone_R.GetComponent<Renderer>().enabled = isVisible;
        phase2CamZone_L.GetComponent<Renderer>().enabled = isVisible;
        phase2CamZone_R.GetComponent<Renderer>().enabled = isVisible;
        phase3CamZone_L.GetComponent<Renderer>().enabled = isVisible;
        phase3CamZone_R.GetComponent<Renderer>().enabled = isVisible;
    }

    void ChangeColors(Color newColor)
    {
        //		for (int i = 0; i < phase2Walls.Length; i++) {
        //			Debug.Log ("new color is:  " + newColor.ToString ());
        //			phase2Walls [i].GetComponent<Renderer> ().material.color = newColor;
        //		}
    }

    bool isPauseButtonPressed = false;
    void GetPauseInput()
    {

        if(Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Pause Button"))
        {//.GetAxis(Input.GetKeyDown(KeyCode.B) || Input.GetKey(KeyCode.JoystickButton2)){ //B JOYSTICK BUTTON TODO: move to input manager.
            Debug.Log("PAUSE BUTTON PRESSED");
            if (!isPauseButtonPressed)
            {
                isPauseButtonPressed = true;
                Debug.Log("PAUSE OR UNPAUSE");
                TogglePause(); //pause
            }
        }
        else
        {
            isPauseButtonPressed = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Experiment.Instance.shopLiftLog.LogPauseEvent(isPaused);

        if (isPaused)
        {
            //exp.player.controls.Pause(true);
            pauseUI.alpha = 1.0f;
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
            //exp.player.controls.Pause(false);
            //exp.player.controls.ShouldLockControls = false;
            pauseUI.alpha = 0.0f;
        }
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

			Experiment.Instance.shopLiftLog.LogRooms (roomOne.name,roomTwo.name);

        }
        else
        {
			leftRoom = roomTwo;
			leftRoomColor = roomTwoColor;
			three_L_Audio = envManager.three_R_Audio;


			rightRoom = roomOne;
			rightRoomColor = roomOneColor;
			three_R_Audio = envManager.three_L_Audio;

			Experiment.Instance.shopLiftLog.LogRooms  (roomTwo.name,roomOne.name);
        }

		GameObject tempSuitcase=null;
		tempSuitcase = leftSuitcase;
		leftSuitcase = rightSuitcase;
		rightSuitcase = tempSuitcase;
		leftRoom.transform.position = envManager.leftRoomTransform.position;
		rightRoom.transform.position = envManager.rightRoomTransform.position;
    }

	public List<int> ShuffleList(List<int> vals)
	{
		List<int> valList = new List<int> ();
		int valCount = vals.Count;
		for(int i=0;i<valCount;i++)
		{
			valList.Add (vals [i]);
		}
		vals.Clear ();
		int valListCount = valList.Count;
		for (int i = 0; i < valListCount; i++) {
			int randomIndex = Random.Range (0, valList.Count);
			vals.Add(valList [randomIndex]);
			valList.RemoveAt (randomIndex);
		}

		return vals;
	}

    IEnumerator PlayInstructionVideo(bool playVideo)
    {
        //inst video
        if (playVideo)
        {
            Debug.Log("set video");
            instructionVideo.SetActive(true);
            Experiment.Instance.shopLiftLog.LogInstructionVideoEvent(true);
            float timer = 0f;
            instructionVideo.GetComponent<VideoPlayer>().Prepare();
            while (!instructionVideo.GetComponent<VideoPlayer>().isPrepared)
            {
                yield return 0;
            }
            float maxTimer = (float)instructionVideo.GetComponent<VideoPlayer>().clip.length;
            Debug.Log("the max timer is : " + maxTimer.ToString());
            TCPServer.Instance.SetState(TCP_Config.DefineStates.INST_VIDEO, true);
            instructionVideo.GetComponent<VideoPlayer>().Play();
            //instructionVideo.gameObject.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.3f);
            Debug.Log("enabled player cam");
            blackScreen.alpha = 0f;
            EnablePlayerCam(true);
            while (!Input.GetButtonDown("Skip Button") && timer < maxTimer)
            {
                timer += Time.deltaTime;
                yield return 0;
            }

            TCPServer.Instance.SetState(TCP_Config.DefineStates.INST_VIDEO, false);
            instructionVideo.GetComponent<VideoPlayer>().Stop();
            instructionVideo.SetActive(false);

            Experiment.Instance.shopLiftLog.LogInstructionVideoEvent(false);
        }
        else
        {
            blackScreen.alpha = 0f;
            EnablePlayerCam(true);
        }

        yield return null;
    }

    void RandomizeTrainingRewards()
    {
        int leftReward = 0;
        int rightReward = 0;
        //get randomized rewards
        while (leftReward == rightReward)
        {
            leftReward = Mathf.CeilToInt(Random.Range(10f, 90f));
            rightReward = Mathf.CeilToInt(Random.Range(10f, 90f));
        }
        //then add the rewards to a list
        trainingReward = new int[2];
        trainingReward[0] = leftReward;
        trainingReward[1] = rightReward;
    }

    IEnumerator RunSliderTrainingPhase()
    {
        
    

        cameraZoneManager.ToggleAllCamZones(false); //turn off all cameras

        //comparative sliders
        for (int i = 0; i < 3; i++)
        {
            RandomizeTrainingRewards();
            bool isLeft = true;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true));
            //now run the other corridor
            isLeft = !isLeft;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true));
            yield return StartCoroutine(AskPreference(2, false, false, true, 0, 0f));
        }

        //solo sliders
        for (int i = 0; i < 3; i++)
        {
            RandomizeTrainingRewards();
            bool isLeft = true;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true));
            //now run the other corridor
            isLeft = !isLeft;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true));
            //we will randomly pick on whether to query the left or right room
            yield return StartCoroutine(AskSoloPreference((Random.value > 0.5f)? 2 : 3, true)); // we have assigned Room 5 (left) and Room 6 (right) as 2 and 3 index in the solo img groups
         }
        yield return null;
    }

    IEnumerator RunMultipleChoiceTrainingPhase()
    {
        yield return null;
    }


	IEnumerator RunCamTrainingPhase()
	{
        //blackScreen.alpha = 1f;
        cameraZoneManager.ToggleAllCamZones(true); //temporarily turn on all cameras
        RandomizeSpeedChangeZones ();
		Debug.Log ("starting cam training phase");
		CameraZone.isTraining = true;
        Experiment.Instance.shopLiftLog.LogTextInstructions(1,true);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.INSTRUCTIONS, true);
        introInstructionGroup.alpha = 1f;
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine (WaitForButtonPress (10000f,didPress =>
			{
				Debug.Log("did press: " + didPress);
			}
		));
		introInstructionGroup.alpha = 0f;
        
        Experiment.Instance.shopLiftLog.LogTextInstructions(1,true);
        Experiment.Instance.shopLiftLog.LogTextInstructions(2, true);
        trainingInstructionsGroup.alpha = 1f;
		yield return StartCoroutine(WaitForButtonPress (10000f,didPress =>
			{
				Debug.Log("did press: " + didPress);
			}
		));
		trainingInstructionsGroup.alpha = 0f;

        Experiment.Instance.shopLiftLog.LogTextInstructions(2, false);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.INSTRUCTIONS, false);

        //training begins here
        TCPServer.Instance.SetState(TCP_Config.DefineStates.TRAINING, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("TRAINING", true);

		trainingPeriodGroup.alpha = 1f;
		bool isLeft = false;
		int numTraining = 0;
		while (numTraining < 4) {

            //check correct responses and reset if it is less than 3
            if (numTraining % 2 == 0)
            {
                if (correctResponses < 3)
                {
                    correctResponses = 0;
                }
            }
			Debug.Log ("about to run phase 1");
			isLeft = !isLeft;
			yield return StartCoroutine (RunPhaseOne ((isLeft) ? 0:1,false,-1,false));

			Debug.Log ("about to run phase 2");
			yield return StartCoroutine (RunPhaseTwo ((isLeft) ? 0:1,false,false,-1,false,false));
//			TurnOffRooms ();
			Debug.Log("about to run phase 3");
			yield return StartCoroutine(RunPhaseThree((isLeft) ? 0:1,false,false));
            if (numTraining < 3)
                yield return StartCoroutine(ShowEndTrialScreen(true, ShouldShowTips()));
            else
                yield return StartCoroutine(ShowEndTrialScreen(false, ShouldShowTips()));
			numTraining++;
			yield return 0;
		}
//		ResetCamZone ();
		CameraZone.isTraining = false;
        //make sure the cameras don't appear outside of training zone
        CameraZone.firstTime = false;

        TCPServer.Instance.SetState(TCP_Config.DefineStates.TRAINING, false);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("TRAINING", false);
		trainingPeriodGroup.alpha = 0f;
		yield return null;
	}

	//adapted from https://bitbucket.org/Superbest/superbest-random
	float NextGaussian(System.Random r, double mu = 0,double sigma=1)
	{
		var u1 = r.NextDouble();
		var u2 = r.NextDouble();

		var rand_std_normal = System.Math.Sqrt(-2.0 *  System.Math.Log(u1)) *
			System.Math.Sin(2.0 *  System.Math.PI * u2);

		float rand_normal =(float) (mu + sigma * rand_std_normal);

		return rand_normal;
	}

	List<int> GiveRandSequenceOfTwoInts(int int1,int int2,int seqLength)
	{
		List<int> tempList = new List<int> ();
		for (int i = 0; i < seqLength/2; i++) {
			tempList.Add (int1);
			tempList.Add (int2);
		}
		tempList=ShuffleList (tempList);
		Debug.Log ("contents of templist are");
		for(int i=0;i<tempList.Count;i++)
		{
			Debug.Log (tempList [i]);
		}
		return tempList;
	}


    IEnumerator PickRegisterValues()
    {
		registerLeft = new List<int> ();
		registerVals = new List<int> ();
		int index = Random.Range(0,registerVal1.Count);
		registerVals.Add(registerVal1[index]);
		registerVals.Add(registerVal2[index]);

		Experiment.Instance.shopLiftLog.LogRegisterValues (registerVal1[index]);
		Experiment.Instance.shopLiftLog.LogRegisterValues (registerVal2[index]);

		registerVal1.RemoveAt (index);
		registerVal2.RemoveAt (index);

		Debug.Log ("register val at 0 is: " + registerVals [0].ToString ());
		Debug.Log ("register val at 1 is: " + registerVals [1].ToString ());
        yield return null;
    }

	IEnumerator RunPhaseOne(int pathIndex, bool isGuided, int guidedChoice, bool terminateWithChoice)
	{
		Experiment.Instance.shopLiftLog.LogPathIndex (pathIndex);
		EnablePlayerCam (true);

		ChangeCamZoneFocus ((pathIndex == 0) ? 0 : 3);
		GameObject targetDoor = (pathIndex==0) ? phase1Door_L : phase1Door_R;
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		clearCameraZoneFlags = false;
		Debug.Log ("running phase one");
		AudioSource baseAudio = (pathIndex == 0) ? one_L_Audio : one_R_Audio;
		float delayOne = Random.Range (0f, baseAudio.clip.length);

		baseAudio.time = delayOne;
		baseAudio.Play ();

        //if number of correct responses are greater than 3, then don't show camera in the next practice round
        if (correctResponses > 3)
        {
            Debug.Log("correct response is eq or above 3");
            CameraZone.firstTime = false;
            //ChangeCameraZoneVisibility(false); //doesn't actually turn off the gameobject, which is why we need to turn them off using the above line
        }

		ChangeCamZoneFocus((pathIndex==0) ? 0:3);
		ToggleMouseLook(false);

		Vector3 startPos = (pathIndex == 0) ? phase1Start_L.transform.position : phase1Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase1End_L.transform.position : phase1End_R.transform.position;
		camVehicle.transform.position = startPos;
		Debug.Log ("start pos " + startPos.ToString ());
		camVehicle.SetActive (true);
		Experiment.Instance.shopLiftLog.LogMoveEvent (1,true);
		yield return StartCoroutine(VelocityPlayerTo (startPos,endPos, phase1Factor));

		Experiment.Instance.shopLiftLog.LogMoveEvent (1,false);

		clearCameraZoneFlags = true;
		if(activeCamZone!=null)
			activeCamZone.GetComponent<CameraZone> ().isFocus = false;
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
					Debug.Log ("phase 1 door L: " + phase1Door_L.transform.GetChild (0).gameObject.name);
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
					Debug.Log ("phase 1 door R: " + phase1Door_R.transform.GetChild (0).gameObject.name);
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
		if (!terminateWithChoice) {
			targetDoor.GetComponent<Doors> ().Close ();
		}
		yield return null;
	}

	IEnumerator RunPhaseTwo(int pathIndex,bool isDirect, bool isGuided, int guidedChoice, bool terminateWithChoice,bool hasRewards)
	{
		EnablePlayerCam (true);
		ChangeCamZoneFocus ((pathIndex == 0) ? 1 : 4);
		GameObject targetDoor = (pathIndex==0) ? phase2Door_L : phase2Door_R;
		Vector3 startPos = (pathIndex == 0) ? phase2Start_L.transform.position : phase2Start_R.transform.position;
		Vector3 endPos = (pathIndex == 0) ? phase2End_L.transform.position : phase2End_R.transform.position;
		if (isDirect) {
			currentAudio = (pathIndex == 0) ? two_L_Audio : two_R_Audio;
			float delay = Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delay;
			currentAudio.Play ();
            //we set this explicitly for re-evaluation as RunPhaseOne isn't run anymore so we have to log it here
            Experiment.Instance.shopLiftLog.LogMoveEvent(2, true);

        }
		clearCameraZoneFlags = false;
		Debug.Log ("running phase two");
			camVehicle.transform.position = startPos;
			Debug.Log("velo player in phase 2");
		yield return StartCoroutine(VelocityPlayerTo(startPos,endPos,phase1Factor));
			Experiment.Instance.shopLiftLog.LogMoveEvent (2, false);

		clearCameraZoneFlags = true;
		float delayThree = 0f;
		if(activeCamZone!=null)
			activeCamZone.GetComponent<CameraZone> ().isFocus = false;
		yield return StartCoroutine (WaitForDoorOpenPress (doorText));
		if (hasRewards) {

            SpawnSuitcase(pathIndex); //we spawn suitcase here for both learning and relearning phase
		}
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
        if (isDirect)
        {
            EnablePlayerCam(true);
            SpawnSuitcase(pathIndex); //this will only run for slider training phase where we are directly spawned into the last room in the corridor
        }
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
		clearCameraZoneFlags = true;
		if (hasRewards) {
			if (pathIndex == 0) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, register_L.transform.position, 0.5f));

			} else if (pathIndex == 1) {
				yield return StartCoroutine (MovePlayerTo (camVehicle.transform.position, register_R.transform.position, 0.5f));
			}
			Experiment.Instance.shopLiftLog.LogWaitEvent ("REGISTER", true);
			if(activeCamZone!=null)
				activeCamZone.GetComponent<CameraZone> ().isFocus = false;
			yield return StartCoroutine (WaitForDoorOpenPress (registerText));
			Experiment.Instance.shopLiftLog.LogWaitEvent ("REGISTER", false);
			yield return StartCoroutine (ShowRegisterReward (pathIndex,isDirect));
			Debug.Log ("closing the third door now");
		}
		currentAudio.Stop ();

		yield return null;
	}

    void SpawnSuitcase(int pathIndex)
    {
        suitcaseObj = Instantiate((pathIndex == 0) ? leftSuitcase : rightSuitcase, ((pathIndex == 0) ? register_L.transform.position : register_R.transform.position) + (new Vector3(0f, 0.35f, directionEnv) * 2f), Quaternion.identity) as GameObject;

        if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
        {
            Debug.Log("NOT WESTERN TOWN");
            suitcaseObj.transform.eulerAngles = (System.Math.Abs(directionEnv - 1f) < double.Epsilon) ? new Vector3(-90f, 0f, 0f) : new Vector3(-90f, 0f, 180f);
            //				suitcaseObj = suitcaseObj.transform.GetChild (0).gameObject;
        }
        else if (ExperimentSettings.env == ExperimentSettings.Environment.Office)
        {
            suitcaseObj.transform.eulerAngles = new Vector3(0f, 90f, 0f);

        }

        else if (ExperimentSettings.env == ExperimentSettings.Environment.VikingVillage)
        {
            suitcaseObj.transform.position = suitcaseObj.transform.position + new Vector3(0f, -1f, 4f);
        }
    }

    public bool ShouldShowTips()
	{
		if (consecutiveIncorrectCameraPresses >= 4 || didTimeout || afterSlider) {
			afterSlider = false;
			return true;
		} else
			return false;
		
	}




	IEnumerator RunLearningPhase(bool isPostTest, int maxTrials)
	{
		Debug.Log("running task");
        CameraZone.firstTime = false;
        int sliderCount = 0;
		if (!isPostTest) {
			//ChangeCameraZoneVisibility (false); // no need to show cam zones as they were already shown during training
			//stage 1
			Experiment.Instance.shopLiftLog.LogPhaseEvent ("LEARNING", true);

		} else {
			numTrials_Learning = 0;
			maxTrials = maxTrials;
			Experiment.Instance.shopLiftLog.LogPhaseEvent ("POST-TEST", true);
		}
		bool isLeft = (Random.value < 0.5f) ? true: false;
		bool showOneTwo = false;


        //current list of slider-trials during learning phase are 3,7,11,15,19,23

        int trialsToNextSlider = 4; //should be 4 in the beginning
		List<int> randOrder = new List<int>();
		int randIndex = 0;
		randOrder = GiveRandSequenceOfTwoInts(0,1,trialsToNextSlider);

        //		while(numTrials < 1)
        bool showOnce = true;
        int maxDeviationQueueLength = 2;
        float deviationThreshold = 0.4f;

        while(numTrials_Learning < maxTrials || (!isPostTest && !hasLearned && numAdditionalTrials < maxAdditionalTrials))
		{ 
			Debug.Log ("about to run phase 1");
			if (randOrder [0] == 0)
				isLeft = true;
			else
				isLeft = false;
			randOrder.RemoveAt (0);
			yield return StartCoroutine (RunPhaseOne ((isLeft) ? 0 : 1, false, -1,false));

			Debug.Log ("about to run phase 2");

			yield return StartCoroutine (RunPhaseTwo((isLeft) ? 0 : 1,false,false,-1,true,true));

			Debug.Log("about to run phase 3");
            if(!isPostTest)
			    yield return StartCoroutine(RunPhaseThree((isLeft) ? 0:1,false,true));
            else
                yield return StartCoroutine(RunPhaseThree((isLeft)?0:1,false,false));
            //			TurnOffRooms ();
            Debug.Log("num trials learning " + numTrials_Learning.ToString());
            if((numTrials_Learning+1)%4==0 && numTrials_Learning >0)
            {
				showOneTwo = !showOneTwo;
					if (showOneTwo) {
                    yield return StartCoroutine (AskPreference (0,false,(!isPostTest)? true : false, false, maxDeviationQueueLength, deviationThreshold));
					} else
                    yield return StartCoroutine (AskPreference (1,false,(!isPostTest) ? true:false, false, maxDeviationQueueLength, deviationThreshold));
					randOrder.Clear ();
					trialsToNextSlider = 4;
                Debug.Log("got new order");
					randOrder = GiveRandSequenceOfTwoInts (0, 1, trialsToNextSlider);
			}


            if (numTrials_Learning >= maxTrials && !hasLearned && showOnce)
            {
                intertrialGroup.alpha = 1f;
                maxDeviationQueueLength = 1;
                deviationThreshold = 0.35f;
                intertrialText.text = "Por favor, tenga en cuenta la estructura de las habitaciones y las recompensas al responder";
                intertrialText.text = "";
                intertrialGroup.alpha = 0f;
                showOnce = false;
            } 

            if (numTrials_Learning < maxTrials - 1 || (!hasLearned && numAdditionalTrials < maxAdditionalTrials-1))
				yield return StartCoroutine (ShowEndTrialScreen (false,ShouldShowTips()));
			else if(!isPostTest)
				yield return StartCoroutine (ShowNextStageScreen ());
			numTrials_Learning++;

            //this indicates that we are in additional_learning phase until the subject has learned the structure
            if(numTrials_Learning >=maxTrials)
            {
                numAdditionalTrials++;
            }
			yield return 0;
		}

		camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled=false;
        if (!isPostTest)
        {
            Experiment.Instance.shopLiftLog.LogPhaseEvent("LEARNING", false);
        }
        else
        {
            Experiment.Instance.shopLiftLog.LogPhaseEvent("POST-TEST", false);
        }
		yield return null;
	}

	IEnumerator RunReevaluationPhase(int reevalConditionIndex)
	{
		int numTrials_Reeval = 0;
		int numBlocks_Reeval = 0;
		Debug.Log("about to start Re-Evaluation Phase");
		stageIndex = 2;
		bool leftChoice = false;

        //do revaluation only if they have learned; otherwise it will be control
        if (hasLearned)
        {
            switch (reevalConditionIndex)
            {
                case 0:
                    Debug.Log("IT'S RR");
                    SetupRewardReeval();
                    break;
                case 1:
                    Debug.Log("IT'S TR");
                    SetupTransitionReeval();
                    break;
            }
        }
        else
        {
            Debug.Log("hasn't learnt so this will be CONTROL");
        }
		Experiment.Instance.shopLiftLog.LogPhaseEvent("RE-EVALUATION",true);
		while (numBlocks_Reeval < maxBlocks_Reeval) {
			while (numTrials_Reeval < maxTrials_Reeval) {
				leftChoice = !leftChoice; //flip it

				Debug.Log ("about to run phase 2");
				yield return StartCoroutine (RunPhaseTwo((leftChoice) ? 0 : 1,true,false,-1,true,true));

				Debug.Log("about to run phase 3");
				yield return StartCoroutine(RunPhaseThree((leftChoice) ? 0:1,false,true));
//				TurnOffRooms ();
				if(numTrials_Reeval < maxTrials_Reeval-1)
					yield return StartCoroutine (ShowEndTrialScreen (false,ShouldShowTips()));
				numTrials_Reeval++;
				yield return 0;
			}
			yield return StartCoroutine (AskPreference (1,false,false,false,0,0f));
//			yield return StartCoroutine (RunRestPeriod());
			numTrials_Reeval = 0;
			numBlocks_Reeval++;
			yield return 0;
		}
		yield return StartCoroutine (ShowNextStageScreen ());
		Experiment.Instance.shopLiftLog.LogPhaseEvent("RE-EVALUATION",false);
		yield return null;
	}


    //part of the baseline, traverses through all the environments without any audio or chest
    IEnumerator RunSilentTraversal()
    {

        TCPServer.Instance.SetState(TCP_Config.DefineStates.SILENT_TRAVERSAL, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("SILENT_TRAVERSAL", true);

        bool isLeft = (Random.value >0.5f) ?  true : false;
        for (int i = 0; i < environments.Count;i++)
        {

            yield return StartCoroutine(PickEnvironment(i,false)); //change environment
            mainSceneListener.enabled = false; //turn off the main audio listener
            AudioListener.pause = true;
            AudioListener.volume = 0f;
            for (int j = 0; j < 2; j++)
            {   
                Debug.Log("about to run phase 1");
                isLeft = !isLeft; //flip the left right
                yield return StartCoroutine(RunPhaseOne((isLeft) ? 0 : 1, false, -1, false));

                Debug.Log("about to run phase 2");
                yield return StartCoroutine(RunPhaseTwo((isLeft) ? 0 : 1, false, false, -1, false, false));
                //          TurnOffRooms ();
                Debug.Log("about to run phase 3");
                yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, false, false));
            }
        }

        mainSceneListener.enabled = true;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.SILENT_TRAVERSAL, false);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("SILENT_TRAVERSAL", false);
        //AudioListener.pause = true;
        //AudioListener.volume = 0f;

        yield return null;
    }
	IEnumerator RunRestPeriod(float waitTime)
	{
		restGroup.alpha = 1f;
		EnablePlayerCam (false);
		yield return new WaitForSeconds (waitTime);
		restGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator RunImaginePeriod(float waitTime)
	{
		dotGroup.alpha = 1f;
		EnablePlayerCam (false);
		yield return new WaitForSeconds (waitTime);
		dotGroup.alpha = 0f;
		yield return null;
	}
	IEnumerator RunTestingPhase()
	{

		Experiment.Instance.shopLiftLog.LogPhaseEvent ("TESTING", true);
		//run one instances of comp slider  + 2sec resting phase
			yield return StartCoroutine (AskPreference (0,false,false,false,0,0f));
		yield return StartCoroutine (RunRestPeriod (2f));
        int caseOrder = 0;
        if (Random.value > 0.5f)
            caseOrder = 1;
        else
            caseOrder = 0;

		for (int i = 0; i < 4; i++) {
            //another instance of comp 1-2 slider
            if (i ==1  || i==3)
            {
                yield return StartCoroutine(AskPreference(0, false,false,false,0,0f));
                yield return StartCoroutine(RunRestPeriod(2f));
            }
            caseOrder = Mathf.Abs(caseOrder - 1);
			imagineGroup.alpha = 1f;
			yield return new WaitForSeconds (8f);
			imagineGroup.alpha = 0f;
            switch (caseOrder) {
			case 0:
				yield return StartCoroutine (RunPhaseOne (0, false, -1, true));
				yield return StartCoroutine (RunImaginePeriod (5f));
				yield return StartCoroutine (AskSoloPreference (0,false));
//				yield return StartCoroutine (AskImageryQualityRating (0));
				yield return StartCoroutine (RunRestPeriod (2f));
				break;
			case 1:
				yield return StartCoroutine(RunPhaseOne (1, false, -1, true));
				yield return StartCoroutine (RunImaginePeriod (5f));
				yield return StartCoroutine (AskSoloPreference (1,false));
//				yield return StartCoroutine (AskImageryQualityRating (1));
				yield return StartCoroutine (RunRestPeriod (2f));
				break;
			}
            yield return StartCoroutine(ShowInstructionScreen(pressToContinueInstruction, true, false, 10f));
		}

		//another instance of comp 1-2 slider
		yield return StartCoroutine (AskPreference (0,false,false, false,0,0f));
		yield return StartCoroutine (RunRestPeriod (2f));

		List<int> multipleChoiceSequence = new List<int> ();
		for (int i = 0; i < 4; i++) {
			multipleChoiceSequence.Add (i);
			
		}
		multipleChoiceSequence = ShuffleList (multipleChoiceSequence);
		for (int i = 0; i < 4; i++) {
			int randIndex = Random.Range (0, multipleChoiceSequence.Count);
            yield return StartCoroutine(AskMultipleChoice(multipleChoiceSequence[randIndex]));
			multipleChoiceSequence.RemoveAt (randIndex);
            yield return StartCoroutine(RunRestPeriod(3f));
		}
		Experiment.Instance.shopLiftLog.LogPhaseEvent ("TESTING", false);
		yield return null;
	}

    IEnumerator RunImageSlideshow()
    {
        //show image slideshow instructions
        TCPServer.Instance.SetState(TCP_Config.DefineStates.IMAGE_BASELINE, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("IMAGE_BASELINE", true);

        intertrialGroup.alpha = 1f;
        tipsGroup.alpha = 0f;
        intertrialText.text = imageSlideshowInstruction;
        yield return new WaitForSeconds(5f);
        intertrialGroup.alpha = 0f;

        int totalSlideshowLength = completeImageList.Count;
        for (int i = 0; i < totalSlideshowLength; i++)
        {
            slideshowImage.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

            int randIndex = Random.Range(0, completeImageList.Count);
            Experiment.Instance.shopLiftLog.LogBaselineImage(completeImageList[randIndex].name);
            slideshowImage.texture = completeImageList[randIndex];
            yield return new WaitForSeconds(imageSlideshowPlaytime);

            slideshowImage.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            completeImageList.RemoveAt(randIndex); //remove the image
            //run rest period in between the images
            yield return StartCoroutine(RunRestPeriod(2f));

        }

        TCPServer.Instance.SetState(TCP_Config.DefineStates.IMAGE_BASELINE, false);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("IMAGE_BASELINE", false);
        yield return null;
    }

	IEnumerator RunMusicBaseline()
	{
        //show music baseline instructions
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MUSIC_BASELINE, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("MUSIC_BASELINE", true);

        intertrialGroup.alpha = 1f;
        tipsGroup.alpha = 0f;
        intertrialText.text = musicBaselineInstruction;
        yield return new WaitForSeconds(5f);
        intertrialGroup.alpha = 0f;

            int totalAudioLength = completeAudioList.Count;
            for (int i = 0; i < totalAudioLength; i++)
            {
                restGroup.alpha = 1f;
                int randIndex = Random.Range(0, completeAudioList.Count);
                Debug.Log("now playing track no : " + randIndex.ToString());

            musicBaselinePlayer.clip = completeAudioList[randIndex];
            musicBaselinePlayer.Play();
            musicBaselinePlayer.gameObject.GetComponent<AudioLogTrack>().LogAudioClip(completeAudioList[randIndex]);
            //musicBaselinePlayer.PlayOneShot(completeAudioList[randIndex]);
                yield return new WaitForSeconds(musicBaselinePlayTime);
            musicBaselinePlayer.Stop();
            musicBaselinePlayer.gameObject.GetComponent<AudioLogTrack>().LogAudioStopped(completeAudioList[randIndex]);
            completeAudioList.RemoveAt(randIndex);
        }

        TCPServer.Instance.SetState(TCP_Config.DefineStates.MUSIC_BASELINE, false);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("MUSIC_BASELINE", false);

        yield return null;
	}

	void SetupTransitionReeval()
	{
		Experiment.Instance.shopLiftLog.LogTransitionReeval ();
		ReassignRooms ();
	}

	void SetupRewardReeval()
	{
		Experiment.Instance.shopLiftLog.LogRewardReeval ();
		Debug.Log ("count: " + registerVals.Count.ToString ());
		registerVals.Reverse ();
		Debug.Log ("count: " + registerVals.Count.ToString ());
		Debug.Log ("register vals at 0 : " + registerVals [0].ToString ());
		Debug.Log ("register vals at 1 : " + registerVals [1].ToString ());
	}

	void UpdateFirstEnvironments ()
	{
//		if (ExperimentSettings.env == ExperimentSettings.Environment.Cybercity) {
//			environments [0] = cybercityEnv;
//			environments [1] = spaceStationEnv;
//		} else {
//			environments [0] = spaceStationEnv;
//		}
	}

	IEnumerator AskImageryQualityRating(int prefIndex)
	{
		imageryQualityGroup.GetComponent<PrefSoloSetup> ().prefSlider.value = 0f;
		EnablePlayerCam (false);
		imageryQualityGroup.gameObject.SetActive (true);
		imageryQualityGroup.GetComponent<PrefSoloSetup> ().SetupPrefs (prefIndex);
		yield return StartCoroutine (WaitForButtonPress (15f,didPress =>
			{
				Debug.Log("did press: " + didPress);
			}
		));
		Experiment.Instance.shopLiftLog.LogFinalSliderValue ("IMAGERY_QUALITY", imageryQualityGroup.GetComponent<PrefSoloSetup> ().prefSlider.value,true);
		imageryQualityGroup.gameObject.SetActive (false);
		Cursor.visible = false;

		yield return null;
	}

	IEnumerator AskSoloPreference(int prefIndex,bool isTraining)
	{

        PrefSoloSetup prefSoloSetup = prefSolo.GetComponent<PrefSoloSetup>();

        prefSoloSetup.prefSlider.value = 0.5f;
//		Cursor.visible = true;
//		Cursor.lockState = CursorLockMode.None;
		EnablePlayerCam (false);
		prefSolo.gameObject.SetActive (true);
        prefSoloSetup.SetupPrefs (prefIndex);

        TCPServer.Instance.SetState(TCP_Config.DefineStates.SOLO_SLIDER, true);
		bool pressed = false;

        float tElapsed = 0f;
        float minSelectTime = 1.5f;

        while (tElapsed < minSelectTime)
        {
            tElapsed += Time.deltaTime;
            if (Input.GetButtonDown("Action Button"))
            {

                infoText.text = "Please take your time to make a choice";
                infoGroup.alpha = 1f;
            }
            yield return 0;
        }

        infoText.text = "";
        infoGroup.alpha = 0f;


        yield return StartCoroutine (WaitForButtonPress (9f,didPress =>
			{
				pressed=didPress;
			}
		));

        infoText.text = "Please make a choice";
        infoGroup.alpha = 1f;
        if(!pressed)
        {
            yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
            {
                pressed = didPress;
            }));
        }
        infoText.text = "";
        infoGroup.alpha = 0f;

        float finalSliderValue = prefSoloSetup.prefSlider.value;

        if (isTraining)
        {
            string focusImg = prefSoloSetup.focusImg.texture.name;

            bool isLeft = false;
            bool leftHigher = false;

            int leftRegisterReward = trainingReward[0];
            int rightRegisterReward = trainingReward[1];

            if (leftRegisterReward > rightRegisterReward)
                leftHigher = true;
            else
                leftHigher = false;

            Debug.Log("left higher " + leftHigher.ToString());

            bool rightSliderIsCorrect = false; //keeps track of whether moving the Solo Slider to the right is the correct response or not

            float deviation = 0f; //how much away from the correct answer was the player's response

            if (focusImg.Contains("Five")) //the focus image room was of a left corridor
            {
                isLeft = true;
                if(leftHigher)
                {
                    deviation = 1f - finalSliderValue;
                    rightSliderIsCorrect = true;
                }
                else
                {
                    deviation = finalSliderValue;
                    rightSliderIsCorrect = false;
                }
                
            }
            else //the focus image room was of a right corridor
            {
                isLeft = false;
                if (leftHigher)
                {
                    deviation = finalSliderValue;
                    rightSliderIsCorrect = false;
                }
                else
                {
                    deviation = 1f - finalSliderValue;
                    rightSliderIsCorrect = true;
                }
            }

            Debug.Log("deviation is " + deviation.ToString());
            Debug.Log("right slider is correct " + rightSliderIsCorrect.ToString());

            if (deviation > 0.5f)
            {
                StartCoroutine(prefSoloSetup.ShowIncorrectFeedback());
            }
            else
            {
                StartCoroutine(prefSoloSetup.ShowCorrectFeedback());
            }
            yield return new WaitForSeconds(1f);
            CanvasGroup assistiveSliderUI = null;
            assistiveSliderUI = prefSoloSetup.GetAssistiveSliderUI(rightSliderIsCorrect);

            //turn the assistive slider on
            assistiveSliderUI.alpha = 1f;
            if (leftHigher)
            {
                while (prefSoloSetup.prefSlider.value > 0.4f)
                {
                   
                    yield return 0;
                }
                Debug.Log("waiting for button press");
                yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                {
                    pressed = didPress;
                }));
            }
            else
            {
                while (1f - prefSoloSetup.prefSlider.value > 0.4f)
                {
                   
                    yield return 0;
                }
                Debug.Log("waiting for button press");
                yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                {
                    pressed = didPress;
                }));
            }




        }


        Experiment.Instance.shopLiftLog.LogFinalSliderValue ("SOLO", finalSliderValue, pressed);
		prefSolo.gameObject.SetActive (false);
		Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.SOLO_SLIDER, false);

        yield return null;
	}

	IEnumerator AskMultipleChoice(int prefIndex)
	{
        Debug.Log("PREF INDEX IS " + prefIndex.ToString());
		EnablePlayerCam (false);
		multipleChoiceGroup.gameObject.SetActive (true);
		multipleChoiceGroup.GetComponent<MultipleChoiceGroup> ().SetupMultipleChoice (prefIndex);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, true);
		bool pressed = false;
        float tElapsed = 0f;
        float minSelectTime = 2f;

        while (tElapsed < minSelectTime)
        {
            tElapsed += Time.deltaTime;
            if (Input.GetButtonDown("Action Button"))
            {

                infoText.text = "Por favor tómese su tiempo para hacer una elección";
                infoGroup.alpha = 1f;
            }
            yield return 0;
        }
        yield return StartCoroutine (WaitForButtonPress (10f,didPress =>
			{
				pressed=didPress;
			}
		));
        Debug.Log("about to ask them to make a choice");
        infoText.text = "Por favor, haga una elección";
        infoGroup.alpha = 1f;
        if (!pressed)
        {
            yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
            {
                pressed = didPress;
            }));
        }
        infoText.text = "";
        infoGroup.alpha = 0f;
        Experiment.Instance.shopLiftLog.LogMultipleChoiceResponse (multipleChoiceGroup.GetComponent<AnswerSelector> ().ReturnSelectorPosition(), pressed);
		multipleChoiceGroup.gameObject.SetActive (false);
		Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE,false);

        yield return null;
	}

	IEnumerator AskPreference(int prefType, bool allowTimeouts, bool isLearningPhase, bool isTraining, int maxDeviationQueueLength, float deviationThreshold)
	{
        //		Cursor.visible = true;
        //		Cursor.lockState = CursorLockMode.None;

        Debug.Log("left reward is " + trainingReward[0].ToString() + " and right reward is " + trainingReward[1].ToString());

        PrefGroupSetup prefGroupSetup = prefGroup.GetComponent<PrefGroupSetup>();


        prefGroupSetup.prefSlider.value = 0.5f;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.COMP_SLIDER, true);

        EnablePlayerCam (false);
		prefGroup.gameObject.SetActive (true);
		switch (prefType) {
		//between 1 and 2
		case 0:
                prefGroupSetup.SetupPrefs(0);
			break;
		//between 3 and 4
		case 1:
                prefGroupSetup.SetupPrefs (1);
			break;
        //between 5 and 6
        case 2:
                prefGroupSetup.SetupPrefs(2);
            break;

        }



		bool pressed = false;
            if(allowTimeouts)
            {
            yield return StartCoroutine(WaitForButtonPress(15f, didPress =>
               {
                   pressed = didPress;
               }
            ));
             }
            else
            {
            float tElapsed = 0f;
            float minSelectTime = 1.5f;

            while(tElapsed<minSelectTime)
            {
                tElapsed += Time.deltaTime;
                if(Input.GetButtonDown("Action Button"))
                {

                    infoText.text = "Por favor tómese su tiempo para hacer una elección";
                    infoGroup.alpha = 1f;
                }
                yield return 0;
            }
            //wait for them to select something on the slider
            while (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && Mathf.Abs(Input.GetAxis("Horizontal")) == 0f)
            {
                infoText.text = "Por favor, mueva el control deslizante para hacer una elección";
                infoGroup.alpha = 1f;
                yield return 0;
            }
            
                infoText.text = "";
                infoGroup.alpha = 0f;

                yield return StartCoroutine(WaitForButtonPress(9f, didPress =>
                {
                    pressed = didPress;
                }
            ));
                Debug.Log("about to ask them to make a choice");
            if (isTraining)
            {
                infoText.text = "Por favor, haga una elección";
                infoGroup.alpha = 1f;
            }
                if(!pressed)
                {
                    yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                    {
                        pressed = didPress;
                    }));
                }
                infoText.text = "";
                infoGroup.alpha = 0f;
            }

        float finalSliderValue = prefGroupSetup.prefSlider.value;

        if (isLearningPhase || isTraining)
        {
            string leftImgName = prefGroupSetup.leftImg.texture.name;
            string rightImgName = prefGroupSetup.rightImg.texture.name;

            bool leftHigher = false; //to indicate if the left IMAGE is of a higher reward value
                                     //left image is of a left-corridor room

            int leftRegisterReward = 0;
            int rightRegisterReward = 0;
            if(isTraining)
            {
                leftRegisterReward = trainingReward[0];
                rightRegisterReward = trainingReward[1];
            }
            else
            {
                leftRegisterReward = registerVals[0];
                rightRegisterReward = registerVals[1];
            }
            if (leftImgName.Contains("One") || leftImgName.Contains("Three") || leftImgName.Contains("Five"))
            {
                Debug.Log("left image is of a left corridor room");
                
                if (leftRegisterReward > rightRegisterReward)
                {
                    leftHigher = true;
                }
                else
                {
                    leftHigher = false;
                }
            }

            //left image is of a right-corridor room
            else
            {

                Debug.Log("left image is of a right corridor room");
                if (leftRegisterReward > rightRegisterReward)
                {
                    leftHigher = false;
                }
                else
                {
                    leftHigher = true;
                }
            }

            float deviation = 0f;


            //the slider should ideally be at 0.0
            if (leftHigher)
            {
                Debug.Log("left reward value is higher");
                deviation = finalSliderValue;
            }
            //the slider should ideally be at 1.0
            else
            {
                Debug.Log("right reward value is higher");
                deviation = 1f - finalSliderValue;
            }

            Debug.Log("deviation is " + deviation.ToString());

            if (isTraining)
            {
                if (deviation > 0.5f)
                {
                    StartCoroutine(prefGroupSetup.ShowIncorrectFeedback());
                }
                else
                {
                    StartCoroutine(prefGroupSetup.ShowCorrectFeedback());
                }
                yield return new WaitForSeconds(1f);
                CanvasGroup assistiveSliderUI=null;
                    assistiveSliderUI = prefGroupSetup.GetAssistiveSliderUI(leftHigher);

                    //turn the assistive slider on
                    assistiveSliderUI.alpha = 1f;
                    if (leftHigher)
                    {
                        while (prefGroupSetup.prefSlider.value > 0.4f)
                    {
                        yield return 0;

                    }
                    Debug.Log("waiting for button press");
                    yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                    {
                        pressed = didPress;
                    }));

                    }   
                    else
                    {
                        while(1f - prefGroupSetup.prefSlider.value > 0.4f)
                        {
                        yield return 0;
                        }
                    Debug.Log("waiting for button press");
                    yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                    {
                        pressed = didPress;
                    }));
                }


            }
            else
            {

                //add deviation to the deviationQueue

                Debug.Log("added " + deviation.ToString() + " to the queue");
                deviationQueue.Enqueue(deviation);
                //we only want to store the last two values
                if (deviationQueue.Count > maxDeviationQueueLength)
                {
                    float dequeuedFloat = deviationQueue.Dequeue();
                    Debug.Log("removed " + dequeuedFloat.ToString() + " from the queue");
                }
                Debug.Log("current deviation average  " + deviationQueue.Average().ToString());
                if (deviationQueue.Average() > deviationThreshold)
                {
                    Debug.Log("NOT LEARNED");
                    hasLearned = false;
                }
                else
                {
                    Debug.Log("HAS LEARNED");
                    hasLearned = true;
                }
            }
        }





        Experiment.Instance.shopLiftLog.LogFinalSliderValue ("COMPARATIVE",finalSliderValue, pressed);
		prefGroup.gameObject.SetActive (false);
		afterSlider = true;
		Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.COMP_SLIDER, false);
        yield return null;
	}

	IEnumerator WaitForDoorOpenPress(string text)
	{
		infoText.text = text;
		infoGroup.alpha = 1f;
		Experiment.Instance.shopLiftLog.LogWaitEvent("DOOR",true);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.DOOR_OPEN, true);
        yield return StartCoroutine (WaitForButtonPress (5f,didPress =>
			{
				Debug.Log("did press: " + didPress);
			}
		));
        TCPServer.Instance.SetState(TCP_Config.DefineStates.DOOR_OPEN, false);
        Experiment.Instance.shopLiftLog.LogWaitEvent("DOOR",false);
		infoGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator WaitForButtonPress(float maxWaitTime,System.Action<bool> didPress)
	{
		float timer = 0f;
		while (!Input.GetButtonDown ("Action Button") && timer < maxWaitTime) {
			timer += Time.deltaTime;
			yield return 0;
		}
		if (timer < maxWaitTime) {
			Experiment.Instance.shopLiftLog.LogButtonPress ();
			didPress (true);
		} else {
			didTimeout = true;
			Experiment.Instance.shopLiftLog.LogTimeout (maxWaitTime);
			didPress (false);
		}
		yield return null;
	}

	IEnumerator MakeCompleteBaselineList(int repeatCount)
	{
        //for audio
		completeAudioList = new List<AudioClip> ();
        //for images
        completeImageList = new List<Texture>();

        EnvironmentManager tempEnv;
        for (int i = 0; i<environments.Count*repeatCount; i++) {
			tempEnv = environments [i%2].GetComponent<EnvironmentManager> ();
			completeAudioList.Add (tempEnv.one_L_Audio.clip);
			completeAudioList.Add (tempEnv.one_R_Audio.clip);
			completeAudioList.Add (tempEnv.two_L_Audio.clip);
			completeAudioList.Add (tempEnv.two_R_Audio.clip);
			completeAudioList.Add (tempEnv.three_L_Audio.clip);
			completeAudioList.Add (tempEnv.three_R_Audio.clip);


            //add images
            for (int k = 0; k < 2; k++)
            {
                completeImageList.Add(tempEnv.groupOne[k]);
                completeImageList.Add(tempEnv.groupTwo[k]);
                completeImageList.Add(tempEnv.groupThree[k]);
            }

        }

		yield return null;
	}


	IEnumerator PickEnvironment(int blockCount, bool pickNew)
	{
//		envIndex = 3;
//		envIndex = ExperimentSettings.envDropdownIndex;
//		envIndex = Random.Range (0, environments.Count);
//		envIndex=blockCount;
		envIndex=blockCount;

		//reset first time
		firstTime=true;

		//first turn off all environments
		for (int i = 0; i<environments.Count; i++) {
			envManager = environments [envIndex].GetComponent<EnvironmentManager> ();
		}
		camVehicle.GetComponent<CapsuleCollider> ().height = 3.2f;
		Debug.Log ("picking environment");
		environments [envIndex].SetActive (true);
		if (environments [envIndex].name == "SpaceStation") {
			Debug.Log ("chosen space station");
			ExperimentSettings.env = ExperimentSettings.Environment.SpaceStation;
			camVehicle.transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
			camVehicle.transform.GetChild (0).GetChild (0).gameObject.SetActive (true);
			camVehicle.transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
			directionEnv = -1;
		} else if (environments [envIndex].name == "WesternTown") { //western town, for now
			Debug.Log ("chosen western town");
			ExperimentSettings.env = ExperimentSettings.Environment.WesternTown;
			camVehicle.transform.localEulerAngles = new Vector3 (0f, 0f, 0f);
			camVehicle.transform.GetChild (0).GetChild (0).gameObject.SetActive (true);
			camVehicle.transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
			camVehicle.GetComponent<CapsuleCollider> ().height =2f;
			directionEnv = 1;
		}
        else if (environments[envIndex].name == "VikingVillage")
        { //viking village, for now
            Debug.Log("chosen viking village");
            ExperimentSettings.env = ExperimentSettings.Environment.VikingVillage;
            camVehicle.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            camVehicle.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            camVehicle.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            directionEnv = -1;
        }
        else if (environments [envIndex].name == "Office") { //office
			Debug.Log ("chosen office");
			ExperimentSettings.env = ExperimentSettings.Environment.Office;
			camVehicle.transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
			camVehicle.transform.GetChild (0).GetChild (0).gameObject.SetActive (false);
			camVehicle.transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
			camVehicle.GetComponent<CapsuleCollider> ().height = 1.6f;
			directionEnv = -1;
		}
		else if (environments [envIndex].name == "Apartment") { //office
			Debug.Log ("chosen office");
			ExperimentSettings.env = ExperimentSettings.Environment.Apartment;
			camVehicle.transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
			camVehicle.transform.GetChild (0).GetChild (0).gameObject.SetActive (false);
			camVehicle.transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
			camVehicle.GetComponent<CapsuleCollider> ().height = 1.6f;
			directionEnv = -1;
		}
		else
		{
			camVehicle.transform.localEulerAngles = new Vector3 (0f, 180f, 0f);
			camVehicle.transform.GetChild (0).GetChild (0).gameObject.SetActive (true);
			camVehicle.transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
			directionEnv = -1;
		}


        //after env is picked, set the cam object for all Camera Zones
        cameraZoneManager.SetCameraObjects();

        envManager = environments [envIndex].GetComponent<EnvironmentManager> ();
		activeEnvLabel = environments [envIndex].name;
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

		//suitcases
		suitcasePrefab = envManager.suitcasePrefab;
		suitcases = new List<GameObject> ();
		for (int i = 0; i < envManager.suitcases.Count; i++) {
			suitcases.Add (envManager.suitcases [i]);
		}

		//for comparative
		prefGroup.gameObject.GetComponent<PrefGroupSetup>().firstGroup[0] = envManager.groupOne[0];
		prefGroup.gameObject.GetComponent<PrefGroupSetup>().firstGroup[1] = envManager.groupOne[1];
		prefGroup.gameObject.GetComponent<PrefGroupSetup>().secondGroup[0] = envManager.groupTwo[0];
		prefGroup.gameObject.GetComponent<PrefGroupSetup>().secondGroup[1] = envManager.groupTwo[1];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().thirdGroup[0] = envManager.groupThree[0];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().thirdGroup[1] = envManager.groupThree[1];

        //for solo
        prefSolo.gameObject.GetComponent<PrefSoloSetup> ().imgGroup [0] = envManager.groupOne [0];
		prefSolo.gameObject.GetComponent<PrefSoloSetup> ().imgGroup [1] = envManager.groupOne [1];

        //for solo training only
        prefSolo.gameObject.GetComponent<PrefSoloSetup>().imgGroup[2] = envManager.groupThree[0];
        prefSolo.gameObject.GetComponent<PrefSoloSetup>().imgGroup[3] = envManager.groupThree[1];

        //for multiple choice
        Debug.Log("ADDED MULTIPLE CHOICE ROOMTEXTURES");
		multipleChoiceGroup.gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[0] = envManager.groupOne[0];
		multipleChoiceGroup.gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[1] = envManager.groupOne[1];
		multipleChoiceGroup.gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[2] = envManager.groupTwo[0];
		multipleChoiceGroup.gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[3] = envManager.groupTwo[1];
		multipleChoiceGroup.gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[4] = envManager.groupThree[0];
		multipleChoiceGroup.gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[5] = envManager.groupThree[1];

		skyboxMat = envManager.envSkybox;
		RenderSettings.skybox = skyboxMat;


        if (pickNew)
        {
            register_L = envManager.register_L;
            register_R = envManager.register_R;

            leftRegisterObj = envManager.leftRegisterObj;
            rightRegisterObj = envManager.rightRegisterObj;

            Experiment.Instance.shopLiftLog.LogEnvironmentSelection(activeEnvLabel);


            //after env has been selected and all necessary object references set, assign rooms and randomize cam zones
            if (blockCount == 0 || blockCount == 1)
            {
                AssignRooms(true,false);
            }
            // for training only
            else if (blockCount==2)
            {
                AssignRooms(true,true); 
            }
            RandomizeSpeedChangeZones();
            yield return StartCoroutine(RandomizeCameraZones(blockCount));
        }

		yield return null;
	}

    IEnumerator RunTask()
    {
		stageIndex = 1;
		Experiment.Instance.CreateSessionStartedFile ();


        //only run if system2 is expected
        if (Config.isSystem2)
        {
            sys2ConnectionGroup.alpha = 1f;

            sys2ConnectionText.text = "Attempting to connect with server...";
            //wait till the SYS2 Server connects
            while (!tcpServer.isConnected)
            {
                yield return 0;
            }
            sys2ConnectionText.text = "Waiting for server to start...";
            while (!tcpServer.canStartGame)
            {
                yield return 0;
            }

            sys2ConnectionGroup.alpha = 0f;
        }
        else
        {
            sys2ConnectionGroup.alpha = 0f;
        }

        int totalEnvCount = environments.Count;
		int currentReevalCondition = 0;

        if (!Config.isDayThree)
        {
            if (Random.value > 0.5f)
            {
                isTransition = true;
            }
            else
                isTransition = false;
        }
		
		int startingIndex = 0;

		if (Experiment.shouldCheckpoint) {
			startingIndex = Experiment.Instance.checkpointedEnvIndex;
			registerVals=new List<int>();
            Debug.Log("TURNED OFF blackscreen");
            blackScreen.alpha = 0f;
            registerVals.Add(Experiment.Instance.leftReward);
			registerVals.Add(Experiment.Instance.rightReward);

		}

        for (int i = startingIndex; i < totalEnvCount; i++)
        {
            numTrials_Learning = 0;
            numTrials = 0;

            currentPhaseName = "TRAINING";
            if (ExperimentSettings.isTraining)
            {
                blackScreen.alpha = 0f;
                yield return StartCoroutine(PickEnvironment(2, true)); //training env
                RandomizeSuitcases();
                yield return StartCoroutine(PlayInstructionVideo(false));
                //disable any kind of camera zone interaction
                CameraZone.isPretraining = true; 
                //yield return StartCoroutine(RunSliderTrainingPhase());
                //yield return StartCoroutine(RunMultipleChoiceTrainingPhase());
                //enable camera zone interaction before camera training
                CameraZone.isPretraining = false;
                Debug.Log("running cam training");
                yield return StartCoroutine(RunCamTrainingPhase());
            }

            yield return StartCoroutine(PickEnvironment(i, true));
            RandomizeSuitcases();

            currentReevalCondition = reevalConditions[i];


            if (!Experiment.shouldCheckpoint)
            {
                if (!Config.isDayThree)
                {

                    AssignRooms(false,false);
                    yield return StartCoroutine(PickRegisterValues()); //new reg values to be picked for each environment
                }
                else
                {
                    if (i == 0)
                    {
                        registerVals[0] = 30;
                        registerVals[1] = 70;
                    }
                    else if (i == 1)
                    {
                        registerVals[0] = 50;
                        registerVals[1] = 90;
                    }
                }
            }

            isTransition = !isTransition; //flip the transition condition before the next round
            //randomize rooms and cam zones again
            //			AssignRooms ();
            //			yield return StartCoroutine(RandomizeCameraZones (i)); //we randomize cameras when picking environment now

            //learning phase

            currentPhaseName = "LEARNING";
            CheckpointSession(i, true);
            if (ExperimentSettings.isLearning)
            {

                TCPServer.Instance.SetState(TCP_Config.DefineStates.LEARNING, true);
                yield return StartCoroutine(RunLearningPhase(false, maxTrials_Learning));
                TCPServer.Instance.SetState(TCP_Config.DefineStates.LEARNING, false);
            }

            //shuffle rewards
            //		ReassignRooms ();
            //			ShuffleRegisterRewards ();

            //re-evaluation phase
            currentPhaseName = "REEVALUATION";
            CheckpointSession(i, true);
            if (ExperimentSettings.isReeval)
            {
                TCPServer.Instance.SetState(TCP_Config.DefineStates.REEVALUATION, true);
                yield return StartCoroutine(RunReevaluationPhase(currentReevalCondition));
                TCPServer.Instance.SetState(TCP_Config.DefineStates.REEVALUATION, false);
            }


            //testing phase
            currentPhaseName = "TESTING";
            CheckpointSession(i, true);
            if (ExperimentSettings.isTesting)
            {

                TCPServer.Instance.SetState(TCP_Config.DefineStates.TESTING, true);
                yield return StartCoroutine(RunTestingPhase());
                TCPServer.Instance.SetState(TCP_Config.DefineStates.TESTING, false);
            }


            //if transition phase, play 10-trial additional learning
            if (currentReevalCondition == 1)
            {
                Debug.Log("RUNNING ADDITIONAL LEARN PHASE");
                currentPhaseName = "POST-TEST";
                TCPServer.Instance.SetState(TCP_Config.DefineStates.POST_TEST, true);
                yield return StartCoroutine(RunLearningPhase(true, maxTrials_PostTest));
                TCPServer.Instance.SetState(TCP_Config.DefineStates.POST_TEST, true);
            }
            //skip if it is the final environment
            if (i != totalEnvCount - 1)
            {
                yield return StartCoroutine(ShowEndEnvironmentStageScreen());
            }

            CheckpointSession(i, true);

            //reset variables
             ResetEnvironmentVariables();
//			SceneManager.LoadScene (0); //load main menu
//			SceneManager.UnloadSceneAsync (1); //then destroy all objects of the current scene
			yield return null;

			environments [envIndex].SetActive (false);
			//remove the environment
//			environments.RemoveAt (envIndex);
		}

		CheckpointSession (totalEnvCount-1,false);

        //run baseline
        yield return StartCoroutine(MakeCompleteBaselineList(2));
		yield return StartCoroutine(RunMusicBaseline());
        yield return StartCoroutine(RunImageSlideshow());
        yield return StartCoroutine(RunSilentTraversal());

        //show the end session screen
        yield return StartCoroutine (ShowEndSessionScreen());
		yield return null;
	}

    void ResetEnvironmentVariables()
    {
        correctResponses = 0;
        CameraZone.firstTime = true;
        ExperimentSettings.isTraining = true;
        ExperimentSettings.isLearning = true;
        ExperimentSettings.isReeval = true;
        ExperimentSettings.isTesting = true;
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
		Debug.Log ("IN POSITIVE");
		positiveFeedbackGroup.alpha = 1f;
//		Debug.Log ("about to wait for 1 second");
		yield return new WaitForSeconds (1f);
//		Debug.Log ("turning it off");
		positiveFeedbackGroup.alpha = 0f; 
		consecutiveIncorrectCameraPresses = 0;
        correctResponses++; //increment correct responses
        Debug.Log("CORRECT RESPONSES " + correctResponses.ToString());
		yield return null;
	}
	public IEnumerator ShowNegativeFeedback()
	{
		Debug.Log ("IN NEGATIVE");
		negativeFeedbackGroup.alpha = 1f;
		negativeFeedbackGroup.gameObject.GetComponent<AudioSource> ().Play ();
//		Debug.Log ("about to wait for 1 second");
		yield return new WaitForSeconds (1f);

		consecutiveIncorrectCameraPresses +=1;
		negativeFeedbackGroup.gameObject.GetComponent<AudioSource> ().Stop ();
//		Debug.Log ("turning it off");
		negativeFeedbackGroup.alpha = 0f;
		yield return null;
	}

	public IEnumerator ShowWarning()
	{
		Debug.Log ("IN WARNING");
		warningFeedbackGroup.alpha = 1f;
		while (!clearCameraZoneFlags) {
			yield return 0;
		}

		CameraZone.showingWarning = false;
		warningFeedbackGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator ShowRegisterReward(int pathIndex, bool isTraining)
    {
		GameObject chosenRegister = null;
		Texture chosenTexture = null;
		choiceOutput = pathIndex;
		switch (choiceOutput) {
		case 0:
			chosenRegister = leftRegisterObj;
			break;
		case 1:
			chosenRegister = rightRegisterObj;
			break;
		
		}

		Debug.Log("chosen register is: " + chosenRegister.name);
        //		suitcaseObj.GetComponent<Suitcase> ().ChooseTexture (chosenTexture);
        //first wait for 1 second to show face/scene
        //		yield return new WaitForSeconds (2f);

        //then open the suitcase
        TCPServer.Instance.SetState(TCP_Config.DefineStates.REWARD_OPEN, true);
        suitcaseObj.GetComponent<Animator> ().SetTrigger ("Open");
//		suitcaseObj.GetComponent<Suitcase>().TurnImageOff();

		//wait until suitcase is fully open
		yield return new WaitForSeconds (0.5f);

		GameObject coinShowerObj = Instantiate(coinShower,((pathIndex==0) ?  register_L.transform.position : register_R.transform.position ) + (new Vector3(0f,0.2f,directionEnv) * 2f), Quaternion.identity) as GameObject;

        //		if (activeEnvLabel == "Cybercity") {
        //			coinShowerObj.transform.GetChild (0).transform.localPosition = Vector3.zero;
        //		}

        int reward = 0;

        //check to see if it's training for slider questions
        if (!isTraining)
        {
            System.Random rand = new System.Random();
            reward = Mathf.CeilToInt(NextGaussian(rand, registerVals[choiceOutput], 1)); //if not training, then retrieve appropriate reward values
        }
        else
        {
            reward = trainingReward[choiceOutput]; //else, obtain a random reward for training
        }
		rewardScore.enabled = true;
		rewardScore.text = "$" + reward.ToString ();

		chosenRegister.GetComponent<AudioSource> ().Play (); //play the cash register audio

		Experiment.Instance.shopLiftLog.LogRegisterReward(reward,choiceOutput);
        Experiment.Instance.shopLiftLog.LogRegisterEvent(true);
        //        infoText.text = "You got $" + registerVals[choiceOutput].ToString() + " from the register";
        Debug.Log("waiting for 2 seconds");
		yield return StartCoroutine(rewardScore.gameObject.GetComponent<FontChanger> ().GrowText (2f));
		rewardScore.enabled = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.REWARD_OPEN, false);
        Experiment.Instance.shopLiftLog.LogRegisterEvent(false);

//        infoGroup.alpha = 0f;
		Destroy(coinShowerObj);
		Destroy (suitcaseObj);
//		camVehicle.GetComponent<RigidbodyFisurstPersonController> ().enabled = true;
        yield return null;
    }

    IEnumerator ShowInstructionScreen(string instText,bool needsButtonPress,bool showTips, float waitTime)
    {
        intertrialText.text = instText;
        intertrialGroup.alpha = 1f;

        tipsGroup.alpha = (showTips) ? 1f : 0f;

        float timer = 0f;
        Debug.Log("needsbuttonpress is  " + needsButtonPress.ToString());
        while (timer < waitTime && !(needsButtonPress && Input.GetButtonDown("Action Button")))
        {
            Debug.Log("the timer is " + timer.ToString());
            timer += Time.deltaTime;
            yield return 0;
        }
        intertrialText.text = "Al día siguiente";
        intertrialGroup.alpha = 0f;
        yield return null;
    }

	IEnumerator ShowNextStageScreen()
	{

		EnablePlayerCam (false);
		intertrialGroup.alpha = 1f;
		intertrialText.text = "";
		Experiment.Instance.shopLiftLog.LogEndTrial ();
		yield return new WaitForSeconds(2f);
		intertrialGroup.alpha = 0f;
		yield return null;
	}

	IEnumerator ShowEndTrialScreen(bool isTraining,bool hasTips)
    {

		EnablePlayerCam (false);
        intertrialGroup.alpha = 1f;
		if (!isTraining) {
			intertrialText.text = "Comenzando la siguiente prueba...";
		} else {
			intertrialText.text = "Comenzando el siguiente ensayo de práctica...";
		}
		Experiment.Instance.shopLiftLog.LogEndTrial ();
        Experiment.Instance.shopLiftLog.LogEndTrialScreen(true,hasTips);
		//if (false) {
		//	if (consecutiveIncorrectCameraPresses >= 4) {
		//		tipsText.text = "TIPS:\nPlease pay attention to the camera location and press (X)";
		//		consecutiveIncorrectCameraPresses = 0;
		//	} else if (didTimeout) {
		//		tipsText.text = "TIPS:\nPlease press the (X) button quicker next time";
		//		didTimeout = false;
		//	} else {
		//		tipsText.text = "TIPS:\nPlease keep in mind the structure of rooms and rewards when responding";
		//		afterSlider = false;
		//	}
		//	tipsGroup.alpha = 1f;
		//	yield return new WaitForSeconds (1f);
		//} else {
			tipsGroup.alpha = 0f;
			yield return new WaitForSeconds(1f);
		//}
        intertrialGroup.alpha = 0f;
        Experiment.Instance.shopLiftLog.LogEndTrialScreen(false, hasTips);
        yield return null;
    }
	IEnumerator ShowEndEnvironmentStageScreen()
	{ 	intertrialGroup.alpha = 1f;
		intertrialText.text = "Felicidades, Has terminado. \n Tenga un breve descanso.";

        //reset deviation queue before beginning the next environment
        deviationQueue = new Queue<float>();

        Experiment.Instance.shopLiftLog.LogEndEnvironmentStage(true);
		yield return new WaitForSeconds(30f);
        Experiment.Instance.shopLiftLog.LogEndEnvironmentStage(false);
        intertrialGroup.alpha = 0f;
		yield return null;
	}
	IEnumerator ShowEndSessionScreen()
	{ 
		intertrialGroup.alpha = 1f;
        intertrialText.text = "Felicidades. Has completado la sesión. \n Presiona Escape para salir de la aplicación.";
		Experiment.Instance.shopLiftLog.LogEndSession(true);
		yield return new WaitForSeconds(1000f);
		intertrialGroup.alpha = 0f;
		yield return null;
		
	}

	public void RandomizeSpeed()
	{
		Debug.Log ("randomizing speed");
		suggestedSpeed = Random.Range (minSpeed, maxSpeed);
		StartCoroutine ("UpdateSpeed", suggestedSpeed);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.SPEED_CHANGE, true);
//		Debug.Log ("randomized speed to: " + currentSpeed.ToString ());
	}

	IEnumerator UpdateSpeed(float suggestedSpeed)
	{
		Debug.Log ("updating speed to: " + suggestedSpeed.ToString ());
		float timer = 0f;
		while (timer < 1f) {
			timer += Time.deltaTime;
			currentSpeed = Mathf.Lerp (currentSpeed, suggestedSpeed, timer);
			yield return 0;
		}
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

    public IEnumerator HaltPlayerMovement()
    {
        while (ShoplifterScript.haltPlayer)
        {
            camVehicle.GetComponent<Rigidbody>().isKinematic = true;
               yield return 0;
        }
        camVehicle.GetComponent<Rigidbody>().isKinematic = false;
       yield return null;
    }


	//uses velocity and distance check to move player
	IEnumerator VelocityPlayerTo(Vector3 startPos, Vector3 endPos, float factor)
	{
		int sign = (int) ((endPos.z - startPos.z) / Mathf.Abs (endPos.z - startPos.z));
//		Debug.Log ("the sign is: " + sign.ToString ());
		Vector3 moveDir = new Vector3 (0f, 0f, sign*1f);
//		Debug.Log ("move dir: " + moveDir.ToString ());
		//set some initial current speed
		currentSpeed = Random.Range(minSpeed,maxSpeed);
//		Debug.Log ("current speed is " + currentSpeed.ToString ());
		float distanceLeft = Vector3.Distance(camVehicle.transform.position,endPos);

//		Debug.Log ("distance left is " + distanceLeft.ToString ());
		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = false;
		float timer = 0f;
//		Debug.Log ("velocity player movement");
		while (distanceLeft > 1.5f) {
			timer += Time.deltaTime;
//			Debug.Log ("velocity : " + camVehicle.GetComponent<Rigidbody> ().velocity.ToString ());
			camVehicle.GetComponent<Rigidbody>().velocity = moveDir * currentSpeed;
			distanceLeft = Vector3.Distance (camVehicle.transform.position, endPos);
			yield return 0;
		}
//		Debug.Log ("stopping the player");
//		Debug.Log("final timer: " + timer.ToString());
		camVehicle.GetComponent<Rigidbody> ().velocity = Vector3.zero;
//		camVehicle.GetComponent<RigidbodyFirstPersonController> ().enabled = true;
		yield return null;
	}

		
}
