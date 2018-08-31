using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Suitcase : MonoBehaviour {

	public List<MeshRenderer> targetRends;
	public List<Material> chosenMats;
	// Use this for initialization
	void Awake () {
		TurnImageOff ();
	}

//	public void ChooseTexture(Texture targetTexture)
//	{
//		for (int i = 0; i < targetRends.Count; i++) {
//			targetRends[i].GetComponent<MeshRenderer> ().material.mainTexture = targetTexture;
//		}
//	}

	public void ChangeTargetRendMaterial(int matIndex)
	{
		Debug.Log ("changing for index: " + matIndex.ToString ());
		for (int i = 0; i < targetRends.Count; i++) {

			if (i == 0) {
				Debug.Log (targetRends [i].materials [1].name);
				targetRends [i].materials [1] = chosenMats [matIndex];
				Debug.Log (targetRends [i].materials [1].name);
			} else {
				Debug.Log (targetRends [i].materials [0].name);
				targetRends [i].material = chosenMats [matIndex];
				Debug.Log (targetRends [i].materials [0].name);
			}
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnImageOff()
	{
	//	imageQuad.SetActive (false);
	}
}
