using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Suitcase : MonoBehaviour {

	public GameObject imageQuad;
	// Use this for initialization
	void Awake () {
		TurnImageOff ();
	}

	public void ChooseTexture(Texture targetTexture)
	{
		imageQuad.GetComponent<MeshRenderer> ().material.mainTexture = targetTexture;
		imageQuad.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnImageOff()
	{
		imageQuad.SetActive (false);
	}
}
