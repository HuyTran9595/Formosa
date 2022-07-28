using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
        inv = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //bool canCraft(ItemData i_item)
    //{
    //    foreach (ItemData item in inv.herbs)
    //    {

    //    }
    //    return false;
    //}
}
