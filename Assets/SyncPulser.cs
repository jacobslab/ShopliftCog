using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Diagnostics;
public class SyncPulser : MonoBehaviour {

	public RawImage pulseImage;
	public Image sensorImageSync;
	float syncPulseInterval = 10f;
	float syncPulseDuration=0.05f;

	public bool ShouldSyncPulse=true;

	// Use this for initialization
	void Start () {
		pulseImage.color = Color.black;
		pulseImage.enabled = false;
		sensorImageSync.color = Color.black;
		StartCoroutine ("RunSyncPulseManual");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StopSyncPulsing()
	{
		StopCoroutine ("RunSyncPulseManual");
	}

	IEnumerator RunSyncPulseManual(){
		UnityEngine.Debug.Log("SyncPulser: RunsyncPulseManual: SyncBox");
		float jitterMin =2f;
		float jitterMax = 4f;

		Stopwatch executionStopwatch = new Stopwatch ();

		while (ShouldSyncPulse) {
			executionStopwatch.Reset();
			//	UnityEngine.Debug.Log ("pulse running");

			float jitter = UnityEngine.Random.Range(jitterMin, jitterMax);//syncPulseInterval - syncPulseDuration);
			yield return StartCoroutine(WaitForShortTime(jitter));

			ToggleLEDOn();
			yield return StartCoroutine(WaitForShortTime(syncPulseDuration));
			ToggleLEDOff();

			float timeToWait = (syncPulseInterval - syncPulseDuration) - jitter;
			if(timeToWait < 0){
				timeToWait = 0;
			}

			yield return StartCoroutine(WaitForShortTime(timeToWait));

			executionStopwatch.Stop();
		}
		yield return null;
	}

	void ToggleLEDOn()
	{
		UnityEngine.Debug.Log("SyncPulser: RunsyncPulseManual: ToggleLEDOn");
		//pulseImage.color = Color.white;
		sensorImageSync.color = Color.white;
		Experiment.Instance.shopLiftLog.LogLEDOn ();
	}

	void ToggleLEDOff()
	{
		UnityEngine.Debug.Log("SyncPulser: RunsyncPulseManual: ToggleLEDOff");
		pulseImage.color = Color.black;
		sensorImageSync.color = Color.black;
		Experiment.Instance.shopLiftLog.LogLEDOff ();
	}

	IEnumerator WaitForShortTime(float jitter)
	{
		float currentTime = 0.0f;
		while (currentTime < jitter) {
			currentTime += Time.deltaTime;
			yield return 0;
		}
		yield return null;
	}
}
