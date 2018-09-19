using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnswerSelector : MonoBehaviour {

	Experiment exp { get { return Experiment.Instance; } }

	//bool shouldCheckForInput = false;

	bool resetToRandomPosition = true;

	public List<float> positions; //should be put in order of left to right
	public GameObject selectorVisuals;

	public AudioSource selectionSwitchAudio;

	int currPositionIndex = 0;

	void Awake(){
//		positions = new List<float> ();
	}

	// Use this for initialization
	void Start () {
		ResetSelectorPosition ();
	}

	void OnEnable()
	{
		GetComponent<MultipleChoiceGroup> ().SetupMultipleChoice (Random.Range (0, 6));
		SetShouldCheckForInput (true);
	}
	void OnDisable()
	{
		SetShouldCheckForInput (false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void SetShouldCheckForInput(bool shouldCheck){
		if (shouldCheck) {
			ResetSelectorPosition ();
			StartCoroutine (GetSelectionInput ());
		} else {
			StopCoroutine (GetSelectionInput ());
		}
	}

	public void ResetSelectorPosition(){
		int resetIndex = 0; //first index
		if (resetToRandomPosition) {
			resetIndex = Random.Range(0, positions.Count);
		}

		if (positions.Count >= 0) {
            currPositionIndex = resetIndex;
            Debug.Log("Set the currposindex to resetindex");
            selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(positions[currPositionIndex], selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.y,selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D .z);
			

		}
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
			if (!isInput) {
				float horizAxisInput = Input.GetAxis ("Horizontal");
				if (horizAxisInput > 0) {
					Move (1);
					isInput = true;
				} 
				else if (horizAxisInput < 0) {
					Move (-1);
					isInput = true;
				} 
				else if (horizAxisInput == 0) {
					isInput = false;
				}

			}

			else{
				if(currDelayTime < delayTime){
					currDelayTime += Time.deltaTime;
				}
				else{
					currDelayTime = 0.0f;
					isInput = false;
				}

			}

			yield return 0;
		}
	}


	void Move(int indicesToMove){
		int oldPositionIndex = currPositionIndex;

		bool isMoved = true;

		currPositionIndex += indicesToMove;

		if (currPositionIndex < 0) {
			currPositionIndex = 0;
			isMoved = false;
		}
		else if (currPositionIndex > positions.Count - 1){
			currPositionIndex = positions.Count - 1;
			isMoved = false;
		}

		//play audio if the selector moved
		if (isMoved) {
			selectionSwitchAudio.PlayOneShot (selectionSwitchAudio.clip);

		}

		Experiment.Instance.shopLiftLog.LogSelectorPosition (currPositionIndex,gameObject.GetComponent<MultipleChoiceGroup>().roomTextureList[currPositionIndex].name);

		//TODO: make nice smooth movement with a coroutine.
		selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(positions[currPositionIndex], selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D.y,selectorVisuals.GetComponent<RectTransform>().anchoredPosition3D .z);


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