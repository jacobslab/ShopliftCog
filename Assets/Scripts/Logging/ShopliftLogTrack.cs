using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopliftLogTrack : LogTrack
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void LogRegisterValues(int regVal)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "REGISTER_VALUE_SET" + separator + regVal.ToString());
	}
	public void LogEnvironmentSelection(string envLabel)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ENVIRONMENT_CHOSEN" + separator + envLabel);
	}

	public void LogWaitEvent(string waitCause, bool hasBegun)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "WAITING_FOR_" + waitCause + "_PRESS" + separator + ((hasBegun == true) ? "STARTED" : "ENDED"));
	}

	public void LogLEDOn()
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PHOTODIODE_SQUARE" + separator + "ON");
	}

	public void LogLEDOff()
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PHOTODIODE_SQUARE" + separator + "OFF");
	}

	public void LogTimeout(float maxTime)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TIMED_OUT " + separator + maxTime.ToString());
	}
	public void LogButtonPress()
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ACTION_BUTTON_PRESSED");
	}

	public void LogPathIndex(int pathIndex)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), (pathIndex == 0) ? "LEFT_CORRIDOR" : "RIGHT_CORRIDOR");
	}

	public void LogInstructionVideoEvent(bool hasStarted)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "INSTRUCTION_VIDEO" + separator + ((hasStarted == true) ? "STARTED" : "ENDED"));
	}

	public void LogPhaseEvent(string phaseName, bool hasStarted)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), phaseName + separator + ((hasStarted == true) ? "STARTED" : "ENDED"));
	}
	public void LogTextInstructions(int instNumber, bool hasStarted)
	{

		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), ((instNumber == 1) ? "INTRO" : "TRAINING") + separator + ((hasStarted == true) ? "STARTED" : "ENDED"));
	}
	public void LogReassignEvent()
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ROOMS_REASSIGNED");
	}
	public void LogRooms(string leftRoom, string rightRoom)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ROOM_CONFIG" + separator + "LEFT_ROOM" + separator + leftRoom + separator + "RIGHT_ROOM" + separator + rightRoom);
	}

	public void LogRewardReeval()
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "REWARD_REEVAL");
	}

	public void LogTransitionReeval()
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRANSITION_REEVAL");
	}
	public void LogDecisionEvent(bool isActive)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "DECISION_EVENT" + separator + isActive.ToString());
	}

	public void LogPauseEvent(bool isPaused)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "GAME_PAUSED" + separator + isPaused.ToString());
	}

	public void LogSoloPrefImage(string prefImageName)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "SOLO_PREF_SLIDER_IMAGE" + separator + prefImageName);
	}

	public void LogComparativePrefImage(int prefGroup, string leftImageName, string rightImageName)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "COMPARATIVE_PREF_SLIDER" + separator + "LEFT_IMAGE" + separator + leftImageName + separator + "RIGHT_IMAGE" + separator + rightImageName);
	}

	public void LogSliderValue(string sliderType, float sliderVal)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PREF_SLIDER" + separator + sliderType + separator + "VALUE" + separator + sliderVal.ToString());
	}

	public void LogFinalSliderValue(string sliderType, float chosenValue, bool isChosen)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "FINAL_SLIDER_VALUE" + separator + sliderType + separator + "VALUE" + separator + chosenValue + separator + ((isChosen) ? "CHOSEN" : "TIMED_OUT"));
	}
	public void LogSelectorPosition(int positionIndex, string roomName)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ANSWER_SELECTOR" + separator + "POSITION" + separator + positionIndex.ToString() + separator + "ROOM" + separator + roomName);
	}
	public void LogRegisterEvent(bool isShowing)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "REWARD_TEXT" + separator + ((isShowing) ? "ON" : "OFF"));
	}
	public void LogBaselineImage(string imageName)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "BASELINE_IMAGE" + separator + imageName);
	}
	public void LogRegisterReward(int registerReward, int pathIndex)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "REGISTER_REWARD" + separator + registerReward.ToString() + separator + ((pathIndex == 0) ? "LEFT" : "RIGHT"));
	}
	public void LogDecision(int choice, int phase)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER_DECISION" + separator + "PHASE_" + phase.ToString() + separator + "CHOICE" + separator + ((choice == 0) ? "LEFT" : "RIGHT"));
	}
	public void LogMoveEvent(int index, bool hasStarted)
	{
		if (hasStarted)
			subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ROOM_" + index.ToString() + "_MOVE" + separator + "STARTED");
		else
			subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ROOM_" + index.ToString() + "_MOVE" + separator + "ENDED");
	}
	public void LogSneaking(Vector3 sneakPos, int camZoneIndex)
	{

		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "CAM_SNEAKING" + separator + sneakPos.ToString() + separator + camZoneIndex.ToString());
	}

	public void LogCameraLerpIndex(float randFactor, int envIndex)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "CAMERA_ZONE_POSITION_INDEX" + separator + randFactor.ToString() + separator + "ENV" + separator + envIndex.ToString());
	}

	public void LogEndTrial()
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "END_TRIAL");
	}

	public void LogEndTrialScreen(bool isActive, bool hasTips)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "END_TRIAL_SCREEN" + separator + ((isActive) ? "ON" : "OFF") + separator + "TIPS" + separator + ((hasTips) ? "TRUE" : "FALSE"));
	}


	//multiple choice
	public void LogMultipleChoiceTexture(int index, string textureName)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "MULTIPLE_CHOICE_TEXTURE" + separator + index.ToString() + separator + textureName);
	}
	public void LogMultipleChoiceFocusImage(string focusImageName)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "MULTIPLE_CHOICE_FOCUS_IMAGE" + separator + focusImageName);
	}
	public void LogMultipleChoiceResponse(float chosenValue, int correctChoice, bool isChosen)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "MULTIPLE_CHOICE_RESPONSE" + separator + chosenValue.ToString() + separator + correctChoice.ToString() + separator + ((isChosen) ? "CHOSEN" : "TIMED_OUT"));
	}


	public void LogEndEnvironmentStage(bool isActive)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "END_ENVIRONMENT_STAGE" + separator + ((isActive) ? "ON" : "OFF"));
	}
	public void LogEndSession(bool isActive)
	{
		subjectLog.Log(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "END_SESSION" + separator + ((isActive) ? "ON" : "OFF"));
	}

	public void LogMetaData()
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TASK_INFO" + separator + "VERSION" + separator + "0");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TASK_INFO" + separator + "BUILD_NUMBER" + separator + "0");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TASK_INFO" + separator + "LANGUAGE" + separator + "English");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TASK_INFO" + separator + "BUILD_PLATFORM" + separator + "Unity");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TASK_INFO" + separator + "RUN_PLATFORM" + separator + "MAC");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "VALIDATION_INFO" + separator + "NUMBER_OF_PHASES" + separator + "5");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "VALIDATION_INFO" + separator + "NUMBER_OF_TRIALS" + separator + "29");
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "ALIGNMENT_APPROACH" + separator + "STRATEGY" + separator + "AUDIO_SYNC_PULSES");

	}

	public void LogDay(int day)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_INFO" + separator + "Day" + separator + day.ToString());
	}

	public void LogExpType(int exp_type)
	{
		if (exp_type == 0)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_INFO" + separator + "REEVALUATION_TYPE" + separator + "REWARD" + separator + "TEST");
		else if (exp_type == 1)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_INFO" + separator + "REEVALUATION_TYPE" + separator + "TRANSITION" + separator + "TEST");
		else if (exp_type == 2)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_INFO" + separator + "REEVALUATION_TYPE" + separator + "REWARD" + separator + "CONTROL");
		else if (exp_type == 3)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_INFO" + separator + "REEVALUATION_TYPE" + separator + "TRANSITION" + separator + "CONTROL");

	}

	public void LogExpXRanges(float min, float max)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_DEFINITION" + separator + "X_POSITION_RANGES" + separator + min.ToString() + separator + max.ToString());
	}

	public void LogExpYRanges(float min, float max)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_DEFINITION" + separator + "Y_POSITION_RANGES" + separator + min.ToString() + separator + max.ToString());
	}

	public void LogExpZRanges(float min, float max)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "EXPERIMENT_DEFINITION" + separator + "Z_POSITION_RANGES" + separator + min.ToString() + separator + max.ToString());
	}

	public void LogExpTrialPhaseStatus(int exp_stage, bool isActive)
	{
		if (exp_stage == 0)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_PHASE" + separator + "INTRODUCTION" + separator + ((isActive) ? "STARTED" : "ENDED"));
		else if (exp_stage == 1)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_PHASE" + separator + "PRETRAINING_1_ROOM_SEQUENCE" + separator + ((isActive) ? "STARTED" : "ENDED"));
		else if (exp_stage == 2)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_PHASE" + separator + "PRETRAINING_3_ROOM_SEQUENCE" + separator + ((isActive) ? "STARTED" : "ENDED"));
		else if (exp_stage == 3)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_PHASE" + separator + "LEARNING" + separator + ((isActive) ? "STARTED" : "ENDED"));
		else if (exp_stage == 4)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_PHASE" + separator + "RELEARNING" + separator + ((isActive) ? "STARTED" : "ENDED"));
		else if (exp_stage == 5)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_PHASE" + separator + "TEST" + separator + ((isActive) ? "STARTED" : "ENDED"));

	}

	public void LogExpTrialInfoStatusStartEnd(int log_event, int number, int SE_num) {
		if (log_event == 1)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "NUMBER" + separator + number.ToString() + separator + ((SE_num == 1)?"STARTED":"ENDED"));
		else if (log_event == 2)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "ROOM_NUMBER" + separator + number.ToString() + separator + ((SE_num == 1) ? "STARTED" : "ENDED"));
		else if (log_event == 3)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "SEQUENCE" + separator + ((number == 0) ? "A" : "B") + separator + ((SE_num == 1) ? "STARTED" : "ENDED"));
	}

	public void LogExpTrialInfoStatus(int log_event, int number)
	{
		if (log_event == 1)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "NUMBER" + separator + number.ToString());
		else if (log_event == 2)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "ROOM_NUMBER" + separator + number.ToString());
		else if (log_event == 3)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "SEQUENCE" + separator + ((number == 0) ? "A" : "B"));
		else if (log_event == 4)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "CHEST_REWARD" + separator + number.ToString());
		else if (log_event == 5)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "CHEST_OPEN" + separator + ((number == 1) ? "STARTED" : "ENDED"));
		else if (log_event == 6)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "TRIAL_INFO" + separator + "DOOR_OPEN" + separator + ((number == 1) ? "STARTED" : "ENDED"));

	}

	public void LogExpQuesTypeSE(int SE_num)
	{
		string str = "";
		str += "QUESTION_TYPE_SE" + separator;

		if (SE_num == 1)
			str += "STARTED";
		else
			str += "ENDED";

		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), str);

	}
	public void LogExpQuesType(int quest_t, int ques_subtype, int AnswerOptions, int RoomDisplayed)
	{
		string str = "";

		str += "QUESTION_TYPE" + separator;
		if (quest_t == 1)
			str += "CASH";
		else if (quest_t == 2)
			str += "STRUCTURE";

		if (ques_subtype == 1)
			str += separator + "INITIAL";
		else if (ques_subtype == 2)
			str += separator + "INTERMEDIATE";
		else if (ques_subtype == 3)
			str += separator + "TEST";

		if (AnswerOptions == 1)
			str += separator + "1/2";
		else if (AnswerOptions == 2)
			str += separator + "3/4";
		else if (AnswerOptions == 3)
			str += separator + "5/6";

		if (RoomDisplayed == 0)
			str += separator + "N/A";
		else
			str += separator + RoomDisplayed.ToString();

		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), str);
	}

	public void LogExpQuesType2(int quest_t, int ques_subtype, int AnswerOptions, int left_right, int seq, bool istransition)
	{
		string str = "";

		str += "QUESTION_LR" + separator;
		if (quest_t == 1)
			str += "CASH";
		else if (quest_t == 2)
			str += "STRUCTURE";

		if (ques_subtype == 1)
			str += separator + "INITIAL";
		else if (ques_subtype == 2)
			str += separator + "INTERMEDIATE";
		else if (ques_subtype == 3)
			str += separator + "TEST";

		if (left_right == 0)
			str += separator + "LEFT";
		else if (left_right == 1)
			str += separator + "RIGHT";
		else
			str += separator + "RIGHT";

		if (istransition == false)
		{
			if (AnswerOptions == 1)
			{
				if (seq == 0)
					str += separator + "1";
				else
					str += separator + "2";
			}
			else if (AnswerOptions == 2)
			{
				if (seq == 0)
					str += separator + "3";
				else
					str += separator + "4";
			}
			else if (AnswerOptions == 3)
			{
				if (seq == 0)
					str += separator + "5";
				else
					str += separator + "6";
			}
		}
		else
		{
			if (AnswerOptions == 1)
			{
				if (seq == 0)
					str += separator + "1";
				else
					str += separator + "2";
			}
			else if (AnswerOptions == 2)
			{
				if (seq == 0)
					str += separator + "3";
				else
					str += separator + "4";
			}
			else if (AnswerOptions == 3)
			{
				if (seq == 0)
					str += separator + "6";
				else
					str += separator + "5";
			}
		}

		str += separator + ((seq == 0)? "A":"B");

		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), str);
	}

	public void LogExpQuesCorrectness(int quest_t, int ques_subtype, int AnswerOptions, int RoomDisplayed, int CorrectAnswer)
	{
		string str = "";

		str += "QUESTION_CORRECTNESS" + separator;
		if (quest_t == 1)
			str += "CASH";
		else if (quest_t == 2)
			str += "STRUCTURE";

		if (ques_subtype == 1)
			str += separator + "INITIAL";
		else if (ques_subtype == 2)
			str += separator + "INTERMEDIATE";
		else if (ques_subtype == 3)
			str += separator + "TEST";

		if (AnswerOptions == 1)
			str += separator + "1/2";
		else if (AnswerOptions == 2)
			str += separator + "3/4";
		else if (AnswerOptions == 3)
			str += separator + "5/6";

		if (RoomDisplayed == 0)
			str += separator + "N/A";
		else
			str += separator + RoomDisplayed.ToString();


		str += separator + CorrectAnswer.ToString();

		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), str);
	}

	public void LogExpQuesSliderCorrectness(int quest_t, int ques_subtype, int AnswerOptions, int RoomDisplayed, float SliderAmount)
	{
		string str = "";

		str += "QUESTION_ANSWER" + separator;
		if (quest_t == 1)
			str += "CASH";
		else if (quest_t == 2)
			str += "STRUCTURE";

		if (ques_subtype == 1)
			str += separator + "INITIAL";
		else if (ques_subtype == 2)
			str += separator + "INTERMEDIATE";
		else if (ques_subtype == 3)
			str += separator + "TEST";

		if (AnswerOptions == 1)
			str += separator + "1/2";
		else if (AnswerOptions == 2)
			str += separator + "3/4";
		else if (AnswerOptions == 3)
			str += separator + "5/6";

		if (RoomDisplayed == 0)
			str += separator + "N/A";
		else
			str += separator + RoomDisplayed.ToString();


		str += separator + SliderAmount.ToString();

		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), str);
	}


	public void LogPressedKey(KeyCode vCode)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "BUTTON_PRESS" + separator + vCode.ToString());

	}

	public void LogDoorPosition(float x, float y, float z)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "DOOR" + separator + "POSITION" + separator + x.ToString() + separator + y.ToString() + separator + z.ToString());
	}

	public void LogChestPosition(float x, float y, float z)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "CHEST" + separator + "POSITION" + separator + x.ToString() + separator + y.ToString() + separator + z.ToString());
	}

	public void LogQuestionExtremes(bool S_E_pos)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "IMAGE" + separator + "QUESTION" + separator + (S_E_pos == true ? "STARTED" : "ENDED"));
	}

	public void LogInstructionExtremes(bool S_E_pos)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "IMAGE" + separator + "INSTRUCTION" + separator + (S_E_pos == true ? "STARTED" : "ENDED"));
	}

	public void LogBlackScreenExtremes(bool S_E_pos)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "IMAGE" + separator + "BLACK_SCREEN" + separator + (S_E_pos == true ? "STARTED" : "ENDED"));
	}

	public void LogExpSpeedChange(float speed, int time_bins)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "SPEED_CHANGE" + separator + "Timebins" + separator + time_bins.ToString() + separator + speed.ToString());
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "SPEED_CHANGE" + separator + "ROOM" + separator + speed.ToString());
	}


	public void LogExpSpeedChangeTransition(float speed)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "SPEED_CHANGE" + separator + "ROOM_TRANSITION" + separator + speed.ToString());
	}

	public void LogSecCameraPos(float x, float y, float z)
	{
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "CAMERA" + separator + "POSITION" + separator + x.ToString() + separator + y.ToString() + separator + z.ToString());
	}

	public void LogSecCameraStatus(int status)
	{
		if (status == 1)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "CAMERA" + separator + "BUTTON_PRESS" + separator + "C");
		else if (status == 2)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "CAMERA" + separator + "NOT_DETECTED");
		else if (status == 3)
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PLAYER" + separator + "CAMERA" + separator + "DETECTED");
	}

	public void LogGamePause(bool isstarted)
	{
		if (isstarted == true)
		{
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PAUSE" + separator + "STARTED");
		}
		else
		{
			subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "PAUSE" + separator + "ENDED");
		}
	}

	public void LogGameEnd() {
		subjectLog.LogNew(GameClock.SystemTime_Milliseconds, subjectLog.GetFrameCount(), "GAME_END");
	}
}