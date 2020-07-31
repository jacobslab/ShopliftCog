
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class Gun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public AudioClip gunSound;
    
    public AudioClip shellSound;
    new AudioSource audio;
    Animator animator;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem gunSmoke;
    public ParticleSystem gunBurst;
   
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;

    public GameObject bulletcasing;
    GameObject PrefabClone;
    public int ejectSpeed = 100;

   

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

    void BulletEjector ()
    {

        PrefabClone = Instantiate(bulletcasing, transform.position, Quaternion.identity) as GameObject;
        PrefabClone.GetComponent<Rigidbody>().AddForce(transform.forward * ejectSpeed);
        audio.PlayOneShot(shellSound, 1.0F);
        Destroy(PrefabClone, 10f);
    }
    
    void Shoot()
    {
        muzzleFlash.Play();
        gunSmoke.Play();
        gunBurst.Play();
       
        BulletEjector();

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            audio.PlayOneShot(gunSound, 1.0F);
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 5f);
        }

    }
}
