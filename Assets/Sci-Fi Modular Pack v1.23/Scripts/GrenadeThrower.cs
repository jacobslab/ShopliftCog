using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour {

    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public AudioClip pinSound;
    public AudioClip explosionSound;
    new AudioSource audio;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();

        
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
        {
            audio.PlayOneShot(pinSound, 1.0F);
            StartCoroutine(PinWait()); //Wait 
           
        }

	}

    IEnumerator PinWait() {
        yield return new WaitForSeconds(2); // Wait two second after the pin was pulled.

        ThrowGrenade();
        audio.PlayOneShot(explosionSound, 1.0F);
    }

    void ThrowGrenade()
    {
       
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        
    }
}
