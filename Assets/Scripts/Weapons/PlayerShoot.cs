using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Import the new Input System namespace

public class PlayerShoot : MonoBehaviour
{
     public static Action shootInput;
     public static Action reloadInput;

    public WeaponManager weaponManager;
    public Gun gun;

    // Create a variable to store the new Input Action
    private InputAction shootAction;
     private InputAction reloadAction;

     private void Awake()
     {
        // Find the current Gun component based on the currentWeapon index
        GameObject currentWeapon = weaponManager.weapons[weaponManager.currentWeapon];
        gun = currentWeapon.GetComponent<Gun>();

        // Initialize the Input Actions
        shootAction = new InputAction(binding: "<Mouse>/leftButton");
         reloadAction = new InputAction(binding: "<Keyboard>/r");

         // Register the callback functions for the Input Actions
         shootAction.performed += _ => Shoot();
         reloadAction.performed += _ => Reload();
     }

     private void OnEnable()
     {
         // Enable the Input Actions
         shootAction.Enable();
         reloadAction.Enable();
     }

     private void OnDisable()
     {
         // Disable the Input Actions
         shootAction.Disable();
         reloadAction.Disable();
     }

     private void Shoot()
     {
         shootInput?.Invoke();
     }

     private void Reload()
     {
         reloadInput?.Invoke();
     }

    public void UpdateGunReference(Gun newGun)
    {
        // Update the gun reference
        gun = newGun;
    }
}
