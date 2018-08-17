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

	private bool active=false;
	// Use this for initialization
	void OnEnable () {
		//update on enable

		active = true;
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
		Experiment.Instance.shopLiftLog.LogSliderValue (0, prefSlider.value);
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
			
		}
		if (Random.value < 0.5f) {
			leftImg.texture = targetGroup [0];
			rightImg.texture = targetGroup [1];
			Experiment.Instance.shopLiftLog.LogComparativePrefImage (prefType,0, 1);
		} else {
			leftImg.texture = targetGroup [1];
			rightImg.texture = targetGroup [0];
			Experiment.Instance.shopLiftLog.LogComparativePrefImage (prefType,1, 0);
		}
	}

	void OnDisable()
	{
		active = false;
	}
}
