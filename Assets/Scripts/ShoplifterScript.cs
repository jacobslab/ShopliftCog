using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using UnityStandardAssets.Characters.FirstPerson;
using System.IO;


public class ShoplifterScript : MonoBehaviour
{

    Experiment exp { get { return Experiment.Instance; } }
    public ExperimentSettings expSettings { get { return ExperimentSettings.Instance; } }

    public bool globalCamSelectDisable;
    public Material SecurityCamMat_Black;
    public Material SecurityCamMat_White;
    public GameObject security_camera;
    public Camera cam;
    bool isFirst = true;
    bool apartment_set = false;
    public IMG2Sprite img2sprite;
    public CanvasGroup instructionRenderer;
    public Image instructionRendererImage;
    bool is_Training;
    float globaltimer = 0f;
    bool count_global = true;
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
#if !FAST_TEST
    private int maxTrials_Learning = 24;
#else
    private int maxTrials_Learning = 2;
#endif

    //variables for additional learning phase
    private int numAdditionalTrials = 0;
    private int maxAdditionalTrials = 12;

    //stage 2 reevaulation variables
    private int maxTrials_Reeval = 2;
#if !FAST_TEST
    private int maxBlocks_Reeval = 10;
#else
    private int maxBlocks_Reeval = 1;
#endif
    private int envIndex = 0;

    private List<float> camZoneFactors;

    //stage 4 post-test variables
#if !FAST_TEST
    private int maxTrials_PostTest = 10;
#else
    private int maxTrials_PostTest = 1;
#endif
    public GameObject leftDoorPos;
    public GameObject rightDoorPos;
    public float phase1Factor = 5f;
    public Animator cartAnim;
    private int playerChoice = -1; //0 for left and 1 for right
    public List<int> registerVals; // 0-1 is L-R for toy , 2-3 is L-R for hardware

    private string activeEnvLabel = "";

    public GameObject instructionVideo;

    public List<GameObject> environments;
    public int environmentNumber;
    private EnvironmentManager envManager;

    private GameObject leftRoom;
    private GameObject rightRoom;

    float suggestedSpeed = 0f;

    float directionEnv = -1f; //-1 for space station, +1 for western town


    private int currentPathIndex = 0;
    private int currentRoomIndex = 0;

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
    private string pressToContinueInstruction = "Press (X) button to continue";
    private string musicBaselineInstruction = "In what follows you will hear music from the game. \n Please maintain your gaze at the fixation cross, relax, and pay attention to the music.";
    private string imageSlideshowInstruction = "In what follows you will see images from the game. \n Please maintain your gaze on the screen, relax, and pay attention to different images that appear on the screen.";

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
    public CanvasGroup pretrainingPeriodGroup;
    public CanvasGroup trainingPeriodGroup;
    public CanvasGroup learningPeriodGroup;
    public CanvasGroup reevaluationPeriodGroup;
    public CanvasGroup counter_Val;
    public Text counter_Val_Text;

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
    public CanvasGroup correctGiantText;
    public CanvasGroup incorrectGiantText;
    public CanvasGroup PauseGroup;
    public CanvasGroup sensorImage;
    public bool isGamePaused;



    //TRAINING environment
    public GameObject vikingEnv;




    private GameObject roomOne;
    private GameObject roomTwo;

    //instr strings
    private string doorText = "Press (X) to open the door";
    private string registerText = "Press (X) to open the ";

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


    private string registerType = "suitcase";
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

    public int _currentReevalCondition = 0;
    private int _startingIndex = 0;
    public Slider MultipleChoiceslider;
    public int reevaluation_stage;
    public string[] training_1_seq = new string[6];
    public string[] training_3_seq = new string[3];
    public string[] learning_seq = new string[10];
    public string[] reeval_seq = new string[10];

    public string[] training_1_ques = new string[6];
    public string[] training_3_ques = new string[3];
    public string[] learning_ques = new string[10];
    public string[] reeval_ques = new string[10];
    public int[] questionpattern = new int[10]; 
    public float speedTimer;
    public int timebins;
    public GameObject mountain_gobj;

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

        globalCamSelectDisable = false;
        timebins = 0;
        reevaluation_stage = -1;
        isGamePaused = false;
        Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        pauseUI.alpha = 0f;
        instructionVideo.SetActive(false);
        EnablePlayerCam(false);
        Application.targetFrameRate = 60;
        deviationQueue = new Queue<float>();
        is_Training = false;
        globaltimer = 0f;
        count_global = true;
        isFirst = true;
        speedTimer = 0f;

        training_1_seq = new string[6];
        training_3_seq = new string[3];
        learning_seq = new string[10];
        reeval_seq = new string[10];

