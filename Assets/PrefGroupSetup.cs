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
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
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
		} else {
			leftImg.texture = targetGroup [1];
			rightImg.texture = targetGroup [0];
		}
	}
}
