using System;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public static class LocalInfo
{
    public static bool IsPaused = false;
    public static GameObject muzzle { get => GameObject.Find("WorldCamera").transform.GetChild(0).gameObject; }
    public static float useDistance = 5f;
    public static object useRaycastHit { 
        get => Physics.RaycastAll(muzzle.transform.position, muzzle.transform.forward, useDistance)[0];
        set => useDistance = (float)value;
    }
}