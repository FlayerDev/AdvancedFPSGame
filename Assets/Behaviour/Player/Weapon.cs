using System;
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
    [Space][Header("Basic Settings")]
    public bool isWeaponAutomatic = true;
    public GameObject massle;
    public float baseDamage = 32;
    [Range(10f, 2000f)] public float maxDamageDistance = 500;
    public float DistanceDropoff = .1f;
    [SerializeField] bool isArmed = true;
    [SerializeField] bool isShooting = false;
    [Header("Recoil")]
    public bool doRecoil = true;
    public float VerticalRecoil = .1f;
    public float MaximumRecoil = 10f;
    public float HorizontalRecoil = .1f;
    public float HorizontalMultiplierOnMaxVertical = 3f;


    #endregion
    void Start()
    {

    }
    void Update()
    {
        isShooting = Input.GetKeyDown(LocalInfo.KeyBinds.Shoot);
    }
    void FixedUpdate()
    {
        if (isShooting) fire();
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
    //
    float calculateDamage(float dmg, RaycastHit hit)
    { 
        hit.collider.Raycast(new Ray(massle.transform.position + massle.transform.forward.normalized * maxDamageDistance
            , massle.transform.position.normalized), out RaycastHit outhit,float.PositiveInfinity);
        //Debug.Log(massle.transform.position + massle.transform.forward.normalized * maxDamageDistance);
        //Debug.DrawRay(massle.transform.position + massle.transform.forward.normalized * maxDamageDistance, outhit.point,Color.red,10000f);
        Vector3 inpoint = hit.point; Vector3 outpoint = outhit.point; // Gets coordinates of hit positions
        printBulletDecal(hit, outhit, inpoint, outpoint);
        if (!DamageDropoffPerMaterial.MaterialValue.TryGetValue(hit.collider.gameObject.tag, out float dropvalue)) return 0f;
        dmg -= Vector3.Distance(inpoint, outpoint) * dropvalue;
        dmg = Math.Abs(dmg);// Must fix
        return dmg;
    }
    void applyDamage(GameObject player, float amount)
    {
        Debug.Log($"Dealt {amount} of damage to {player.name}");
    }
    void printBulletDecal(RaycastHit hit, RaycastHit outhit, Vector3 inpoint, Vector3 outpoint)
    {
        if (hit.collider.gameObject.CompareTag("Player")) return;
        var decal = decalDictionary(hit.collider.gameObject.tag) != null ? decalDictionary(hit.collider.gameObject.tag) : defaultDecal;
        Instantiate(decal, inpoint, Quaternion.LookRotation(hit.normal));
        Instantiate(decal, outpoint, Quaternion.LookRotation(outhit.normal));
        Debug.Log($"{outpoint.x} {outpoint.y} {outpoint.z}, {Quaternion.LookRotation(outhit.normal)}");
    }
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
        {"SYNTHETIC", .2f},
        {"WOOD" , .4f},
        {"METAL", .8f},
        {"COBBLE", 1f},
        {"CONCRETE", 1.5f},
        {"Player", .5f}
    };
}
