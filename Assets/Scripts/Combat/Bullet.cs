using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Bullet properties

    //public float lifetime = 3f;
    public GunData gunData;
    private float damage;

    // Private variables
    private float spawnTime;

    private void Awake()
    {

       //Destroy(gameObject);
    }

    private void Start()
    {
        // Set the spawn time
        spawnTime = Time.time;
    }

    private void Update()
    {
        if(Time.time > spawnTime+10f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit a target
        if (collision.collider.CompareTag("Target"))
        {
            // Get the target's health script

            EnemyController target = collision.collider.GetComponent<EnemyController>();
            //Debug.Log("Hit Target, health left: ");

            // Apply damage to the target
            if (target != null)
            {
                Debug.Log("Hit Target " + target);

                target.TakeDamage(gunData.damage);
            }
            // Destroy the bullet
            
        }
        Destroy(gameObject);
    }
}
