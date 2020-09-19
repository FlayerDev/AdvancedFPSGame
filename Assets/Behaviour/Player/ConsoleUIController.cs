using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConsoleUIController : MonoBehaviour
{
    public bool isConsoleActive = false;
    public KeyCode consoleKeyCode;
    public ScrollView scrollView;
    public GameObject consoleCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(consoleKeyCode)) isConsoleActive = !isConsoleActive;
        consoleCanvas.SetActive(isConsoleActive);
    }
}
