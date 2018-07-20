using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	//PHASE 1
	public GameObject phase1Start;
	public GameObject phase1End;
	public GameObject phase1LeftDoor;
	public GameObject phase1RightDoor;

	//PHASE 2 LEFT
	public GameObject phase2Start_L;
	public GameObject phase2End_L;
	public GameObject phase2RightRegister_L;
	public GameObject phase2LeftRegister_L;


	//PHASE 2 RIGHT
	public GameObject phase2Start_R;
	public GameObject phase2End_R;
	public GameObject phase2RightRegister_R;
	public GameObject phase2LeftRegister_R;

	public Transform leftRoomTransform;
	public Transform rightRoomTransform;

	public GameObject roomOne;
	public GameObject roomTwo;
	public GameObject leftDoor;
	public GameObject rightDoor;

	//camera
	public GameObject phase1CamZone;
	public GameObject phase2CamZone_L;
	public GameObject phase2CamZone_R;

	//registerobj

	public GameObject oneLeftRegisterObj;
	public GameObject oneRightRegisterObj;
	public GameObject twoLeftRegisterObj;
	public GameObject twoRightRegisterObj;


	//audio
	public AudioSource baseAudio;
	public AudioSource roomOneAudio;
	public AudioSource roomTwoAudio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
