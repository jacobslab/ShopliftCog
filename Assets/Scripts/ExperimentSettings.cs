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
		SpaceStation
	}

	public static Environment env;
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
	public static bool isTesting=true;
	public static bool isReeval=true;
	public static bool isLearning=true;

	public static bool isRewardReeval = false;
	public static bool isTransitionReeval = false;

	public Toggle trainingToggle;
	public Toggle learningToggle;
	public Toggle reevalToggle;
	public Toggle testingToggle;

	public Dropdown firstEnvDropdown;

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
//		DoMicTest ();
//		if(Application.loadedLevel==0)
//			InitMainMenuLabels ();
//		CheckGamifiedStatus ();
//		if (SceneManager.GetActiveScene ().name == "EndMenu") {
//			AttachSceneController ();
//		}
	}
	// Update is called once per frame
	void Update () {
		
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
		switch (firstEnvDropdown.value) {
		case 0:
			env = Environment.Cybercity;
			break;
		case 1: 
			env = Environment.SpaceStation;
			break;
		default:
			env = Environment.Cybercity;
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
