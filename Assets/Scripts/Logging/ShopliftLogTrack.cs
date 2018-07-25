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

	public void LogStageEvent(int stageIndex, bool hasBegun)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "STAGE_" + stageIndex.ToString () + separator + ((hasBegun == true) ? "STARTED" : "ENDED"));
	}
	public void LogReassignEvent()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ROOMS_REASSIGNED");
	}
	public void LogRooms(string leftRoom, string rightRoom)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "ROOM_CONFIG" + separator + "LEFT_ROOM" + separator + leftRoom + separator + "RIGHT_ROOM" + separator + rightRoom);
	}

	public void LogDecisionEvent(bool isActive)
	{
			subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "DECISION_EVENT" + separator + isActive.ToString ());
	}

	public void LogRegisterReward(int phase1Choice,int phase2Choice,int registerReward)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "REGISTER_REWARD" + separator + registerReward.ToString () +separator + "PHASE_1" + separator + ((phase1Choice==0) ? "LEFT":"RIGHT") + separator + "PHASE_2" + separator + ((phase2Choice == 0 ) ? "LEFT" : "RIGHT" ));
	}
	public void LogDecision(int choice, int phase)
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "PLAYER_DECISION" + separator + "PHASE_" + phase.ToString () + separator + "CHOICE" + separator + ((choice == 0) ? "LEFT" : "RIGHT"));
	}
	public void LogMoveEvent(int index, bool hasStarted)
	{
		if (hasStarted)
			subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "PHASE_" + index.ToString () + "_MOVE" + separator + "STARTED");
		else
			subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PHASE_" + index.ToString () + "_MOVE" + separator + "ENDED");
	}
	public void LogSneaking(Vector3 sneakPos, int camZoneIndex)
	{

		subjectLog.Log (GameClock.SystemTime_Milliseconds,subjectLog.GetFrameCount(), "CAM_SNEAKING" + separator + sneakPos.ToString () + separator + camZoneIndex.ToString ());
	}

	public void LogEndTrial()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "END_TRIAL");
	}
	public void LogEndSession()
	{
		subjectLog.Log (GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount (), "END_SESSION");
	}
}
