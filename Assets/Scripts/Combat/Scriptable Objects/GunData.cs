using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{

    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    [Tooltip("In RPM")] public float fireRate;
     public float bulletSpeed;

    [Header("Reloading")]
    public int magSize;
    public int maxAmmo;

    
    public float reloadTime;
    [HideInInspector] public bool reloading;
    [HideInInspector] public int currentAmmo;
    [HideInInspector] public int currentAmmoInMag;

    public void OnEnable()
    {
        currentAmmoInMag = magSize;
        currentAmmo = maxAmmo - magSize;
    }

    public int getMagSize()
    {
        return magSize;
    }

    public int getMaxAmmo()
    {
        return maxAmmo;
    }
}