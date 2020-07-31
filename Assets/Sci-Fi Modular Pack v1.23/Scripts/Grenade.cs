using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float delay = 3f;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;

    

    [SerializeField] private CameraShake cameraShake;

    float countdown;
    public bool hasExploded = false;
	
    // Use this for initialization
	void Start () {
        countdown = delay;
        
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {

        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
           
          
            hasExploded = true;
        }
    }

    void Explode()
    {
        // Show Explosion Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

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

        // Destroy Grenade
        Destroy(gameObject);
    }
}
