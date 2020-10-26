﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Properties
    #region Decals
    [Header("Decals")]
    public GameObject woodDecal;
    public GameObject metalDecal;
    public GameObject cobbleDecal;
    public GameObject syntheticDecal;
    public GameObject concreteDecal;
    public GameObject defaultDecal;
    #endregion
    [Space]
    [Header("Basic Settings")]
    public WeaponType WeaponType;
    public bool isWeaponAutomatic = true; //While enabled the weapon will automatically fire when the Shoot button is held
    public GameObject mussle;
    public float baseDamage = 32; // Initial damage of the weapon
    [Range(10f, 2000f)] public float effectiveDistance = 500; // Max distance the bullet/Raycast will travel
    public float DistanceDropoff = .1f;// ![TO BE IMPLEMENTED]!
    public float PenetrationPower = 1f;// Νeutralizes the wallbang's DamageDropoffPerMaterial
    [SerializeField] bool isArmed = true; // If enabled weapon will fire upon Fire button click
    public bool allowADS = false;
    [SerializeField] int RPM = 200; // Rounds Per Minute: MS between shots = 1000 / (RPM / 60)
    [Header("Recoil")]
    public bool doRecoil = true;
    public float VerticalRecoil = .1f;
    public float MaximumRecoil = 10f;
    public float HorizontalRecoil = .1f;
    public float HorizontalMultiplierOnMaxVertical = 3f;


    #endregion
    #region others
    private Action update;
    #endregion
    private void Awake()
    {
        if (isWeaponAutomatic) update += () => { if (Input.GetKey(LocalInfo.KeyBinds.Shoot)) fire(); };
        else update += () => { if (Input.GetKeyDown(LocalInfo.KeyBinds.Shoot)) fire(); };

        if (allowADS) update += () => { if (Input.GetKeyDown(LocalInfo.KeyBinds.ADS)) ; };
    }
    public void Update() => update();
    async void rearm()
    {
        isArmed = false;
        var ms = RPM / 60;
        ms = 1000 / ms;
        await System.Threading.Tasks.Task.Delay(ms);
        isArmed = true;
    }

    public void fire()
    {
        if (!isArmed) return;
        rearm();
        float dmg = baseDamage;
        RaycastHit[] hitarr = Physics.RaycastAll(mussle.transform.position, mussle.transform.forward, effectiveDistance);
        Array.Sort(hitarr, (x, y) => x.distance.CompareTo(y.distance)); // Sorts hit objects by distance
        foreach (RaycastHit item in hitarr)
        {
            if (item.collider.gameObject.CompareTag("Player")) applyDamage(item.collider.gameObject, dmg);
            dmg = calculateDamage(dmg, item);
        }
    }
    #region Fire Functions
    float calculateDamage(float dmg, RaycastHit hit)
    {
        //RaycastHit outhit = findOppositeSide(new Ray(massle.transform.position + massle.transform.forward.normalized * maxDamageDistance
        //    , massle.transform.TransformDirection(Vector3.back)), hit.collider.gameObject);
        hit.collider.Raycast(new Ray(mussle.transform.position + mussle.transform.forward.normalized * effectiveDistance
            , mussle.transform.TransformDirection(Vector3.back)), out RaycastHit outhit, effectiveDistance * 2);

        Vector3 inpoint = hit.point; Vector3 outpoint = outhit.point; // Gets coordinates of hit positions
        printBulletDecal(hit, outhit, inpoint, outpoint);
        if (!DamageDropoffPerMaterial.MaterialValue.TryGetValue(hit.collider.gameObject.tag, out float dropvalue)) return 0f;
        dmg -= Vector3.Distance(inpoint, outpoint) * dropvalue / PenetrationPower;
        Debug.Log($"{Vector3.Distance(inpoint, outpoint)} Distance");
        dmg = dmg < 999f && dmg > 0f ? dmg : (dmg > 999f ? 999f : 0f);
        return dmg;
    }
    static RaycastHit findOppositeSide(Ray ray, GameObject gO)
    {
        var res = Physics.RaycastAll(ray, 9999f);
        foreach (RaycastHit item in res) if (item.collider.gameObject == gO) return item;
        return new RaycastHit();
    }
    void printBulletDecal(RaycastHit hit, RaycastHit outhit, Vector3 inpoint, Vector3 outpoint)
    {
        if (hit.collider.gameObject.CompareTag("Player")) return;
        var decal = decalDictionary(hit.collider.gameObject.tag) != null ? decalDictionary(hit.collider.gameObject.tag) : defaultDecal;
        Instantiate(decal, inpoint, Quaternion.LookRotation(hit.normal));
        Instantiate(decal, outpoint, Quaternion.LookRotation(outhit.normal));
    }
    void applyDamage(GameObject player, float amount)
    {
        Debug.Log($"Dealt {amount} of damage to {player.name}");
    }
    #endregion
    #region Dictionaries And Enums
    GameObject decalDictionary(string decal)
    {
        Dictionary<string, GameObject> decalDictionary = new Dictionary<string, GameObject>
        {
        {"SYNTHETIC", syntheticDecal},
        {"WOOD" , woodDecal},
        {"METAL", metalDecal},
        {"COBBLE", cobbleDecal},
        {"CONCRETE", concreteDecal}
        };
        if (decalDictionary.TryGetValue(decal, out GameObject returnObj)) return returnObj;
        else return null;
    }
}
static class DamageDropoffPerMaterial
{
    public static Dictionary<string, float> MaterialValue = new Dictionary<string, float>
    {
        {"SYNTHETIC", 8f},
        {"WOOD" , 16f},
        {"METAL", 32f},
        {"COBBLE", 40f},
        {"CONCRETE", 60f},
        {"Player", 20f}
    };
}
public enum WeaponType
{
    Main,
    Secondary,
    Utility
}
#endregion