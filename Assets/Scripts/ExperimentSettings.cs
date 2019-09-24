using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Reflection;
using System;
public class ExperimentSettings : MonoBehaviour {

    public int trialCount = 0;
	public static bool practice=false;

	//Experiment exp { get { return Experiment.Instance; } }

//	public bool isRelease { get { return GetIsRelease (); } }
	public static bool shouldStim=false;
	public static bool isLogging = true;

    //build info
    string buildDate;
    public Text buildType;

    //CONFIG ENUMS

    //which environment
    public enum Environment
	{
		Cybercity,
		SpaceStation,
		WesternTown,
		VikingVillage,
		Office,
		Apartment,
	}
	public static Environment env;
	public static int envDropdownIndex=0;

    //what re-evaluation type
	public enum ReevalType
	{
		Transition,
		Reward
	}
	public static ReevalType reevalType;

    //to specify the type of connection method used for EEG synchronization
    public enum ConnectionMethod
    {
        BlackrockSync,
        Syncbox,
        Photosync,
        Demo
    }
    public ConnectionMethod connectionMethod;

    //to specify the type of control device to be used
    public enum ControlDevice
    {
        Controller,
        Keyboard
    }
    public ControlDevice controlDevice;

    public enum Language
    {
        English,
        Spanish
    }
    public Language currentLanguage;

	//LOGGING
	public static string defaultLoggingPath = ""; //SET IN RESETDEFAULTLOGGINGPATH();
	//string DB3Folder = "/" + Config.BuildVersion.ToString() + "/";
	//public Text defaultLoggingPathDisplay;
	//public InputField loggingPathInputField;


	public static Subject currentSubject{ 
		get{ return _currentSubject; } 
		set{ 
			_currentSubject = value;
			//fileName = "TextFiles/" + _currentSubject.name + "Log.txt";
		}
	}

	private static Subject _currentSubject;
	public SubjectSelectionController subjectSelectionController;


	public Text gamifiedText;
	public Dropdown sessionType;


    public enum EnvironmentType
    {
        WA,
        SO
    }

    public EnvironmentType currentEnvironmentType; 

    public enum Stage
    {
        Pretraining,
        Training,
        Learning,
        Reevaluation,
        Test,
        PostTest,
        Baseline,
        None
    }

    public Stage stage;

    public static bool isRewardReeval = false;
	public static bool isTransitionReeval = false;


	public Dropdown firstEnvDropdown;
	public Dropdown reevalDropdown;


	public InputField subjectName;
	public Button checkpointButton;
	public Button resumeFromCheckpointButton;
	public Text checkpointDataText;
	public bool checkpointExists = false;

    //scene controller reference
    public SceneController sceneController;


    //session config options
    public Dropdown connectionMethodDropdown;
    public Dropdown controlDeviceDropdown;
    public Dropdown languageDropdown;
    public Dropdown sessionDayDropdown;
    public Toggle isControlToggle;

    public static int envIndex= 0;

	bool isWeb = false;

	//SINGLETON
	private static ExperimentSettings _instance;

	public static ExperimentSettings Instance{
		get{
			return _instance;
		}
	}

