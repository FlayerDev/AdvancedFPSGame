using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ConsoleEventSystem : MonoBehaviour
{ 
    GameObject thisObject;
    bool isAdmin = false;
    public ConsoleEventSystem(string param, string[] args, GameObject thisObject, bool allowAdministratorPermissions = false)
    {
        this.thisObject = thisObject;
        isAdmin = allowAdministratorPermissions;
        InitialiseEvent(out Dictionary<string, Func<string[], int>> commandDict);
        Debug.Log(commandDict.TryGetValue(param, out Func<string[], int> method) ?
            method(args) == 0 ? $" Event: \"{param}\" with Arguments: \"{args}\" Executed Successfully"
                              : $"[ERROR]: Invalid Arguments \"{args}\""
                              : $"[ERROR]: Unknown Command \"{param}\"");
    }

    private void InitialiseEvent(out Dictionary<string, Func<string[], int>> commandDict)
    {
        commandDict = new Dictionary<string, Func<string[], int>>();
        
        commandDict.Add("DebugLog", (string[] args) =>
        {
            Debug.Log(args);
            return 0;
        });

    }
}




