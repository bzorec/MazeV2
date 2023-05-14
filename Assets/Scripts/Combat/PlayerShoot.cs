using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Import the new Input System namespace
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerShoot : MonoBehaviour
{
     public static Action shootInput;
     public static Action reloadInput;

    public WeaponManager weaponManager;
    public Gun gun;

    // Create a variable to store the new Input Action
    private InputAction shootAction;
     private InputAction reloadAction;
    public Sprite crosshairImage;

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

        // Create a new game object
        GameObject crosshairObject = new GameObject("Crosshair");

        // Set the new game object's parent to be the current canvas
        // You may need to replace "Canvas" with the name of your canvas
        crosshairObject.transform.SetParent(GameObject.Find("Canvas").transform);

        // Add an Image component to the game object
        Image crosshair = crosshairObject.AddComponent<Image>();

        // Set the image's sprite to be the crosshair sprite
        crosshair.sprite = crosshairImage;

        // Set the position and size of the crosshair
        crosshair.rectTransform.anchoredPosition = Vector2.zero; // This sets the position to the center of the canvas
        crosshair.rectTransform.sizeDelta = new Vector2(50, 50); // This sets the size of the crosshair
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
