using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	//PHASE 1
	public GameObject phase1Start_L;
	public GameObject phase1End_L;
	public GameObject phase1Start_R;
	public GameObject phase1End_R;


	//PHASE 2
	public GameObject phase2Start_L;
	public GameObject phase2End_L;
	public GameObject phase2Start_R;
	public GameObject phase2End_R;

	//PHASE 3
	public GameObject phase3Start_L;
	public GameObject phase3End_L;
	public GameObject phase3Start_R;
	public GameObject phase3End_R;

	//doors
	public GameObject phase1Door_L;
	public GameObject phase1Door_R;
	public GameObject phase2Door_L;
	public GameObject phase2Door_R;

	//registers
	public GameObject register_L;
	public GameObject register_R;

	public Transform leftRoomTransform;
	public Transform rightRoomTransform;

	public GameObject roomOne;
	public GameObject roomTwo;
	public GameObject leftDoor;
	public GameObject rightDoor;

	//camera
	public GameObject phase1CamZone_L;
	public GameObject phase1CamZone_R;
	public GameObject phase2CamZone_L;
	public GameObject phase2CamZone_R;
	public GameObject phase3CamZone_L;
	public GameObject phase3CamZone_R;

	//registerobj
	public GameObject leftRegisterObj;
	public GameObject rightRegisterObj;


	//audio
	public AudioSource one_L_Audio;
	public AudioSource two_L_Audio;
	public AudioSource three_L_Audio;
	public AudioSource one_R_Audio;
	public AudioSource two_R_Audio;
	public AudioSource three_R_Audio;

	//roadblocks
	public GameObject p1Roadblock;
	public GameObject roomOneRoadblock;
	public GameObject roomTwoRoadblock;

	//for comparative and solo sliders
	public List<Texture> groupOne;
	public List<Texture> groupTwo;
	public List<Texture> groupThree;

	public Material envSkybox;

	public GameObject suitcasePrefab;
	public List<GameObject> suitcases;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
