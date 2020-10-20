using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventoryItems;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    void refreshInventory()
    {
        List<GameObject> itemsToAdd = new List<GameObject>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
            itemsToAdd.Add(gameObject.transform.GetChild(i).gameObject);
        
        inventoryItems = itemsToAdd.ToArray();
    }
}
