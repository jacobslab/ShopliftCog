using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefSoloSetup : MonoBehaviour {

	public Slider prefSlider;
	public RawImage focusImg;

	public List<Texture> imgGroup;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

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
}
