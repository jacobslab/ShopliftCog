using UnityEngine;
using System.Collections;

public class TrialLogTrack : LogTrack {


	bool firstLog = false;

	//log on late update so that everything for that frame gets set first
	void LateUpdate () {
		//just log the environment info on the first frame
		if (ExperimentSettings.isLogging && !firstLog) {
			//presumably testing logging
//			LogBegin ();
//			LogEnd ();
//			LogBegin ();
			//log session
			LogSessionStart();
			//LogMicTest ();

			firstLog = true;
		}
	}

	//gets called from trial controller instead of in update!
	public void Log(int trialNumber){
		if (ExperimentSettings.isLogging) {
			LogTrial (trialNumber);
		}
	}

	void LogBegin()
	{
		Debug.Log ("LOGGING BEGINS");
		subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "B" + separator + "Logging Begins");
	}
	void LogEnd()
	{
		Debug.Log ("LOGGING ENDS");
		subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "E" + separator + "Logging Ends");
	}

	public void LogPauseEvent(bool isPaused)
	{
		Debug.Log ("game paused");
		if(isPaused)
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "TASK_PAUSED");
		else
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "TASK_RESUMED");
	}

	public void LogMicTest()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "MIC_TEST");
	}

	void LogSessionStart(){
		Debug.Log ("LOGGED SESSION START");
		string buildVersion = Config.BuildVersion.ToString ();
		subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "SESS_START" + separator + "1" + separator + buildVersion + " v"+Config.VersionNumber);
	}


	//LOGGED ON THE START OF THE TRIAL.
	public void LogTrial(int trialNumber){
		if(ExperimentSettings.practice)
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "PRACTICE_TRIAL");
		else
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "TRIAL" + separator + trialNumber + separator + "NONSTIM");
	}



}