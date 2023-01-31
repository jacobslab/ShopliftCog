using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrefGroupSetup : MonoBehaviour {


	public Slider prefSlider;
	public RawImage leftImg;
	public RawImage rightImg;

	public List<Texture> firstGroup;
	public List<Texture> secondGroup;
    public List<Texture> thirdGroup;

    public CanvasGroup correctFeedback;
    public CanvasGroup incorrectFeedback;

    public CanvasGroup leftSliderRange;
    public CanvasGroup rightSliderRange;

    public Text instructionText;

	private bool active=false;
	// Use this for initialization
	void OnEnable () {
        //update on enable
        if (ExperimentSettings.Instance.controlDevice == ExperimentSettings.ControlDevice.Keyboard)
        {
            if (ExperimentSettings.Instance.currentLanguage == ExperimentSettings.Language.English)
                instructionText.text = "Left and right arrow keys moves the slider";
            else
                instructionText.text = "Teclas de flecha izquierda y derecha \n mueve el control deslizante";
        }
        else
        {
            if (ExperimentSettings.Instance.currentLanguage == ExperimentSettings.Language.English)
                instructionText.text = "Left joystick moves slider";
            else
                instructionText.text = "El joystick izquierdo mueve el control";
        }


        active = true;

        correctFeedback.alpha = 0f;
        incorrectFeedback.alpha = 0f;
        leftSliderRange.alpha = 0f;
        rightSliderRange.alpha = 0f;

		if(Experiment.Instance!=null)
			UpdateSlider ();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			float move = Input.GetAxis ("Horizontal");
			prefSlider.value += move * 0.05f;
		}
	}
	public void UpdateSlider()
	{
		Experiment.Instance.shopLiftLog.LogSliderValue ("COMPARATIVE", prefSlider.value);
	}

    public IEnumerator ShowCorrectFeedback()
    {
        correctFeedback.alpha = 1f;
        yield return new WaitForSeconds(5f);
        correctFeedback.alpha = 0f;
        yield return null;

    }

    public IEnumerator ShowIncorrectFeedback()
    {
        incorrectFeedback.alpha = 1f;
        yield return new WaitForSeconds(5f);
        incorrectFeedback.alpha = 0f;
        yield return null;
    }

    public CanvasGroup GetAssistiveSliderUI(bool isLeft)
    {
        if(isLeft)
        {
            return leftSliderRange;

        }
        else
        {
            return rightSliderRange;
        }
    }

	public void SetupPrefs(int prefType)
	{
		List<Texture> targetGroup = new List<Texture> ();
		prefSlider.value = 0.5f;
		switch (prefType) {
		case 0:
			targetGroup = firstGroup;
			break;
		case 1:
			targetGroup = secondGroup;
			break;
        case 2:
        targetGroup = thirdGroup;
        break;

        }

        leftImg.texture = targetGroup[0];
        rightImg.texture = targetGroup[1];
        Experiment.Instance.shopLiftLog.LogComparativePrefImage(prefType, leftImg.texture.name, rightImg.texture.name);

        /*if (Random.value < 0.5f) {
			leftImg.texture = targetGroup [0];
			rightImg.texture = targetGroup [1];
			Experiment.Instance.shopLiftLog.LogComparativePrefImage (prefType,leftImg.texture.name,rightImg.texture.name);
		} else {
			leftImg.texture = targetGroup [1];
			rightImg.texture = targetGroup [0];
            Experiment.Instance.shopLiftLog.LogComparativePrefImage (prefType,leftImg.texture.name,rightImg.texture.name);
		}*/

    }

	void OnDisable()
	{
		active = false;
	}
}
