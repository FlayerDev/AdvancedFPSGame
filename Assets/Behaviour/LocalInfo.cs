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

    public static class KeyBinds
    {
        public static KeyCode Shoot = KeyCode.Mouse0;
        public static KeyCode ADS = KeyCode.Mouse1;
        public static KeyCode Jump = KeyCode.Space;
        public static KeyCode Reload = KeyCode.R;
        public static KeyCode Use = KeyCode.E;
        public static KeyCode InventoryMain = KeyCode.Alpha1;
        public static KeyCode InventorySecondary = KeyCode.Alpha2;
        public static KeyCode InventoryUtility = KeyCode.Alpha3;
        public static KeyCode InventoryDrop = KeyCode.G;
        public static KeyCode Console = KeyCode.F1;

        public static KeyCode GetKeyPressed(bool GetKeyDown = false)
        {
            KeyCode[] keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
            foreach (var item in keyCodes)
            {
                if (GetKeyDown)
                {
                    if (Input.GetKeyDown(item)) return item;
                }
                else
                {
                    if (Input.GetKey(item)) return item;
                }
            }
            return KeyCode.None;
        }
    }
}