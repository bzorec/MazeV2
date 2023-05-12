using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    // Health properties
    public float maxHealth = 100f;

    // Private variables
    private float currentHealth;

    private void Start()
    {
        // Initialize the current health to the max health
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // Subtract damage from the current health
        currentHealth -= damage;

        // Check if the target is dead
        if (currentHealth <= 0f)
        {
            // Destroy the target object
            Destroy(gameObject);
        }
    }
}
