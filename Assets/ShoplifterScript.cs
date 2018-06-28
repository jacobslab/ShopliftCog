using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class ShoplifterScript : MonoBehaviour {

	public GameObject camVehicle;
	private MouseLook mouseLook;
	public GameObject phase1Start;
	public GameObject phase1End;
	public GameObject leftDoorPos;
	public GameObject rightDoorPos;
	public float phase1Factor = 5f;
	public Animator cartAnim;
	private int playerChoice = -1; //0 for left and 1 for right

	// Use this for initialization
	void Start () {
		cartAnim.enabled = true;

		cartAnim.Play ("Phase1Start");
		//camVehicle.transform.position = phase1Start.transform.position;
		mouseLook = camVehicle.GetComponent<RigidbodyFirstPersonController> ().mouseLook;
		mouseLook.XSensitivity = 0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
			StartCoroutine ("RunTask");
		}
		
	}

	IEnumerator RunTask()
	{
		Debug.Log ("running coroutine");
		cartAnim.enabled = true;
		cartAnim.Play ("Phase1Move");
		yield return new WaitForSeconds (5f);

		//yield return StartCoroutine (MovePlayerTo (phase1Start.transform.position,phase1End.transform.position,phase1Factor));
		//activate mouse look
		mouseLook.XSensitivity = 2f;
		cartAnim.enabled = false;
		yield return StartCoroutine (WaitForPlayerDecision ());
		cartAnim.enabled = true;
		if (playerChoice == 1) {
			cartAnim.Play ("Phase1MoveRight");

		} else if (playerChoice == 0) {
			cartAnim.Play ("Phase1MoveLeft");
		}
		yield return null;
	}

	IEnumerator WaitForPlayerDecision()
	{
		while (!Input.GetKeyDown(KeyCode.Return) || (camVehicle.transform.eulerAngles.y < 125f && camVehicle.transform.eulerAngles.y > 55f)) {
			yield return 0;
		}
		//right
		if (camVehicle.transform.eulerAngles.y > 125f) {
			playerChoice = 1;
			
		} 
		//left
		else if (camVehicle.transform.eulerAngles.y < 55f) {
			playerChoice = 0;
			
		}

		Debug.Log ("the player choice is: " + playerChoice.ToString ());

		yield return null;

	}

	IEnumerator MovePlayerTo(Vector3 startPos, Vector3 endPos, float factor)
	{
		float timer = 0f;
		Debug.Log ("about to move player");
		while (timer / factor < 1f) {
			timer += Time.deltaTime;
			Debug.Log ("timer " + timer.ToString ());
			camVehicle.transform.position = Vector3.Lerp (startPos, endPos, timer / factor);
			yield return 0;
		}
		yield return null;
	}

		
}
