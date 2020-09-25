using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Decals
    public GameObject woodDecal;
    public GameObject metalDecal;
    public GameObject cobbleDecal;
    public GameObject syntheticDecal;
    public GameObject concreteDecal;
    public GameObject defaultDecal;
    #endregion
    public bool isWeaponAutomatic = true;
    public GameObject massle;
    public float baseDamage = 32;
    [Range(10f,2000f)]public float maxDamageDistance = 500;

    [SerializeField] bool isArmed = true;
    [SerializeField] bool isShooting = false;
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
        float dmg = baseDamage; RaycastHit playerhit = new RaycastHit();
        RaycastHit[] hitarr = Physics.RaycastAll(massle.transform.position, massle.transform.forward, maxDamageDistance);
        Array.Sort(hitarr, (x, y) => x.distance.CompareTo(y.distance)); // Sorts hit objects by distance
        if (findPlayer(hitarr,ref playerhit))
        {
            foreach (RaycastHit item in hitarr)
            {
                if (!item.collider.gameObject.CompareTag("Player")) dmg = calculateDamageDropoff(dmg, item, playerhit);
                else applyDamage(playerhit.collider.gameObject);
            }
        }
    }
    //
    bool findPlayer(RaycastHit[] arr, ref RaycastHit player)//Returns the player Game Object, if one is found 
    {
        foreach (var item in arr)
        {
            if (item.collider.gameObject.CompareTag("Player"))
            {
                player = item;
                return true;
            }
        }
        return false;
    }
    float calculateDamageDropoff(float dmg, RaycastHit hit,RaycastHit playerhit)
    {
        hit.collider.Raycast(new Ray(playerhit.point, gameObject.transform.position),out RaycastHit outhit , 1000f);
        Vector3 inpoint = hit.point;
        Vector3 outpoint = outhit.point;
        var decal = decalDictionary(hit.collider.gameObject.tag) != null ? decalDictionary(hit.collider.gameObject.tag) : defaultDecal;
        Instantiate(decal, inpoint, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(decal, outpoint, Quaternion.FromToRotation(Vector3.up, outhit.normal));
        if (!DamageDropoffPerMaterial.MaterialValue.TryGetValue(hit.collider.gameObject.tag, out float dropvalue)) return 0f;
        dmg -= Vector3.Distance(inpoint, outpoint) * dropvalue;
        return dmg;
    }
    void applyDamage(GameObject player)
    {
        throw new NotImplementedException("Damage Not Applied");
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
        {"CONCRETE", 1.5f}
    };
}
