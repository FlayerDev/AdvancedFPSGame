using System.Collections;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    string label = "";
    float count;
    public int updateDelay = 1;
    public UnityEngine.UI.Text text;
    
    void Update()
    {
        if (Time.timeScale == 1)
        {
            count = (1 / Time.deltaTime);
            label = "FPS:" + (Mathf.Round(count));
        }
        else
        {
            label = "Pause";
        }
        text.text = label;
    }
}
