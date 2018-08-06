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
//			if (meshRend [i].gameObject.layer == 11) {
//				meshRend [i].material.color = Color.white;
//				meshRend [i].material.DisableKeyword ("_EMISSION");
//			} 
			if (meshRend [i].gameObject.layer == 13) {

				meshRend [i].material.color = selColor;
				meshRend [i].material.DisableKeyword ("_EMISSION");
//				meshRend [i].material.SetColor ("_EmissionColor", selColor);
//				meshRend [i].material.SetTexture ("_EmissionMap", null);
			}
			else {
				meshRend [i].material.color = Color.white;
				meshRend [i].material.DisableKeyword ("_EMISSION");
//				meshRend [i].material.EnableKeyword ("_EMISSION");
//				meshRend [i].material.SetColor ("_EmissionColor", emissionColor);
//				meshRend [i].material.SetTexture ("_EmissionMap", null);
			}
		}
	}
}
