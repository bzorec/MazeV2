using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weapons;
    public int currentWeapon = 0;
    public PlayerShoot playerShoot;

    private InputAction scrollAction;

    private void Awake()
    {
        // Initialize the scroll action
        scrollAction = new InputAction(binding: "<Mouse>/scroll");
        scrollAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the scroll action
        scrollAction.Disable();
    }

    private void Start()
    {
        // Enable the current weapon
        EnableWeapon(currentWeapon);
    }

    private void Update()
    {
        // Handle weapon switching using the scroll wheel or number keys
        float scroll = scrollAction.ReadValue<float>();
        if (scroll > 0)
        {
            // Switch to the next weapon
            currentWeapon = (currentWeapon + 1) % weapons.Length;
            EnableWeapon(currentWeapon);
        }
        else if (scroll < 0)
        {
            // Switch to the previous weapon
            currentWeapon = (currentWeapon - 1 + weapons.Length) % weapons.Length;
            EnableWeapon(currentWeapon);
        }
        else
        {
            // Handle weapon switching using number keys
            for (int i = 0; i < weapons.Length; i++)
            {
                if (Keyboard.current[(Key)(i + 1)].wasPressedThisFrame)
                {
                    // Switch to the selected weapon
                    currentWeapon = i;
                    EnableWeapon(currentWeapon);
                }
            }
        }
    }

    private void EnableWeapon(int index)
    {
        // Disable all weapons except the selected one
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        GameObject currentWeapon = weapons[index];
        Gun currentGun = currentWeapon.GetComponent<Gun>();
        playerShoot.UpdateGunReference(currentGun);
    }
}