	void Awake(){

		if (_instance != null) {
			Debug.Log("Instance already exists!");
			Destroy(transform.gameObject);
			return;
		}
		_instance = this;
		checkpointButton.gameObject.SetActive(true);
		resumeFromCheckpointButton.gameObject.SetActive(false);
        //		DoMicTest ();
        //		if(Application.loadedLevel==0)
        //			InitMainMenuLabels ();
        //		CheckGamifiedStatus ();
        //		if (SceneManager.GetActiveScene ().name == "EndMenu") {
        //			AttachSceneController ();


        ChangeFirstEnvironment ();
        //		ChangeReevalType ();
        reevalType = ReevalType.Transition;
        InitMainMenuLabels();

        SetConnectionMethod();
        SetSessionDay();
        SetControlDevice();
        SetControlToggle();

//		reevalType = ReevalType.Reward;
//		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCheckpointStatus()
	{
		string subjName = subjectName.text;
		checkpointExists=CheckSubjectFolderForCheckpoints (subjName);
		if (checkpointExists) {

			checkpointButton.gameObject.SetActive(false);
			resumeFromCheckpointButton.gameObject.SetActive(true);
		}
		else
			checkpointDataText.text = "No checkpoint data found!";
	}
	public bool CheckSubjectFolderForCheckpoints(string subjName)
	{

		string subjectDirectory = ExperimentSettings.defaultLoggingPath + ExperimentSettings.currentSubject.name + "/";
		Debug.Log ("subject dir: " + subjectDirectory.ToString ());

		string sessionStartedFileName= "sessionStarted.txt";
		int sessionID = 0;
        string sessionIDString = "session_"+sessionID.ToString()+"/";
        string tempDir = subjectDirectory + sessionIDString;
        string[] checkpointData = new string[8];
		bool shouldBreak = false;
		bool hasCheckpoint = false;
        string checkpointFilePath = "";
        string chosenCheckpointFile = "";
        while (File.Exists(tempDir + sessionStartedFileName) && !shouldBreak)
        {

            sessionIDString = "session_" + sessionID.ToString() + "/";
            //check if the session crashed
            checkpointFilePath = tempDir + "checkpoint.txt";
            if(File.Exists(checkpointFilePath))
            {
                chosenCheckpointFile = checkpointFilePath;
            }
            //update tempdir now
            tempDir = subjectDirectory + sessionIDString;
            sessionID++;
        }

            Debug.Log("sanity checking " + checkpointFilePath);
            checkpointData = new string[8];
			if (File.Exists (chosenCheckpointFile)) {
				string checkpointText = File.ReadAllText (chosenCheckpointFile);
				if (checkpointText.Contains ("ONGOING")) {
					checkpointData = checkpointText.Split ("\t" [0]);
					hasCheckpoint = true;
					shouldBreak = true;
					checkpointDataText.text = string.Format("Status: {0} \nEnv Index: {1} \n Phase Index: {2} ", checkpointData[0],checkpointData[1],checkpointData[2]);
				}
				if (sessionID >= 5) {
					checkpointDataText.text = "No checkpoint data found!";
					shouldBreak = true;
				}
			}
			else
				checkpointDataText.text = "No checkpoint data found!";
		return hasCheckpoint;
	}

	public void SetSessionType()
	{
		if (sessionType.value == 0) {
			Config.sessionType = Config.SessionType.NonAdaptive;
		} else {
			Config.sessionType = Config.SessionType.Adaptive;
		}
	}

	//to show what version we're running on the Main Menu
	void InitMainMenuLabels()
	{

  //      Debug.Log("syncbox " + Config.isSyncbox.ToString());
		//if (Config.isSyncbox) {
		//	buildType.text = "Syncbox";
		//}
  //      else if(Config.isSystem2)
  //      {
  //          buildType.text = "Blackrock";
  //      }
		//else
		//    buildType.text = "Demo";

		buildDate = 
			new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToString();
		UnityEngine.Debug.Log (buildDate);
		buildType.text = "v" + Config.VersionNumber + " [ " + buildDate + " ] ";

	}

//	void AttachSceneController()
//	{
//		GameObject sceneControl = GameObject.Find ("SceneController");
//		if (sceneControl != null) {
//			returnMenuButton.onClick.RemoveAllListeners ();
//			returnMenuButton.onClick.AddListener (() => SceneController.Instance.LoadMainMenu ());
//
//			quitButton.onClick.RemoveAllListeners ();
//			quitButton.onClick.AddListener (() => SceneController.Instance.Quit ());
//		}
//	}
//	bool GetIsRelease(){
//		if (nonPilotOptions.activeSelf) {
//			return false;
//		}
//		return true;
//	}
//	public void SetReplayTrue(){
//		isReplay = true;
//		isLogging = false;
//		loggingToggle.isOn = false;
//	}


//	public void SetReplayFalse(){
//		isReplay = false;
//		//shouldLog = true;
//	}

	void CheckGamifiedStatus()
	{
		#if GAMIFIED
			gamifiedText.text = "(GAMIFIED)";
	#else
			gamifiedText.text = " (VANILLA)";
		#endif
			
	}

//	public void SetLogging(){
//		if(isReplay){
//			isLogging = false;
//		}
//		else{
//			if(loggingToggle){
//				isLogging = loggingToggle.isOn;
//				Debug.Log("should log?: " + isLogging);
//			}
//		}
//
//	}

	//this currently justs shows if a valid audio output is attached; the mic test functionality is in InputMic.cs
//	void DoMicTest(){
//		if (micTestIndicator != null) {
//			if (AudioRecorder.CheckForRecordingDevice ()) {
//				micTestIndicator.color = Color.green;
//			} else {
//				micTestIndicator.color = Color.red;
//			}
//		}
//	}
	public void ChangeFirstEnvironment()
	{
		//Debug.Log("env dropdown val: " + firstEnvDropdown.value.ToString());
		envIndex = firstEnvDropdown.value;
		envDropdownIndex = firstEnvDropdown.value;
		switch (firstEnvDropdown.value) {

		case 0:
			env = Environment.SpaceStation;
			break;
		case 1: 
			env = Environment.WesternTown;
			break;
		case 2:
			env = Environment.Office;
			break;
		case 3:
			env = Environment.Apartment;
			break;
		default:
			env = Environment.SpaceStation;
			break;
		}
	}

	public void ChangeReevalType()
	{
		Debug.Log("reeval dropdown val: " + reevalDropdown.value.ToString());
		switch (reevalDropdown.value) {
		case 0:
			reevalType = ReevalType.Transition;
			break;
		case 1: 
			reevalType = ReevalType.Reward;
			break;
		default:
			reevalType = ReevalType.Transition;
			break;
		}
	}

	//public void ChangeTestingStatus()
	//{
	//	isTesting = testingToggle.isOn;
	//}
	//public void ChangeLearningStatus()
	//{
	//	isLearning = learningToggle.isOn;
	//}
	//public void ChangeReevalStatus()
	//{
	//	isReeval = reevalToggle.isOn;
	//}
	//public void ChangeTrainingStatus()
	//{
	//	isTraining = trainingToggle.isOn;
	//}


    public void SetConnectionMethod()
    {
        Debug.Log("setting connection method: " + connectionMethodDropdown.value.ToString());
        switch(connectionMethodDropdown.value)
        {
            case 0:
                connectionMethod = ConnectionMethod.BlackrockSync;
                break;
            case 1:
                connectionMethod = ConnectionMethod.Syncbox;
                break;
            case 2:
                connectionMethod = ConnectionMethod.Photosync;
                break;
            case 3:
                connectionMethod = ConnectionMethod.Demo;
                break;
            default:
                connectionMethod = ConnectionMethod.BlackrockSync;
                break;
        }

        //then update the connection method
        UpdateConnectionMethod();
    }

    void UpdateConnectionMethod()
    {
        if (connectionMethod == ConnectionMethod.BlackrockSync)
        {
            Config.isSystem2 = true;
            Config.isSyncbox = true;
            Config.isSystem3 = false;
        }
        else if (connectionMethod == ConnectionMethod.Syncbox || connectionMethod == ConnectionMethod.Photosync)
        {
            Config.isSyncbox = true;
            Config.isSystem2 = false;
            Config.isSystem3 = false;
        }
        //demo
        else
        {
            Config.isSyncbox = false;
            Config.isSystem2 = false;
            Config.isSystem3 = false;
        }

           sceneController.UpdateSceneConfig();
    }

    public void SetControlDevice()
    {
        Debug.Log("setting control device: " + controlDeviceDropdown.value.ToString());
        switch (controlDeviceDropdown.value)
        {
            case 0:
                controlDevice = ControlDevice.Keyboard;
                break;
            case 1:
                controlDevice = ControlDevice.Controller;
                break;
            default:
                controlDevice = ControlDevice.Keyboard;
                break;

        }

        sceneController.UpdateSceneConfig();
    }


    public void SetControlToggle()
    {
        Debug.Log("setting control toggle:  " + isControlToggle.isOn.ToString());
        Config.shouldForceControl = isControlToggle.isOn;

        sceneController.UpdateSceneConfig();
        
    }

    public void SetLanguage()
    {
        switch(languageDropdown.value)
        {
            case 0:
                currentLanguage = Language.English;
                break;
            case 1:
                currentLanguage = Language.Spanish;
                break;
            default:
                currentLanguage = Language.English;
                break;
        }
    }

    public void SetSessionDay()
    {
        switch(sessionDayDropdown.value)
        {
            case 0:
                currentEnvironmentType = EnvironmentType.WA; 
                break;
            case 1:
                currentEnvironmentType = EnvironmentType.SO;
                break;
            default:
                currentEnvironmentType = EnvironmentType.WA;
                break;
        }
        sceneController.UpdateSessionEnvironments(); //directly updates the environments for that particular day

        sceneController.UpdateSessionReevalConditions(sessionDayDropdown.value); // Day 1 = RR first, TR second ; Day 2 = TR first, RR second
    }


	// Use this for initialization
	void Start () {

	//	StartCoroutine(WordListGenerator.Instance.GenerateWordList());
		//StartCoroutine("RunExperiment");

	}

}
