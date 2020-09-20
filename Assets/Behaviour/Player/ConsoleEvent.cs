using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ConsoleEvent
{ 
    GameObject thisObject;
    bool isAdmin = false;
    public ConsoleEvent(string param, string[] args, GameObject thisObject, bool allowAdministratorPermissions = false)
    {
        var callerConsole = thisObject.GetComponent<ConsoleUIController>(); 
        this.thisObject = thisObject;
        isAdmin = allowAdministratorPermissions;
        InitialiseEvent(out Dictionary<string, Func<string[], int>> commandDict);
        callerConsole.appendText(commandDict.TryGetValue(param, out Func<string[], int> method) ?
            method(args) == 0 ? $" Event: \"{param}\" with Arguments: \"{parseArgs(args)}\" Executed Successfully"
                              : $"[ERROR]: Invalid Arguments \"{parseArgs(args)}\""
                              : $"[ERROR]: Unknown Command \"{param}\"");
    }

    private void InitialiseEvent(out Dictionary<string, Func<string[], int>> commandDict)
    {
        commandDict = new Dictionary<string, Func<string[], int>>();
        
        commandDict.Add("log", (string[] args) =>
        {
            thisObject.GetComponent<ConsoleUIController>().appendText($"'{parseArgs(args)}'");
            return 0;
        });
        commandDict.Add("quit", (string[] args) => { Application.Quit(); return 0; });

    }
    string parseArgs(string[] args, bool spaceIndexes = true)
    {
        string word = string.Empty;
        for (int i = 0; i < args.Length; i++)
        {
            if (spaceIndexes && i > 0) word += (char)32;
            word += args[i];
        }
        return word;
    }
}




