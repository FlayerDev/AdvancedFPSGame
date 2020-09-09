using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experimental : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit[] hit = Physics.RaycastAll(Vector3.zero, Vector3.forward, 1000);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
