using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

	public Color selColor;
	public Color emissionColor;
	// Use this for initialization
	void Start () {
		ChangeColors (selColor);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeColors(Color selColor)
	{
		MeshRenderer[] meshRend = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < meshRend.Length; i++) {
			meshRend [i].material.color = selColor;
			meshRend [i].material.EnableKeyword ("_EMISSION");
			meshRend [i].material.SetColor ("_EmissionColor", emissionColor);
			meshRend [i].material.SetTexture ("_EmissionMap", null);
		}
	}
}
