using UnityEngine;
using System.Collections;
using System.IO;

public class Experiment : MonoBehaviour {

	//logging
	private string subjectLogfile; //gets set based on the current subject in Awake()
	public Logger_Threading subjectLog;
	private string eegLogfile; //gets set based on the current subject in Awake()
	public Logger_Threading eegLog;
	public string sessionDirectory;
	public static string sessionStartedFileName = "sessionStarted.txt";
	public static int sessionID;

	//avatar
	public Player player;

	public ShoplifterScript shopLift;


	//logging
	public ShopliftLogTrack shopLiftLog;

	//state enum
	public ExperimentState currentState = ExperimentState.instructionsState;

	//checkpoint
	public static bool shouldCheckpoint=false;
	public string[] checkpointData;
	public int checkpointedEnvIndex = 0;
	public string checkpointedPhaseName = "NONE";
	public int checkpointedReevalIndex = 0;
	public string leftRoomName="";
	public int leftReward=0;
	public string rightRoomName="";
	public int rightReward=0;
	public static bool loadFromCheckpoint = false;


	public SubjectSelectionController subjectController;
	private string wordsLogged="";
	public enum ExperimentState
	{
		instructionsState,
		inExperiment,
		inExperimentOver,
	}

	//bools for whether we have started the state coroutines
	bool isRunningInstructions = false;
	bool isRunningExperiment = false;


	//EXPERIMENT IS A SINGLETON
	private static Experiment _instance;

	public static Experiment Instance{
		get{
			return _instance;
		}
	}

	void Awake(){
        Cursor.visible = false;
		if (_instance != null) {
			Debug.Log("Instance already exists!");
			return;
		}
		_instance = this;

	//	cameraController.SetInGame();

		if (ExperimentSettings.isLogging) {
			InitLogging();
		}
//		else if(ExperimentSettings.isReplay) {
//			instructionsController.TurnOffInstructions();
//		}

	}


	
	//TODO: move to logger_threading perhaps?
	void InitLogging(){
		string subjectDirectory = ExperimentSettings.defaultLoggingPath + ExperimentSettings.currentSubject.name + "/";
		sessionDirectory = subjectDirectory + "session_0" + "/";
		
		sessionID = 0;
		string sessionIDString = "_0";
		
		if(!Directory.Exists(subjectDirectory)){
			Directory.CreateDirectory(subjectDirectory);
		}
		Debug.Log ("does " + sessionDirectory + "and" + sessionStartedFileName + " exist");
		while (File.Exists(sessionDirectory + sessionStartedFileName)){
			sessionID++;
			
			sessionIDString = "_" + sessionID.ToString();
			//check if the session crashed
			if (loadFromCheckpoint) {
				string checkpointFilePath = sessionDirectory + "checkpoint.txt";
				checkpointData = new string[8];
				if (File.Exists (checkpointFilePath)) {
					string checkpointText = File.ReadAllText (checkpointFilePath);
					if (checkpointText.Contains ("ONGOING")) {
						Debug.Log ("previous session crashed; use checkpoint details to resume that session");
						checkpointData = checkpointText.Split ("\t" [0]);
						shouldCheckpoint = true;
					}
					for (int i = 0; i < checkpointData.Length; i++) {
						Debug.Log (checkpointData [i]);
					}
					UpdateCheckpointedVariables (checkpointData);

				}
			}
			sessionDirectory = subjectDirectory + "session" + sessionIDString + "/";
		}

		Debug.Log ("current session is: " + sessionID.ToString ());


		//delete old files.
		if(Directory.Exists(sessionDirectory)){
			DirectoryInfo info = new DirectoryInfo(sessionDirectory);
			FileInfo[] fileInfo = info.GetFiles();
			for(int i = 0; i < fileInfo.Length; i++){
				File.Delete(fileInfo[i].ToString());
			}
		}
		else{ //if directory didn't exist, make it!
			Directory.CreateDirectory(sessionDirectory);
		}
		
		subjectLog.fileName = sessionDirectory + ExperimentSettings.currentSubject.name + "Log" + ".txt";
		eegLog.fileName = sessionDirectory + ExperimentSettings.currentSubject.name + "EEGLog" + ".txt";
	}

