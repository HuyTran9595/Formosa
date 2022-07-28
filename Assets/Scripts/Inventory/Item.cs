using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData MyStats;   
    public void OnTriggerEnter(Collider Other)
    {
        if(Other.GetComponent<Inventory>() != null)
        {
            FindObjectOfType<Inventory>().AddItem(MyStats);
        }
    }
    public void OnMouseDown()
    {
        FindObjectOfType<Inventory>().AddItem(MyStats);
    }
}
