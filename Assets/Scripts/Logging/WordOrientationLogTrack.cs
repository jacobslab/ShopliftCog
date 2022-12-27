using UnityEngine;
using System.Collections;

public class WordOrientationLogTrack : LogTrack {

	public void LogOrientationStarted(){
		if (ExperimentSettings.isLogging) {
			if(ExperimentSettings.practice)
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "PRACTICE_ORIENT");
			else
				subjectLog.Log (GameClock.SystemTime_Milliseconds,"1" + separator +  "ORIENT");
		}
	}

	public void LogOrientationStopped(){
		if (ExperimentSettings.isLogging) {
			if (ExperimentSettings.practice)
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "PRACTICE_ORIENT_OFF");
			else
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "ORIENT_OFF");
		}
	}

	public void LogRetrievalOrientationStarted()
	{
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "RETRIEVAL_ORIENT");
		}
	}

}