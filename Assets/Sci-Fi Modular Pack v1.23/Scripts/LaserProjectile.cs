
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class LaserProjectile : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public int impactForce = 5000;
    public float fireRate = 15f;
    public AudioClip laserGunSound;
    
   
    new AudioSource audio;
    Animator animator;

    public Camera fpsCam;
  //  public ParticleSystem laserFlash;

    public Rigidbody laserProjectile;
    public Transform barrelEnd;
   
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

            Rigidbody projectileInstance;

            projectileInstance = Instantiate(laserProjectile, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
            projectileInstance.AddForce(barrelEnd.forward * impactForce);

            //laserFlash.Play();
            audio.PlayOneShot(laserGunSound, 1.0F);
           
        }
	}

    
    
    

   
}
