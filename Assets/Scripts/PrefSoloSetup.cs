﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefSoloSetup : MonoBehaviour
{

    public Slider prefSlider;
    public RawImage focusImg;
    public RawImage negativeFocusImg;
    public Text instructionText;


    public CanvasGroup correctFeedback;
    public CanvasGroup incorrectFeedback;

    public CanvasGroup leftSliderRange;
    public CanvasGroup rightSliderRange;

    public List<Texture> imgGroup;
    private bool active = false;
    // Use this for initialization

    void OnEnable () {

#if KEYBOARD
        instructionText.text = "Left and right arrow keys moves slider \nPress(X) to confirm";
#else
        instructionText.text = "El joystick izquierdo mueve el control. \n Presiona(X) para continuar";
#endif

        correctFeedback.alpha = 0f;
        incorrectFeedback.alpha = 0f;
        leftSliderRange.alpha = 0f;
        rightSliderRange.alpha = 0f;
        active = true;
		//update on enable
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

    public CanvasGroup GetAssistiveSliderUI(bool rightSliderIsCorrect)
    {
        if (rightSliderIsCorrect)
        {
            return rightSliderRange;

        }
        else
        {
            return leftSliderRange;
        }
    }

    public void UpdateSlider()
	{
		Experiment.Instance.shopLiftLog.LogSliderValue ("SOLO", prefSlider.value);
	}

	public void SetupPrefs(int prefIndex)
	{
		List<Texture> targetGroup = new List<Texture> ();
		prefSlider.value = 0.5f;
		Experiment.Instance.shopLiftLog.LogSoloPrefImage (imgGroup[prefIndex].name);
		focusImg.texture = imgGroup [prefIndex];
		if (negativeFocusImg != null) {
			negativeFocusImg.texture = imgGroup [prefIndex];
		}

	}

	void OnDisable()
	{
		active = false;
	}

}
