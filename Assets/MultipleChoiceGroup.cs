using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceGroup : MonoBehaviour {
	
	public List<Texture> roomTextureList;

	public RawImage focusImage;
	public List<RawImage> choiceImageList;

    public Text instructionText;
	// Use this for initialization
	void Start () {
#if KEYBOARD
        instructionText.text = "Elegir habitación con las teclas de flecha izquierda y derecha";
#else
        instructionText.text = "Choose room with left joystick";
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

	public void SetupMultipleChoice(int focusIndex)
	{
        gameObject.GetComponent<AnswerSelector>().ResetSelectorPosition();

        List<int> intVals = new List<int>();
        for (int i = 0; i < roomTextureList.Count;i++)
        {
            intVals.Add(i);
        }

        //shuffled indices
        intVals = Experiment.Instance.shopLift.ShuffleList(intVals);


		//first pick the focus index
        focusImage.texture= roomTextureList[focusIndex];
        Debug.Log("FOCUS IMAGE IS " + focusImage.texture.name);
        Experiment.Instance.shopLiftLog.LogMultipleChoiceFocusImage(roomTextureList[focusIndex].name);

        //then create a temp copy of the texture list using those shuffled indices
        List<Texture> tempTextureList = new List<Texture>();
        for (int i = 0; i < roomTextureList.Count; i++)
        {
            if(intVals[i]!=focusIndex)
              tempTextureList.Add(roomTextureList[intVals[i]]);
        }


        //then, distribute rest of images as choice images
        for (int i = 0; i < choiceImageList.Count; i++)
        {
            Experiment.Instance.shopLiftLog.LogMultipleChoiceTexture(i, tempTextureList[i].name);
            choiceImageList [i].texture = tempTextureList [i];
		}

	}
}
