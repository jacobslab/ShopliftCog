using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceGroup : MonoBehaviour {
	
	public List<Texture> roomTextureList;

	public RawImage focusImage;
	public List<RawImage> choiceImageList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetupMultipleChoice(int focusIndex)
	{
        gameObject.GetComponent<AnswerSelector>().ResetSelectorPosition();
        //first create a temp copy of the texture list
        List<Texture> tempTextureList = new List<Texture> ();
		for (int i = 0; i < roomTextureList.Count; i++) {
			tempTextureList.Add (roomTextureList [i]);
		}

		//then, pick the focus index and remove it
		focusImage.texture= tempTextureList[focusIndex];
        Experiment.Instance.shopLiftLog.LogMultipleChoiceFocusImage(tempTextureList[focusIndex].name);
		tempTextureList.RemoveAt (focusIndex);

		//then, distribute rest of images as choice images
		for (int i = 0; i < choiceImageList.Count; i++)
        {
            Experiment.Instance.shopLiftLog.LogMultipleChoiceTexture(i, roomTextureList[i].name);
            choiceImageList [i].texture = tempTextureList [i];
		}

	}
}
