using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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
        isShooting = Input.GetMouseButton(0);
    }
    void FixedUpdate()
    {
        if (isShooting) fire();
    }
    void fire()
    {
        float dmg = baseDamage; GameObject player = null;
        RaycastHit[] hitarr = Physics.RaycastAll(massle.transform.position, massle.transform.forward, maxDamageDistance);
        Array.Sort(hitarr, (x, y) => x.distance.CompareTo(y.distance)); // Sorts hit objects by distance
        if (findPlayer(hitarr, ref player))
        {
            foreach (RaycastHit item in hitarr)
            {
                if (!item.collider.gameObject.CompareTag("Player")) dmg = calculateDamageDropoff(dmg, item,player);
                else applyDamage(player);
            }
        }
    }
    //
    bool findPlayer(RaycastHit[] arr, ref GameObject player)//Returns the player Game Object, if one is found 
    {
        foreach (var item in arr)
        {
            if (item.collider.gameObject.CompareTag("Player"))
            {
                player = item.collider.gameObject;
                return true;
            }
        }
        return false;
    }
    float calculateDamageDropoff(float dmg, RaycastHit hit, GameObject player)
    {
        
        return dmg;
    }
    void applyDamage(GameObject player)
    {
        throw new NotImplementedException("Damage Not Applied");
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
