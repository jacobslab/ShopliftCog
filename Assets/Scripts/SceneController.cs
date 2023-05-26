using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{ //there can be a separate scene controller in each scene
    ExperimentSettings expSettings { get { return ExperimentSettings.Instance; } }

    public GameObject menuObj;
    public GameObject sceneObj;

    public GameObject syncPulser;
    public GameObject syncPulsingImage;

    public List<GameObject> environmentList_D3;
    public List<GameObject> environmentList_SO;
    public List<GameObject> environmentList_WA;

    public ShoplifterScript shoplifterRef; 

    //SINGLETON
    private static SceneController _instance;

    public VideoClip keyboard_control_video;
    public VideoClip keyboard_video;

    public VideoClip controller_control_video;
    public VideoClip controller_video;

    public GameObject instructionVideoPlayer;

    public static SceneController Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
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

    public void UpdateSceneConfig()
    {
        if (expSettings.connectionMethod == ExperimentSettings.ConnectionMethod.Photosync)
        {
            syncPulser.SetActive(true);
            syncPulsingImage.SetActive(true);
            shoplifterRef.sensorImage.alpha = 1f;
            syncPulser.GetComponent<SyncPulser>().enabled = true;
        }
        else
        {
            if (!Config.isSyncbox)
            {
                syncPulser.SetActive(false);
            }
            else
            {
                syncPulser.SetActive(true);
                syncPulser.GetComponent<SyncPulser>().enabled = false;
                syncPulser.GetComponent<SyncboxControl>().enabled = true;
            }
            syncPulsingImage.SetActive(false);
            shoplifterRef.sensorImage.alpha = 0f;
        }

        if (expSettings.controlDevice == ExperimentSettings.ControlDevice.Keyboard)
        {
            if (Config.shouldForceControl)
                instructionVideoPlayer.GetComponent<VideoPlayer>().clip = keyboard_control_video;
            else
                instructionVideoPlayer.GetComponent<VideoPlayer>().clip = keyboard_video;
        }
        else
        {
            if (Config.shouldForceControl)
                instructionVideoPlayer.GetComponent<VideoPlayer>().clip = controller_control_video;
            else
                instructionVideoPlayer.GetComponent<VideoPlayer>().clip = controller_video;
        }
    }

    public void UpdateSessionEnvironments()
    {
        if(expSettings.currentEnvironmentType == ExperimentSettings.EnvironmentType.WA)
        {
            shoplifterRef.environments[0] = environmentList_WA[0];
            shoplifterRef.environments[1] = environmentList_WA[1];

        }
        else if(expSettings.currentEnvironmentType == ExperimentSettings.EnvironmentType.SO)
        {
            shoplifterRef.environments[0] = environmentList_SO[0];
            shoplifterRef.environments[1] = environmentList_SO[1];
        }
        else
        {
            shoplifterRef.environments[0] = environmentList_D3[0];
            shoplifterRef.environments[1] = environmentList_D3[1];


        }
    }

    public void UpdateExperimentNumber(int ExpNo)
    {
        shoplifterRef.environmentNumber = ExpNo;
    }
    public void UpdateSessionReevalConditions(int reevalConditionIndex)
    {
        switch(reevalConditionIndex)
        {
            case 0:
                //shoplifterRef.reevalConditions[0] = 0;    //0 - RR; 1 - TR //2- RC //3- TC
                shoplifterRef.reevalConditions[0] = 0;
                shoplifterRef.reevalConditions[1] = 1;
                break;
            case 1:
                shoplifterRef.reevalConditions[0] = 0;
                //shoplifterRef.reevalConditions[1] = 0;
                shoplifterRef.reevalConditions[1] = 2;
                break;
            case 2:
                shoplifterRef.reevalConditions[0] = 1;
                //shoplifterRef.reevalConditions[1] = 0;
                shoplifterRef.reevalConditions[1] = 3;
                break;
            default:
                //shoplifterRef.reevalConditions[0] = 0;
                shoplifterRef.reevalConditions[0] = 1;
                shoplifterRef.reevalConditions[1] = 1;
                break;
        }

        ExperimentSettings.staticCurrentExpType = shoplifterRef.reevalConditions[shoplifterRef.environmentNumber];
}


    public void LoadMainMenu(){
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}

		Debug.Log("loading main menu!");
		//SubjectReaderWriter.Instance.RecordSubjects();
		SceneManager.LoadScene(0);
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
			SceneManager.LoadScene(2);
		}
	}

	public void LoadEndMenu(){
		if(Experiment.Instance != null){
			Experiment.Instance.OnExit();
		}

		//SubjectReaderWriter.Instance.RecordSubjects();
		Debug.Log("loading end menu!");
		SceneManager.LoadScene(2);
	}

	public void Quit(){
#if !UNITY_WEBPLAYER
		//SubjectReaderWriter.Instance.RecordSubjects();
#endif
		Application.Quit();
	}

	void OnApplicationQuit(){
		Debug.Log("On Application Quit!");
#if !UNITY_WEBPLAYER
		//SubjectReaderWriter.Instance.RecordSubjects();
#endif
	}
}
