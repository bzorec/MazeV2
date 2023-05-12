using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Gun properties
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public GunData gunData;


    // Private variables
    private Vector3 screenCenter;
    private float nextFireTime = 0f;
    private Animator animator;

    private void Awake()
    {
        // Get the animator component of the gun
        animator = GetComponent<Animator>();
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }

    private void Start()
    {
        //gunData.currentAmmoInMag = gunData.getMagSize();
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += Reload;
        //PlayerShoot.reloadInput += StartReload;
    }

    private void Update()
    {
        // Check if the left mouse button is pressed and enough time has passed since the last shot
        /*if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            // Set the next fire time
            nextFireTime = Time.time + gunData.fireRate;

            // Call the Shoot method
            Shoot();
        }*/
    }

    private void Shoot()
    {
        if (gunData == null)
        {
            Debug.LogError("GunData is not assigned to an instance of GunData.");
            return;
        }

        if (gunData.currentAmmoInMag <= 0 || Time.time < nextFireTime)
        {
            return;
        }

        // Play the shooting animation
        if (animator != null)
        {
            animator.Play("Shoot");
        }
        gunData.currentAmmoInMag--;


        // Cast a ray from the camera to the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Instantiate a new bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // Calculate the direction vector from the bullet spawn point to the hit point
            Vector3 direction = hit.point - bulletSpawnPoint.position;

            // Normalize the direction vector to get a unit vector
            Vector3 normalizedDirection = direction.normalized;

            // Multiply the unit vector by the desired bullet speed
            Vector3 velocity = normalizedDirection * gunData.bulletSpeed;

            // Set the velocity of the bullet
            bullet.GetComponent<Rigidbody>().velocity = velocity;
            //Debug.Log("Bullet speed: " + velocity);
        }

    }

    public void Reload()
    {
        if (gunData == null)
        {
            Debug.LogError("GunData is not assigned to an instance of GunData.");
            return;
        }

        int ammoToReload = gunData.magSize - gunData.currentAmmoInMag;

        if (ammoToReload <= 0)
        {
            return;
        }

        int ammoToRemoveFromInventory = Mathf.Min(ammoToReload, gunData.currentAmmo);

        gunData.currentAmmoInMag += ammoToRemoveFromInventory;
        gunData.currentAmmo -= ammoToRemoveFromInventory;
    }


}