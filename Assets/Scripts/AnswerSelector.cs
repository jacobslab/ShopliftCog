using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnswerSelector : MonoBehaviour {

	Experiment exp { get { return Experiment.Instance; } }

	//bool shouldCheckForInput = false;

	bool resetToRandomPosition = true;

	public List<float> ref_positions; //should be put in order of left to right
	public List<float> positions;
	public GameObject selectorVisuals;
    private GameObject correctIndicator;
    public float blankPosition;

	public Slider slider;
	public AudioSource selectionSwitchAudio;


	int currPositionIndex = 0;
	bool active = false;
	void Awake(){
        //		positions = new List<float> ();
        correctIndicator = gameObject.GetComponent<MultipleChoiceGroup>().correctIndicator.gameObject;
	}

	// Use this for initialization
	void Start () {
		ResetSelectorPosition ();
		//active = false;
	}

	void OnEnable()
	{
		//GetComponent<MultipleChoiceGroup> ().SetupMultipleChoice (Random.Range (0, 4)); // 5th and 6th rooms aren't valid to be the focus image
		//SetShouldCheckForInput (true);

		slider.value = 0.5f;
		active = true;
	}
	void OnDisable()
	{
		active = false;
		//SetShouldCheckForInput (false);
	}

	// Update is called once per frame
	void Update () {
		bool canvasPatch = exp.shopLift.multipleChoiceGroup.GetComponent<MultipleChoiceGroup>().IsCanvasPatchSet();

		if (active && !canvasPatch && Experiment.Instance.shopLift.isGamePaused == false) {
			Debug.Log("I am active");
			float move = Input.GetAxis("Horizontal");
			slider.value += move * 0.05f;
		}
	}

	public void SetShouldCheckForInput(bool shouldCheck){
		if (shouldCheck) {
			ResetSelectorPosition ();
			StartCoroutine (GetSelectionInput ());
		} else {
			StopCoroutine (GetSelectionInput ());
		}
	}

    //this should reset the selector rect to the empty "not sure" box on the left
	public void ResetSelectorPosition(){

        if (positions.Count >= 0)
        {
            
            currPositionIndex = 0; //reset it to move it to the blank option at the beginning
            //Experiment.Instance.shopLiftLog.LogSelectorPosition(-1, "BLANK");
            Experiment.Instance.shopLiftLog.LogSelectorPosition(currPositionIndex, gameObject.GetComponent<MultipleChoiceGroup>().choiceImageList[currPositionIndex].name);
            selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(blankPosition, selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.y, selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.z);
        }
        //int resetIndex = 0; //first index
        //if (resetToRandomPosition) {
        //	resetIndex = Random.Range(0, positions.Count);
        //}

        //if (positions.Count >= 0) {
        //          currPositionIndex = resetIndex;
        //          Debug.Log("Set the currposindex to resetindex");
        //          selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(positions[currPositionIndex], selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.y,selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D .z);


        //}
    }

    public int ReturnSelectorPosition()
	{
		return currPositionIndex;
	}


	//MODIFIED FROM BOXSWAPPER.CS
	IEnumerator GetSelectionInput(){
		bool isInput = false;
		float delayTime = 0.3f;
		float currDelayTime = 0.0f;

		while (true) {
			if (Experiment.Instance.shopLift.isGamePaused == false)
			{
				if (!isInput)
				{
					float horizAxisInput = Input.GetAxis("Horizontal");
					if (horizAxisInput > 0)
					{
						Move(1);
						isInput = true;
					}
					else if (horizAxisInput < 0)
					{
						Move(-1);
						isInput = true;
					}
					else if (horizAxisInput == 0)
					{
						isInput = false;
					}

				}

				else
				{
					if (currDelayTime < delayTime)
					{
						currDelayTime += Time.deltaTime;
					}
					else
					{
						currDelayTime = 0.0f;
						isInput = false;
					}

				}
			}
			yield return 0;
		}
	}

    public void MoveDirectlyTo(GameObject visualsObj, int targetIndex)
    {
        visualsObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(positions[targetIndex], selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.y, selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.z);
    }


	void Move(int indicesToMove){
		int oldPositionIndex = currPositionIndex;

		bool isMoved = true;

		currPositionIndex += indicesToMove;

		if (currPositionIndex < 0) {
			currPositionIndex = positions.Count-1;
			isMoved = true;
		}
		else if (currPositionIndex > positions.Count - 1){
			currPositionIndex = 0;
			isMoved = true;
		}

		//play audio if the selector moved
		if (isMoved) {
			selectionSwitchAudio.PlayOneShot (selectionSwitchAudio.clip);

		}
        Experiment.Instance.shopLiftLog.LogSelectorPosition (currPositionIndex,gameObject.GetComponent<MultipleChoiceGroup>().choiceImageList[currPositionIndex].name);

		//TODO: make nice smooth movement with a coroutine.
		selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(positions[currPositionIndex], selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.y,selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.z);


	}

	/*void SetExplanationText(float colorLerpTime){
		if(GetMemoryState()){
			StartCoroutine(SetYesExplanationActive(colorLerpTime));
		}
		else if(IsNoPosition()){
			StartCoroutine(SetNoExplanationActive(colorLerpTime));
		}
	}*/

	//TODO: combine these next two methods.
	/*IEnumerator SetYesExplanationActive(float colorLerpTime){
		//TODO: REFACTOR.
		if(yesExplanationText && noExplanationText && yesExplanationColorChanger && noExplanationColorChanger){
			yesExplanationColorChanger.StopLerping();
			noExplanationColorChanger.StopLerping();
			yield return 0;
			StartCoroutine(yesExplanationColorChanger.LerpChangeColor( new Color(yesExplanationText.color.r, yesExplanationText.color.g, yesExplanationText.color.b, 1.0f), colorLerpTime));
			StartCoroutine(noExplanationColorChanger.LerpChangeColor( new Color(noExplanationText.color.r, noExplanationText.color.g, noExplanationText.color.b, 0.0f), colorLerpTime));
		}
	}
	IEnumerator SetNoExplanationActive(float colorLerpTime){
		//TODO: REFACTOR.
		if(yesExplanationText && noExplanationText && yesExplanationColorChanger && noExplanationColorChanger){
			yesExplanationColorChanger.StopLerping();
			noExplanationColorChanger.StopLerping();
			yield return 0;
			StartCoroutine(yesExplanationColorChanger.LerpChangeColor( new Color(yesExplanationText.color.r, yesExplanationText.color.g, yesExplanationText.color.b, 0.0f), colorLerpTime));
			StartCoroutine(noExplanationColorChanger.LerpChangeColor( new Color(noExplanationText.color.r, noExplanationText.color.g, noExplanationText.color.b, 1.0f), colorLerpTime));
		}
	}*/
}