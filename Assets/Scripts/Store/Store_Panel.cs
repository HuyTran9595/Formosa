using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store_Panel : MonoBehaviour
{
    [Header("Store")]
    public GameObject Store;
    public bool StoreActive = false;
    [Header("PetStore")]
    public GameObject PetStore;
    public bool PetStoreActive = false;
    public BuySellButtons buy = null;
    public BuySellButtons sell = null;
    public void Start()
    {
        //Store.SetActive(false);
        //PetStore.SetActive(false);
    }

    public void OnTriggerEnter(Collider Other)
    {
        //Store.SetActive(true);
        //StoreActive = true;
        if(buy != null)
        {
            buy.gameObject.SetActive(true);
        }

        if (sell != null)
        {
            sell.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider Other)
    {
        //Store.SetActive(false);
        //StoreActive = false;
        //if(PetStoreActive == true)
        //{
        //    PetStore.SetActive(!PetStoreActive);
        //}

        if (buy != null)
        {
            buy.gameObject.SetActive(false);
        }

        if (sell != null)
        {
            sell.gameObject.SetActive(false);
        }
    }

    public void Pet_Store()
    {
        PetStoreActive = !PetStoreActive;
        PetStore.SetActive(PetStoreActive);
        Store.SetActive(!PetStoreActive);
    }
}
