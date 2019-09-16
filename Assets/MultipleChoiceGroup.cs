using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceGroup : MonoBehaviour {
	
	public List<Texture> roomTextureList;

	public RawImage focusImage;
	public List<RawImage> choiceImageList;

    //feedback canvas groups
    public CanvasGroup positiveFeedbackGroup;
    public CanvasGroup negativeFeedbackGroup;

    public CanvasGroup correctIndicator;

    public Dictionary<int, int> roomMappings;
    private Dictionary<int, int> texturePositionToRoomMapping;

    public Text instructionText;
	// Use this for initialization
	void Start () {
        if(ExperimentSettings.Instance.controlDevice==ExperimentSettings.ControlDevice.Keyboard)
             instructionText.text = "Choose room with left and right arrow keys";
        else
             instructionText.text = "Choose room with left joystick";

        roomMappings = new Dictionary<int, int>();
        CreateDefaultRoomMappings();
        positiveFeedbackGroup.alpha = 0f;
        negativeFeedbackGroup.alpha = 0f;
        correctIndicator.alpha = 0f;
    }

    // Update is called once per frame
    void Update () {
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

	public int SetupMultipleChoice(int focusIndex)
	{
        gameObject.GetComponent<AnswerSelector>().ResetSelectorPosition();


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
        int correctChoice = roomMappings[focusIndex + 1];


		//first pick the focus index
        focusImage.texture= roomTextureList[focusIndex];
        Debug.Log("FOCUS IMAGE IS " + focusImage.texture.name);
        Experiment.Instance.shopLiftLog.LogMultipleChoiceFocusImage(roomTextureList[focusIndex].name);

        //then create a temp copy of the texture list using those shuffled indices
        List<Texture> tempTextureList = new List<Texture>();
        for (int i = 0; i < roomTextureList.Count; i++)
        {
            if (intVals[i] != focusIndex)
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
