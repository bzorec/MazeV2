using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    // Reference to the WeaponManager script on the player character
    public WeaponManager weaponManager;

    // Reference to the Text component of this game object
    private TextMeshProUGUI ammoText;

    private void Start()
    {
        // Get the Text component
        ammoText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // Get the current weapon and its ammo count
        GameObject currentWeapon = weaponManager.weapons[weaponManager.currentWeapon];
        Gun gun = currentWeapon.GetComponent<Gun>();
        int currentAmmo = gun.gunData.currentAmmo;
        int currentAmmoInMag = gun.gunData.currentAmmoInMag;

        // Update the ammo display
        ammoText.text = currentAmmoInMag.ToString() + "/" + currentAmmo.ToString();
    }
}


