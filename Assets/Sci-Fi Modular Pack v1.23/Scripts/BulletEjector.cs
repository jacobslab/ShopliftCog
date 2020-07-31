using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEjector : MonoBehaviour {


    public GameObject bulletcasing;
    GameObject PrefabClone;

    public int ejectSpeed = 100;

   

    public 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        
       PrefabClone = Instantiate(bulletcasing, transform.position, Quaternion.identity) as GameObject;
        PrefabClone.GetComponent<Rigidbody>().AddForce(transform.forward * ejectSpeed);
        Destroy(PrefabClone,3f);

    }

    
}
