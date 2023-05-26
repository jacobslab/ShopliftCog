using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceGroup : MonoBehaviour {
	
	public List<Texture> roomTextureList;

	public RawImage focusImage;
	public List<RawImage> choiceImageList;
    public int displayOptions;

    //feedback canvas groups
    public CanvasGroup positiveFeedbackGroup;
    public CanvasGroup negativeFeedbackGroup;

    public CanvasGroup correctIndicator;

    public Dictionary<int, int> roomMappings;
    private Dictionary<int, int> texturePositionToRoomMapping;

    public Text instructionText;
    public Text PrefText;
    public Slider slider;
    public CanvasGroup canvasLeftFeedColor;
    public Image canvasLeftFeedImage;
    public CanvasGroup canvasRightFeedColor;
    public Image canvasRightFeedImage;
    public bool canvasPatch_isset;

    // Use this for initialization
    void Start () {
        canvasPatch_isset = false;
        displayOptions = 0;
        if (ExperimentSettings.Instance.controlDevice == ExperimentSettings.ControlDevice.Keyboard)
        {
            if (ExperimentSettings.Instance.currentLanguage == ExperimentSettings.Language.English)
                instructionText.text = "Choose room with left and right arrow keys";
            else
                instructionText.text = "Elegir habitación con las teclas de flecha izquierda y derecha";
        }
        else
        {
            if (ExperimentSettings.Instance.currentLanguage == ExperimentSettings.Language.English)
                instructionText.text = "Choose room with left joystick";
            else
                instructionText.text = "Elige la sala con el joystick izquierdo";
        }

        roomMappings = new Dictionary<int, int>();
        CreateDefaultRoomMappings();
        positiveFeedbackGroup.alpha = 0f;
        negativeFeedbackGroup.alpha = 0f;
        correctIndicator.alpha = 0f;
    }

    // Update is called once per frame
    void Update () {
	}

    public float sliderValue() {
        return slider.value;
    }

    public void setCorrectPatches(int correctAns) {
        if (correctAns == 0)
        {
            canvasLeftFeedImage.color = Color.green;
            canvasRightFeedImage.color = Color.red;
        }
        else {
            //canvasLeftFeedColo
            canvasLeftFeedImage.color = Color.red;
            canvasRightFeedImage.color = Color.green;

        }
    }

    public void setCanvas(bool is_set)
    {
        canvasPatch_isset = is_set;
        if (is_set)
        {
            canvasLeftFeedColor.alpha = 1f;
            canvasRightFeedColor.alpha = 1f;
        }
        else
        {
            canvasLeftFeedColor.alpha = 0f;
            canvasRightFeedColor.alpha = 0f;
        }
    }

    public bool IsCanvasPatchSet() {
        return canvasPatch_isset;
    }

    private void CreateDefaultRoomMappings()
    {
        Debug.Log("creating default room mappings");
        roomMappings.Clear();

        roomMappings.Add(1, 3);//room 1 is connected to 3
        roomMappings.Add(2, 4);//room 2 is connected to 4
        roomMappings.Add(3, 5);//room 3 is connected to 5
        roomMappings.Add(4, 6);//room 4 is connected to 6
    }

    public IEnumerator ShowFeedback(int chosenIndex, int correctIndex,bool waitForButtonPress)
    {
        Debug.Log("showing multiple choice feedback");
        positiveFeedbackGroup.alpha = 0f;
        negativeFeedbackGroup.alpha = 0f;

        bool isCorrect = false;

        //highlight the correct response
        int correctPositionIndex  = HighlightCorrectResponse(correctIndex);
        correctIndicator.alpha = 1f;

        if (roomMappings[chosenIndex + 1] == correctIndex)
            isCorrect = true;
        else
            isCorrect = false;

        //show appropriate feedback based on whether the player response was correct or not
        if(isCorrect)
        {
            positiveFeedbackGroup.alpha = 1f;
        }
        else
        {
            negativeFeedbackGroup.alpha = 1f;
        }
        //wait for button press, otherwise just wait for a few seconds
        if (waitForButtonPress)
        {
            bool pressed = false;
            int selecterPos = 0;
            while (!pressed || correctPositionIndex != selecterPos)
            {
                yield return StartCoroutine(ShoplifterScript.Instance.WaitForButtonPress(100000f, didPress =>
                {
                    pressed = didPress;
                }));
                selecterPos = gameObject.GetComponent<AnswerSelector>().ReturnSelectorPosition();
                Debug.Log("correct index " + correctPositionIndex.ToString());
                Debug.Log("selecter pos " + selecterPos.ToString());
                yield return 0;
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        correctIndicator.alpha = 0f;
        negativeFeedbackGroup.alpha = 0f;
        positiveFeedbackGroup.alpha = 0f;
        yield return null;
    }

    public int HighlightCorrectResponse(int correctIndex)
    {
        Debug.Log("correct index " + correctIndex.ToString());

        //now we need to convert the "correctIndex" which is in room number to the actual randomized position of the texture that indicates it
        texturePositionToRoomMapping.TryGetValue(correctIndex,out int correctPositionIndex);
        Debug.Log("correct position index is " + correctPositionIndex.ToString());
        gameObject.GetComponent<AnswerSelector>().MoveDirectlyTo(correctIndicator.gameObject, correctPositionIndex);
        return correctPositionIndex;
    }

    public void UpdateRoomMappings(Dictionary<int,int> newMapping)
    {
        Debug.Log("updating room mappings");
        //if dictionary hasn't been initialized, do it now
        if(roomMappings==null)
        {
            Debug.Log("room mappings null;creating new");
            roomMappings = new Dictionary<int, int>();
            CreateDefaultRoomMappings();
        }

        //just check if the dictionaries are of the same length
        if (roomMappings.Keys.Count == newMapping.Keys.Count)
        {
            Debug.Log("setting roomMapping to newMapping");
            roomMappings = newMapping;
        }

    }

    public Dictionary<int,int> GetCurrentRoomMappings()
    {
        return roomMappings;
    }

    private void PrintRoomMappings()
    {

        for (int i = 0; i < roomMappings.Keys.Count; i++)
        {
            Debug.Log("Room " + (1+i).ToString() + "->" + roomMappings[1+i]);
        }
    }

    public void SetCashQuesOptions(int SetdisplayOptions) {
        displayOptions = SetdisplayOptions;
    }

    public int ReturnDisplayOpt()
    {
        return displayOptions;
    }

    public void SetFocusImage(bool focus_status, Vector3 instructTextpos, Vector3 prefTextpos, int questionType, int swap)
    {
        focusImage.enabled = focus_status;
        PrefText.transform.localPosition = prefTextpos;
        instructionText.transform.localPosition = instructTextpos;

        if (questionType == 0)
        {
            PrefText.text = " Which room from the BOTTOM is in the same path with the ABOVE room?";
                //" Which room comes after the ABOVE room?";
        }
        else if (questionType == 1) {
            PrefText.text = "Which room will lead you to more cash?";

            /* "displayOptions" determines the INITIAL/INTERMEDIATE */
            if (displayOptions == 0)
            {
                Experiment.Instance.shopLiftLog.LogExpQuesType(1, 1, 1, 0);
                if (swap == 0)
                {
                    choiceImageList[1].texture = roomTextureList[0];
                    choiceImageList[3].texture = roomTextureList[1];
                    if (Experiment.Instance.shopLift._currentReevalCondition != 1)
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 0, 0, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 1, 1, false);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 0, 0, true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 1, 1, true);
                    }
                }
                else
                {
                    choiceImageList[1].texture = roomTextureList[1];
                    choiceImageList[3].texture = roomTextureList[0];
                    if (Experiment.Instance.shopLift._currentReevalCondition != 1)
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 0, 1, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 1, 0, false);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 0, 1, true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 1, 1, 1, 0, true);
                    }
                }
                

                //Experiment.Instance.shopLiftLog.LogExpQuesTyp2(1, 1, 1, 0);
                if ((Experiment.Instance.shopLift.expSettings.stage == ExperimentSettings.Stage.Reevaluation) &&
                    ((Experiment.Instance.shopLift._currentReevalCondition == 1) || (Experiment.Instance.shopLift._currentReevalCondition == 0)))
                    Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 1, 1, 0, 2);
                else
                    Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 1, 1, 0, 1);
                displayOptions = 1;
            }
            /* "displayOptions" determines the INITIAL/INTERMEDIATE */
            else
            {
                Experiment.Instance.shopLiftLog.LogExpQuesType(1, 2, 2, 0);
                if (swap == 0)
                {
                    choiceImageList[1].texture = roomTextureList[2];
                    choiceImageList[3].texture = roomTextureList[3];
                    if (Experiment.Instance.shopLift._currentReevalCondition != 1)
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 0, 0, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 1, 1, false);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 0, 0, true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 1, 1, true);
                    }
                }
                else
                {
                    choiceImageList[1].texture = roomTextureList[3];
                    choiceImageList[3].texture = roomTextureList[2];
                    if (Experiment.Instance.shopLift._currentReevalCondition != 1)
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 0, 1, false);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 1, 0, false);
                    }
                    else
                    {
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 0, 1, true);
                        Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 2, 2, 1, 0, true);
                    }
                }
                
                if ((Experiment.Instance.shopLift.expSettings.stage == ExperimentSettings.Stage.Reevaluation) &&
                    (Experiment.Instance.shopLift._currentReevalCondition == 1))
                    Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 2, 2, 0, 4);
                else
                    Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 2, 2, 0, 3);
                displayOptions = 0;
            }
        }
    }

    public void SetFocusImageCashTest(bool focus_status, Vector3 instructTextpos, Vector3 prefTextpos, int questionType, int swap, int is_test)
    {
        focusImage.enabled = focus_status;
        PrefText.transform.localPosition = prefTextpos;
        instructionText.transform.localPosition = instructTextpos;

        PrefText.text = "Which room will lead you to more cash?";

        Experiment.Instance.shopLiftLog.LogExpQuesType(1, 3, 3, 0);
        if (swap == 0)
        {
            choiceImageList[1].texture = roomTextureList[4];
            choiceImageList[3].texture = roomTextureList[5];
            if (Experiment.Instance.shopLift._currentReevalCondition != 1)
            {
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 0, 0, false);
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 1, 1, false);
            }
            else
            {
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 0, 0, true);
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 1, 1, true);
            }
        }
        else
        {
            choiceImageList[1].texture = roomTextureList[5];
            choiceImageList[3].texture = roomTextureList[4];
            if (Experiment.Instance.shopLift._currentReevalCondition != 1)
            {
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 0, 1, false);
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 1, 0, false);
            }
            else
            {
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 0, 1, true);
                Experiment.Instance.shopLiftLog.LogExpQuesType2(1, 3, 3, 1, 0, true);
            }
        }


        //Experiment.Instance.shopLiftLog.LogExpQuesTyp2(1, 1, 1, 0);
        if ((Experiment.Instance.shopLift.expSettings.stage == ExperimentSettings.Stage.Reevaluation) &&
            ((Experiment.Instance.shopLift._currentReevalCondition == 1) || (Experiment.Instance.shopLift._currentReevalCondition == 0)))
            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 1, 1, 0, 6);
        else
            Experiment.Instance.shopLiftLog.LogExpQuesCorrectness(1, 1, 1, 0, 5);

    }

    public void ChangeToInterm(bool Chg_req) {
        if (Chg_req) {
            //choiceImageList[1].texture = roomTextureList[2];
            //choiceImageList[3].texture = roomTextureList[3];
            //ocusImage.texture = roomTextureList[focusIndex + 2];
        }
    }

    public void ChangeFocusImagetoInterm(int focusIndex) {
        focusImage.texture = roomTextureList[focusIndex+2];
    }
    public int SetupMultipleChoice_v2(int focusIndex, bool isLastRoom, int swap)
    {
        gameObject.GetComponent<AnswerSelector>().ResetSelectorPosition();
        Debug.Log("roomMappings KEys Count: " + roomMappings.Keys.Count);

        //we will store texture position to the actual rooms contained in those textures in this dictionary
        if (texturePositionToRoomMapping == null)
            texturePositionToRoomMapping = new Dictionary<int, int>();
        else
            texturePositionToRoomMapping.Clear();

        PrintRoomMappings();

        List<int> intVals = new List<int>();
        for (int i = 0; i < roomTextureList.Count; i++)
        {
            intVals.Add(i);
        }

        //shuffled indices
        intVals = Experiment.Instance.shopLift.ShuffleList(intVals);

        //focus index is zero-inclusive (0 is the first index) while roomMapping keys begin with 1
        Debug.Log("focus index is " + focusIndex.ToString());
        int correctChoice;
        if (isLastRoom)
            correctChoice = roomMappings[roomMappings[focusIndex + 1]];
        else
            correctChoice = roomMappings[focusIndex + 1];


        //first pick the focus index
        focusImage.texture = roomTextureList[focusIndex];
        Debug.Log("FOCUS IMAGE IS " + focusImage.texture.name);
        Experiment.Instance.shopLiftLog.LogMultipleChoiceFocusImage(roomTextureList[focusIndex].name);

        choiceImageList[0].gameObject.SetActive(false);
        choiceImageList[2].gameObject.SetActive(false);
        choiceImageList[4].gameObject.SetActive(false);

        //then create a temp copy of the texture list using those shuffled indices
        List<Texture> tempTextureList = new List<Texture>();
        for (int i = 0; i < roomTextureList.Count; i++)
        {
            if (intVals[i] != (focusIndex))
            {
                tempTextureList.Add(roomTextureList[intVals[i]]);
                texturePositionToRoomMapping.Add(intVals[i] + 1, tempTextureList.Count - 1); //intVals starts from 0; we also use tempTextureList.Count as a more reliable way to tell the index
                Debug.Log("room " + (intVals[i] + 1).ToString() + " --> position " + (tempTextureList.Count - 1).ToString());
            }
        }


        //then, distribute rest of images as choice images
        for (int i = 0; i < choiceImageList.Count; i++)
        {
            Experiment.Instance.shopLiftLog.LogMultipleChoiceTexture(i, tempTextureList[i].name);
            choiceImageList[i].texture = tempTextureList[i];
        }

        if (swap == 0)
        {
            choiceImageList[1].texture = roomTextureList[4];
            choiceImageList[3].texture = roomTextureList[5];
        }
        else
        {
            choiceImageList[1].texture = roomTextureList[5];
            choiceImageList[3].texture = roomTextureList[4];
        }
        //correctChoice = 

        return correctChoice;

    }

    public int SetupMultipleChoice(int focusIndex, bool isLastRoom)
	{
        gameObject.GetComponent<AnswerSelector>().ResetSelectorPosition();
        Debug.Log("roomMappings KEys Count: " + roomMappings.Keys.Count);

        //we will store texture position to the actual rooms contained in those textures in this dictionary
        if (texturePositionToRoomMapping == null)
            texturePositionToRoomMapping = new Dictionary<int, int>(); 
        else
            texturePositionToRoomMapping.Clear(); 

        PrintRoomMappings();

        List<int> intVals = new List<int>();
        for (int i = 0; i < roomTextureList.Count;i++)
        {
            intVals.Add(i);
        }

        //shuffled indices
        intVals = Experiment.Instance.shopLift.ShuffleList(intVals);

        //focus index is zero-inclusive (0 is the first index) while roomMapping keys begin with 1
        Debug.Log("focus index is " + focusIndex.ToString());
        int correctChoice;
        if (isLastRoom)
            correctChoice = roomMappings[roomMappings[focusIndex + 1]];
        else
            correctChoice = roomMappings[focusIndex + 1];

        choiceImageList[0].gameObject.SetActive(true);
        choiceImageList[2].gameObject.SetActive(true);
        choiceImageList[4].gameObject.SetActive(true);

        //first pick the focus index
        focusImage.texture= roomTextureList[focusIndex];
        Debug.Log("FOCUS IMAGE IS " + focusImage.texture.name);
        Experiment.Instance.shopLiftLog.LogMultipleChoiceFocusImage(roomTextureList[focusIndex].name);

        //then create a temp copy of the texture list using those shuffled indices
        List<Texture> tempTextureList = new List<Texture>();
        for (int i = 0; i < roomTextureList.Count; i++)
        {
            if (intVals[i] != (focusIndex))
            {
                tempTextureList.Add(roomTextureList[intVals[i]]);
                texturePositionToRoomMapping.Add(intVals[i] + 1, tempTextureList.Count-1); //intVals starts from 0; we also use tempTextureList.Count as a more reliable way to tell the index
                Debug.Log("room " + (intVals[i] + 1).ToString() + " --> position " + (tempTextureList.Count-1).ToString());
            }
        }


        //then, distribute rest of images as choice images
        for (int i = 0; i < choiceImageList.Count; i++)
        {
            Experiment.Instance.shopLiftLog.LogMultipleChoiceTexture(i, tempTextureList[i].name);
            choiceImageList [i].texture = tempTextureList [i];
		}

        return correctChoice;

	}
}
