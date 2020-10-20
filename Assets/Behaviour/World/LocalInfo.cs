using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public static class LocalInfo
{
    public static bool isPaused = false;

    public static class KeyBinds
    {
        public static KeyCode Shoot = KeyCode.Mouse0;
        public static KeyCode ADS = KeyCode.Mouse1;
        public static KeyCode Forward = KeyCode.W;//Unused
        public static KeyCode Right = KeyCode.D;//Unused
        public static KeyCode Left = KeyCode.A;//Unused
        public static KeyCode Back = KeyCode.S;//Unused
        public static KeyCode Jump = KeyCode.Space;
        public static KeyCode Reload = KeyCode.R;
        public static KeyCode InventoryMain = KeyCode.Alpha1;
        public static KeyCode InventorySecondary = KeyCode.Alpha2;


        public static KeyCode Console = KeyCode.F1;
    }
}
