using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour {

    public float delay = 3f;
    public float blastRadius = 3f;
    public float explosionForce = 200f;
    public GameObject smokeEffect;
   
    

    [SerializeField] private CameraShake cameraShake;

    float countdown;
    public bool smokehasExploded = false;
	
    // Use this for initialization
	void Start () {
        countdown = delay;

     cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

       
       
    }

    // Update is called once per frame
    void Update()
    {

        countdown -= Time.deltaTime;
        if (countdown <= 0f && !smokehasExploded)
        {
            SmokeExplosion();
           
          
            smokehasExploded = true;
        }
    }

   
    void SmokeExplosion()
    {
        // Show Explosion Effect
        Instantiate(smokeEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius); //Get all nearby objects

        foreach (Collider nearbyObject in colliders) //Search for each object
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>(); //Get their rigidbodies
            if (rb != null) // If they have a rigidbody 
            {
                 rb.AddExplosionForce(explosionForce, transform.position, blastRadius); // Add a force for the explosion
              
               
            }

        }

        StartCoroutine(cameraShake.Shake(.3f, .4f));

        // Destroy Smoke Grenade
        Destroy(gameObject);
    }
}
