using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isWeaponAutomatic = true;
    public GameObject massle;
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
        Ray ray = new Ray(massle.transform.position, massle.transform.forward);
        var hitarr = Physics.RaycastAll(ray, maxDamageDistance);
        sortHitArray(out hitarr);
    }
    void sortHitArray(out RaycastHit[] multiHitInfo)
    {
        multiHitInfo = (Physics.RaycastAll(transform.parent.position, transform.parent.forward, Mathf.Infinity));
        System.Array.Sort(multiHitInfo, (x, y) => x.distance.CompareTo(y.distance));
    }
}
