using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeThrower : MonoBehaviour {

    public float throwForce = 10f;
    public GameObject smokeGrenadePrefab;
    public AudioClip smokepinSound;
    public AudioClip smokeExplosionSound;
    new AudioSource audio;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();

        
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
        {
            audio.PlayOneShot(smokepinSound, 1.0F);
            StartCoroutine(PinWait()); //Wait StartCoroutine(PinWait()); //Wait
           
        }

	}

    IEnumerator PinWait() {
        yield return new WaitForSeconds(2); // Wait two second after the pin was pulled.

        ThrowSmokeGrenade();
        audio.PlayOneShot(smokeExplosionSound, 1.0F);
    }

    void ThrowSmokeGrenade()
    {
       
        GameObject smokegrenade = Instantiate(smokeGrenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = smokegrenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        
    }
}
