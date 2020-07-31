
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class LaserGun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public AudioClip laserGunSound;
    
   
    new AudioSource audio;
    Animator animator;

    public Camera fpsCam;
    public ParticleSystem laserFlash;
    
   
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;

   

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        
        animator.Play("WeaponBWalkMove");
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
	}

    
    
    void Shoot()
    {
         laserFlash.Play();
       
       
       

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            audio.PlayOneShot(laserGunSound, 1.0F);
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 5f);
        }

    }
}
