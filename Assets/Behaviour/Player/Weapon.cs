using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [Space][Header("Basic Settings")]
    public bool isWeaponAutomatic = true;
    public GameObject massle;
    public float baseDamage = 32;
    [Range(10f, 2000f)] public float maxDamageDistance = 500;
    public float DistanceDropoff = .1f;
    [SerializeField] bool isArmed = true;
    [Header("Recoil")]
    public bool doRecoil = true;
    public float VerticalRecoil = .1f;
    public float MaximumRecoil = 10f;
    public float HorizontalRecoil = .1f;
    public float HorizontalMultiplierOnMaxVertical = 3f;


    #endregion
    void Update()
    {
        if (Input.GetKeyDown(LocalInfo.KeyBinds.Shoot)) fire();
    }
    
    void fire()
    {
        float dmg = baseDamage;
        RaycastHit[] hitarr = Physics.RaycastAll(massle.transform.position, massle.transform.forward, maxDamageDistance);
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
        RaycastHit outhit = findOppositeSide(new Ray(massle.transform.position + massle.transform.forward.normalized * maxDamageDistance
            , massle.transform.TransformDirection(Vector3.back)), hit.collider.gameObject);
        Vector3 inpoint = hit.point; Vector3 outpoint = outhit.point; // Gets coordinates of hit positions
        printBulletDecal(hit, outhit, inpoint, outpoint);
        if (!DamageDropoffPerMaterial.MaterialValue.TryGetValue(hit.collider.gameObject.tag, out float dropvalue)) return 0f;
        dmg -= Vector3.Distance(inpoint, outpoint) * dropvalue;
        Debug.Log($"{Vector3.Distance(inpoint, outpoint)} Distance");
        dmg = dmg < 999f && dmg > 0f ? dmg : (dmg > 999f ? 999f : 0f); 
        return dmg;
    }
    RaycastHit findOppositeSide(Ray ray,GameObject gO)
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
    #region Dictionaries
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
        {"SYNTHETIC", 2f},
        {"WOOD" , 4f},
        {"METAL", 8f},
        {"COBBLE", 10f},
        {"CONCRETE", 15f},
        {"Player", 5f}
    };
}
#endregion