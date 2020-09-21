using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class ConsoleUIController : MonoBehaviour
{
    public bool isConsoleActive = false;
    public KeyCode consoleKeyCode;
    public TextMeshProUGUI consoleTMPUGUI;
    public InputField commandField;
    public GameObject consoleCanvas;

    List<string> consoleContent = new List<string>();
    private void Start() => consoleTMPUGUI.autoSizeTextContainer = true;
    void Update()
    {
        if (Input.GetKeyDown(consoleKeyCode)) counterState();
        consoleCanvas.SetActive(isConsoleActive);
        if (isConsoleActive) consoleUpdate();
    }
    void consoleUpdate() // Updates only when console is drawn
    {
        if (Input.GetKeyDown(KeyCode.Return) && commandField.text != string.Empty) 
            eventCaller(); // calls eventCaller() if Enter (Return) is pressed

    }
    void counterState() // Alters the state of isConsoleActive and frees cursor
    {
        if (isConsoleActive)
        {
            isConsoleActive = false;
            LocalInfo.isPaused = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        else
        {
            isConsoleActive = true;
            LocalInfo.isPaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            commandField.Select();
        }
    }

    /////////////////////////////////
    // Occasionally called methods //
    /////////////////////////////////
    public void appendContent(string text)
    {
        consoleContent.Add(text);
        if(consoleContent.Count > 28)
        {
            consoleContent.RemoveAt(0);
        }
        consoleTMPUGUI.text = string.Empty;
        foreach (var item in consoleContent)
        {
            appendText(item);
        }
    }
    public void appendText(string text)
    {
        consoleTMPUGUI.text += text + '\n';
    }
    public void setText(string text)
    {
        consoleTMPUGUI.text = text;
    }
    void eventCaller()
    {
        commandParse(commandField.text,out string command, out string[] args);
        new ConsoleEvent(command, args, gameObject);
        commandField.text = string.Empty;
    }
    void commandParse(string input,out string command, out string[] args)
    {
        var inputArr = input.Split(' ');
        command = inputArr[0];
        args = new string[inputArr.Length - 1];
        for (int i = 1; i < inputArr.Length; i++)
        {
            args[i - 1] = inputArr[i];
        }

    }
    
}
