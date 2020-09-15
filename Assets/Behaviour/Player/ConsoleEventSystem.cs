using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConsoleEventSystem : MonoBehaviour
{
    public ConsoleEventSystem(string param, string[] args)
    {
        Dictionary<string, Func<string[], int>> commandDict;
        InitialiseEvent(out commandDict);
        commandDict.TryGetValue(param, out Func<string[], int> method);
        int result = method(args);
        if (method(args) == 0) Debug.Log($" Event: \"{param}\" with Arguments: \"{args}\" Executed Successfully  (0x0)");
        else Debug.Log($"[ERROR] Execution Failed (0x1)");
    }

    private void InitialiseEvent(out Dictionary<string, Func<string[], int>> commandDict)
    {
        commandDict = new Dictionary<string, Func<string[], int>>();

        commandDict.Add("DebugLog", (string[] args) =>
        {
            Debug.Log(args);
            return 0;
        });
        commandDict.Add("quit", (string[] args) => { Application.Quit(); return 0; });

    }
}




