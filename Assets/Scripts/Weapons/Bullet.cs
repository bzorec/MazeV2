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
        // Destroy the bullet after its lifetime expires
        /*if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }*/
    }

    /*private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hit a target
        if (other.CompareTag("Target"))
        {
            // Get the target's health script

            TargetHealth target = other.GetComponent<TargetHealth>();



            // Apply damage to the target
            if (target != null)
            {
                target.TakeDamage(gunData.damage);
            }

        }
        // Destroy the bullet
        Destroy(gameObject);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit a target
        if (collision.collider.CompareTag("Target"))
        {
            // Get the target's health script

            Target target = collision.rigidbody.GetComponent<Target>();



            // Apply damage to the target
            if (target != null)
            {
                Debug.Log("Hit Target, health left: " + target.health);
                target.TakeDamage(gunData.damage);
            }
            Debug.Log("Hit Target " + target);

        }

        // Destroy the bullet
        Destroy(gameObject);
    }
}
