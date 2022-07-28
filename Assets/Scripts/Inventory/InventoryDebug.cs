using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDebug : MonoBehaviour
{
    Inventory inventory;
    [Range(0,999)]
    public int itemID;
    [Range(1,10)]
    public int amount;
    public ItemData.Tier tier;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem()
    {
        ItemData itemData =  DatabaseScript.GetItem(itemID, tier);
        inventory.AddItem(itemData, amount);
    }
}
