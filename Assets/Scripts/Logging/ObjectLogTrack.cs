using UnityEngine;
using System.Collections;

public class ObjectLogTrack : LogTrack {
	string nameToLog { get { return GetNameToLog (); } }

	static public bool isFirst = true;
	public bool firstLog = false; //should log spawned on the first log

	//if we want to only log objects when something has changed... should start with keep track of last positions/rotations.
	//or I could set up some sort of a delegate system.
	Vector3 lastPosition;
	Quaternion lastRotation;
	Vector3 lastScale;
	bool lastVisibility;

	void Awake(){
		
	}

	//log on late update so that everything for that frame gets set first
	void LateUpdate(){
		if (ExperimentSettings.isLogging) {
			if (isFirst == true)
			{
				Experiment.Instance.shopLiftLog.LogMetaData();
				Experiment.Instance.shopLiftLog.LogDay(ExperimentSettings.staticSessionDay + 1);
				Experiment.Instance.shopLiftLog.LogExpType(ExperimentSettings.staticCurrentExpType);
				Experiment.Instance.shopLiftLog.LogExpXRanges(-885, -735);
				Experiment.Instance.shopLiftLog.LogExpYRanges(1.3f, 1.7f);
				Experiment.Instance.shopLiftLog.LogExpZRanges(-38, 26);
				switch (((ExperimentSettings.staticSessionDay*2)+1) + ExperimentSettings.Instance.ExperimentValue)
				{
					case 1:
						Experiment.Instance.shopLiftLog.LogExpRoomDefinition(1);
						break;
					case 2:
						Experiment.Instance.shopLiftLog.LogExpRoomDefinition(2);
						break;
					case 3:
						Experiment.Instance.shopLiftLog.LogExpRoomDefinition(3);
						break;
					case 4:
						Experiment.Instance.shopLiftLog.LogExpRoomDefinition(4);
						break;
					case 5:
						Experiment.Instance.shopLiftLog.LogExpRoomDefinition(5);
						break;
					case 6:
						Experiment.Instance.shopLiftLog.LogExpRoomDefinition(6);
						break;
				}
				
				isFirst = false;
			}
			Log ();
		}
	}

	void Log ()
	{
		//the following is set up to log properties only when they change, or on an initial log.

		if (firstLog){
			//			LogSpawned();
			
			LogPosition();
			LogPositionNew();
			LogRotation ();
			LogScale ();
			firstLog = false;
		}

		if (lastPosition != transform.position) {
			LogPosition ();
			LogPositionNew();
		}
		if (lastRotation != transform.rotation) {
			LogRotation ();
		}
		if (lastScale != transform.localScale) {
			LogScale ();
		}
//		if (visibilityToggler != null) {
//			if (lastVisibility != visibilityToggler.GetVisibility() || !firstLog) {
//				LogVisibility ();
//
//				firstLog = true; //set this to true so we don't log the other properties twice on the first log...
//
//				//log all basic properties -- easier for replay, when there are UI copies of objects...
//				//...then the main object reappears -- but the position was not being set properly.
//				if(lastVisibility && firstLog){
//					LogPosition();
//					LogRotation();
//					LogScale();
//				}
//			}
//		}

//		firstLog = true;
	}

	void LogSpawned(){
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "SPAWNED" + separator + gameObject.tag);
	}

	void LogPosition(){
		lastPosition = transform.position;
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "POSITION" + separator + transform.position.x + separator + transform.position.y + separator + transform.position.z);
	}

	void LogPositionNew() {
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "POSITION" + separator + transform.position.x + separator + transform.position.y + separator + transform.position.z);
	}


	void LogRotation(){
		lastRotation = transform.rotation;
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "ROTATION" + separator + transform.rotation.eulerAngles.x + separator + transform.rotation.eulerAngles.y + separator + transform.rotation.eulerAngles.z);
	}

	void LogScale(){
		lastScale = transform.localScale;
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "SCALE" + separator + transform.localScale.x + separator + transform.localScale.y + separator + transform.localScale.z);
	}
	
//	void LogVisibility(){
//		lastVisibility = visibilityToggler.GetVisibility();
//		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "VISIBILITY" + separator + lastVisibility);
//	}

	public void LogShadowSettings(UnityEngine.Rendering.ShadowCastingMode shadowMode){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "SHADOW_SETTING" + separator + shadowMode);
		}
	}

	public void LogLayerChange(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "LAYER_CHANGE" + separator + gameObject.layer);
		}
	}

	void LogDestroy(){
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), nameToLog + separator + "DESTROYED");
	}

	void OnDestroy(){
		if (ExperimentSettings.isLogging) {
			LogDestroy();
		}
	}

	string GetNameToLog(){
		string name = gameObject.name;
		return name;
	}

}