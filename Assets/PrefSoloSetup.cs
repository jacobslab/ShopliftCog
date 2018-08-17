using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefSoloSetup : MonoBehaviour {

	public Slider prefSlider;
	public RawImage focusImg;

	public List<Texture> imgGroup;
	private bool active=false;
	// Use this for initialization
	void OnEnable () {
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
		Experiment.Instance.shopLiftLog.LogSliderValue (1, prefSlider.value);
	}

	public void SetupPrefs(int prefIndex)
	{
		List<Texture> targetGroup = new List<Texture> ();
		prefSlider.value = 0.5f;
		Experiment.Instance.shopLiftLog.LogSoloPrefImage (prefIndex);
		focusImg.texture = imgGroup [prefIndex];

	}

	void OnDisable()
	{
		active = false;
	}

}
