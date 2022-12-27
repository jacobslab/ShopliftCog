using UnityEngine;
using System.Collections;

public class WordEncodingLogTrack : LogTrack {

	public void LogWordTextOn(string word, int wordCount)
	{
		if (ExperimentSettings.isLogging) {
			string stimStatus = ExperimentSettings.shouldStim ? "STIM" : "NON_STIM";
			if(!ExperimentSettings.practice)
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "WORD" + separator + "text" + separator + word + separator + wordCount.ToString() + separator + stimStatus);
			else
				subjectLog.Log (GameClock.SystemTime_Milliseconds, "1" + separator + "PRACTICE_WORD" + separator + word);
				
		}
	}
	public void LogWordTextOff()
	{
		if(ExperimentSettings.isLogging)
		{
			if(!ExperimentSettings.practice)
				subjectLog.Log(GameClock.SystemTime_Milliseconds, "1" + separator +"WORD_OFF");
			else
				subjectLog.Log(GameClock.SystemTime_Milliseconds, "1" + separator +"PRACTICE_WORD_OFF");
				
		}
	}
}
