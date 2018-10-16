using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefSoloSetup : MonoBehaviour
{

    public Slider prefSlider;
    public RawImage focusImg;
    public RawImage negativeFocusImg;
    public Text instructionText;

    public List<Texture> imgGroup;
    private bool active = false;
    // Use this for initialization

    void OnEnable () {

#if KEYBOARD
        instructionText.text = "Left and right arrow keys moves slider \nPress(X) to confirm";
#else
        instructionText.text = "Left joystick moves slider \nPress(X) to confirm";
#endif

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
