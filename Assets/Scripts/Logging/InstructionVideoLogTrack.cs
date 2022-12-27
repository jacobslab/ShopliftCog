using UnityEngine;
using System.Collections;

public class InstructionVideoLogTrack : LogTrack {

	public void LogInstructionVideoStarted(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "INSTRUCT_VIDEO" + separator + "ON");
		}
	}

	public void LogInstructionVideoStopped(){
		if (ExperimentSettings.isLogging) {
			subjectLog.Log (GameClock.SystemTime_Milliseconds, "0" + separator + "INSTRUCT_VIDEO" + separator + "OFF");
		}
	}

}
