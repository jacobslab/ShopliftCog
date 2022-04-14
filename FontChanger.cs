using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontChanger : MonoBehaviour {

	private Text targetText;
	public float minSize = 30f;
	public float maxSize = 180f;
	// Use this for initialization
	void Start () {
		targetText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public IEnumerator GrowText(float maxTime)
	{
		maxTime -=1f;
		float timer = 0f;
		while (timer < maxTime) {
			timer += Time.deltaTime;
			targetText.fontSize = (int) Mathf.Lerp (minSize, maxSize, timer/maxTime);
			yield return 0;
		}
		yield return new WaitForSeconds (1f);
		yield return null;
	}
}
