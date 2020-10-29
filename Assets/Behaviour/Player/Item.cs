using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public WeaponType weaponType;
    private void Awake()
    {
        
    }
}
public enum WeaponType
{
    Main,
    Secondary,
    Utility
}