	//In order to increment the session, this file must be present. Otherwise, the session has not actually started.
	//This accounts for when we don't successfully connect to hardware -- wouldn't want new session folders.
	//Gets created in ShoplifterScript when the task is run.
	public void CreateSessionStartedFile(){
		StreamWriter newSR = new StreamWriter (sessionDirectory + sessionStartedFileName);
	}


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//Proceed with experiment if we're not in REPLAY mode
//		if (!ExperimentSettings.isReplay) { //REPLAY IS HANDLED IN REPLAY.CS VIA LOG FILE PARSING
////			Debug.Log("not replay");
//			if (currentState == ExperimentState.instructionsState && !isRunningInstructions) {
//				Debug.Log("running instructions");
//
//				StartCoroutine(RunInstructions());
//
//			}
//			else if (currentState == ExperimentState.inExperiment && !isRunningExperiment) {
//				Debug.Log("running experiment");
//				StartCoroutine(BeginExperiment());
//			}
//
//		}
	}

	public void UpdateCheckpointedVariables(string[] checkpointData)
	{
		checkpointedEnvIndex = int.Parse(checkpointData [1]);
		checkpointedPhaseName = checkpointData [2];
		checkpointedReevalIndex = int.Parse(checkpointData [3]);
		leftRoomName = checkpointData [4];
		leftReward = int.Parse (checkpointData [5]);
		rightRoomName = checkpointData [6];
		rightReward = int.Parse (checkpointData [7]);

		switch (checkpointedPhaseName) {
		case "TRAINING":
			//no action needed, assuming all the flags point to true, as they do by default
			break;
		case "LEARNING":
			ExperimentSettings.isTraining = false;
			break;
		case "REEVALUATION":
			ExperimentSettings.isTraining = false;
			ExperimentSettings.isLearning = false;
			break;
		case "TESTING":
			ExperimentSettings.isTraining = false;
			ExperimentSettings.isLearning = false;
			ExperimentSettings.isReeval = false;
			break;
		case "POST-TEST":
			ExperimentSettings.isTraining = false;
			ExperimentSettings.isLearning = false;
			ExperimentSettings.isReeval = false;
			ExperimentSettings.isTesting = false;
			break;
		case "NONE":
			//do nothing
		default:
			//do nothing
			break;
		}
	}

	public IEnumerator RunOutOfTrials(){
	//	yield return StartCoroutine(ShowSingleInstruction("You have finished your trials! \nPress the button to proceed.", true, true, false, 0.0f));
//		instructionsController.SetInstructionsColorful(); //want to keep a dark screen before transitioning to the end!
//		instructionsController.DisplayText("...loading end screen...");
		EndExperiment();

		yield return 0;
	}

	public IEnumerator RunInstructions(){
		isRunningInstructions = true;

		//IF THERE ARE ANY PRELIMINARY INSTRUCTIONS YOU WANT TO SHOW BEFORE THE EXPERIMENT STARTS, YOU COULD PUT THEM HERE...

		currentState = ExperimentState.inExperiment;
		isRunningInstructions = false;

		yield return 0;

	}


	public IEnumerator BeginExperiment(){
		isRunningExperiment = true;
		Debug.Log ("about to begin experiment");
		
		yield return StartCoroutine(RunOutOfTrials()); //calls EndExperiment()

		yield return 0;

	}

	public void EndExperiment(){
		Debug.Log ("Experiment Over");
		currentState = ExperimentState.inExperimentOver;
		isRunningExperiment = false;
	}

	//TODO: move to instructions controller...
	public IEnumerator ShowSingleInstruction(string line, bool isDark, bool waitForButton, bool addRandomPostJitter, float minDisplayTimeSeconds){
		if(isDark){
	//		instructionsController.SetInstructionsColorful();
		}
		else{
	//		instructionsController.SetInstructionsTransparentOverlay();
		}
	//	instructionsController.DisplayText(line);

		yield return new WaitForSeconds (minDisplayTimeSeconds);

		if (waitForButton) {
			yield return StartCoroutine (WaitForActionButton ());
		}

		if (addRandomPostJitter) {
			yield return StartCoroutine(WaitForJitter ( Config.randomJitterMin, Config.randomJitterMax ) );
		}

	//	instructionsController.TurnOffInstructions ();
	}
	
	public IEnumerator WaitForActionButton(){
		bool hasPressedButton = false;
		while(Input.GetAxis("Action Button") != 0f){
			yield return 0;
		}
		while(!hasPressedButton){
			if(Input.GetAxis("Action Button") == 1.0f){
				hasPressedButton = true;
			}
			yield return 0;
		}
	}

//	public void LogWords(string wordToBeLogged)
//	{
//		UnityEngine.Debug.Log ("logging: " + wordToBeLogged);
//		wordsLogged += wordToBeLogged.ToLower() + "\n";
//		if (BillboardText.billboardCount >= BillboardText.listLength) {
//			int trialCount = SceneController.Instance.GetTrialCount ();
//			string trialTxtName = Experiment.Instance.sessionDirectory + trialCount.ToString () + ".txt";
//			System.IO.File.WriteAllText (trialTxtName, wordsLogged);
//			wordsLogged = "";
//		}
//	}
//
	public IEnumerator WaitForJitter(float minJitter, float maxJitter){
		float randomJitter = Random.Range(minJitter, maxJitter);
//		trialController.GetComponent<TrialLogTrack>().LogWaitForJitterStarted(randomJitter);
		
		float currentTime = 0.0f;
		while (currentTime < randomJitter) {
			currentTime += Time.deltaTime;
			yield return 0;
		}

	//	trialController.GetComponent<TrialLogTrack>().LogWaitForJitterEnded(currentTime);
	}


	public void OnExit(){ //call in scene controller when switching to another scene!
		if (ExperimentSettings.isLogging) {
			subjectLog.close ();
			eegLog.close ();
		}
	}

	public void OnExperimentEnd(){
		if (ExperimentSettings.isLogging) {
			subjectLog.close ();
			eegLog.close ();
//			File.Copy ("/Users/" + System.Environment.UserName + "/Library/Logs/Unity/Player.log", sessionDirectory+"Player.log");
			Application.Quit ();
		}
	}

	void OnApplicationQuit()
	{
		subjectLog.close ();
		eegLog.close ();

//		File.Copy ("/Users/" + System.Environment.UserName + "/Library/Logs/Unity/Player.log", sessionDirectory+"Player.log");
	}


}
