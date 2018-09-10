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

	public enum ReevalType
	{
		Transition,
		Reward
	}

	public static ReevalType reevalType;
	//build info
	string buildDate;
	public Text buildType;

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

	public static bool isTraining=true;
	public static bool isLearning=true;
	public static bool isReeval=true;
	public static bool isTesting=true;

	public static bool isRewardReeval = false;
	public static bool isTransitionReeval = false;

	public Toggle trainingToggle;
	public Toggle learningToggle;
	public Toggle reevalToggle;
	public Toggle testingToggle;

	public Dropdown firstEnvDropdown;
	public Dropdown reevalDropdown;


	public InputField subjectName;
	public Button checkpointButton;
	public Button resumeFromCheckpointButton;
	public Text checkpointDataText;
	public bool checkpointExists = false;

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
		string tempDir = subjectDirectory + "session_0" + "/";
		string sessionStartedFileName= "sessionStarted.txt";
		int sessionID = 0;
		string sessionIDString = "_0";
		string[] checkpointData = new string[8];
		bool shouldBreak = false;
		bool hasCheckpoint = false;
		while (File.Exists(tempDir + sessionStartedFileName) && !shouldBreak){
			sessionID++;

			sessionIDString = "_" + sessionID.ToString();
			//check if the session crashed
			string checkpointFilePath = tempDir + "checkpoint.txt";

			checkpointData = new string[8];
			if (File.Exists (checkpointFilePath)) {
				string checkpointText = File.ReadAllText (checkpointFilePath);
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
		}
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


		if (Config.isSyncbox) {
			buildType.text = "Syncbox";
		}
//		} else if (Config.isSystem3) {
//			if (Config.BuildVersion == Config.Version.RAA1) {
//				buildType.text = "SYS3 Record-Only";
//			} else {
//				buildType.text = "SYS3 Stim"; 
//			}
//		} else
			buildType.text = "Demo";

		buildDate = 
			new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToString();
		UnityEngine.Debug.Log (buildDate);
		buildType.text += " | " + buildDate;

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
		Debug.Log("env dropdown val: " + firstEnvDropdown.value.ToString());
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

	public void ChangeTestingStatus()
	{
		isTesting = testingToggle.isOn;
	}
	public void ChangeLearningStatus()
	{
		isLearning = learningToggle.isOn;
	}
	public void ChangeReevalStatus()
	{
		isReeval = reevalToggle.isOn;
	}
	public void ChangeTrainingStatus()
	{
		isTraining = trainingToggle.isOn;
	}


	// Use this for initialization
	void Start () {

	//	StartCoroutine(WordListGenerator.Instance.GenerateWordList());
		//StartCoroutine("RunExperiment");

	}

}
