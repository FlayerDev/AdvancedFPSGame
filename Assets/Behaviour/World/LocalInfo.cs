using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalInfo
{
    public static bool isPaused = false;

    public static class KeyBinds
    {
        public static KeyCode Shoot = KeyCode.Mouse0;
        public static KeyCode ADS = KeyCode.Mouse1;
        public static KeyCode Forward = KeyCode.W;
        public static KeyCode Right = KeyCode.D;
        public static KeyCode Left = KeyCode.A;
        public static KeyCode Back = KeyCode.S;
        public static KeyCode Jump = KeyCode.Space;
        public static KeyCode Console = KeyCode.C;
    }
}