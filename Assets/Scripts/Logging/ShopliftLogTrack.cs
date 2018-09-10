using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopliftLogTrack : LogTrack {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LogRegisterValues(int regVal)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "REGISTER_VALUE_SET" + separator + regVal.ToString ());
	}
	public void LogEnvironmentSelection(string envLabel)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ENVIRONMENT_CHOSEN" + separator + envLabel);
	}

	public void LogWaitEvent(string waitCause, bool hasBegun)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "WAITING_FOR_"+waitCause+"_PRESS" + separator + ((hasBegun == true) ? "STARTED" : "ENDED"));
	}

	public void LogLEDOn()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "PHOTODIODE_SQUARE" + separator + "ON");
	}

	public void LogLEDOff()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "PHOTODIODE_SQUARE" + separator + "OFF");
	}

	public void LogTimeout(float maxTime)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "TIMED_OUT " + separator  + maxTime.ToString());
	}
	public void LogButtonPress()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ACTION_BUTTON_PRESSED");
	}

	public void LogPathIndex(int pathIndex)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), (pathIndex==0) ? "LEFT_CORRIDOR" : "RIGHT_CORRIDOR");
	}

	public void LogPhaseEvent(int stageIndex, bool hasBegun)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "PHASE_" + stageIndex.ToString () + separator + ((hasBegun == true) ? "STARTED" : "ENDED"));
	}
	public void LogReassignEvent()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ROOMS_REASSIGNED");
	}
	public void LogRooms(string leftRoom, string rightRoom)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ROOM_CONFIG" + separator + "LEFT_ROOM" + separator + leftRoom + separator + "RIGHT_ROOM" + separator + rightRoom);
	}

	public void LogRewardReeval()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "REWARD_REEVAL");
	}

	public void LogTransitionReeval()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "TRANSITION_REEVAL");
	}
	public void LogDecisionEvent(bool isActive)
	{
			subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "DECISION_EVENT" + separator + isActive.ToString ());
	}

	public void LogSoloPrefImage(string prefImageName)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "SOLO_PREF_SLIDER_IMAGE" + separator + prefImageName);
	}

	public void LogComparativePrefImage(int prefGroup, string leftImageName,string rightImageName)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "COMPARATIVE_PREF_SLIDER" + separator + "LEFT_IMAGE" + separator + leftImageName + separator + "RIGHT_IMAGE" + separator + rightImageName);
	}

	public void LogSliderValue(string sliderType, float sliderVal)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "PREF_SLIDER" + separator + sliderType + separator + "VALUE" + separator + sliderVal.ToString());
	}

	public void LogFinalSliderValue(string sliderType,float chosenValue,bool isChosen)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "FINAL_SLIDER_VALUE" + separator + sliderType + separator + "VALUE" + separator + chosenValue + separator + ((isChosen) ? "CHOSEN" : "TIMED_OUT"));
	}
	public void LogSelectorPosition(int positionIndex,string roomName)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ANSWER_SELECTOR" + separator + "POSITION" + separator + positionIndex.ToString() + separator  + "ROOM" +  separator + roomName);
	}
	public void LogRegisterReward(int registerReward, int pathIndex)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "REGISTER_REWARD" + separator + registerReward.ToString () + separator  + ((pathIndex==0) ? "LEFT" : "RIGHT"));
	}
	public void LogDecision(int choice, int phase)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "PLAYER_DECISION" + separator + "PHASE_" + phase.ToString () + separator + "CHOICE" + separator + ((choice == 0) ? "LEFT" : "RIGHT"));
	}
	public void LogMoveEvent(int index, bool hasStarted)
	{
		if (hasStarted)
			subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "ROOM_" + index.ToString () + "_MOVE" + separator + "STARTED");
		else
			subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ROOM_" + index.ToString () + "_MOVE" + separator + "ENDED");
	}
	public void LogSneaking(Vector3 sneakPos, int camZoneIndex)
	{

		subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "CAM_SNEAKING" + separator + sneakPos.ToString () + separator + camZoneIndex.ToString ());
	}

	public void LogCameraLerpIndex (float randFactor, int envIndex)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "CAMERA_ZONE_POSITION_INDEX" + separator + randFactor.ToString () + separator +  "ENV" + separator + envIndex.ToString ());
	}

	public void LogEndTrial()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "END_TRIAL");
	}

	public void LogEndEnvironmentStage()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "END_ENVIRONMENT_STAGE");
	}
	public void LogEndSession()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "END_SESSION");
	}
}
