using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store_Slot : MonoBehaviour
{
    public ItemData HeldItem;
    public Image Icon;
    public Text PriceText;
    Inventory PInv;
    public void Start()
    {
        Icon.sprite = HeldItem.Icon;
        PriceText.text = HeldItem.Price.ToString();
        PInv = FindObjectOfType<Inventory>();
        if(PInv == null)
        {
            Debug.Log("Player Inventory wasn't found");
        }
    }
    public void OnMouseDown()
    {
        Debug.Log(HeldItem + " was bought for " + HeldItem.Price.ToString() + "!");
        if(PInv.Coins >= HeldItem.Price)
        {
            PInv.CoinUpdate(-HeldItem.Price);
            PInv.AddItem(DatabaseScript.GetItem(HeldItem.ID));
        }
        else
        {
            Debug.Log("Player has attempted to buy but doesnt have enough coins");
        }
    }
}
