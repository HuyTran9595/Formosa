using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public GameObject[] popUps;
//    public GameObject Store;
    public bool StoreActive = false;
//    public BuySellButtons buy = null;
//    public BuySellButtons sell = null;
    public GameObject tileOneObj;
    private Tiles tileScrpt;
    public GameObject tileTwoObj;
    private Tiles tileScrpt2;
    public GameObject tileThreeObj;
    private Tiles tileScrpt3;
    public GameObject tileFourObj;
    private Tiles tileScrpt4;
    private int temp1 = 20;
    private int temp2 = 20;
    private int temp3 = 20;
    private int temp4 = 20;
    private bool plantclick = false;
    private bool plantclick2 = false;
    private bool plantclick3 = false;
    private bool plantclick4 = false;
    public GameObject shopObj;
    private Shop shopScript;
    private int seedSold = 0;
    public GameObject journalObj;
    private GlowButton journalScrpt;
    private bool journalClick = false;
    public GameObject transactionObj;
    private TransactionManager transactionScrpt;
    private int storeItems;
    public float waitTime = 5f;
    private int popUpIndex;

    public bool tutorialDone = false;
 //   private bool atStore = false;

 //   public void OnTriggerEnter(Collider Other)
 //   {
        //Store.SetActive(true);
        //StoreActive = true;
 //       if (buy != null && sell != null)
 //       {
 //           atStore = true;
 //       }
 //   }

    private void Start()
    {
        tileScrpt = tileOneObj.GetComponent<Tiles>();
        tileScrpt2 = tileTwoObj.GetComponent<Tiles>();
        tileScrpt3 = tileThreeObj.GetComponent<Tiles>();
        tileScrpt4 = tileFourObj.GetComponent<Tiles>();
        shopScript = shopObj.GetComponent<Shop>();
        journalScrpt = journalObj.GetComponent<GlowButton>();
        transactionScrpt = transactionObj.GetComponent<TransactionManager>();
    }

    private void Update()
    {
        if (tutorialDone == false)
        {
            for (int i = 0; i < popUps.Length; i++)
            {
                if (i == popUpIndex)
                {
                    popUps[i].SetActive(true);
                }
                else
                {
                    popUps[i].SetActive(false);
                }
            }

            // The pop up window to tell the player to walk.
            if (popUpIndex == 0)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
                {
                    popUpIndex++;
                }
            }
            // The pop up window to guide the player to the store.
            else if (popUpIndex == 1)
            {
                // if (atStore == true)
                //{
                //    popUpIndex++;
                //}
                if (Input.GetMouseButtonDown(0))
                {
                    popUpIndex++;
                }

            }
            // The pop up window for the player to buy a grass seed
            else if (popUpIndex == 2)
            {
                storeItems = transactionScrpt.currAmount;
                if (storeItems < transactionScrpt.maxAmount)
                {
                    popUpIndex++;
                }

                //seedSold = shopScript.SoldItems.Count;

                //if (seedSold > 0)
                //{
                //   popUpIndex++;
                //}
            }
            // The pop up window to guide players to a planter
            else if (popUpIndex == 3)
            {
                if (waitTime <= 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        waitTime = 5f;
                        popUpIndex++;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }

                //if (tileScrpt.IsPointerOverUIObject() || tileScrpt2.IsPointerOverUIObject()
                //    || tileScrpt3.IsPointerOverUIObject() || tileScrpt4.IsPointerOverUIObject())
                // {
                //     popUpIndex++;
                //}
            }
            // Tell the player how to change the planter temperature
            else if (popUpIndex == 4)
            {
                temp1 = tileScrpt.CurrentTemperature;
                temp2 = tileScrpt2.CurrentTemperature;
                temp3 = tileScrpt3.CurrentTemperature;
                temp4 = tileScrpt4.CurrentTemperature;
                if (temp1 != 20 || temp2 != 20 || temp3 != 20 || temp4 != 20)
                {
                    popUpIndex++;
                }
            }
            // Check the menu to see what the ideal temperature is. ???
            else if (popUpIndex == 5)
            {
                journalClick = journalScrpt.wasPressed;
                if (journalClick == true)
                {
                    popUpIndex++;
                }
            }
            // Let the player know about plant grades
            else if (popUpIndex == 6)
            {
                if (waitTime <= 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        waitTime = 5f;
                        popUpIndex++;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
            // Explain the drying lab
            else if (popUpIndex == 7)
            {
                if (waitTime <= 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        waitTime = 5f;
                        popUpIndex++;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
            // Say how plant grades interact with drying
            else if (popUpIndex == 8)
            {
                if (waitTime <= 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        waitTime = 5f;
                        popUpIndex++;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
            // Talk maintenance
            else if (popUpIndex == 9)
            {
                if (waitTime <= 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        waitTime = 5f;
                        popUpIndex++;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
            // Enjoy the game
            else if (popUpIndex == 10)
            {
                if (Input.GetMouseButtonDown(0))
                {                
                    popUpIndex++;

                    //tutorialDone = true;
                    
                }
            }
            else if (popUpIndex == 11)
            {
                tutorialDone = true;
            }
        }
    }
}