        training_1_ques = new string[6];
        training_3_ques = new string[3];
        learning_ques = new string[10];
        reeval_ques = new string[10];
}

    void InitializeInstructionsByLanguage()
    {
        try
        {
            if (expSettings.currentLanguage == ExperimentSettings.Language.English)
            {
                pressToContinueInstruction = "Press (X) button to continue";
                musicBaselineInstruction = "In what follows you will hear music from the game. \n Please maintain your gaze at the fixation cross, relax, and pay attention to the music.";
                imageSlideshowInstruction = "In what follows you will see images from the game. \n Please maintain your gaze on the screen, relax, and pay attention to different images that appear on the screen.";
                doorText = "Press (X) to open the door";
                registerText = "Press (X) to open the ";
            }
            else
            {
                pressToContinueInstruction = "Presiona (X) para continuar";
                musicBaselineInstruction = "A continuar escucharas música del juego.\n Por favor mantenga su mirada en la cruz. \n Relájate y preste atención a la música.";
                imageSlideshowInstruction = "A continuar escucharas imagenes del juego.\n Por favor mantenga su mirada en la cruz. \n Relájate y preste atención a la imagenes.";
                doorText = "Presiona el botón (X) para abrir la puertar";
                registerText = "Presiona el botón (X) para abrir la maleta";
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Encountered Error: " + e.Message + ": " + e.StackTrace);
        }
    }

    void EnablePlayerCam(bool shouldEnable)
    {
        mainCam.enabled = shouldEnable;
    }

    // Use this for initialization
    void Start()
    {
        apartment_set = false;
        camZoneFactors = new List<float>();
        camZoneFactors = GetRandomNumbers(environments.Count); //get as many unique random numbers as there are environments
                                                               //		reevalConditions = new List<int>();
                                                               //		reevalConditions= ShuffleReevalConditions();
        introInstructionGroup.alpha = 0f;
        infoGroup.alpha = 0f;
        imagineGroup.alpha = 0f;
        positiveFeedbackGroup.alpha = 0f;
        negativeFeedbackGroup.alpha = 0f;
        intertrialGroup.alpha = 0f;
        trainingInstructionsGroup.alpha = 0f;
        pretrainingPeriodGroup.alpha = 0f;
        trainingPeriodGroup.alpha = 0f;
        prefSolo.gameObject.SetActive(false);
        prefGroup.gameObject.SetActive(false);
        correctGiantText.alpha = 0f;
        incorrectGiantText.alpha = 0f;
        slideshowImage.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

        suitcaseObj = null;
        camVehicle.SetActive(true);
        camTrans = camVehicle.GetComponent<RigidbodyFirstPersonController>().cam.transform;
        RandomizeSpeed();
        rewardScore.enabled = false;

        Cursor.visible = false;
        currentSpeed = (UnityEngine.Random.Range(minSpeed, maxSpeed)) * 1.2f;
        timebins = 1;
        InitializeInstructionsByLanguage();

        UnityEngine.Debug.Log("SAI_DEBUG: Running Task ");

        //LoadMaterial_mountain();
        StartCoroutine("RunTask");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (ExperimentSettings.isLogging && (isFirst == true))
        {
            Experiment.Instance.shopLiftLog.LogMetaData();
            isFirst = false;
        }*/
    
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                PauseGroup.alpha = 0f;
                isGamePaused = false;
                Experiment.Instance.shopLiftLog.LogGamePause(false);
            }
            else
            {
                Time.timeScale = 0;
                PauseGroup.alpha = 1f;
                isGamePaused = true;
                Experiment.Instance.shopLiftLog.LogGamePause(true);
            }
        }

        if (globalCamSelectDisable == true)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                security_camera.gameObject.GetComponent<MeshRenderer>().enabled = false;
                Experiment.Instance.shopLiftLog.LogSecCameraStatus(1);
                Experiment.Instance.shopLiftLog.LogSecCameraStatus(3);
                globalCamSelectDisable = false;
            }
        }


        if (environments[envIndex].name == "Apartment")
        { //office
            envManager.phase2Door_L.transform.GetChild(1).gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);

            if (apartment_set == true)
                phase2Door_L.transform.GetChild(1).gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }


        if (count_global)
            globaltimer += Time.deltaTime;

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {

                Debug.Log("Pressed: " + vKey);
                Experiment.Instance.shopLiftLog.LogPressedKey(vKey);

            }
        }

        speedTimer += Time.deltaTime;

        if (speedTimer >= 1.5f)
        {
            timebins += 1;
            speedTimer = 0f;
            currentSpeed = (UnityEngine.Random.Range(minSpeed, maxSpeed)) * 1.2f;
            UnityEngine.Debug.Log("Unity Speed: " + currentSpeed);
            Experiment.Instance.shopLiftLog.LogExpSpeedChange(currentSpeed, timebins);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        GetPauseInput();


    }

    void RandomizeSuitcases()
    {
        if (UnityEngine.Random.value > 0.5f)
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
        string line = "";
        if (currentPhaseName != "PRE-TRAINING")
        {
            line = ((isOngoing) ? "ONGOING" : "FINISHED") + separator + envIndex.ToString() + separator + currentPhaseName + separator + reevalConditions[currentBlockCount].ToString() + separator + leftRoom.name + separator + registerVals[0].ToString() + separator + rightRoom.name + separator + registerVals[1].ToString();
        }
        //for pre-training, we just log the phase 
        else
        {
            line = ((isOngoing) ? "ONGOING" : "FINISHED") + separator + currentPhaseName;
        }
        Debug.Log("checkpointed line is: " + line);
        System.IO.File.WriteAllText(Experiment.Instance.sessionDirectory + "checkpoint.txt", line);
    }

    void RandomizeSpeedChangeZones()
    {
        Debug.Log("randomized speed change zones");
        phase1SpeedChangeZones_L[0].transform.position = new Vector3(phase1Start_L.transform.position.x, phase1Start_L.transform.position.y, UnityEngine.Random.Range(phase1Start_L.transform.position.z, Vector3.Lerp(phase1Start_L.transform.position, phase1End_L.transform.position, 0.5f).z));
        phase1SpeedChangeZones_L[1].transform.position = new Vector3(phase1Start_L.transform.position.x, phase1Start_L.transform.position.y, UnityEngine.Random.Range(Vector3.Lerp(phase1Start_L.transform.position, phase1End_L.transform.position, 0.5f).z, phase1End_L.transform.position.z));

        phase1SpeedChangeZones_R[0].transform.position = new Vector3(phase1Start_R.transform.position.x, phase1Start_R.transform.position.y, UnityEngine.Random.Range(phase1Start_R.transform.position.z, Vector3.Lerp(phase1Start_R.transform.position, phase1End_R.transform.position, 0.5f).z));
        phase1SpeedChangeZones_R[1].transform.position = new Vector3(phase1Start_R.transform.position.x, phase1Start_R.transform.position.y, UnityEngine.Random.Range(Vector3.Lerp(phase1Start_R.transform.position, phase1End_R.transform.position, 0.5f).z, phase1End_R.transform.position.z));

        phase2SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x, phase2Start_L.transform.position.y, UnityEngine.Random.Range(envManager.phase2Start_L.transform.position.z, Vector3.Lerp(envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z));
        phase2SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase2Start_L.transform.position.x, phase2Start_L.transform.position.y, UnityEngine.Random.Range(Vector3.Lerp(envManager.phase2Start_L.transform.position, envManager.phase2End_L.transform.position, 0.5f).z, envManager.phase2End_L.transform.position.z));

        phase2SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x, phase2Start_R.transform.position.y, UnityEngine.Random.Range(envManager.phase2Start_R.transform.position.z, Vector3.Lerp(envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z));
        phase2SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase2Start_R.transform.position.x, phase2Start_R.transform.position.y, UnityEngine.Random.Range(Vector3.Lerp(envManager.phase2Start_R.transform.position, envManager.phase2End_R.transform.position, 0.5f).z, envManager.phase2End_R.transform.position.z));

        phase3SpeedChangeZones_L[0].transform.position = new Vector3(envManager.phase3Start_L.transform.position.x, phase3Start_L.transform.position.y, UnityEngine.Random.Range(envManager.phase3Start_L.transform.position.z, Vector3.Lerp(envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f).z));
        phase3SpeedChangeZones_L[1].transform.position = new Vector3(envManager.phase3Start_L.transform.position.x, phase3Start_L.transform.position.y, UnityEngine.Random.Range(Vector3.Lerp(envManager.phase3Start_L.transform.position, envManager.phase3End_L.transform.position, 0.5f).z, envManager.phase3End_L.transform.position.z));

        phase3SpeedChangeZones_R[0].transform.position = new Vector3(envManager.phase3Start_R.transform.position.x, phase3Start_R.transform.position.y, UnityEngine.Random.Range(envManager.phase3Start_R.transform.position.z, Vector3.Lerp(envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f).z));
        phase3SpeedChangeZones_R[1].transform.position = new Vector3(envManager.phase3Start_R.transform.position.x, phase3Start_R.transform.position.y, UnityEngine.Random.Range(Vector3.Lerp(envManager.phase3Start_R.transform.position, envManager.phase3End_R.transform.position, 0.5f).z, envManager.phase3End_R.transform.position.z));

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
            //Debug.Log("rand int is " + randInt.ToString());
            float nextDouble = (float)randInt / 100f;
            // Debug.Log("next double is  " + nextDouble.ToString());
            //float nextDouble = (float)rand.NextDouble();
            nextDouble = Mathf.Clamp(nextDouble, 0.5f, 0.8f);
            randList.Add((float)(nextDouble));
            // Debug.Log("cam zone factor: " + randList[i]);
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
        cameraZoneManager.SetActiveCameraZone(activeCamZone.GetComponent<CameraZone>());

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

        Dictionary<int, int> newMap = new Dictionary<int, int>();
        newMap.Add(1, 3);
        newMap.Add(2, 4);

        //by default, 
        int finalRoomLeft = 5;
        int finalRoomRight = 6;

        //now, if the above two values have changed we will update them accordingly below 


        /*if (!Config.isDayThree && !isTraining)
        {

            if (UnityEngine.Random.value < 0.5f)
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
                finalRoomLeft = 6;

                rightRoom = roomOne;
                three_R_Audio = envManager.three_L_Audio;
                rightRoomColor = roomOneColor;
                finalRoomRight = 5;


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
                finalRoomLeft = 6;

                rightRoom = roomOne;
                three_R_Audio = envManager.three_L_Audio;
                rightRoomColor = roomOneColor;
                finalRoomRight = 5;


                Experiment.Instance.shopLiftLog.LogRooms(roomTwo.name, roomOne.name);
            }
        }*/

        leftRoom = roomOne;
        three_L_Audio = envManager.three_L_Audio;
        leftRoomColor = roomOneColor;
        rightRoom = roomTwo;
        three_R_Audio = envManager.three_R_Audio;
        rightRoomColor = roomTwoColor;


        Experiment.Instance.shopLiftLog.LogRooms(roomOne.name, roomTwo.name);

        //finally update the mappings to Room 3 and 4
        newMap.Add(3, finalRoomLeft);
        newMap.Add(4, finalRoomRight);

        Debug.Log("new map keys count " + newMap.Keys.Count);
        //finally send the mappings to the multipleChoice script
        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().UpdateRoomMappings(newMap);




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

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Pause Button"))
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
        Experiment.Instance.shopLiftLog.LogReassignEvent();
        if (leftRoom == roomTwo)
        {
            leftRoom = roomOne;
            leftRoomColor = roomOneColor;
            three_L_Audio = envManager.three_L_Audio;


            rightRoom = roomTwo;
            rightRoomColor = roomTwoColor;
            three_R_Audio = envManager.three_R_Audio;

            Experiment.Instance.shopLiftLog.LogRooms(roomOne.name, roomTwo.name);

        }
        else
        {
            leftRoom = roomTwo;
            leftRoomColor = roomTwoColor;
            three_L_Audio = envManager.three_R_Audio;


            rightRoom = roomOne;
            rightRoomColor = roomOneColor;
            three_R_Audio = envManager.three_L_Audio;

            Experiment.Instance.shopLiftLog.LogRooms(roomTwo.name, roomOne.name);
        }

        GameObject tempSuitcase = null;
        tempSuitcase = leftSuitcase;
        leftSuitcase = rightSuitcase;
        rightSuitcase = tempSuitcase;
        leftRoom.transform.position = envManager.leftRoomTransform.position;
        rightRoom.transform.position = envManager.rightRoomTransform.position;
    }

    public List<int> ShuffleList(List<int> vals)
    {
        List<int> valList = new List<int>();
        int valCount = vals.Count;
        for (int i = 0; i < valCount; i++)
        {
            valList.Add(vals[i]);
        }
        vals.Clear();
        int valListCount = valList.Count;
        for (int i = 0; i < valListCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, valList.Count);
            vals.Add(valList[randomIndex]);
            valList.RemoveAt(randomIndex);
        }

        return vals;
    }

    IEnumerator PlayInstructionVideo(bool playVideo)
    {
        //inst video
        playVideo = false;
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
            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
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
            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;
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
            leftReward = Mathf.CeilToInt(UnityEngine.Random.Range(10f, 90f));
            rightReward = Mathf.CeilToInt(UnityEngine.Random.Range(10f, 90f));
        }
        //then add the rewards to a list
        trainingReward = new int[2];
        trainingReward[0] = leftReward;
        trainingReward[1] = rightReward;
    }

    IEnumerator RunInitTrainingPhase()
    {
        currentPhaseName = "PRE-TRAINING";
        cameraZoneManager.ToggleAllCamZones(false); //turn off all cameras
        yield return StartCoroutine(RunPhaseSequence());
        yield return null;
    }

    IEnumerator RunSliderTrainingPhase()
    {


        Debug.Log("turning off all cam zones");

        //show instructions first
        intertrialText.text = "Welcome to PRE-TRAINING 1/3!\n Let's practice which rooms lead to higher cash!\n You will learn how to use sliders to respond.\n Press(X) to begin!";
        /*intertrialGroup.alpha = 1f;
        yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
        {
            Debug.Log("did press: " + didPress);
        }
        ));
        intertrialGroup.alpha = 0f;
        */
        cameraZoneManager.ToggleAllCamZones(false); //turn off all cameras
        //comparative sliders
        for (int i = 0; i < 3; i++)
        {
            RandomizeTrainingRewards();
            bool isLeft = true;

            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true, 0));
            //now run the other corridor
            isLeft = !isLeft;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true, 0));
            yield return StartCoroutine(AskPreference(2, false, false, true, 0, 0f));
        }

        //solo sliders
        for (int i = 0; i < 3; i++)
        {
            RandomizeTrainingRewards();
            bool isLeft = true;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true, 0));
            //now run the other corridor
            isLeft = !isLeft;
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true, 0));
            //we will randomly pick on whether to query the left or right room
            yield return StartCoroutine(AskSoloPreference((UnityEngine.Random.value > 0.5f) ? 2 : 3, true)); // we have assigned Room 5 (left) and Room 6 (right) as 2 and 3 index in the solo img groups
        }
        yield return null;
    }

    IEnumerator RunMultipleChoiceTrainingPhase()
    {
        //show instructions
        intertrialText.text = "Welcome to PRE-TRAINING 2/3!\n Learning room arrangements.\n Where does each room's door open to?\n You will answer by choosing a room.\n Press(X) to begin!";
        /*intertrialGroup.alpha = 1f;
        yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
        {
            Debug.Log("did press: " + didPress);
        }
        ));
        intertrialGroup.alpha = 0f;
        */
        cameraZoneManager.ToggleAllCamZones(false);
        bool isLeft = true;
        for (int i = 0; i < 2; i++)
        {
            yield return StartCoroutine(RunPhaseTwo((isLeft) ? 0 : 1, true, false, 0));
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, false, false, 0));
            yield return StartCoroutine(AskMultipleChoice(2 + i, true));
            isLeft = !isLeft;

        }

        yield return null;
    }

    IEnumerator ShowIntroInstructions()
    {
        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;
        Experiment.Instance.shopLiftLog.LogTextInstructions(1, true);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.INSTRUCTIONS, true);
        /*introInstructionGroup.alpha = 1f;
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
        {
            Debug.Log("did press: " + didPress);
        }
        ));*/
        introInstructionGroup.alpha = 0f;

        Experiment.Instance.shopLiftLog.LogTextInstructions(1, false);
        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return null;
    }


    IEnumerator RunCamTrainingPhase()
    {
        //blackScreen.alpha = 1f;
        if (expSettings.stage == ExperimentSettings.Stage.Pretraining)
        {
            intertrialText.text = "Welcome to PRE-TRAINING 3/3!\n In each room, PRESS(X) as you pass \n camera location, &*memorize cam location*!\n When cam is invisible, PRESS(X)\n as you pass remembered cam location.\n Press(X) to begin!";
            /*intertrialGroup.alpha = 1f;
            positiveFeedbackGroup.alpha = 0f;
            negativeFeedbackGroup.alpha = 0f;
            //trainingPeriodGroup.transform.GetChild(0).gameObject.GetComponent<Text>().text = "PRE-TRAINING \n PERIOD";
            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));
            intertrialGroup.alpha = 0f;*/
        }
        else if (expSettings.stage == ExperimentSettings.Stage.Training)
        {
            Debug.Log("starting cam training phase");
            Experiment.Instance.shopLiftLog.LogTextInstructions(2, true);
            /*trainingInstructionsGroup.alpha = 1f;
            //trainingPeriodGroup.transform.GetChild(0).gameObject.GetComponent<Text>().text = "TRAINING \n PERIOD";
            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));*/
            trainingInstructionsGroup.alpha = 0f;

            Experiment.Instance.shopLiftLog.LogTextInstructions(2, false);

        }


        
        cameraZoneManager.ResetAllCamZones();
        //cameraZoneManager.ToggleAllCamZones(true); //temporarily turn on all cameras
        RandomizeSpeedChangeZones();

        TCPServer.Instance.SetState(TCP_Config.DefineStates.INSTRUCTIONS, false);

        //training begins here
        TCPServer.Instance.SetState(TCP_Config.DefineStates.TRAINING, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("TRAINING", true);

        trainingPeriodGroup.alpha = 1f;
        bool isLeft = false;
        int numTraining = 0;


        CameraZone.isTraining = true;

        CameraZone.isPretraining = false;

        int count = 0;
        while (numTraining < 6)
        {

            if (numTraining % 2 == 0)
            {
                count++;
                if (training_3_seq[numTraining / 2] == "A")
                {
                    isLeft = true;
                }
                else
                {
                    isLeft = false;
                }
            }
            else {
                isLeft = !isLeft;
            }

            if (numTraining % 2 == 0)
                counter_Val_Text.text = count.ToString();
            else
                counter_Val_Text.text = count.ToString();
            //check correct responses and reset if it is less than 3
            if (numTraining % 2 == 0)
            {
                if (correctResponses < 3)
                {
                    correctResponses = 0;
                }


            }

            if (numTraining % 2 == 0)
                Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, count + 6, 1);

            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(3, ((isLeft == true) ? 0 : 1), 1);
            counter_Val.alpha = 1f;
            Debug.Log("about to run phase 1");

            List<int> l = new List<int>() { 1, 2, 3 };
            System.Random rnd = new System.Random();
            int index = rnd.Next(l.Count);
            int selected = l[index];

            List<int> xl = new List<int>() { -1, 1 };
            int indexx = rnd.Next(xl.Count);
            int selectedx = xl[indexx];

            float value_fixed = 0.9f;


            security_camera.gameObject.GetComponent<MeshRenderer>().enabled = true;
            if (selected == 3)
            {
                value_fixed = 0.99f;
                security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
            }
            else if (selected == 2)
            {
                security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
            }
            else if (selected == 1)
            {
                if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
                    security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
                else
                    security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
            }

            if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
            {
                security_camera.gameObject.transform.localScale = new Vector3(20f, 20f, 20f);
                if (isLeft)
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_L.transform.position + value_fixed * phase1End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_L.transform.position + value_fixed * phase2End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_L.transform.position + value_fixed * phase3End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));

                }
                else
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_R.transform.position + value_fixed * phase1End_R.transform.position)) + (selectedx * new Vector3(0f, selectedx * 5f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_R.transform.position + value_fixed * phase2End_R.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_R.transform.position + value_fixed * phase3End_R.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                }
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.Apartment)
            {
                security_camera.gameObject.transform.localScale = new Vector3(8f, 8f, 8f);
                if (isLeft)
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_L.transform.position + value_fixed * phase1End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_L.transform.position + value_fixed * phase2End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_L.transform.position + value_fixed * phase3End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));

                }
                else
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_R.transform.position + value_fixed * phase1End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_R.transform.position + value_fixed * phase2End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_R.transform.position + value_fixed * phase3End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                }
            }

            security_camera.gameObject.transform.localEulerAngles = new Vector3(0, -selectedx * 45, 0);



            yield return StartCoroutine(RunPhaseOne((isLeft) ? 0 : 1, false, selected));
            Experiment.Instance.shopLiftLog.LogDoorPosition(((isLeft) ? phase1Door_L : phase1Door_R).transform.position.x, ((isLeft) ? phase1Door_L : phase1Door_R).transform.position.y, ((isLeft) ? phase1Door_L : phase1Door_R).transform.position.z);

            Debug.Log("about to run phase 2");
            yield return StartCoroutine(RunPhaseTwo((isLeft) ? 0 : 1, false, false, selected));
            Experiment.Instance.shopLiftLog.LogDoorPosition(((isLeft) ? phase2Door_L : phase2Door_R).transform.position.x, ((isLeft) ? phase2Door_L : phase2Door_R).transform.position.y, ((isLeft) ? phase2Door_L : phase2Door_R).transform.position.z);

            //			TurnOffRooms ();
            Debug.Log("about to run phase 3");
            yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, true, true, selected));
            Experiment.Instance.shopLiftLog.LogChestPosition(((isLeft) ? register_L : register_R).transform.position.x, ((isLeft) ? register_L : register_R).transform.position.y, ((isLeft) ? register_L : register_R).transform.position.z);


            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(3, ((isLeft==true)? 0:1), 0);
            if (numTraining % 2 == 1)
                Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, count + 6, 0);

            counter_Val.alpha = 0f;
            if (numTraining < 3)
                yield return StartCoroutine(ShowEndTrialScreen(true, ShouldShowTips()));
            else
                yield return StartCoroutine(ShowEndTrialScreen(false, ShouldShowTips()));



            if (numTraining < 2)
            {
                if (numTraining % 2 == 1)
                {
                    //Experiment.Instance.shopLiftLog.LogExpQuesTypeSE(1);
                    UnityEngine.Debug.Log("Case 1 training: AskMultipleChoice_v2");
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                    Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1);
                    
                    
                    //v2 is for Ques with Focus Image
                    if (training_3_ques[0] == "A")
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 5);
                        yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 0));
                        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 5.0f + MultipleChoiceslider.value);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 5);
                        yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 1));
                        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 6.0f - MultipleChoiceslider.value);
                    }
                    

                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                }
                //StopCoroutine("AskMultipleChoice_v2");
            }
            else if (numTraining % 2 == 1)
            {
                UnityEngine.Debug.Log("Case 2 training: AskMultipleChoice_v3");
                Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                //v2 is for Ques without Focus Image
                if (training_3_ques[numTraining / 2] == "A")
                    yield return StartCoroutine(AskMultipleChoice_v3((numTraining) % 2, 0, 0));
                else
                    yield return StartCoroutine(AskMultipleChoice_v3((numTraining) % 2, 1, 0));
                Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
            }


            numTraining++;

            if (numTraining >= 2)
            {
                //make cameras invisible 
                cameraZoneManager.MakeAllCamInvisible(true);
                CameraZone.firstTime = false;
            }
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
    float NextGaussian(System.Random r, double mu = 0, double sigma = 1)
    {
        var u1 = r.NextDouble();
        var u2 = r.NextDouble();

        Debug.Log("Reward u1: " + u1);
        Debug.Log("Reward u2: " + u2);
        var rand_std_normal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) *
            System.Math.Sin(2.0 * System.Math.PI * u2);

        Debug.Log("Reward rand_std_normal: " + rand_std_normal);
        float rand_normal = (float)(mu + sigma * rand_std_normal);

        Debug.Log("Reward rand_normal: " + rand_normal);

        return rand_normal;
    }

    List<int> GiveRandSequenceOfTwoInts(int int1, int int2, int seqLength)
    {
        List<int> tempList = new List<int>();
        for (int i = 0; i < seqLength / 2; i++)
        {
            tempList.Add(int1);
            tempList.Add(int2);
        }
        tempList = ShuffleList(tempList);
        Debug.Log("contents of templist are");
        for (int i = 0; i < tempList.Count; i++)
        {
            Debug.Log(tempList[i]);
        }
        return tempList;
    }


    IEnumerator PickRegisterValues()
    {
        registerLeft = new List<int>();
        registerVals = new List<int>();
        int index = UnityEngine.Random.Range(0, registerVal1.Count);
        registerVals.Add(registerVal1[index]);
        registerVals.Add(registerVal2[index]);

        Experiment.Instance.shopLiftLog.LogRegisterValues(registerVal1[index]);
        Experiment.Instance.shopLiftLog.LogRegisterValues(registerVal2[index]);

        registerVal1.RemoveAt(index);
        registerVal2.RemoveAt(index);

        Debug.Log("register val at 0 is: " + registerVals[0].ToString());
        Debug.Log("register val at 1 is: " + registerVals[1].ToString());
        yield return null;
    }

    void RemoveIndex(List<int> valueList, int matchedInt)
    {
        for (int i = 0; i < valueList.Count; i++)
        {
            if (valueList[i] == matchedInt)
            {
                valueList.RemoveAt(i);
            }
        }
    }


    IEnumerator RunPhaseOne(int pathIndex, bool terminateWithChoice, int selected)
    {
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, (pathIndex % 2) + 1, 1);
        Experiment.Instance.shopLiftLog.LogPathIndex(pathIndex);
        currentPathIndex = pathIndex;
        currentRoomIndex = 1;
        EnablePlayerCam(true);

        ChangeCamZoneFocus((pathIndex == 0) ? 0 : 3);
        GameObject targetDoor = (pathIndex == 0) ? phase1Door_L : phase1Door_R;
        camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        clearCameraZoneFlags = false;
        Debug.Log("running phase one");
        /*
		AudioSource baseAudio = (pathIndex == 0) ? one_L_Audio : one_R_Audio;
		float delayOne = UnityEngine.Random.Range (0f, baseAudio.clip.length);

		baseAudio.time = delayOne;
		baseAudio.Play ();
        */

        //if number of correct responses are greater than 3, then don't show camera in the next practice round
        if (correctResponses > 3)
        {
            Debug.Log("correct response is eq or above 3");
            CameraZone.firstTime = false;
            //ChangeCameraZoneVisibility(false); //doesn't actually turn off the gameobject, which is why we need to turn them off using the above line
        }

        ChangeCamZoneFocus((pathIndex == 0) ? 0 : 3);
        ToggleMouseLook(false);

        Vector3 startPos = (pathIndex == 0) ? phase1Start_L.transform.position : phase1Start_R.transform.position;
        Vector3 endPos = (pathIndex == 0) ? phase1End_L.transform.position : phase1End_R.transform.position;
        camVehicle.transform.position = startPos;
        Debug.Log("start pos " + startPos.ToString());
        camVehicle.SetActive(true);
        Experiment.Instance.shopLiftLog.LogMoveEvent(1, true);
        Debug.Log("Hello There!! I am here ");
        if (selected == 1)
        {
            Experiment.Instance.shopLiftLog.LogSecCameraPos(security_camera.transform.position.x,
                                                            security_camera.transform.position.y,
                                                            security_camera.transform.position.z);
            globalCamSelectDisable = true;
        }
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));
        if (selected == 1)
        {
            if (globalCamSelectDisable == true)
            {
                Experiment.Instance.shopLiftLog.LogSecCameraStatus(2);
            }
            globalCamSelectDisable = false;
        }
        Debug.Log("Hello There!! I am here 2222222");

        Experiment.Instance.shopLiftLog.LogMoveEvent(1, false);
        clearCameraZoneFlags = true;
        if (activeCamZone != null)
            activeCamZone.GetComponent<CameraZone>().isFocus = false;
        /*while(expSettings.stage == ExperimentSettings.Stage.Pretraining && !activeCamZone.GetComponent<CameraZone>().hasSneaked)
         {
             yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));
             yield return 0;
         }*/
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, (pathIndex % 2) + 1, 0);
        Debug.Log("Hello There!! I am here 22");
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(6, 1);
        yield return StartCoroutine(WaitForDoorOpenPress(doorText));
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(6, 0);
        float delayTwo = 0f;
        if (!terminateWithChoice)
        {
            //Doors.canOpen = true;
            Debug.Log("opening doors");
            //yield return StartCoroutine (targetDoor.GetComponent<Doors> ().Open ());

            //		ToggleMouseLook(false);


            if (pathIndex == 0)
            {

                ChangeCamZoneFocus(1);
                currentAudio = two_L_Audio;
                ChangeColors(rightRoomColor);

                phase2Start_L = envManager.phase2Start_L;
                phase2End_L = envManager.phase2End_L;

                Experiment.Instance.shopLiftLog.LogMoveEvent(2, true);
                if (!terminateWithChoice)
                {
                    Debug.Log("phase 1 door L: " + phase1Door_L.transform.GetChild(0).gameObject.name);
                    //yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase1Door_L.transform.GetChild(0).position, 0.5f));
                    /*baseAudio.Stop ();
					delayTwo = UnityEngine.Random.Range (0f, currentAudio.clip.length);
					currentAudio.time = delayTwo;
					currentAudio.Play ();
                    */
                    //yield return StartCoroutine(MovePlayerTo(phase1Door_L.transform.GetChild(0).position, phase2Start_L.transform.position, 0.5f));
                    yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase2Start_L.transform.position, 0.5f));
                }

            }
            else if (pathIndex == 1)
            {

                ChangeCamZoneFocus(4);
                currentAudio = two_R_Audio;
                ChangeColors(leftRoomColor);

                phase2Start_R = envManager.phase2Start_R;
                phase2End_R = envManager.phase2End_R;
                Experiment.Instance.shopLiftLog.LogMoveEvent(2, true);
                if (!terminateWithChoice)
                {
                    Debug.Log("phase 1 door R: " + phase1Door_R.transform.GetChild(0).gameObject.name);
                    //yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase1Door_R.transform.GetChild(0).position, 0.5f));

                    /*baseAudio.Stop ();
					delayTwo = UnityEngine.Random.Range (0f, currentAudio.clip.length);
					currentAudio.time = delayTwo;
					currentAudio.Play ();
                    */
                    //yield return StartCoroutine(MovePlayerTo(phase1Door_R.transform.GetChild(0).position, phase2Start_R.transform.position, 0.5f));
                    yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase2Start_R.transform.position, 0.5f));
                }
            }
            //Doors.canOpen = false;
        }
        
        Debug.Log("Hello There!! I am here 2222");
        if (!terminateWithChoice)
        {
            //yield return StartCoroutine(targetDoor.GetComponent<Doors> ().Close ());
        }
        yield return null;
    }

    IEnumerator RunPhaseTwo(int pathIndex, bool isDirect, bool hasRewards, int selected)
    {
        if (expSettings.stage == ExperimentSettings.Stage.Reevaluation && (_currentReevalCondition == 1))
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ((pathIndex + 1) % 2) + 3, 1);
        else
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, (pathIndex % 2) + 3, 1);
        EnablePlayerCam(true);
        ChangeCamZoneFocus((pathIndex == 0) ? 1 : 4);

        currentPathIndex = pathIndex;
        currentRoomIndex = 2;

        GameObject targetDoor = (pathIndex == 0) ? phase2Door_L : phase2Door_R;
        Vector3 startPos = (pathIndex == 0) ? phase2Start_L.transform.position : phase2Start_R.transform.position;
        Vector3 endPos = (pathIndex == 0) ? phase2End_L.transform.position : phase2End_R.transform.position;
        /*
                if (isDirect) {
                    currentAudio = (pathIndex == 0) ? two_L_Audio : two_R_Audio;
                    float delay = UnityEngine.Random.Range (0f, currentAudio.clip.length);
                    currentAudio.time = delay;
                    currentAudio.Play ();

                }
        */

        //we set this explicitly for re-evaluation as RunPhaseOne isn't run anymore so we have to log it here
        Experiment.Instance.shopLiftLog.LogMoveEvent(2, true);
        clearCameraZoneFlags = false;
        Debug.Log("running phase two");
        Debug.Log("Error: CamVehicle Transform1: " + camVehicle.transform.position);
        Debug.Log("Error: CamVehicle Transform2: " + startPos);

        /*if (expSettings.stage == ExperimentSettings.Stage.Training)
        {
            yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, startPos, phase1Factor));
        }
        else*/   //if ()

        //if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation && camVehicle.transform.position.z > startPos.z)
        if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
        {
            //camVehicle.transform.position = new Vector3(startPos.x, startPos.y, camVehicle.transform.position.z);
            Debug.Log("velo player in phase 2");

            if (selected == 2)
            {
                Experiment.Instance.shopLiftLog.LogSecCameraPos(security_camera.transform.position.x,
                                                security_camera.transform.position.y,
                                                security_camera.transform.position.z);
                globalCamSelectDisable = true;
            }
            yield return StartCoroutine(VelocityPlayerTo(camVehicle.transform.position, endPos, phase1Factor));
            if (selected == 2)
            {
                if (globalCamSelectDisable == true)
                {
                    Experiment.Instance.shopLiftLog.LogSecCameraStatus(2);
                }
                globalCamSelectDisable = false;
            }
        }
        else
        {
            camVehicle.transform.position = startPos;
            Debug.Log("velo player in phase 2");

            if (selected == 2)
            {
                Experiment.Instance.shopLiftLog.LogSecCameraPos(security_camera.transform.position.x,
                                                security_camera.transform.position.y,
                                                security_camera.transform.position.z);
                globalCamSelectDisable = true;
            }
            yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));
            if (selected == 2)
            {
                if (globalCamSelectDisable == true)
                {
                    Experiment.Instance.shopLiftLog.LogSecCameraStatus(2);
                }
                globalCamSelectDisable = false;
            }
        }

        Experiment.Instance.shopLiftLog.LogMoveEvent(2, false);

        clearCameraZoneFlags = true;
        float delayThree = 0f;
        Debug.Log("[RunPhase2] in phase 2 End");
        if (activeCamZone != null)
            activeCamZone.GetComponent<CameraZone>().isFocus = false;

        if (expSettings.stage == ExperimentSettings.Stage.Reevaluation && (_currentReevalCondition == 1))
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ((pathIndex + 1) % 2) + 3, 0);
        else
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, (pathIndex % 2) + 3, 0);

        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(6, 1);
        yield return StartCoroutine(WaitForDoorOpenPress(doorText));
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(6, 0);

        if (hasRewards)
        {

            SpawnSuitcase(pathIndex); //we spawn suitcase here for both learning and relearning phase
        }
        //open the door
        //yield return StartCoroutine(targetDoor.GetComponent<Doors> ().Open ());

        if (pathIndex == 0)
        {
            Debug.Log("[RunPhase2 LS1] in phase 2 End");
            //yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase2Door_L.transform.GetChild(0).position, 0.5f));
            Debug.Log("[RunPhase2 LS2] in phase 2 End");
            /*
			currentAudio.Stop ();
			currentAudio = three_L_Audio;
			delayThree = UnityEngine.Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delayThree;
			currentAudio.Play ();
            */
            //yield return StartCoroutine(MovePlayerTo(phase2Door_L.transform.GetChild(0).position, phase3Start_L.transform.position, 0.5f));
            yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase3Start_L.transform.position, 0.5f));
            Debug.Log("[RunPhase2 LS3] in phase 2 End");

        }
        else if (pathIndex == 1)
        {
            Debug.Log("[RunPhase2 LS4] in phase 2 End");
            /*
			currentAudio.Stop ();
			currentAudio = three_R_Audio;
			delayThree = UnityEngine.Random.Range (0f, currentAudio.clip.length);
			currentAudio.time = delayThree;
			currentAudio.Play();
            */
            //yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase2Door_R.transform.GetChild(0).position, 0.5f));
            Debug.Log("[RunPhase2 LS5] in phase 2 End");
            //yield return StartCoroutine(MovePlayerTo(phase2Door_R.transform.GetChild(0).position, phase3Start_R.transform.position, 0.5f));
            yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, phase3Start_R.transform.position, 0.5f));
            Debug.Log("[RunPhase2 LS6] in phase 2 End");
        }



        //yield return StartCoroutine(targetDoor.GetComponent<Doors>().Close());
        yield return null;
    }

    public int ConvertToInteger(string seq_no)
    {
        int output32;
        bool isNumber = Int32.TryParse(seq_no, out output32);

        return output32;
    }
    public Vector3 findStartPosSeqphaseone(string seq_no)
    {
        int output32;
        bool isNumber = Int32.TryParse(seq_no, out output32);
        Debug.Log("[RunPhase1 findStartPosSeqphaseone()] " + output32);

        switch (output32) {
            case 1:
                return phase1Start_L.transform.position;
                break;
            case 2:
                return phase1Start_R.transform.position;
                break;
            case 3:
                return phase2Start_L.transform.position;
                break;
            case 4:
                return phase2Start_R.transform.position;
                break;
            case 5:
                return phase3Start_L.transform.position;
                break;
            case 6:
                return phase3Start_R.transform.position;
                break;
            default:
                return new Vector3(0, 0, 0);
                break;
        }
        return new Vector3(0, 0, 0);
    }

    public Vector3 findEndPosSeqphaseone(string seq_no)
    {
        int output32;
        bool isNumber = Int32.TryParse(seq_no, out output32);
        Debug.Log("[RunPhase1 findEndPosSeqphaseone()] " + output32);
        switch (output32)
        {
            case 1:
                return phase1End_L.transform.position;
                break;
            case 2:
                return phase1End_R.transform.position;
                break;
            case 3:
                return phase2End_L.transform.position;
                break;
            case 4:
                return phase2End_R.transform.position;
                break;
            case 5:
                return phase3End_L.transform.position;
                break;
            case 6:
                return phase3End_R.transform.position;
                break;
            default:
                return new Vector3(0, 0, 0);
                break;
        }
        return new Vector3(0, 0, 0);
    }

    public bool IsDoorPosSeqphaseone(string seq_no)
    {
        int output32;
        bool isNumber = Int32.TryParse(seq_no, out output32);
        Debug.Log("[RunPhase1 IsDoorPosSeqphaseone()] " + output32);

        switch (output32)
        {
            case 1:
                return true;
                break;
            case 2:
                return true;
                break;
            case 3:
                return false;
                break;
            case 4:
                return true;
                break;
            case 5:
                return true;
                break;
            case 6:
                return false;
                break;
            default:
                return false;
                break;
        }

        return false;

    }

    public Vector3 findDoorPosSeqphaseone(string seq_no)
    {
        int output32;
        bool isNumber = Int32.TryParse(seq_no, out output32);
        Debug.Log("[RunPhase1 findDoorPosSeqphaseone()] " + output32);

        switch (output32)
        {
            case 1:
                return phase1Door_L.transform.position;
                break;
            case 2:
                return phase1Door_R.transform.position;
                break;
            case 3:
                return phase2Door_L.transform.position;
                break;
            case 4:
                return phase2Door_R.transform.position;
                break;
            default:
                return new Vector3(0, 0, 0);
                break;
        }
        return new Vector3(0, 0, 0);
    }

    IEnumerator RunPhaseSequence()
    {
        EnablePlayerCam(true);
        clearCameraZoneFlags = false;
        pretrainingPeriodGroup.alpha = 1f;
        //Vector3 startPos = phase1Start_L.transform.position;
        //Vector3 endPos = phase1End_L.transform.position;

        Vector3 startPos = findStartPosSeqphaseone(training_1_seq[0]);
        Vector3 endPos = findEndPosSeqphaseone(training_1_seq[0]);
        camVehicle.transform.position = startPos;

        counter_Val.alpha = 1f;
        counter_Val_Text.text = "1";
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 1, 1);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[0]), 1);
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));

        if (IsDoorPosSeqphaseone(training_1_seq[0]))
            Experiment.Instance.shopLiftLog.LogDoorPosition(findDoorPosSeqphaseone(training_1_seq[0]).x,
                                                        findDoorPosSeqphaseone(training_1_seq[0]).y,
                                                        findDoorPosSeqphaseone(training_1_seq[0]).z);
        Debug.Log("RunPhaseSequence: 1");

        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[0]), 0);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 1, 0);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return new WaitForSeconds(1f);
        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;

        yield return StartCoroutine(ExecuteDirectoryD6());

        counter_Val_Text.text = "2";
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 2, 1);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[1]), 1);
        //startPos = phase2Start_L.transform.position;
        //endPos = phase2End_L.transform.position;
        startPos = findStartPosSeqphaseone(training_1_seq[1]);
        endPos = findEndPosSeqphaseone(training_1_seq[1]);
        camVehicle.transform.position = startPos;
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));

        if (IsDoorPosSeqphaseone(training_1_seq[1]))
            Experiment.Instance.shopLiftLog.LogDoorPosition(findDoorPosSeqphaseone(training_1_seq[1]).x,
                                                        findDoorPosSeqphaseone(training_1_seq[1]).y,
                                                        findDoorPosSeqphaseone(training_1_seq[1]).z);

        Debug.Log("RunPhaseSequence: 2");
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[1]), 0);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 2, 0);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return new WaitForSeconds(1f);
        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;


        yield return StartCoroutine(ExecuteDirectoryD6());

        counter_Val_Text.text = "3";
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 3, 1);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[2]), 1);
        //startPos = phase3Start_L.transform.position;
        //endPos = phase3End_L.transform.position;
        startPos = findStartPosSeqphaseone(training_1_seq[2]);
        endPos = findEndPosSeqphaseone(training_1_seq[2]);
        camVehicle.transform.position = startPos;
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));

        if (IsDoorPosSeqphaseone(training_1_seq[2]))
            Experiment.Instance.shopLiftLog.LogDoorPosition(findDoorPosSeqphaseone(training_1_seq[2]).x,
                                        findDoorPosSeqphaseone(training_1_seq[2]).y,
                                        findDoorPosSeqphaseone(training_1_seq[2]).z);

        Debug.Log("RunPhaseSequence: 3");
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[2]), 0);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 3, 0);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return new WaitForSeconds(1f);

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;


        yield return StartCoroutine(ExecuteDirectoryD6());

        counter_Val_Text.text = "4";
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 4, 1);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[3]), 1);
        //startPos = phase1Start_R.transform.position;
        //endPos = phase1End_R.transform.position;
        startPos = findStartPosSeqphaseone(training_1_seq[3]);
        endPos = findEndPosSeqphaseone(training_1_seq[3]);
        camVehicle.transform.position = startPos;
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));

        if (IsDoorPosSeqphaseone(training_1_seq[3]))
            Experiment.Instance.shopLiftLog.LogDoorPosition(findDoorPosSeqphaseone(training_1_seq[3]).x,
                                                        findDoorPosSeqphaseone(training_1_seq[3]).y,
                                                        findDoorPosSeqphaseone(training_1_seq[3]).z);

        Debug.Log("RunPhaseSequence: 4");
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[3]), 0);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 4, 0);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return new WaitForSeconds(1f);

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;


        yield return StartCoroutine(ExecuteDirectoryD6());

        counter_Val_Text.text = "5";
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 5, 1);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[4]), 1);
        //startPos = phase2Start_R.transform.position;
        //endPos = phase2End_R.transform.position;
        startPos = findStartPosSeqphaseone(training_1_seq[4]);
        endPos = findEndPosSeqphaseone(training_1_seq[4]);
        camVehicle.transform.position = startPos;
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));

        if (IsDoorPosSeqphaseone(training_1_seq[4]))
            Experiment.Instance.shopLiftLog.LogDoorPosition(findDoorPosSeqphaseone(training_1_seq[4]).x,
                                                        findDoorPosSeqphaseone(training_1_seq[4]).y,
                                                        findDoorPosSeqphaseone(training_1_seq[4]).z);

        Debug.Log("RunPhaseSequence: 5");
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[4]), 0);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 5, 0);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return new WaitForSeconds(1f);

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;

        yield return StartCoroutine(ExecuteDirectoryD6());

        counter_Val_Text.text = "6";
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 6, 1);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[5]), 1);
        //startPos = phase3Start_R.transform.position;
        //endPos = phase3End_R.transform.position;
        startPos = findStartPosSeqphaseone(training_1_seq[5]);
        endPos = findEndPosSeqphaseone(training_1_seq[5]);
        camVehicle.transform.position = startPos;
        yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));

        if (IsDoorPosSeqphaseone(training_1_seq[5]))
            Experiment.Instance.shopLiftLog.LogDoorPosition(findDoorPosSeqphaseone(training_1_seq[5]).x,
                                                findDoorPosSeqphaseone(training_1_seq[5]).y,
                                                findDoorPosSeqphaseone(training_1_seq[5]).z);

        Debug.Log("RunPhaseSequence: 6");
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, ConvertToInteger(training_1_seq[5]), 0);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, 6, 0);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;
        yield return new WaitForSeconds(1f);

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;

        counter_Val.alpha = 0f;
        clearCameraZoneFlags = true;
        pretrainingPeriodGroup.alpha = 0f;
        yield return null;
    }


    IEnumerator RunPhaseThree(int pathIndex, bool isDirect, bool hasRewards, int selected)
    {
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, (pathIndex % 2) + 5, 1);
        if (isDirect)
        {
            EnablePlayerCam(true);
            SpawnSuitcase(pathIndex); //this will only run for slider training phase where we are directly spawned into the last room in the corridor
        }
        ChangeCamZoneFocus((pathIndex == 0) ? 2 : 5);


        currentPathIndex = pathIndex;
        currentRoomIndex = 3;

        Vector3 startPos = (pathIndex == 0) ? phase3Start_L.transform.position : phase3Start_R.transform.position;
        Vector3 endPos = (pathIndex == 0) ? phase3End_L.transform.position : phase3End_R.transform.position;
        /*if (isDirect) {

        currentAudio = (pathIndex == 0) ? three_L_Audio : three_R_Audio;
        float delay = UnityEngine.Random.Range (0f, currentAudio.clip.length);
        currentAudio.time = delay;
        currentAudio.Play ();
        }
        */
        UnityEngine.Debug.Log("Hey Stage update Env 0 L: " + envManager.phase3End_L.transform.position.z);
        UnityEngine.Debug.Log("Hey Stage update Env 0 R: " + envManager.phase3End_R.transform.position.z);
        UnityEngine.Debug.Log("Hey Stage update L: " + phase3End_L.transform.position.z);
        UnityEngine.Debug.Log("Hey Stage update R: " + phase3End_R.transform.position.z);
        UnityEngine.Debug.Log("Hey Stage update endPos: " + endPos.z);

        clearCameraZoneFlags = false;
        Debug.Log("running phase three");
        camVehicle.transform.position = startPos;
        Debug.Log("velo player in phase 3");
        Experiment.Instance.shopLiftLog.LogMoveEvent(3, true);

        //if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation && camVehicle.transform.position.z > startPos.z)
        if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
        {
            //camVehicle.transform.position = new Vector3(startPos.x, startPos.y, camVehicle.transform.position.z);
            Debug.Log("velo player in phase 3");

            if (selected == 3)
            {
                Experiment.Instance.shopLiftLog.LogSecCameraPos(security_camera.transform.position.x,
                                                security_camera.transform.position.y,
                                                security_camera.transform.position.z);
                globalCamSelectDisable = true;
            }
            yield return StartCoroutine(VelocityPlayerTo(camVehicle.transform.position, endPos, phase1Factor));
            if (selected == 3)
            {
                if (globalCamSelectDisable == true)
                {
                    Experiment.Instance.shopLiftLog.LogSecCameraStatus(2);
                }
                globalCamSelectDisable = false;
            }
        }
        else
        {
            camVehicle.transform.position = startPos;
            Debug.Log("velo player in phase 3");

            if (selected == 3)
            {
                Experiment.Instance.shopLiftLog.LogSecCameraPos(security_camera.transform.position.x,
                                                security_camera.transform.position.y,
                                                security_camera.transform.position.z);
                globalCamSelectDisable = true;
            }
            yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));
            if (selected == 3)
            {
                if (globalCamSelectDisable == true)
                {
                    Experiment.Instance.shopLiftLog.LogSecCameraStatus(2);
                }
                globalCamSelectDisable = false;
            }
        }

        //yield return StartCoroutine(VelocityPlayerTo(startPos, endPos, phase1Factor));
        Experiment.Instance.shopLiftLog.LogMoveEvent(3, false);
        clearCameraZoneFlags = true;
        if (hasRewards)
        {
            if (!((ExperimentSettings.env == ExperimentSettings.Environment.Apartment &&
                expSettings.stage == ExperimentSettings.Stage.Reevaluation) || (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)))
            {

                if (pathIndex == 0)
                {
                    yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, register_L.transform.position, 0.5f));

                }
                else if (pathIndex == 1)
                {
                    yield return StartCoroutine(MovePlayerTo(camVehicle.transform.position, register_R.transform.position, 0.5f));
                }
            }
            Experiment.Instance.shopLiftLog.LogWaitEvent("REGISTER", true);
            if (activeCamZone != null)
                activeCamZone.GetComponent<CameraZone>().isFocus = false;

            string newText = registerText + registerType;
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(5, 1);
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(2, (pathIndex % 2) + 5, 0);
            yield return StartCoroutine(WaitForDoorOpenPress(newText));
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(5, 0);
            Experiment.Instance.shopLiftLog.LogWaitEvent("REGISTER", false);
            yield return StartCoroutine(ShowRegisterReward(pathIndex, isDirect));

            
            Debug.Log("closing the third door now");
        }
        //	currentAudio.Stop ();

        yield return null;
    }

    void SpawnSuitcase(int pathIndex)
    {

        //register_L.transform.position = envManager.register_L.transform.position;
        //register_R.transform.position = envManager.register_R.transform.position;
        suitcaseObj = Instantiate((pathIndex == 0) ? leftSuitcase : rightSuitcase, ((pathIndex == 0) ? register_L.transform.position : register_R.transform.position) + (new Vector3(0f, 0.35f, directionEnv) * 2f), Quaternion.identity) as GameObject;

        if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
        {
            Debug.Log("NOT WESTERN TOWN");
            suitcaseObj.transform.eulerAngles = (System.Math.Abs(directionEnv - 1f) < double.Epsilon) ? new Vector3(180f, 0f, 0f) : new Vector3(180f, 0f, 180f);
            //				suitcaseObj = suitcaseObj.transform.GetChild (0).gameObject;
        }
        else if (ExperimentSettings.env == ExperimentSettings.Environment.Office)
        {
            suitcaseObj.transform.eulerAngles = new Vector3(0f, 90f, 0f);



        }
        else if (ExperimentSettings.env == ExperimentSettings.Environment.Apartment)
        {
            suitcaseObj.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            if (expSettings.stage == ExperimentSettings.Stage.Reevaluation && reevaluation_stage == 0)
            {
                suitcaseObj.transform.position += new Vector3(0f, 0f, 1.63f);
                phase3End_L.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_L.transform.position.z);
                phase3End_R.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_R.transform.position.z);
                envManager.phase3End_L.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_L.transform.position.z);
                envManager.phase3End_R.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_R.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage Env 0 L: " + envManager.phase3End_L.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage Env 0 R: " + envManager.phase3End_R.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage 0 L: " + phase3End_L.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage 0 R: " + phase3End_R.transform.position.z);
                //register_L.transform.position.z = phase3End_L.transform.position.z;
                //register_R.transform.position.z = phase3End_R.transform.position.z;
            }
            else if (expSettings.stage == ExperimentSettings.Stage.Reevaluation && reevaluation_stage == 1)
            {
                suitcaseObj.transform.position += new Vector3(0f, 0f, -1.56f);
                phase3End_L.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_L.transform.position.z);
                phase3End_R.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_R.transform.position.z);
                envManager.phase3End_L.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_L.transform.position.z);
                envManager.phase3End_R.transform.position += new Vector3(0, 0, suitcaseObj.transform.position.z + 2f - phase3End_R.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage Env 1 L: " + envManager.phase3End_L.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage Env 1 R: " + envManager.phase3End_R.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage 1 L: " + phase3End_L.transform.position.z);
                UnityEngine.Debug.Log("Hey Stage 1 R: " + phase3End_R.transform.position.z);
                //register_L.transform.position = phase3End_L.transform.position;
                //register_R.transform.position = phase3End_R.transform.position;
            }

        }

        else if (ExperimentSettings.env == ExperimentSettings.Environment.VikingVillage)
        {
            suitcaseObj.transform.position = suitcaseObj.transform.position + new Vector3(0f, -1f, 4f);
        }
    }

    public bool ShouldShowTips()
    {
        if (consecutiveIncorrectCameraPresses >= 4 || didTimeout || afterSlider)
        {
            afterSlider = false;
            return true;
        }
        else
            return false;

    }




    IEnumerator RunLearningPhase(bool isPostTest, int maxTrials)
    {
        Debug.Log("running task");
        CameraZone.firstTime = false;
        int sliderCount = 0;
        if (!isPostTest)
        {
            //ChangeCameraZoneVisibility (false); // no need to show cam zones as they were already shown during training
            //stage 1
            Experiment.Instance.shopLiftLog.LogPhaseEvent("LEARNING", true);

        }
        else
        {
            numTrials_Learning = 0;
            maxTrials = maxTrials;
            Experiment.Instance.shopLiftLog.LogPhaseEvent("POST-TEST", true);
        }
        bool isLeft = (UnityEngine.Random.value < 0.5f) ? true : false;
        bool showOneTwo = false;


        //current list of slider-trials during learning phase are 3,7,11,15,19,23

        int trialsToNextSlider = 4; //should be 4 in the beginning
        List<int> randOrder = new List<int>();
        int randIndex = 0;
        randOrder = GiveRandSequenceOfTwoInts(0, 1, trialsToNextSlider);

        //		while(numTrials < 1)
        bool showOnce = true;
        int maxDeviationQueueLength = 2;
        float deviationThreshold = 0.4f;

        int count = 0;
        maxAdditionalTrials = 0;
        int Real_trail = 0;
        int temp_Real_trail = 0;
        var Rlist = new List<int> { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 };
        var random = new System.Random();
        var temp_Rlist = new List<int> { 0, 1 };
        int Sum_Real_trail = 0;
        int ind = 0;
        while (numTrials_Learning < maxTrials || (!isPostTest && !Config.shouldForceControl && !hasLearned && numAdditionalTrials < maxAdditionalTrials))
        {
            if (numTrials_Learning % 2 == 0)
            {
                //int ind = random.Next(Rlist.Count);
                Real_trail = Rlist[ind];
                //Rlist.RemoveAt(ind);

                if (learning_seq[numTrials_Learning / 2] == "A")
                    temp_Rlist = new List<int> { 0, 1 };
                else
                    temp_Rlist = new List<int> { 1, 0 };
                //int ind2 = random.Next(temp_Rlist.Count);
                int ind2 = 0;
                temp_Real_trail = temp_Rlist[ind2];
                temp_Rlist.RemoveAt(ind2);

                ind = ind + 1;

            }
            else
            {
                int ind2 = random.Next(temp_Rlist.Count);
                temp_Real_trail = temp_Rlist[ind2];
                temp_Rlist.RemoveAt(ind2);
            }

            Sum_Real_trail = Real_trail + temp_Real_trail;

            randOrder[0] = Sum_Real_trail % 2;

            if (numTrials_Learning % 2 == 0)
            {
                count += 1;
            }
            Debug.Log("about to run phase 1");
            if (randOrder[0] == 0)
            {
                isLeft = true;

            }
            else
                isLeft = false;

            if (numTrials_Learning % 2 == 0)
                counter_Val_Text.text = count.ToString();
            else
                counter_Val_Text.text = count.ToString();

            //randOrder.RemoveAt(0);
            if (numTrials_Learning % 2 == 0)
                Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, count + 9, 1);
            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(3, ((isLeft == true) ? 0 : 1), 1);
            counter_Val.alpha = 1f;
            learningPeriodGroup.alpha = 1f;

            List<int> l = new List<int>() { 1, 2, 3 };
            System.Random rnd = new System.Random();
            int index = rnd.Next(l.Count);
            int selected = l[index];

            List<int> xl = new List<int>() { -1, 1 };
            int indexx = rnd.Next(xl.Count);
            int selectedx = xl[indexx];

            float value_fixed = 0.9f;

            security_camera.gameObject.GetComponent<MeshRenderer>().enabled = true;
            if (selected == 3)
            {
                value_fixed = 0.99f;
                security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
            }
            else if (selected == 2)
            {
                security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
            }
            else if (selected == 1)
            {
                security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
            }

            if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
            {
                security_camera.gameObject.transform.localScale = new Vector3(20f, 20f, 20f);
                if (isLeft)
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_L.transform.position + value_fixed * phase1End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_L.transform.position + value_fixed * phase2End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_L.transform.position + value_fixed * phase3End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));

                }
                else
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_R.transform.position + value_fixed * phase1End_R.transform.position)) + (selectedx * new Vector3(0f, selectedx * 5f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_R.transform.position + value_fixed * phase2End_R.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_R.transform.position + value_fixed * phase3End_R.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                }
            }
            else if (ExperimentSettings.env == ExperimentSettings.Environment.Apartment)
            {
                security_camera.gameObject.transform.localScale = new Vector3(8f, 8f, 8f);
                if (isLeft)
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_L.transform.position + value_fixed * phase1End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_L.transform.position + value_fixed * phase2End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_L.transform.position + value_fixed * phase3End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));

                }
                else
                {
                    if (selected == 1)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_R.transform.position + value_fixed * phase1End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 2)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_R.transform.position + value_fixed * phase2End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    else if (selected == 3)
                        security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_R.transform.position + value_fixed * phase3End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                }
            }

            security_camera.gameObject.transform.localEulerAngles = new Vector3(0, -selectedx * 45, 0);

            yield return StartCoroutine(RunPhaseOne((isLeft) ? 0 : 1, false, selected));
            Experiment.Instance.shopLiftLog.LogDoorPosition(((isLeft) ? phase1Door_L : phase1Door_R).transform.position.x, ((isLeft) ? phase1Door_L : phase1Door_R).transform.position.y, ((isLeft) ? phase1Door_L : phase1Door_R).transform.position.z);

            Debug.Log("about to run phase 2");

            yield return StartCoroutine(RunPhaseTwo((isLeft) ? 0 : 1, false, true, selected));
            Experiment.Instance.shopLiftLog.LogDoorPosition(((isLeft) ? phase2Door_L : phase2Door_R).transform.position.x, ((isLeft) ? phase2Door_L : phase2Door_R).transform.position.y, ((isLeft) ? phase2Door_L : phase2Door_R).transform.position.z);

            Debug.Log("about to run phase 3");
            if (!isPostTest)
                yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, false, true, selected));
            else
                yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, false, false, selected));
            Experiment.Instance.shopLiftLog.LogChestPosition(((isLeft) ? register_L : register_R).transform.position.x, ((isLeft) ? register_L : register_R).transform.position.y, ((isLeft) ? register_L : register_R).transform.position.z);

            //			TurnOffRooms ();
            Debug.Log("num trials learning " + Sum_Real_trail.ToString() + " " + Real_trail.ToString());
            learningPeriodGroup.alpha = 0f;
            counter_Val.alpha = 0f;

            //blackScreen.alpha = 0f;

            Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(3, ((isLeft == true) ? 0 : 1), 0);
            if (numTrials_Learning % 2 == 1)
                Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, count + 9, 0);

            /* Enable below for the dynamic sequence of questions. For nowm this is enabled.
             * Instead of Static
             * Q0, Q1, Q2 - Cash
             * Q3, Q4, Q5, Q6 - Structure
             * (Taken from the Resources_IGNORE/question_sequence.txt)
             */

            if ((numTrials_Learning) % 2 == 1)
            {
                switch (questionpattern[numTrials_Learning / 2])
                {
                    case 0:
                        //Q0
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 1:
                        //Q1
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 2:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 1));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 1));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 3:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        //(Real_trail / 2) % 2 == 0 : Q3
                        //(Real_trail / 2) % 2 == 1 : Q4
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1);
                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);

                        break;
                    case 4:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        //(Real_trail / 2) % 2 == 0 : Q3
                        //(Real_trail / 2) % 2 == 1 : Q4
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1 + 1);

                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1 + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1 + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1 + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1 + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 5:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        //(Real_trail / 2) % 2 == 0 : Q5
                        //(Real_trail / 2) % 2 == 1 : Q6

                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, 3);

                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 6:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        //(Real_trail / 2) % 2 == 0 : Q5
                        //(Real_trail / 2) % 2 == 1 : Q6

                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, 1 + 3);

                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 1 + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 1 + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 1 + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 1 + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;

                }
            }

            /*Enable the below code to trigger the static question sequence pattern 
             * Q0, Q1, Q2 - Cash
             * Q3, Q4, Q5, Q6 - Structure
             * Sequence: Q0, Q1, Q3, Q4, Q0, Q1, Q5, Q6, Q0, Q3
             */

            /*if (Real_trail < 15)
            {
                if (((((Real_trail + 1) % 16 == 1) || (Real_trail + 1) % 16 == 9)) && ((numTrials_Learning) % 2 == 1))
                {

                    //Q0
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                    multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                    if (learning_ques[(numTrials_Learning) / 2] == "A")
                        yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0));
                    else
                        yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1));
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                }
                else if (((((Real_trail + 1) % 16 == 3) || (Real_trail + 1) % 16 == 11)) && ((numTrials_Learning) % 2 == 1))
                {

                    //Q1
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                    multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                    if (learning_ques[(numTrials_Learning) / 2] == "A")
                        yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0));
                    else
                        yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1));
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                }
                else if (((((Real_trail + 1) % 16 == 5) || (Real_trail + 1) % 16 == 7)) && ((numTrials_Learning) % 2 == 1))
                {
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                    //(Real_trail / 2) % 2 == 0 : Q3
                    //(Real_trail / 2) % 2 == 1 : Q4
                    Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, (((Real_trail / 2)) % 2) + 1);

                    if (learning_ques[(numTrials_Learning) / 2] == "A")
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                        if ((((Real_trail / 2)) % 2) == 0)
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 5);
                        else
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 6);
                        yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2)) % 2, 0, 0));
                        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 5.0f + MultipleChoiceslider.value);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                        if ((((Real_trail / 2)) % 2) == 0)
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 5);
                        else
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 6);
                        yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2)) % 2, 0, 1));
                        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 6.0f - MultipleChoiceslider.value);
                    }
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                }
                else if (((((Real_trail + 1) % 16 == 13) || (Real_trail + 1) % 16 == 15)) && ((numTrials_Learning) % 2 == 1))
                {
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                    //(Real_trail / 2) % 2 == 0 : Q5
                    //(Real_trail / 2) % 2 == 1 : Q6

                    Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, (((Real_trail / 2)) % 2) + 3);

                    if (learning_ques[(numTrials_Learning) / 2] == "A")
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                        if ((((Real_trail / 2)) % 2) == 0)
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, (((Real_trail / 2)) % 2) + 3, 5);
                        else
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, (((Real_trail / 2)) % 2) + 3, 6);
                        yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2)) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, (((Real_trail / 2)) % 2) + 3, 5.0f + MultipleChoiceslider.value);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                        if ((((Real_trail / 2)) % 2) == 0)
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, (((Real_trail / 2)) % 2) + 3, 5);
                        else
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, (((Real_trail / 2)) % 2) + 3, 6);
                        yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2)) % 2, 1, 1));
                        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, (((Real_trail / 2)) % 2) + 3, 6.0f - MultipleChoiceslider.value);
                    }
                    Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                }
            }
            else
            {
                if (_currentReevalCondition != 1)
                {
                    if (((Real_trail + 1) % 8 == 1) && ((numTrials_Learning) % 2 == 1))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((Real_trail + 1) % 8 == 3) && ((numTrials_Learning) % 2 == 1))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1);

                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            if ((((Real_trail / 2) + 1) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2) + 1) % 2, 0, 0));   //0
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            if ((((Real_trail / 2) + 1) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2) + 1) % 2, 0, 1));   //0*
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, (((Real_trail / 2) + 1) % 2) + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                }
                else
                {
                    if (((Real_trail + 1) % 8 == 1) && ((numTrials_Learning) % 2 == 1))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((Real_trail + 1) % 8 == 3) && ((numTrials_Learning) % 2 == 1))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, (((Real_trail / 2)) % 2) + 1);

                        if (learning_ques[(numTrials_Learning) / 2] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            if ((((Real_trail / 2)) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2)) % 2, 0, 0));   //1
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            if ((((Real_trail / 2)) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(((Real_trail / 2)) % 2, 0, 1));   //1
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, (((Real_trail / 2)) % 2) + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                }

            }*/


            //blackScreen.alpha = 0f;

            /*if ((numTrials_Learning+1)%4==0 && numTrials_Learning >0)
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
			}*/


            if (numTrials_Learning >= maxTrials && !hasLearned && showOnce && !Config.shouldForceControl)
            {
                //intertrialGroup.alpha = 1f;
                maxDeviationQueueLength = 1;
                deviationThreshold = 0.35f;
                /*if(expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
                    intertrialText.text = "Por favor, tenga en cuenta la estructura de las habitaciones y las recompensas al responder";
                else
                    intertrialText.text = "Please, consider the structure of the rooms and the rewards when responding";
                intertrialText.text = "";
                intertrialGroup.alpha = 0f;*/
                showOnce = false;
            }


            if (numTrials_Learning < maxTrials - 1 || (!hasLearned && numAdditionalTrials < maxAdditionalTrials - 1))
                yield return StartCoroutine(ShowEndTrialScreen(false, ShouldShowTips()));
            else if (!isPostTest)
                yield return StartCoroutine(ShowNextStageScreen());
            numTrials_Learning++;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;
            //this indicates that we are in additional_learning phase until the subject has learned the structure
            if (numTrials_Learning >= maxTrials)
            {
                numAdditionalTrials++;
            }
            yield return 0;
        }

        camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled = false;
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
        /*if (hasLearned && !Config.shouldForceControl)
        {*/
        switch (reevalConditionIndex)
        {
            case 0:
                Debug.Log("IT'S RR");
                SetupRewardReeval();
                leftChoice = false;
                break;
            case 1:
                Debug.Log("IT'S TR");
                SetupTransitionReeval();
                leftChoice = true;
                break;
            case 2:
                Debug.Log("IT'S RC");
                leftChoice = false;
                break;
            case 3:
                Debug.Log("IT'S TC");
                leftChoice = false;
                break;
        }
        /*}
        else
        {
            Debug.Log("hasn't learnt so this will be CONTROL");
        }*/
        Experiment.Instance.shopLiftLog.LogPhaseEvent("RE-EVALUATION", true);

        int count = 0;

        int Real_trail = 0;
        int temp_Real_trail = 0;
        var Rlist = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var random = new System.Random();
        var temp_Rlist = new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        int Sum_Real_trail = 0;
        int ind = 0;
        while (numBlocks_Reeval < maxBlocks_Reeval)
        {

            count += 1;

            //int ind = random.Next(Rlist.Count);
            Real_trail = Rlist[ind];
            //Rlist.RemoveAt(ind);
            ind = ind + 1;

            if (reeval_seq[numBlocks_Reeval] == "A")
                temp_Rlist = new List<int> { 0, 1 };
            else
                temp_Rlist = new List<int> { 1, 0 };

            while (numTrials_Reeval < maxTrials_Reeval)
            {

                if (numTrials_Reeval % 2 == 0)
                {
                    //temp_Rlist = new List<int> { 0, 1 };
                    //int ind2 = random.Next(temp_Rlist.Count);
                    int ind2 = 0;
                    temp_Real_trail = temp_Rlist[ind2];
                    temp_Rlist.RemoveAt(ind2);

                }
                else
                {
                    temp_Real_trail = (temp_Real_trail + 1) % 2;
                }
                Debug.Log("Reevaluation: " + temp_Real_trail);
                Sum_Real_trail = Real_trail + temp_Real_trail;

                if (temp_Real_trail == 0)
                    leftChoice = true;
                else
                    leftChoice = false;

                //leftChoice = !leftChoice; //flip it

                reevaluationPeriodGroup.alpha = 1f;

                if (numTrials_Reeval % 2 == 0)
                {

                    counter_Val_Text.text = count.ToString();
                }
                else
                    counter_Val_Text.text = count.ToString();

                if (numTrials_Reeval % 2 == 0)
                    Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, count + 19, 1);
                Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(3, ((leftChoice == true) ? 0 : 1), 1);
                reevaluation_stage = (temp_Real_trail + 1) % 2;
                counter_Val.alpha = 1f;

                List<int> l = new List<int>() { 1, 2, 3 };
                System.Random rnd = new System.Random();
                int index = rnd.Next(l.Count);
                int selected = l[index];

                List<int> xl = new List<int>() { -1, 1 };
                int indexx = rnd.Next(xl.Count);
                int selectedx = xl[indexx];

                float value_fixed = 0.9f;


                security_camera.gameObject.GetComponent<MeshRenderer>().enabled = true;
                if (selected == 3)
                {
                    value_fixed = 0.99f;
                    security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
                }
                else if (selected == 2)
                {
                    security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
                }
                else if (selected == 1)
                {
                    security_camera.gameObject.GetComponent<MeshRenderer>().material = SecurityCamMat_Black;
                }

                if (ExperimentSettings.env == ExperimentSettings.Environment.SpaceStation)
                {
                    security_camera.gameObject.transform.localScale = new Vector3(20f, 20f, 20f);
                    if (leftChoice)
                    {
                        if (selected == 1)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_L.transform.position + value_fixed * phase1End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                        else if (selected == 2)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_L.transform.position + value_fixed * phase2End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                        else if (selected == 3)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_L.transform.position + value_fixed * phase3End_L.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));

                    }
                    else
                    {
                        if (selected == 1)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_R.transform.position + value_fixed * phase1End_R.transform.position)) + (selectedx * new Vector3(0f, selectedx * 5f, 0));
                        else if (selected == 2)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_R.transform.position + value_fixed * phase2End_R.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                        else if (selected == 3)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_R.transform.position + value_fixed * phase3End_R.transform.position)) + (selectedx * new Vector3(5f, selectedx * 5f, 0));
                    }
                }
                else if (ExperimentSettings.env == ExperimentSettings.Environment.Apartment)
                {
                    security_camera.gameObject.transform.localScale = new Vector3(8f, 8f, 8f);
                    if (leftChoice)
                    {
                        if (selected == 1)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_L.transform.position + value_fixed * phase1End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                        else if (selected == 2)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_L.transform.position + value_fixed * phase2End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                        else if (selected == 3)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_L.transform.position + value_fixed * phase3End_L.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));

                    }
                    else
                    {
                        if (selected == 1)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase1Start_R.transform.position + value_fixed * phase1End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                        else if (selected == 2)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase2Start_R.transform.position + value_fixed * phase2End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                        else if (selected == 3)
                            security_camera.gameObject.transform.position = (((1 - value_fixed) * phase3Start_R.transform.position + value_fixed * phase3End_R.transform.position)) + (selectedx * new Vector3(1.2f, selectedx * 1.9f, 0));
                    }
                }

                security_camera.gameObject.transform.localEulerAngles = new Vector3(0, -selectedx * 45, 0);

                Debug.Log("about to run phase 1");
                yield return StartCoroutine(RunPhaseOne((leftChoice) ? 0 : 1, false, selected));
                Experiment.Instance.shopLiftLog.LogDoorPosition(((leftChoice) ? phase1Door_L : phase1Door_R).transform.position.x, ((leftChoice) ? phase1Door_L : phase1Door_R).transform.position.y, ((leftChoice) ? phase1Door_L : phase1Door_R).transform.position.z);

                Debug.Log("about to run phase 2");
                yield return StartCoroutine(RunPhaseTwo((leftChoice) ? 0 : 1, true, true, selected));
                Experiment.Instance.shopLiftLog.LogDoorPosition(((leftChoice) ? phase2Door_L : phase2Door_R).transform.position.x, ((leftChoice) ? phase2Door_L : phase2Door_R).transform.position.y, ((leftChoice) ? phase2Door_L : phase2Door_R).transform.position.z);

                Debug.Log("about to run phase 3");
                yield return StartCoroutine(RunPhaseThree((leftChoice) ? 0 : 1, false, true, selected));
                Experiment.Instance.shopLiftLog.LogChestPosition(((leftChoice) ? register_L : register_R).transform.position.x, ((leftChoice) ? register_L : register_R).transform.position.y, ((leftChoice) ? register_L : register_R).transform.position.z);

                
                counter_Val.alpha = 0f;


                Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(3, ((leftChoice == true) ? 0 : 1), 0);
                if (numTrials_Reeval % 2 == 1)
                    Experiment.Instance.shopLiftLog.LogExpTrialInfoStatusStartEnd(1, count + 19, 0);

                //				TurnOffRooms ();
                if (numTrials_Reeval < maxTrials_Reeval - 1)
                    yield return StartCoroutine(ShowEndTrialScreen(false, ShouldShowTips()));
                numTrials_Reeval++;

                reevaluationPeriodGroup.alpha = 0f;

                if (numTrials_Reeval < maxTrials_Reeval)
                {
                    if (blackScreen.alpha == 1f)
                        Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
                    blackScreen.alpha = 0f;
                }
                yield return 0;
            }

            /* Enable below for the dynamic sequence of questions. For nowm this is enabled.
             * Instead of Static
             * Q0, Q1, Q2 - Cash
             * Q3, Q4, Q5, Q6 - Structure
             * (Taken from the Resources_IGNORE/question_sequence.txt)
             */

            if (reevalConditionIndex != 1)
            {
                switch (questionpattern[Real_trail])
                {
                    case 0:
                        //Q0
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 1:
                        //Q1
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(0, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(0, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 2:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 1));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 1));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 3:
                        //(Real_trail + 1) % 8 == 3   - Q3
                        //(Real_trail + 1) % 8 == 4   - Q4
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);

                        break;
                    case 4:
                        //(Real_trail + 1) % 8 == 3   - Q3
                        //(Real_trail + 1) % 8 == 4   - Q4
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1 + 1);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1 + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1 + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1 + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1 + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 5:
                        //(Real_trail + 1) % 8 == 7     - Q5
                        //(Real_trail + 1) % 8 == 8/0   - Q6
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, 3);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 6:
                        //(Real_trail + 1) % 8 == 7     - Q5
                        //(Real_trail + 1) % 8 == 8/0   - Q6
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, 1 + 3);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 1 + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 1 + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 1 + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 1 + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;

                }
            }
            else
            {
                switch (questionpattern[Real_trail])
                {
                    case 0:
                        //Q0
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 1:
                        //Q1
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(0, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(0, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 2:
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 0, 1));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3(1, 1, 1));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 3:
                        //(Real_trail + 1) % 8 == 3   - Q3
                        //(Real_trail + 1) % 8 == 4   - Q4
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);

                        break;
                    case 4:
                        //(Real_trail + 1) % 8 == 3   - Q3
                        //(Real_trail + 1) % 8 == 4   - Q4
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, 1 + 1);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1 + 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1 + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, 1 + 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, 1 + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 5:
                        //(Real_trail + 1) % 8 == 7     - Q5
                        //(Real_trail + 1) % 8 == 8/0   - Q6
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, 3);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2(0, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;
                    case 6:
                        //(Real_trail + 1) % 8 == 7     - Q5
                        //(Real_trail + 1) % 8 == 8/0   - Q6
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, 1 + 3);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 1 + 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 1 + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, 1 + 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2(1, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, 1 + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                        break;

                }
            }

            /*Enable the below code to trigger the static question sequence pattern 
             * Q0, Q1, Q2 - Cash
             * Q3, Q4, Q5, Q6 - Structure
             * Sequence: Q0, Q1, Q3, Q4, Q0, Q1, Q5, Q6, Q0, Q3
             */

            //yield return StartCoroutine (AskPreference (1,false,false,false,0,0f));
            //			yield return StartCoroutine (RunRestPeriod());
            /*if (reevalConditionIndex != 1)
            {
                if (Real_trail <= 7)
                {
                    if (((Real_trail + 1) % 8 == 1) || ((Real_trail + 1) % 8 == 5))
                    {
                        //Q0
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((Real_trail + 1) % 8 == 2) || ((Real_trail + 1) % 8 == 6))
                    {
                        //Q1
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((((Real_trail + 1) % 8 == 3) || (Real_trail + 1) % 8 == 4)))
                    {
                        //(Real_trail + 1) % 8 == 3   - Q3
                        //(Real_trail + 1) % 8 == 4   - Q4
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, ((Real_trail) % 2) + 1);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, false);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, false);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((((Real_trail + 1) % 8 == 7) || (Real_trail + 1) % 8 == 0)))
                    {
                        //(Real_trail + 1) % 8 == 7     - Q5
                        //(Real_trail + 1) % 8 == 8/0   - Q6
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, ((Real_trail) % 2) + 3);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                }
                else
                {
                    if ((Real_trail + 1) % 4 == 1)
                    {
                        //Q0
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if ((Real_trail + 1) % 4 == 2)
                    {
                        //Q3
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, ((Real_trail + 1) % 2) + 3);

                        if (reeval_ques[numBlocks_Reeval] == "A")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, false);
                            if (((Real_trail + 1) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail + 1) % 2, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, false);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, false);
                            if (((Real_trail + 1) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 5);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 6);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail + 1) % 2, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }

                }
            }
            else
            {
                if (Real_trail <= 7)
                {
                    if (((Real_trail + 1) % 8 == 1) || ((Real_trail + 1) % 8 == 5))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((Real_trail + 1) % 8 == 2) || ((Real_trail + 1) % 8 == 6))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((((Real_trail + 1) % 8 == 3) || (Real_trail + 1) % 8 == 4)))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 1, 3, ((Real_trail) % 2) + 1);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 0, true);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 6);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 0, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 1, 3, 1, 1, true);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 6);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 0, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 1, 3, ((Real_trail) % 2) + 1, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if (((((Real_trail + 1) % 8 == 7) || (Real_trail + 1) % 8 == 0)))
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, ((Real_trail) % 2) + 3);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, true);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, true);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                }
                else
                {
                    if ((Real_trail + 1) % 4 == 1)
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(0);
                        if (reeval_ques[numBlocks_Reeval] == "A")
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 0, 0));
                        else
                            yield return StartCoroutine(AskMultipleChoice_v3((Real_trail + 1) % 2, 1, 0));
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                    else if ((Real_trail + 1) % 4 == 2)
                    {
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, ((Real_trail) % 2) + 3);

                        if (reeval_ques[numBlocks_Reeval] == "B")
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, true);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 0));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5.0f + MultipleChoiceslider.value);
                        }
                        else
                        {
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, true);
                            Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, true);
                            if (((Real_trail) % 2) == 0)
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6);
                            else
                                Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 5);
                            yield return StartCoroutine(AskMultipleChoice_v2((Real_trail) % 2, 1, 1));
                            Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail) % 2) + 3, 6.0f - MultipleChoiceslider.value);
                        }
                        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
                    }
                }
            }*/

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;
            /*if (reevalConditionIndex != 1)
            {
                Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetCashQuesOptions(1);
                if (reeval_ques[numBlocks_Reeval] == "A")
                    yield return StartCoroutine(AskMultipleChoice_v3(0, 0));
                else
                    yield return StartCoroutine(AskMultipleChoice_v3(0, 1));
                Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
            }
            else
            {
                Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
                Experiment.Instance.shopLiftLog.LogExpQuesType(2, 2, 3, ((Real_trail + 1) % 2) + 3);

                if (reeval_ques[numBlocks_Reeval] == "B")
                {
                    Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 1, true);
                    Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 0, true);
                    if (((Real_trail + 1) % 2) == 0)
                        Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 6);
                    else
                        Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 5);
                    yield return StartCoroutine(AskMultipleChoice_v2((Real_trail + 1) % 2, 1, 0));
                    Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 5.0f + MultipleChoiceslider.value);
                }
                else
                {
                    Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 0, 0, true);
                    Experiment.Instance.shopLiftLog.LogExpQuesType2(2, 2, 3, 1, 1, true);
                    if (((Real_trail + 1) % 2) == 0)
                        Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 6);
                    else
                        Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 5);
                    yield return StartCoroutine(AskMultipleChoice_v2((Real_trail + 1) % 2, 1, 1));
                    Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(2, 2, 3, ((Real_trail + 1) % 2) + 3, 6.0f - MultipleChoiceslider.value);
                }
                Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);
            }*/
            numTrials_Reeval = 0;
            numBlocks_Reeval++;
            yield return 0;
        }
        yield return StartCoroutine(ShowNextStageScreen());
        Experiment.Instance.shopLiftLog.LogPhaseEvent("RE-EVALUATION", false);
        yield return null;
    }


    //part of the baseline, traverses through all the environments without any audio or chest
    IEnumerator RunSilentTraversal()
    {

        TCPServer.Instance.SetState(TCP_Config.DefineStates.SILENT_TRAVERSAL, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("SILENT_TRAVERSAL", true);

        bool isLeft = (UnityEngine.Random.value > 0.5f) ? true : false;

        //we only want to run the silent traversal on all environments except the Pre-Training environment (which is the last one by default in "environments" List)
        for (int i = 0; i < environments.Count - 1; i++)
        {

            yield return StartCoroutine(PickEnvironment(i, false)); //change environment
            mainSceneListener.enabled = false; //turn off the main audio listener
            AudioListener.pause = true;
            AudioListener.volume = 0f;
            for (int j = 0; j < 2; j++)
            {
                Debug.Log("about to run phase 1");
                isLeft = !isLeft; //flip the left right
                yield return StartCoroutine(RunPhaseOne((isLeft) ? 0 : 1, false, 0));

                Debug.Log("about to run phase 2");
                yield return StartCoroutine(RunPhaseTwo((isLeft) ? 0 : 1, false, false, 0));
                //          TurnOffRooms ();
                Debug.Log("about to run phase 3");
                yield return StartCoroutine(RunPhaseThree((isLeft) ? 0 : 1, false, false, 0));
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
        EnablePlayerCam(false);
        yield return new WaitForSeconds(waitTime);
        restGroup.alpha = 0f;
        yield return null;
    }

    IEnumerator RunImaginePeriod(float waitTime)
    {
        dotGroup.alpha = 1f;
        EnablePlayerCam(false);
        yield return new WaitForSeconds(waitTime);
        dotGroup.alpha = 0f;
        yield return null;
    }
    IEnumerator RunTestingPhase()
    {

        Experiment.Instance.shopLiftLog.LogPhaseEvent("TESTING", true);
        //run one instances of comp slider  + 2sec resting phase

        Experiment.Instance.shopLiftLog.LogQuestionExtremes(true);
        Experiment.Instance.shopLiftLog.LogExpQuesType(1, 3, 1, 0);
        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 1, 0, 0, false);
        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 1, 1, 1, false);
        if (_currentReevalCondition == 1 || _currentReevalCondition == 0)
            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 3, 1, 0, 2);
        else
            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 3, 1, 0, 1);
        yield return StartCoroutine(AskPreference(0, false, false, false, 0, 0f));

        Experiment.Instance.shopLiftLog.LogQuestionExtremes(false);

        /*yield return StartCoroutine (RunRestPeriod (2f));
        int caseOrder = 0;
        if (UnityEngine.Random.value > 0.5f)
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
				yield return StartCoroutine (RunPhaseOne (0, true));
				yield return StartCoroutine (RunImaginePeriod (5f));
				yield return StartCoroutine (AskSoloPreference (0,false));
//				yield return StartCoroutine (AskImageryQualityRating (0));
				yield return StartCoroutine (RunRestPeriod (2f));
				break;
			case 1:
				yield return StartCoroutine(RunPhaseOne (1, true));
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
			int randIndex = UnityEngine.Random.Range (0, multipleChoiceSequence.Count);
            yield return StartCoroutine(AskMultipleChoice(multipleChoiceSequence[randIndex],false));
			multipleChoiceSequence.RemoveAt (randIndex);
            yield return StartCoroutine(RunRestPeriod(3f));
		}
		Experiment.Instance.shopLiftLog.LogPhaseEvent ("TESTING", false);*/
        yield return null;
    }

    IEnumerator RunImageSlideshow()
    {
        //show image slideshow instructions
        TCPServer.Instance.SetState(TCP_Config.DefineStates.IMAGE_BASELINE, true);
        Experiment.Instance.shopLiftLog.LogPhaseEvent("IMAGE_BASELINE", true);

        /*intertrialGroup.alpha = 1f;
        tipsGroup.alpha = 0f;
        intertrialText.text = imageSlideshowInstruction;
        yield return new WaitForSeconds(5f);
        intertrialGroup.alpha = 0f;
        */
        int totalSlideshowLength = completeImageList.Count;
        for (int i = 0; i < totalSlideshowLength; i++)
        {
            slideshowImage.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

            int randIndex = UnityEngine.Random.Range(0, completeImageList.Count);
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

        /*intertrialGroup.alpha = 1f;
        tipsGroup.alpha = 0f;
        intertrialText.text = musicBaselineInstruction;
        yield return new WaitForSeconds(5f);
        intertrialGroup.alpha = 0f;
        */
        int totalAudioLength = completeAudioList.Count;
        for (int i = 0; i < totalAudioLength; i++)
        {
            restGroup.alpha = 1f;
            int randIndex = UnityEngine.Random.Range(0, completeAudioList.Count);
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
        Experiment.Instance.shopLiftLog.LogTransitionReeval();
        ReassignRooms();
        registerVals[0] = 30;
        registerVals[1] = 70;
    }

    void SetupRewardReeval()
    {
        Experiment.Instance.shopLiftLog.LogRewardReeval();
        Debug.Log("count: " + registerVals.Count.ToString());
        registerVals.Reverse();

        registerVals[0] = 30;
        registerVals[1] = 70;
        Debug.Log("count: " + registerVals.Count.ToString());
        Debug.Log("register vals at 0 : " + registerVals[0].ToString());
        Debug.Log("register vals at 1 : " + registerVals[1].ToString());
    }

    void UpdateFirstEnvironments()
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
        imageryQualityGroup.GetComponent<PrefSoloSetup>().prefSlider.value = 0f;
        EnablePlayerCam(false);
        imageryQualityGroup.gameObject.SetActive(true);
        imageryQualityGroup.GetComponent<PrefSoloSetup>().SetupPrefs(prefIndex);
        yield return StartCoroutine(WaitForButtonPress(15f, didPress =>
           {
               Debug.Log("did press: " + didPress);
           }
        ));
        Experiment.Instance.shopLiftLog.LogFinalSliderValue("IMAGERY_QUALITY", imageryQualityGroup.GetComponent<PrefSoloSetup>().prefSlider.value, true);
        imageryQualityGroup.gameObject.SetActive(false);
        Cursor.visible = false;

        yield return null;
    }

    IEnumerator AskSoloPreference(int prefIndex, bool isTraining)
    {

        PrefSoloSetup prefSoloSetup = prefSolo.GetComponent<PrefSoloSetup>();

        prefSoloSetup.prefSlider.value = 0.5f;
        //		Cursor.visible = true;
        //		Cursor.lockState = CursorLockMode.None;
        EnablePlayerCam(false);
        prefSolo.gameObject.SetActive(true);
        prefSoloSetup.SetupPrefs(prefIndex);

        TCPServer.Instance.SetState(TCP_Config.DefineStates.SOLO_SLIDER, true);
        bool pressed = false;

        float tElapsed = 0f;
        float minSelectTime = 1.5f;

        while (tElapsed < minSelectTime)
        {
            if (Experiment.Instance.shopLift.isGamePaused == false)
            {
                tElapsed += Time.deltaTime;
                if (Input.GetButtonDown("Action Button"))
                {

                    infoText.text = "Please take your time to make a choice";
                    infoGroup.alpha = 1f;
                }
            }
            yield return 0;
        }

        infoText.text = "";
        infoGroup.alpha = 0f;


        yield return StartCoroutine(WaitForButtonPress(9f, didPress =>
           {
               pressed = didPress;
           }
        ));

        infoText.text = "Please make a choice";
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

            bool rightSliderIsCorrect = false; //keeps track of whether moving the Solo Slider all the way to the right is the correct response or not

            float deviation = 0f; //how much away from the correct answer was the player's response

            if (focusImg.Contains("Five")) //the focus image room was of a left corridor
            {
                isLeft = true;
                if (leftHigher)
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
            if (rightSliderIsCorrect)
            {
                while (prefSoloSetup.prefSlider.value < 0.6f)
                {

                    yield return 0;
                }
                yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                {
                    pressed = didPress;
                }));
            }
            else
            {
                while (1f - prefSoloSetup.prefSlider.value < 0.6f)
                {

                    yield return 0;
                }
                yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                {
                    pressed = didPress;
                }));
            }




        }


        Experiment.Instance.shopLiftLog.LogFinalSliderValue("SOLO", finalSliderValue, pressed);
        prefSolo.gameObject.SetActive(false);
        Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.SOLO_SLIDER, false);

        yield return null;
    }

    //Used for Structure question
    IEnumerator AskMultipleChoice_v2(int prefIndex, int Start_or_Inter, int swap)
    {
        int correctChoice;
        bool pressed = false;
        float tElapsed = 0f;
        //float minSelectTime = 2f;
        float minSelectTime = 0f;

        Debug.Log("PREF INDEX IS " + prefIndex.ToString());
        EnablePlayerCam(false);
        multipleChoiceGroup.gameObject.SetActive(true);

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;
        multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetFocusImage(true, new Vector3(0, 23, 0), new Vector3(0, 43, 0), 0, 0);
        correctChoice = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetupMultipleChoice_v2(prefIndex, true, swap);
        correctChoice = prefIndex;

        if (Start_or_Inter == 1)
        {
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().ChangeToInterm(true);
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().ChangeFocusImagetoInterm(prefIndex);
        }

        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, true);


        Debug.Log("AskMultiplChoice1: " + pressed);
        Debug.Log("AskMultiplChoice2: " + tElapsed);
        Debug.Log("AskMultiplChoice3: " + minSelectTime);
        while (tElapsed < minSelectTime)
        {
            if (Experiment.Instance.shopLift.isGamePaused == false)
            {
                tElapsed += Time.deltaTime;
                if (Input.GetButtonDown("Action Button"))
                {

                    infoText.text = "Please take your time to make your choice";
                    infoGroup.alpha = 1f;
                }
            }
            yield return null;
        }
        yield return StartCoroutine(WaitForButtonPress(10f, didPress =>
        {
            pressed = didPress;
        }
        ));
        infoText.text = "Please make a choice";
        infoGroup.alpha = 1f;
        float val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
        if (!pressed || (val == 0.5f))
        {
            while (!pressed)
            {
                yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
                {
                    pressed = didPress;
                }));

                val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
            }
        }

        infoText.text = "";
        infoGroup.alpha = 0f;
        /*if (is_Training)
        {
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCorrectPatches(0);
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCanvas(true);

            //yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
            //{
            //    pressed = didPress;
            //}));
            //yield return StartCoroutine(WaitForJitter(3f));
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCanvas(false);
        }*/

        Experiment.Instance.shopLiftLog.LogMultipleChoiceResponse(multipleChoiceGroup.GetComponent<AnswerSelector>().ReturnSelectorPosition(), correctChoice, pressed);
        multipleChoiceGroup.gameObject.SetActive(false);
        Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, false);
        tElapsed = 0f;
        minSelectTime = 2f;
        pressed = false;
        yield return null;
    }

    //Used for Cash Question
    IEnumerator AskMultipleChoice_v3(int prefIndex, int swap, int is_test)
    {
        int correctChoice;
        bool pressed = false;
        float tElapsed = 0f;
        //float minSelectTime = 2f;
        float minSelectTime = 0f;

        Debug.Log("PREF INDEX IS " + prefIndex.ToString());
        EnablePlayerCam(false);
        multipleChoiceGroup.gameObject.SetActive(true);

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;
        correctChoice = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetupMultipleChoice_v2(prefIndex, true, 0);

        if (is_test == 0)
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetFocusImage(false, new Vector3(0, 100, 0), new Vector3(0, 125, 0), 1, swap);
        else
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetFocusImageCashTest(false, new Vector3(0, 100, 0), new Vector3(0, 125, 0), 1, swap, is_test);


        correctChoice = prefIndex;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, true);


        Debug.Log("AskMultiplChoice1: " + pressed);
        Debug.Log("AskMultiplChoice2: " + tElapsed);
        Debug.Log("AskMultiplChoice3: " + minSelectTime);
        while (tElapsed < minSelectTime)
        {
            if (Experiment.Instance.shopLift.isGamePaused == false)
            {
                tElapsed += Time.deltaTime;
                if (Input.GetButtonDown("Action Button"))
                {

                    infoText.text = "Please take your time to make your choice";
                    infoGroup.alpha = 1f;
                }
            }
            yield return null;
        }
        yield return StartCoroutine(WaitForButtonPress(10f, didPress =>
        {
            pressed = didPress;
        }
        ));
        infoText.text = "Please make a choice";
        infoGroup.alpha = 1f;
        float val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
        if (!pressed || (val == 0.5f))
        {
            while (!pressed)
            {
                yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
                {
                    pressed = didPress;
                }));
                val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
            }
        }
        infoText.text = "";
        infoGroup.alpha = 0f;

        if (is_test == 0)
        {
            int dOpt = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().ReturnDisplayOpt();
            val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();

            if (dOpt == 1)
            {
                if (swap == 0)
                    Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 1, 1, 0, 1.0f + val);
                else
                    Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 1, 1, 0, 2.0f - val);
            }
            else
            {
                if (swap == 0)
                    Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 2, 2, 0, 3.0f + val);
                else
                    Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 2, 2, 0, 4.0f - val);
            }
        }
        else
        {
            val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
            if (swap == 0)
                Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 1, 1, 0, 5.0f + val);
            else
                Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 1, 1, 0, 6.0f - val);

        }

        /*if (is_Training)
        {
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCorrectPatches(0);
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCanvas(true);

            //yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
            //{
            //    pressed = didPress;
            //}));
            //yield return StartCoroutine(WaitForJitter(3f));
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCanvas(false);
        }*/


        Experiment.Instance.shopLiftLog.LogMultipleChoiceResponse(multipleChoiceGroup.GetComponent<AnswerSelector>().ReturnSelectorPosition(), correctChoice, pressed);
        multipleChoiceGroup.gameObject.SetActive(false);
        Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, false);
        tElapsed = 0f;
        minSelectTime = 2f;
        pressed = false;
        yield return null;
    }

    IEnumerator AskMultipleChoice(int prefIndex, bool isTraining)
    {
        Debug.Log("PREF INDEX IS " + prefIndex.ToString());
        EnablePlayerCam(false);
        multipleChoiceGroup.gameObject.SetActive(true);
        int correctChoice = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().SetupMultipleChoice(prefIndex, false);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, true);
        bool pressed = false;
        float tElapsed = 0f;
        float minSelectTime = 2f;

        Debug.Log("AskMultiplChoice1: " + pressed);
        Debug.Log("AskMultiplChoice2: " + tElapsed);
        Debug.Log("AskMultiplChoice3: " + minSelectTime);
        while (tElapsed < minSelectTime)
        {
            if (Experiment.Instance.shopLift.isGamePaused == false)
            {
                tElapsed += Time.deltaTime;
                if (Input.GetButtonDown("Action Button"))
                {

                    infoText.text = "Please take your time to make your choice";
                    infoGroup.alpha = 1f;
                }
            }
            yield return 0;
        }
        yield return StartCoroutine(WaitForButtonPress(10f, didPress =>
           {
               pressed = didPress;
           }
        ));
        infoText.text = "Please make a choice";
        infoGroup.alpha = 1f;
        float val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
        if (!pressed || (val == 0.5f))
        {
            while (!pressed)
            {
                yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
                {
                    pressed = didPress;
                }));
                val = multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().sliderValue();
            }
        }
        infoText.text = "";
        infoGroup.alpha = 0f;

        if (is_Training)
        {
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCorrectPatches(0);
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCanvas(true);

            /*yield return StartCoroutine(WaitForButtonPress(100000f, didPress =>
            {
                pressed = didPress;
            }));*/
            yield return StartCoroutine(WaitForJitter(3f));
            multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().setCanvas(false);
        }

        if (isTraining)
        {
            yield return StartCoroutine(multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().ShowFeedback(prefIndex, correctChoice, true));
        }
        Experiment.Instance.shopLiftLog.LogMultipleChoiceResponse(multipleChoiceGroup.GetComponent<AnswerSelector>().ReturnSelectorPosition(), correctChoice, pressed);
        multipleChoiceGroup.gameObject.SetActive(false);
        Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.MULTIPLE_CHOICE, false);
        tElapsed = 0f;
        minSelectTime = 2f;
        pressed = false;
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

        EnablePlayerCam(false);
        prefGroup.gameObject.SetActive(true);
        switch (prefType)
        {
            //between 1 and 2
            case 0:
                prefGroupSetup.SetupPrefs(0);
                break;
            //between 3 and 4
            case 1:
                prefGroupSetup.SetupPrefs(1);
                break;
            //between 5 and 6
            case 2:
                prefGroupSetup.SetupPrefs(2);
                break;

        }



        bool pressed = false;
        if (allowTimeouts)
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
            //float minSelectTime = 1.5f;
            float minSelectTime = 0f;

            while (tElapsed < minSelectTime)
            {
                if (Experiment.Instance.shopLift.isGamePaused == false)
                {
                    tElapsed += Time.deltaTime;
                    if (Input.GetButtonDown("Action Button"))
                    {
                        if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
                            infoText.text = "Por favor tómese su tiempo para hacer una elección";
                        else
                            infoText.text = "Please take your time to make a choice";
                        infoGroup.alpha = 1f;
                    }
                }
                yield return 0;
            }
            //wait for them to select something on the slider
            while (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && Mathf.Abs(Input.GetAxis("Horizontal")) == 0f)
            {
                if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
                    infoText.text = "Por favor, mueva el control deslizante para hacer una elección";
                else
                    infoText.text = "Please move the slider to make a choice";
                infoGroup.alpha = 1f;
                yield return 0;
            }

            infoText.text = "";
            infoGroup.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(9f, didPress =>
            {
                pressed = didPress;
            }));

            Debug.Log("about to ask them to make a choice");
            if (isTraining)
            {
                if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
                    infoText.text = "Por favor, haga una elección";
                else
                    infoText.text = "Please make a choice";

                infoGroup.alpha = 1f;
            }
            if (!pressed)
            {
                yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
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
            if (isTraining)
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
                CanvasGroup assistiveSliderUI = null;
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
                    while (1f - prefGroupSetup.prefSlider.value > 0.4f)
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





        Experiment.Instance.shopLiftLog.LogFinalSliderValue("COMPARATIVE", finalSliderValue, pressed);
        Experiment.Instance.shopLiftLog.LogExpQuesSliderCorrectness(1, 3, 1, 0, 1.0f + finalSliderValue);
        prefGroup.gameObject.SetActive(false);
        afterSlider = true;
        Cursor.visible = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.COMP_SLIDER, false);
        yield return null;
    }

    IEnumerator WaitForDoorOpenPress(string text)
    {
        infoText.text = text;
        infoGroup.alpha = 1f;

        Experiment.Instance.shopLiftLog.LogWaitEvent("DOOR", true);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.DOOR_OPEN, true);
        yield return StartCoroutine(WaitForButtonPress(5f, didPress =>
           {
               Debug.Log("did press: " + didPress);
           }
        ));
        TCPServer.Instance.SetState(TCP_Config.DefineStates.DOOR_OPEN, false);
        Experiment.Instance.shopLiftLog.LogWaitEvent("DOOR", false);
        infoGroup.alpha = 0f;
        yield return null;
    }

    public IEnumerator WaitForButtonPress(float maxWaitTime, System.Action<bool> didPress)
    {
        float timer = 0f;

        while (Experiment.Instance.shopLift.isGamePaused == true)
        {
            yield return 0;
        }
        while (true)
        {
            if (!Input.GetButtonDown("Action Button") && timer < maxWaitTime)
            {
                if (Experiment.Instance.shopLift.isGamePaused == true)
                {
                    yield return 0;
                }
                else
                {
                    timer += Time.deltaTime;
                    yield return 0;
                }
            }
            else
            {
                if (Experiment.Instance.shopLift.isGamePaused == true)
                {
                    yield return 0;
                }
                else
                {
                    break;
                }
            }
        }
        if (timer < maxWaitTime)
        {
            Experiment.Instance.shopLiftLog.LogButtonPress();
            didPress(true);
        }
        else
        {
            didTimeout = true;
            Experiment.Instance.shopLiftLog.LogTimeout(maxWaitTime);
            didPress(false);
        }
        yield return null;
    }

    public IEnumerator WaitForJitter(float maxWaitTime)
    {
        globaltimer = 0f;
        count_global = true;

        while (globaltimer < maxWaitTime)
        {
            yield return 0;
        }
        count_global = false;
        yield return null;
    }


    IEnumerator MakeCompleteBaselineList(int repeatCount)
    {
        //for audio
        completeAudioList = new List<AudioClip>();
        //for images
        completeImageList = new List<Texture>();

        EnvironmentManager tempEnv;
        for (int i = 0; i < (environments.Count - 1) * repeatCount; i++)
        {
            tempEnv = environments[i % 2].GetComponent<EnvironmentManager>();
            completeAudioList.Add(tempEnv.one_L_Audio.clip);
            completeAudioList.Add(tempEnv.one_R_Audio.clip);
            completeAudioList.Add(tempEnv.two_L_Audio.clip);
            completeAudioList.Add(tempEnv.two_R_Audio.clip);
            completeAudioList.Add(tempEnv.three_L_Audio.clip);
            completeAudioList.Add(tempEnv.three_R_Audio.clip);


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

    void ActivateEnvironmentAvatar(int activeEnvIndex)
    {
        //first deactivate all of them
        for (int i = 0; i < 4; i++)
        {
            camVehicle.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }


        camVehicle.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }


    IEnumerator PickEnvironment(int blockCount, bool pickNew)
    {
        //		envIndex = 3;
        //		envIndex = ExperimentSettings.envDropdownIndex;
        //		envIndex = Random.Range (0, environments.Count);
        //		envIndex=blockCount;
        envIndex = blockCount;

        //reset first time
        firstTime = true;

        //first turn off all environments
        for (int i = 0; i < environments.Count; i++)
        {
            envManager = environments[envIndex].GetComponent<EnvironmentManager>();
        }
        camVehicle.GetComponent<CapsuleCollider>().height = 3.2f;
        Debug.Log("picking environment");
        environments[envIndex].SetActive(true);



        // #TODO: Make sure environment checks are not reliant on string checks 
        if (environments[envIndex].name == "SpaceStation")
        {
            //cam.clearFlags = CameraClearFlags.SolidColor;
            Debug.Log("chosen space station");
            ExperimentSettings.env = ExperimentSettings.Environment.SpaceStation; //space station, for now - envInce: 3
            camVehicle.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            ActivateEnvironmentAvatar(2);
            registerType = "suitcase";
            directionEnv = -1;
        }
        else if (environments[envIndex].name == "WesternTown")
        { //western town, for now - envInce: 0
            //cam.clearFlags = CameraClearFlags.Skybox;
            Debug.Log("chosen western town");
            ExperimentSettings.env = ExperimentSettings.Environment.WesternTown;
            camVehicle.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            ActivateEnvironmentAvatar(0);
            registerType = "chest";
            camVehicle.GetComponent<CapsuleCollider>().height = 2f;
            directionEnv = 1;
        }
        else if (environments[envIndex].name == "VikingVillage") //office - envInce: 2
        { //viking village, for now
            //cam.clearFlags = CameraClearFlags.Skybox;
            Debug.Log("chosen viking village");
            ExperimentSettings.env = ExperimentSettings.Environment.VikingVillage;
            camVehicle.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            ActivateEnvironmentAvatar(0);
            registerType = "chest";
            directionEnv = -1;
        }
        else if (environments[envIndex].name == "Office")
        { //office - envInce: 1
            //cam.clearFlags = CameraClearFlags.Skybox;
            Debug.Log("chosen office");
            ExperimentSettings.env = ExperimentSettings.Environment.Office;
            camVehicle.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            ActivateEnvironmentAvatar(3);
            registerType = "safe";
            camVehicle.GetComponent<CapsuleCollider>().height = 1.6f;
            directionEnv = -1;
        }
        else if (environments[envIndex].name == "Apartment")
        { //office
            //cam.clearFlags = CameraClearFlags.Skybox;
            Debug.Log("chosen office");
            ExperimentSettings.env = ExperimentSettings.Environment.Apartment;
            camVehicle.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            ActivateEnvironmentAvatar(1);
            registerType = "chest";
            camVehicle.GetComponent<CapsuleCollider>().height = 1.6f;
            directionEnv = -1;

            apartment_set = true;

            envManager.phase2Door_L.transform.localEulerAngles = new Vector3(0, 0, 0);

        }
        else
        {
            camVehicle.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            ActivateEnvironmentAvatar(0);
            directionEnv = -1;
        }


        //after env is picked, set the cam object for all Camera Zones
        cameraZoneManager.SetCameraObjects();

        envManager = environments[envIndex].GetComponent<EnvironmentManager>();
        activeEnvLabel = environments[envIndex].name;
        phase1Start_L = envManager.phase1Start_L;
        phase1Start_R = envManager.phase1Start_R;
        phase1End_L = envManager.phase1End_L;
        phase1End_R = envManager.phase1End_R;

        phase1Door_L = envManager.phase1Door_L;
        phase1Door_R = envManager.phase1Door_R;
        phase2Door_L = envManager.phase2Door_L;
        phase2Door_R = envManager.phase2Door_R;

        phase2Start_L = envManager.phase2Start_L;
        phase2Start_R = envManager.phase2Start_R;
        phase2End_L = envManager.phase2End_L;
        phase2End_R = envManager.phase2End_R;

        phase3Start_L = envManager.phase3Start_L;
        phase3Start_R = envManager.phase3Start_R;
        phase3End_L = envManager.phase3End_L;
        phase3End_R = envManager.phase3End_R;
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
        suitcases = new List<GameObject>();
        for (int i = 0; i < envManager.suitcases.Count; i++)
        {
            suitcases.Add(envManager.suitcases[i]);
        }

        //for comparative
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().firstGroup[0] = envManager.groupOne[0];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().firstGroup[1] = envManager.groupOne[1];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().secondGroup[0] = envManager.groupTwo[0];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().secondGroup[1] = envManager.groupTwo[1];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().thirdGroup[0] = envManager.groupThree[0];
        prefGroup.gameObject.GetComponent<PrefGroupSetup>().thirdGroup[1] = envManager.groupThree[1];

        //for solo
        prefSolo.gameObject.GetComponent<PrefSoloSetup>().imgGroup[0] = envManager.groupOne[0];
        prefSolo.gameObject.GetComponent<PrefSoloSetup>().imgGroup[1] = envManager.groupOne[1];

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
            Debug.Log("Picking Rooms Afresh");
            register_L = envManager.register_L;
            register_R = envManager.register_R;

            leftRegisterObj = envManager.leftRegisterObj;
            rightRegisterObj = envManager.rightRegisterObj;

            Experiment.Instance.shopLiftLog.LogEnvironmentSelection(activeEnvLabel);


            //after env has been selected and all necessary object references set, assign rooms and randomize cam zones
            if (blockCount == 0 || blockCount == 1)
            {
                AssignRooms(true, false);
            }
            // for training only
            else if (blockCount == 2)
            {
                AssignRooms(true, true);
            }
            RandomizeSpeedChangeZones();
            yield return StartCoroutine(RandomizeCameraZones(blockCount));
        }

        yield return null;
    }

    IEnumerator ShowInstructionsTillButtonPress(string text)
    {
        intertrialText.text = text;
        /*intertrialGroup.alpha = 1f;
        yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
        {
            Debug.Log("did press: " + didPress);
        }
        ));
        intertrialGroup.alpha = 0f;
        */
        yield return null;
    }

    IEnumerator ConnectToBlackrock()
    {

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
        yield return null;
    }


    IEnumerator LoadCheckpoints()
    {

        if (Experiment.shouldCheckpoint)
        {
            _startingIndex = Experiment.Instance.checkpointedEnvIndex;
            registerVals = new List<int>();
            Debug.Log("TURNED OFF blackscreen");

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;
            registerVals.Add(Experiment.Instance.leftReward);
            registerVals.Add(Experiment.Instance.rightReward);

            //remove rewards from the existing pool of rewards so they don't get reused in the future
            RemoveIndex(registerVal1, Experiment.Instance.leftReward);
            RemoveIndex(registerVal2, Experiment.Instance.rightReward);


        }
        //if not checkpointed, then begin from pre-training
        else
        {
            expSettings.stage = ExperimentSettings.Stage.Pretraining;
        }
        yield return null;
    }

    IEnumerator RunPretraining()
    {
        currentPhaseName = "PRE-TRAINING";
        CheckpointSession(0, true);
        yield return StartCoroutine(ShowIntroInstructions());

        if (blackScreen.alpha == 1f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
        blackScreen.alpha = 0f;


        UnityEngine.Debug.Log("Experiment Settings Stage: " + expSettings.stage);
        //pretraining; will only run before the first environment
        if (expSettings.stage == ExperimentSettings.Stage.Pretraining)
        {
            CameraZone.enableCamZones = false;

            if (blackScreen.alpha == 0f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
            blackScreen.alpha = 1f;
            yield return StartCoroutine(PickEnvironment(0, true)); //training env
            RandomizeSuitcases();
            yield return StartCoroutine(PlayInstructionVideo(true));

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;
            //disable any kind of camera zone interaction

            //yield return StartCoroutine(RunSliderTrainingPhase());
            //yield return StartCoroutine(RunMultipleChoiceTrainingPhase());
            CameraZone.enableCamZones = true;
            //enable camera zone interaction before camera training
            Debug.Log("running cam pretraining");
            CameraZone.isPretraining = true;
            //yield return StartCoroutine(RunCamTrainingPhase());
            string pretrainingEndText = "Congrats! You've completed PRE-TRAINING!\n GOAL: learn which rooms lead to*more cash*!! \n But first, let's memorize *cam locations*\n to deactivate cams too! \n Press(X) to begin camera practice!";
            //yield return StartCoroutine(ShowInstructionsTillButtonPress(pretrainingEndText));

            //set for next stage
            expSettings.SetNextStage();
        }
        yield return null;
    }

    IEnumerator RunTraining(int index)
    {
        currentPhaseName = "TRAINING";
        CheckpointSession(index, true);
        Debug.Log("current stage " + expSettings.stage.ToString());

        if (index == 0)
        {
            registerVals[0] = 70;
            registerVals[1] = 30;
        }
        else if (index == 1)
        {
            registerVals[0] = 70;
            registerVals[1] = 30;
        }

        if (expSettings.stage == ExperimentSettings.Stage.Training)
        {

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;
            //enable camera zone interaction before camera training
            Debug.Log("running cam training");
            yield return StartCoroutine(RunCamTrainingPhase());

            //set for next stage
            expSettings.SetNextStage();
        }
        RandomizeSuitcases();
        cameraZoneManager.ResetAllCamZones();
        cameraZoneManager.ToggleAllCamZones(false);

        _currentReevalCondition = reevalConditions[index];


        if (!Experiment.shouldCheckpoint)
        {
            if (!Config.isDayThree)
            {

                AssignRooms(false, false);
                yield return StartCoroutine(PickRegisterValues()); //new reg values to be picked for each environment

                registerVals[0] = 70;
                registerVals[1] = 30;
            }
            else
            {
                if (index == 0)
                {
                    registerVals[0] = 30;
                    registerVals[1] = 70;
                }
                else if (index == 1)
                {
                    registerVals[0] = 50;
                    registerVals[1] = 90;
                }
            }
        }

        isTransition = !isTransition; //flip the transition condition before the next round
        yield return null;
    }


    IEnumerator RunLearning(int index)
    {
        currentPhaseName = "LEARNING";
        CheckpointSession(index, true);
        Debug.Log("current stage " + expSettings.stage.ToString());
        if (expSettings.stage == ExperimentSettings.Stage.Learning || !expSettings.isCheckpointed)
        {
            maxTrials_Learning = 20;
            Debug.Log("MAX TRIALS " + maxTrials_Learning.ToString());
            TCPServer.Instance.SetState(TCP_Config.DefineStates.LEARNING, true);
            yield return StartCoroutine(RunLearningPhase(false, maxTrials_Learning));
            TCPServer.Instance.SetState(TCP_Config.DefineStates.LEARNING, false);

            //set for next stage
            expSettings.SetNextStage();
        }
        yield return null;
    }


    IEnumerator RunReevaluation(int index)
    {
        //re-evaluation phase
        currentPhaseName = "REEVALUATION";
        CheckpointSession(index, true);
        Debug.Log("current stage " + expSettings.stage.ToString());
        if (expSettings.stage == ExperimentSettings.Stage.Reevaluation || !expSettings.isCheckpointed)
        {
            TCPServer.Instance.SetState(TCP_Config.DefineStates.REEVALUATION, true);
            yield return StartCoroutine(RunReevaluationPhase(_currentReevalCondition));
            TCPServer.Instance.SetState(TCP_Config.DefineStates.REEVALUATION, false);

            //set for next stage
            expSettings.SetNextStage();
        }

        yield return null;
    }


    IEnumerator RunTesting(int index)
    {
        currentPhaseName = "TESTING";
        CheckpointSession(index, true);
        Debug.Log("current stage " + expSettings.stage.ToString());
        if (expSettings.stage == ExperimentSettings.Stage.Test || !expSettings.isCheckpointed)
        {

            TCPServer.Instance.SetState(TCP_Config.DefineStates.TESTING, true);
            yield return StartCoroutine(RunTestingPhase());
            TCPServer.Instance.SetState(TCP_Config.DefineStates.TESTING, false);

            //set for next stage
            expSettings.SetNextStage();
        }

        yield return null;
    }

    IEnumerator RunPostTest()
    {

        //if transition phase and not control, play 10-trial additional learning
        if (_currentReevalCondition == 1 && !hasLearned && !Config.shouldForceControl)
        {
            Debug.Log("RUNNING ADDITIONAL LEARN PHASE");
            expSettings.stage = ExperimentSettings.Stage.PostTest;
            currentPhaseName = "POST-TEST";
            TCPServer.Instance.SetState(TCP_Config.DefineStates.POST_TEST, true);
            yield return StartCoroutine(RunLearningPhase(true, maxTrials_PostTest));
            TCPServer.Instance.SetState(TCP_Config.DefineStates.POST_TEST, true);

            //set for next stage
            expSettings.SetNextStage();

        }

        yield return null;
    }


    IEnumerator RunTask()
    {

        UnityEngine.Debug.Log("SAI_DEBUG: Started the Task");
        stageIndex = 1;
        Experiment.Instance.CreateSessionStartedFile();

        int totalEnvCount = environments.Count - 1; //since we're excluding VikingVillage which is used only for Pre-Training
        _currentReevalCondition = 0;

        yield return StartCoroutine(ConnectToBlackrock());

        if (!Config.isDayThree)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                isTransition = true;
            }
            else
                isTransition = false;
        }

        _startingIndex = 0;


        UnityEngine.Debug.Log("SAI_DEBUG: Load Checkpoints ");
        yield return StartCoroutine(LoadCheckpoints());

        UnityEngine.Debug.Log("SAI_DEBUG: Load PReTraining ");


        yield return StartCoroutine(ParseSequence());
        yield return StartCoroutine(ParseQuestion());
        yield return StartCoroutine(ParseQuestionPattern());
        yield return StartCoroutine(RunPretraining());


        /*for (int i = _startingIndex; i < totalEnvCount; i++)
        {*/
        int i = environmentNumber;
        UnityEngine.Debug.Log("SAI_DEBUG: Environment: " + i);
        numTrials_Learning = 0;
        numTrials = 0;

        is_Training = true;

        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(0, true);
        yield return StartCoroutine(ExecuteDirectoryD1());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(0, false);

        yield return StartCoroutine(PickEnvironment(i, true));

        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(1, true);
        yield return StartCoroutine(RunInitTrainingPhase());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(1, false);

        UnityEngine.Debug.Log("SAI_DEBUG: Running Training ");
        yield return StartCoroutine(ExecuteDirectoryD2());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(2, true);
        yield return StartCoroutine(RunTraining(i));
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(2, false);


        is_Training = false;
        UnityEngine.Debug.Log("SAI_DEBUG: Running Learning ");
        yield return StartCoroutine(ExecuteDirectoryD3());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(3, true);
        yield return StartCoroutine(RunLearning(i));
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(3, false);

        UnityEngine.Debug.Log("SAI_DEBUG: Running Testing ");
        yield return StartCoroutine(ExecuteDirectoryD11());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(5, true);
        yield return StartCoroutine(RunTesting(i));
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(5, false);

        UnityEngine.Debug.Log("SAI_DEBUG: Running Reevaluation ");
        yield return StartCoroutine(ExecuteDirectoryD4());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(4, true);
        yield return StartCoroutine(RunReevaluation(i));
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(4, false);

        //testing phase

        UnityEngine.Debug.Log("SAI_DEBUG: Running Testing ");
        yield return StartCoroutine(ExecuteDirectoryD5());
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(5, true);
        yield return StartCoroutine(RunTesting(i));
        Experiment.Instance.shopLiftLog.LogExpTrialPhaseStatus(5, false);
        Experiment.Instance.shopLiftLog.LogGameEnd();

        /*UnityEngine.Debug.Log("SAI_DEBUG: Running PostTest ");
        yield return StartCoroutine(RunPostTest());*/

        //skip if it is the final environment
        /*if (i != totalEnvCount - 1)
        {
            UnityEngine.Debug.Log("SAI_DEBUG: End Enviroment stage screen ");
            yield return StartCoroutine(ShowEndEnvironmentStageScreen());
        }
        else {*/
        UnityEngine.Debug.Log("SAI_DEBUG: End Enviroment stage screen ");
        //yield return StartCoroutine(ShowEndEnvironmentStageScreenFinal());
        //}


        Experiment.Instance.subjectLog.QueueisEmpty();
        //yield return new WaitForSeconds(10f);
        yield return StartCoroutine(ExecuteDirectoryD10());
        //Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);

        if (blackScreen.alpha == 0f)
            Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
        blackScreen.alpha = 1f;


        CheckpointSession(i, true);

        //reset variables
        ResetEnvironmentVariables();
        //turn off this
        Experiment.shouldCheckpoint = false;
        yield return null;

        environments[envIndex].SetActive(false);
        UnityEngine.Debug.Log("SAI_DEBUG: Looping out ");
        //}

        //run baseline tests

        /*yield return StartCoroutine(MakeCompleteBaselineList(2));

        currentPhaseName = "MUSIC_BASELINE";
        CheckpointSession(totalEnvCount - 1, true);
        yield return StartCoroutine(RunMusicBaseline());


        currentPhaseName = "IMAGE_BASELINE";
        CheckpointSession(totalEnvCount - 1, true);
        yield return StartCoroutine(RunImageSlideshow());


        currentPhaseName = "SILENT_TRAVERSAL";
        CheckpointSession(totalEnvCount - 1, true);
        CameraZone.enableCamZones = false;
        yield return StartCoroutine(RunSilentTraversal());
        CameraZone.enableCamZones = true;

        CheckpointSession(totalEnvCount - 1, false);

        //show the end session screen
        UnityEngine.Debug.Log("SAI_DEBUG: Running Endsession ");
        yield return StartCoroutine (ShowEndSessionScreen());
        */

        Experiment.Instance.subjectLog.close();
        Experiment.Instance.eegLog.close();
        //Experiment.Instance.subjectLogNew.close();
        //Application.Quit();
        yield return null;
    }

    void ResetEnvironmentVariables()
    {
        correctResponses = 0;
        CameraZone.firstTime = true;
        expSettings.stage = ExperimentSettings.Stage.Training;
    }

    void TurnOffRooms()
    {
        roomTwo.SetActive(false);
        roomOne.SetActive(false);

    }

    public IEnumerator ShowPositiveFeedback()
    {
        Debug.Log("IN POSITIVE");
        if (expSettings.stage != ExperimentSettings.Stage.Pretraining)
        {
            //CAMERA is no more present
            //positiveFeedbackGroup.alpha = 1f;
            //		Debug.Log ("about to wait for 1 second");
            yield return new WaitForSeconds(1f);
            //		Debug.Log ("turning it off");
            positiveFeedbackGroup.alpha = 0f;
        }
        else
        {
            negativeFeedbackGroup.alpha = 0f;
            positiveFeedbackGroup.alpha = 0f;
            incorrectGiantText.alpha = 0f;
            //NO MORE RELEVANT
            //correctGiantText.alpha = 1f;
            yield return new WaitForSeconds(1f);
            correctGiantText.alpha = 0f;
        }
        consecutiveIncorrectCameraPresses = 0;
        correctResponses++; //increment correct responses
        Debug.Log("CORRECT RESPONSES " + correctResponses.ToString());
        yield return null;
    }
    public IEnumerator ShowNegativeFeedback()
    {
        Debug.Log("IN NEGATIVE");
        if (expSettings.stage != ExperimentSettings.Stage.Pretraining)
        {
            Debug.Log("turning negative on");
            //CAMERA is no more present
            //negativeFeedbackGroup.alpha = 1f;
            //negativeFeedbackGroup.gameObject.GetComponent<AudioSource> ().Play ();
            Debug.Log("about to wait for 1 second");
            yield return new WaitForSeconds(1f);
            negativeFeedbackGroup.alpha = 0f;
            Debug.Log("turned negative off");
        }
        else
        {
            negativeFeedbackGroup.alpha = 0f;
            positiveFeedbackGroup.alpha = 0f;
            correctGiantText.alpha = 0f;
            //NO MORE RELEVANT
            //incorrectGiantText.alpha = 1f;
            yield return new WaitForSeconds(1f);
            incorrectGiantText.alpha = 0f;
        }
        consecutiveIncorrectCameraPresses += 1;
        yield return null;
    }

    public IEnumerator RepeatRoom()
    {
        yield return StartCoroutine(ShowNegativeFeedback());

        //repeat room
        Vector3 startPos = Vector3.zero; //where to move the camera back to
        switch (currentRoomIndex)
        {
            case 1:
                startPos = (currentPathIndex == 0) ? phase1Start_L.transform.position : phase1Start_R.transform.position;
                break;
            case 2:
                startPos = (currentPathIndex == 0) ? phase2Start_L.transform.position : phase2Start_R.transform.position;
                break;
            case 3:
                startPos = (currentPathIndex == 0) ? phase3Start_L.transform.position : phase3Start_R.transform.position;
                break;
        }
        Debug.Log("TRANSPORTING PLAYER BACK TO ROOM START");
        camVehicle.transform.position = startPos;

        yield return null;
    }

    public IEnumerator ShowWarning()
    {
        Debug.Log("IN WARNING");
        warningFeedbackGroup.alpha = 1f;
        while (!clearCameraZoneFlags)
        {
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
        switch (choiceOutput)
        {
            case 0:
                chosenRegister = leftRegisterObj;
                break;
            case 1:
                chosenRegister = rightRegisterObj;
                break;

        }

        Debug.Log("chosen register is: " + chosenRegister.name);

        //then open the suitcase
        TCPServer.Instance.SetState(TCP_Config.DefineStates.REWARD_OPEN, true);
        suitcaseObj.GetComponent<Animator>().SetTrigger("Open");

        //wait until suitcase is fully open
        yield return new WaitForSeconds(0.5f);

        //register_L.transform.position = envManager.register_L.transform.position;
        //register_R.transform.position = envManager.register_R.transform.position;
        GameObject coinShowerObj = Instantiate(coinShower, ((pathIndex == 0) ? register_L.transform.position : register_R.transform.position) + (new Vector3(0f, 0.2f, directionEnv) * 2f), Quaternion.identity) as GameObject;

        int reward = 0;

        System.Random rand = new System.Random();
        reward = Mathf.CeilToInt(NextGaussian(rand, registerVals[choiceOutput], 1)); //if not training, then retrieve appropriate reward values

        Debug.Log(" Reward: " + reward);
        Experiment.Instance.shopLiftLog.LogExpTrialInfoStatus(4, reward);

        //check to see if it's training for slider questions
        /*if (!isTraining)
        {
            System.Random rand = new System.Random();
            reward = Mathf.CeilToInt(NextGaussian(rand, registerVals[choiceOutput], 1)); //if not training, then retrieve appropriate reward values
        }
        else
        {
            reward = trainingReward[choiceOutput]; //else, obtain a random reward for training
        }*/
        rewardScore.enabled = true;
        rewardScore.text = "$" + reward.ToString();

        chosenRegister.GetComponent<AudioSource>().Play(); //play the cash register audio

        Experiment.Instance.shopLiftLog.LogRegisterReward(reward, choiceOutput);
        Experiment.Instance.shopLiftLog.LogRegisterEvent(true);
        Debug.Log("waiting for 2 seconds");
        yield return StartCoroutine(rewardScore.gameObject.GetComponent<FontChanger>().GrowText(2f));
        rewardScore.enabled = false;
        TCPServer.Instance.SetState(TCP_Config.DefineStates.REWARD_OPEN, false);
        Experiment.Instance.shopLiftLog.LogRegisterEvent(false);

        //        infoGroup.alpha = 0f;
        Destroy(coinShowerObj);
        Destroy(suitcaseObj);

        if (reward > 60)
        {
            yield return StartCoroutine(ExecuteDirectoryD8());
            //Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);

            if (blackScreen.alpha == 0f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
            blackScreen.alpha = 1f;
        }
        else
        {
            yield return StartCoroutine(ExecuteDirectoryD9());
            //Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);

            if (blackScreen.alpha == 0f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
            blackScreen.alpha = 1f;

        }
        yield return null;
    }

    IEnumerator ShowInstructionScreen(string instText, bool needsButtonPress, bool showTips, float waitTime)
    {
        intertrialText.text = instText;
        //intertrialGroup.alpha = 1f;

        //tipsGroup.alpha = (showTips) ? 1f : 0f;

        float timer = 0f;
        Debug.Log("needsbuttonpress is  " + needsButtonPress.ToString());
        /*while (timer < waitTime && !(needsButtonPress && Input.GetButtonDown("Action Button")))
        {
            Debug.Log("the timer is " + timer.ToString());
            timer += Time.deltaTime;
            yield return 0;
        }*/
        intertrialGroup.alpha = 0f;
        yield return null;
    }

    IEnumerator ShowNextStageScreen()
    {

        EnablePlayerCam(false);
        //intertrialGroup.alpha = 1f;
        if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
            intertrialText.text = "Al día siguiente";
        else
            intertrialText.text = "On the next day...";

        Experiment.Instance.shopLiftLog.LogEndTrial();
        //yield return new WaitForSeconds(2f);
        intertrialGroup.alpha = 0f;
        yield return null;
    }

    IEnumerator ShowEndTrialScreen(bool isTraining, bool hasTips)
    {

        EnablePlayerCam(false);
        //intertrialGroup.alpha = 1f;
        if (!isTraining)
        {
            if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
                intertrialText.text = "Comenzando la siguiente prueba...";
            else
                intertrialText.text = "Starting the next test...";

        }
        else
        {

            if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
                intertrialText.text = "Comenzando el siguiente ensayo de práctica...";
            else
                intertrialText.text = "Starting the next practice trial...";

        }


        /*if (!isTraining)
        {*/
        yield return StartCoroutine(ExecuteDirectoryD7());
        //}

        Experiment.Instance.shopLiftLog.LogEndTrial();
        Experiment.Instance.shopLiftLog.LogEndTrialScreen(true, hasTips);
        tipsGroup.alpha = 0f;
        //yield return new WaitForSeconds(1f);
        //}
        intertrialGroup.alpha = 0f;
        Experiment.Instance.shopLiftLog.LogEndTrialScreen(false, hasTips);
        yield return null;
    }
    IEnumerator ShowEndEnvironmentStageScreen()
    {
        //intertrialGroup.alpha = 1f;
        if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
            intertrialText.text = "Felicidades, Has terminado. \n Tenga un breve descanso.";
        else
            intertrialText.text = "Congratulations, you’re done! \n Have a short break";


        //reset deviation queue before beginning the next environment
        deviationQueue = new Queue<float>();

        Experiment.Instance.shopLiftLog.LogEndEnvironmentStage(true);
        //yield return new WaitForSeconds(30f);
        Experiment.Instance.shopLiftLog.LogEndEnvironmentStage(false);
        intertrialGroup.alpha = 0f;
        yield return null;
    }

    IEnumerator ShowEndEnvironmentStageScreenFinal()
    {
        intertrialGroup.alpha = 1f;
        if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
            intertrialText.text = "Felicidades, Has terminado.";
        else
            intertrialText.text = "Congratulations, you are done!";


        //reset deviation queue before beginning the next environment
        deviationQueue = new Queue<float>();

        Experiment.Instance.shopLiftLog.LogEndEnvironmentStage(true);
        yield return new WaitForSeconds(5f);
        //Experiment.Instance.shopLiftLog.LogEndEnvironmentStage(false);
        yield return null;
    }
    IEnumerator ShowEndSessionScreen()
    {
        //intertrialGroup.alpha = 1f;
        if (expSettings.currentLanguage == ExperimentSettings.Language.Spanish)
            intertrialText.text = "Felicidades. Has completado la sesión. \n Presiona Escape para salir de la aplicación.";
        else
            intertrialText.text = "Congratulations, you have completed a session \n Press Escape key to exit the application.";

        Experiment.Instance.shopLiftLog.LogEndSession(true);
        //yield return new WaitForSeconds(1000f);
        intertrialGroup.alpha = 0f;
        yield return null;

    }

    public void RandomizeSpeed()
    {
        Debug.Log("randomizing speed");
        suggestedSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        StartCoroutine("UpdateSpeed", suggestedSpeed);
        TCPServer.Instance.SetState(TCP_Config.DefineStates.SPEED_CHANGE, true);
        //		Debug.Log ("randomized speed to: " + currentSpeed.ToString ());
    }

    IEnumerator UpdateSpeed(float suggestedSpeed)
    {
        Debug.Log("updating speed to: " + suggestedSpeed.ToString());
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, suggestedSpeed, timer);
            yield return 0;
        }
        yield return null;
    }

    void ToggleMouseLook(bool shouldActivate)
    {

        if (shouldActivate)
        {

            camVehicle.SetActive(true);
        }
        else
        {
            camVehicle.SetActive(false);
        }
    }

    IEnumerator ForcePlayerDecision(Vector3 chosenPoint, Vector3 otherPoint)
    {
        while (!Input.GetButtonDown("Action Button") || (Vector3.Distance(camVehicle.transform.position, chosenPoint) >= Vector3.Distance(camVehicle.transform.position, otherPoint)))
        {
            yield return 0;
        }
        yield return null;
    }

    IEnumerator WaitForPlayerDecision(Vector3 leftPoint, Vector3 rightPoint)
    {
        while (!Input.GetButtonDown("Action Button") || (Vector3.Distance(camVehicle.transform.position, rightPoint) == Vector3.Distance(camVehicle.transform.position, leftPoint)))
        {
            yield return 0;
        }
        //right
        if (Vector3.Distance(camVehicle.transform.position, rightPoint) < Vector3.Distance(camVehicle.transform.position, leftPoint))
        {
            playerChoice = 1;

        }
        //left
        else
        {
            playerChoice = 0;

        }

        Debug.Log("the player choice is: " + playerChoice.ToString());

        yield return null;

    }

    IEnumerator MovePlayerTo(Vector3 startPos, Vector3 endPos, float factor)
    {
        camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        float timer = 0f;
        //		Debug.Log ("about to move player normally");
        while (timer / factor < 1f)
        {
            timer += Time.deltaTime;
            camVehicle.transform.position = Vector3.Lerp(startPos, endPos, timer / factor);
            Experiment.Instance.shopLiftLog.LogExpSpeedChangeTransition((endPos - startPos).sqrMagnitude / factor);
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


    //PLAYER MOVEMENT LOGIC
    IEnumerator VelocityPlayerTo(Vector3 startPos, Vector3 endPos, float factor)
    {
        Debug.Log("[VelocityPlayerTo] in phase 2");
        int sign = (int)((endPos.z - startPos.z) / Mathf.Abs(endPos.z - startPos.z));
        Vector3 moveDir = new Vector3(0f, 0f, sign * 1f);
        //currentSpeed = (UnityEngine.Random.Range(minSpeed, maxSpeed)) * 1.2f;
        float distanceLeft = Vector3.Distance(camVehicle.transform.position, endPos);

        camVehicle.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        float timer = 0f;
        while (distanceLeft > 2f)
        {
            timer += Time.deltaTime;
            camVehicle.GetComponent<Rigidbody>().velocity = moveDir * currentSpeed;
            distanceLeft = Vector3.Distance(camVehicle.transform.position, endPos);
            yield return 0;
        }
        camVehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Debug.Log("[VelocityPlayerTo] in phase 2 End");
        yield return null;
    }

    public IEnumerator ExecuteDirectoryD1()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D1", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);


        yield return null;
    }

    public IEnumerator ExecuteDirectoryD2()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D2", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);


        yield return null;
    }

    public IEnumerator ExecuteDirectoryD3()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D3", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD4()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D4", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD5()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D5", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD6()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D6", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD7()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D7", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD8()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D8", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD9()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D9", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));


            if (blackScreen.alpha == 0f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(true);
            blackScreen.alpha = 1f;
            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD10()
    {
        
        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D10", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ExecuteDirectoryD11()
    {

        string[] Images = Directory.GetFiles(Application.dataPath + "/Resources_IGNORE/D11", "0*.png", SearchOption.AllDirectories);

        Experiment.Instance.shopLiftLog.LogInstructionExtremes(true);
        foreach (string path_n in Images)
        {
            Debug.Log("InstManager: Path_n: " + path_n);
            Sprite image_new = img2sprite.LoadNewSprite(path_n);
            instructionRendererImage.sprite = image_new;
            instructionRenderer.alpha = 1f;

            if (blackScreen.alpha == 1f)
                Experiment.Instance.shopLiftLog.LogBlackScreenExtremes(false);
            blackScreen.alpha = 0f;

            yield return StartCoroutine(WaitForButtonPress(10000f, didPress =>
            {
                Debug.Log("did press: " + didPress);
            }
            ));

            instructionRenderer.alpha = 0f;
        }
        Experiment.Instance.shopLiftLog.LogInstructionExtremes(false);

        yield return null;
    }

    public IEnumerator ParseSequence()
    {
        string filePath = Application.dataPath + "/Resources_IGNORE" + "/" + "SH_ParseSequence.txt";

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            int line_no = 0;
            while ((line = reader.ReadLine()) != null)
            {
                line_no++;
                string[] values = line.Split('\t');
                UnityEngine.Debug.Log("ParseSequence: line_no: " + line_no);
                UnityEngine.Debug.Log("ParseSequence: values_length: " + values.Length);
                UnityEngine.Debug.Log("ParseSequence: learning_seq_length: " + learning_seq.Length);
                switch (line_no)
                {
                    case 1:
                        for (int i = 0; i < values.Length; i++)
                        {
                            training_1_seq[i] = values[i];
                        }
                        break;
                    case 2:
                        for (int i = 0; i < values.Length; i++)
                        {
                            training_3_seq[i] = values[i];
                        }
                        break;
                    case 3:
                        for (int i = 0; i < values.Length; i++)
                        {
                            learning_seq[i] = values[i];
                        }
                        break;
                    case 4:
                        for (int i = 0; i < values.Length; i++)
                        {
                            reeval_seq[i] = values[i];
                        }
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }

    }

    public IEnumerator ParseQuestion()
    {
        string filePath = Application.dataPath + "/Resources_IGNORE" + "/" + "SH_ParseQuestion.txt";

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            int line_no = 0;
            while ((line = reader.ReadLine()) != null)
            {
                line_no++;
                string[] values = line.Split('\t');
                UnityEngine.Debug.Log("ParseQuestion: line_no: " + line_no);
                UnityEngine.Debug.Log("ParseQuestion: values_length: " + values.Length);
                switch (line_no)
                {
                    case 1:
                        for (int i = 0; i < values.Length; i++)
                        {
                            training_1_ques[i] = values[i];
                        }
                        break;
                    case 2:
                        for (int i = 0; i < values.Length; i++)
                        {
                            training_3_ques[i] = values[i];
                        }
                        break;
                    case 3:
                        for (int i = 0; i < values.Length; i++)
                        {
                            learning_ques[i] = values[i];
                        }
                        break;
                    case 4:
                        for (int i = 0; i < values.Length; i++)
                        {
                            reeval_ques[i] = values[i];
                        }
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }

    }

    public IEnumerator ParseQuestionPattern()
    {
        string filePath = Application.dataPath + "/Resources_IGNORE" + "/" + "question_sequence.txt";

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            int line_no = 0;
            while ((line = reader.ReadLine()) != null)
            {
                line_no++;
                string[] values = line.Split('\t');

                for (int i = 0; i < 10; i++) {
                    questionpattern[i] = Int32.Parse(values[i]);
                }

            }
        }

        yield return null;
    }


        void LoadMaterial_mountain()
    {
        string material = Application.dataPath + "/Resources" + "/Materials/" + "Terrain.mat";

        //Material m = Resources.Load("Material/Terrain.mat", typeof(Material)) as Material;
        Material m = Resources.Load<Material>("Materials/Terrain.mat") as Material;
        mountain_gobj.gameObject.GetComponent<MeshRenderer>().material = m;

     }

}