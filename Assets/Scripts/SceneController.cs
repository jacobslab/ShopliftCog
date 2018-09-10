using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour { //there can be a separate scene controller in each scene

	public GameObject menuObj;
	public GameObject sceneObj;
	//SINGLETON
	private static SceneController _instance;
	
	public static SceneController Instance{
		get{
			return _instance;
		}
	}
	
	void Awake(){
//		if (_instance != null) {
//			Debug.Log("Instance already exists!");
//			Destroy(transform.gameObject);
//			return;
//		}
		_instance = this;
	}


	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		DontDestroyOnLoad (this.gameObject);

	}


	// Update is called once per frame
	void Update () {

	}

	public void LoadMainMenu(){
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}

		Debug.Log("loading main menu!");
		SubjectReaderWriter.Instance.RecordSubjects();
		Application.LoadLevel(0);
	}

	public void CheckForPreviousSessions()
	{
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}
		ExperimentSettings.Instance.subjectSelectionController.SendMessage("AddNewSubject");
		if (ExperimentSettings.currentSubject != null) {
			ExperimentSettings.Instance.UpdateCheckpointStatus ();
		} else
			Debug.Log ("SUBJECT DOES NOT EXIST");
	}

	public void LoadFromCheckpoint()
	{
		//should be no new data to record for the subject
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}
		Experiment.loadFromCheckpoint = true;
		ExperimentSettings.Instance.subjectSelectionController.SendMessage("AddNewSubject");
		if(ExperimentSettings.currentSubject != null){
			LoadExperimentLevel();
		}
	}

	public void LoadExperiment(){
		//should be no new data to record for the subject
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}
			
			ExperimentSettings.Instance.subjectSelectionController.SendMessage("AddNewSubject");
			if(ExperimentSettings.currentSubject != null){
				LoadExperimentLevel();
			}
		


	}

	void LoadExperimentLevel(){
		if (ExperimentSettings.currentSubject.trials < Config.GetTotalNumTrials ()) {
			Debug.Log ("loading experiment!");
//			Application.LoadLevel (1);
			menuObj.SetActive(false);
			Cursor.visible = false;
			sceneObj.SetActive (true);
		} else {
			Debug.Log ("Subject has already finished all blocks! Loading end menu.");
			Application.LoadLevel (2);
		}
	}

	public void LoadEndMenu(){
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}

		SubjectReaderWriter.Instance.RecordSubjects();
		Debug.Log("loading end menu!");
		Application.LoadLevel(2);
	}

	public void Quit(){
#if !UNITY_WEBPLAYER
		SubjectReaderWriter.Instance.RecordSubjects();
#endif
		Application.Quit();
	}

	void OnApplicationQuit(){
		Debug.Log("On Application Quit!");
#if !UNITY_WEBPLAYER
		SubjectReaderWriter.Instance.RecordSubjects();
#endif
	}
}
