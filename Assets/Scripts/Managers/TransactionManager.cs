using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TJayEnums;

public class TransactionManager : MonoBehaviour
{
    public Inventory inventory;
    public InventoryUI inventoryUI;
    ItemData targetItem;
    public int maxAmount;
    public int currAmount;

    [Header("UI")]
    public BuySellButtons displayText;
    public BuySellButtons yesPrompt;
    public BuySellButtons noPrompt;
    public BuySellButtons amountToBuyObject;

    Journal journal;
    Shop shop;


    enum TransationType
    {
        None, Buy, Sell, ConfirmBuy, ConfirmSell
    }

    TransationType transationType = TransationType.None;


    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null) inventory = FindObjectOfType<Inventory>();
        if (inventoryUI == null) inventoryUI = FindObjectOfType<InventoryUI>();
    }
    /// <summary>
    /// Check how many item I can buy with current hold and current gold
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int CheckMaxBuy(ItemData item)
    {
        ItemData searchItem = inventory.GetItemData(item);
        int currGold = inventory.Coins;

        if (searchItem == null) // Cant find same item in the inventory
        {
            return Mathf.Min(item.MaxHeld, currGold / item.Price);
        }
        else
        {
            return Mathf.Min(searchItem.MaxHeld - searchItem.CurrHeld, currGold / item.Price);
        }
    }
    /// <summary>
    /// Check how many item I can sell in inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int CheckMaxSell(ItemData item)
    {
        ItemData searchItem = inventory.GetItemData(item);

        if (searchItem == null) // Cant find same item in the inventory
        {
            Debug.Log("Cant find the item in inventory");
            return 0;
        }
        else
        {
            return searchItem.CurrHeld;
        }
    }

    public void CheckMax(bool isBuying)
    {
        if (isBuying)
        {

        }
    }

    public void SetTargetItem(ItemData item)
    {
        targetItem = item;
    }

    public void AttemptBuy(ItemData itemData)
    {
        if (itemData != null)
        {
            if (displayText != null)
            {
                displayText.MakeActive();
                currAmount = 1;
                string dataName = "";
                string finalString = "So you wanted to buy a ";

                if (itemData != null)
                {
                    dataName = itemData.ItemName;
                    finalString += "" + dataName + "? \nHow many would you like?";
                    if (amountToBuyObject != null)
                    {
                        amountToBuyObject.MakeActive();
                        amountToBuyObject.SetText("1");
                    }
                }

                displayText.SetText(finalString);
            }

            if (yesPrompt != null)
            {
                yesPrompt.MakeActive();
                yesPrompt.SetText("Buy");
            }

            if (noPrompt != null)
            {
                noPrompt.MakeActive();
                noPrompt.SetText("Cancel");
            }

            maxAmount = CheckMaxBuy(itemData);
            SetTargetItem(itemData);

            transationType = TransationType.Buy;
        }

        //Play Sound

        //if (menuSettings != null)
        //    menuSettings.PlayCloseSound();
    }

    public void AttemptSell(ItemData itemData)
    {
        if (itemData != null)
        {
            if (displayText != null)
            {
                displayText.MakeActive();
                currAmount = 1;
                string dataName = "";
                string finalString = "So you wanted to sell your ";

                if (itemData != null)
                {
                    dataName = itemData.ItemName;
                    finalString += "" + dataName + "? \nHow many would you like?";
                    if (amountToBuyObject != null)
                    {
                        amountToBuyObject.MakeActive();
                        amountToBuyObject.SetText("1");
                    }
                }

                displayText.SetText(finalString);
            }

            if (yesPrompt != null)
            {
                yesPrompt.MakeActive();
                yesPrompt.SetText("Sell");
            }

            if (noPrompt != null)
            {
                noPrompt.MakeActive();
                noPrompt.SetText("Cancel");
            }

            maxAmount = CheckMaxSell(itemData);
            SetTargetItem(itemData);

            transationType = TransationType.Sell;
        }

        //Play Sound

        //if (menuSettings != null)
        //    menuSettings.PlayCloseSound();
    }

    public void ConfirmTransaction()
    {
        switch (transationType)
        {
            case TransationType.None:
                Debug.Log("Error Type");
                break;
            case TransationType.Buy:
                {
                    if (displayText != null)
                    {
                        displayText.SetText("So you want to buy " + currAmount + " " + targetItem.ItemName + "? \n " +
                            "That will cost " + (targetItem.Price * currAmount) + ". Is that ok?");
                    }

                    if (amountToBuyObject != null)
                    {
                        amountToBuyObject.gameObject.SetActive(false);
                    }

                    if (yesPrompt != null)
                    {
                        yesPrompt.SetText("Yes");
                    }

                    if (noPrompt != null)
                    {
                        noPrompt.SetText("No");
                    }

                    transationType = TransationType.ConfirmBuy;
                    break;
                }
            case TransationType.Sell:
                {
                    if (displayText != null)
                    {
                        if(targetItem is PetData)
                        {
                            displayText.SetText("You cannot sell your pet!");
                        }
                        else
                        {
                            displayText.SetText("So you want to sell " + currAmount + " " + targetItem.ItemName + "? \n" +
                            "I'll pay you " + (targetItem.Price * currAmount) + ". Is that ok?");
                        }
                    }
                        

                    if (amountToBuyObject != null)
                    {
                        amountToBuyObject.gameObject.SetActive(false);
                    }

                    if (yesPrompt != null)
                    {
                        yesPrompt.SetText("Yes");
                    }

                    if (noPrompt != null)
                    {
                        noPrompt.SetText("No");
                    }

                    transationType = TransationType.ConfirmSell;
                    break;
                }
            case TransationType.ConfirmBuy:
                {
                    ConfirmTransaction_Buy(targetItem, currAmount);
                    transationType = TransationType.None;
                    break;
                }
            case TransationType.ConfirmSell:
                {
                    ConfirmTransaction_Sell(targetItem, currAmount);
                    transationType = TransationType.None;
                    break;
                }
            default:
                Debug.Log("Error Type");
                break;
        }

    }

    public void CancelTransaction()
    {
        targetItem = null;
        transationType = TransationType.None;
        currAmount = -1;
        amountToBuyObject.gameObject.SetActive(false);
        displayText.gameObject.SetActive(false);
        yesPrompt.gameObject.SetActive(false);
        noPrompt.gameObject.SetActive(false);
    }

    void FinishTransaction()
    {
        //Play sound
        CancelTransaction();
    }

    public void Increment()
    {
        if (currAmount < maxAmount) currAmount++;
        amountToBuyObject.SetText(currAmount.ToString());
    }

    public void Decrement()
    {
        if (currAmount > 1) currAmount--;
        amountToBuyObject.SetText(currAmount.ToString());
    }

    void ConfirmTransaction_Buy(ItemData item, int amount = 1)
    {
        if (inventory)
        {
            int sum = item.Price * amount;
            inventory.CoinUpdate(-sum);

            if (item is PetData)
            {
                inventory.AddPet(item as PetData);
                if (shop == null)
                    shop = FindObjectOfType<Shop>();
                shop.SoldPet(item as PetData);
            }
            else if (item is PetFood) inventory.AddFood(item as PetFood, amount);
            else inventory.AddItem(item, amount);
            updateTasks();
            inventoryUI.UpdateContent();
            FinishTransaction();

            

        }
        else
            Debug.LogError("No inventory attached");

    }

    void ConfirmTransaction_Sell(ItemData item, int amount = 1)
    {
        if (inventory)
        {
            int sum = item.Price * amount;
            if(item is PlantData || item is DryHerbData)
            {

            }
            //inventory.CoinUpdate(sum);

            //if the item is removed, we add the coins to inventory
            bool isRemoved = inventory.RemoveItem(item, amount);
            if (isRemoved)
            {
                inventory.CoinUpdate(sum);
            }


            updateTasks(TaskType.sell);
            inventoryUI.UpdateContent();
            HandleDelegatesAfterTransaction(TransationType.ConfirmSell, item, amount);
            FinishTransaction();
        }
        else
            Debug.LogError("No inventory attached");
    }

    void updateTasks(TaskType task = TaskType.buy)
    {
        if (journal == null)
        {
            journal = GameObject.FindObjectOfType<Journal>();
        }

        for (int i = 0; i < inventory.taskObjectives.Count; i++)
        {
            ItemData someTask = inventory.taskObjectives[i];

            TaskType ttype = (someTask as TaskData).TaskType;
            if (task == ttype)
            {

                switch (ttype)
                {
                    case TaskType.buy:
                        {

                            //currently storing item id for tasks in ideal temperature
                            //checking to make sure player bought the specified item
                            if (targetItem != null)
                            {

                                if ((someTask as TaskData).IDForPlant == targetItem.ID)
                                {
                                    //evolution id being used to show wether or not a task has been completed
                                    (someTask as TaskData).isFinished = true;
                                    if (journal != null)
                                    {
                                        //notify the player
                                        journal.Pulse();

                                    }
                                }
                            }
                        }
                        break;
                    case TaskType.sell:
                        {
                            if ((someTask as TaskData).IDForPlant == targetItem.ID)
                            {
                                //evolution id being used to show wether or not a task has been completed
                                (someTask as TaskData).isFinished = true;
                                if (journal != null)
                                {
                                    //notify the player
                                    journal.Pulse();

                                }
                            }
                        }
                        break;
                    /*
                    case TaskType.plant:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (journal != null)
                            {
                                //notify the player
                                journal.Pulse();

                            }
                        }
                        break;
                    case TaskType.lab:
                        {

                            //currently storing item id for tasks in ideal temperature

                            if ((someTask as TaskData).IDForPlant == referenceData.ID)
                            {

                                (someTask as TaskData).isFinished = true;
                                if (journal != null)
                                {
                                    //notify the player
                                    journal.Pulse();

                                }
                            }
                        }
                        break;
                    case TaskType.magic:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (journal != null)
                            {
                                //notify the player
                                journal.Pulse();

                            }
                        }
                        break;
                    case TaskType.harvest:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (journal != null)
                            {
                                //notify the player
                                journal.Pulse();

                            }
                        }
                        break;
                    case TaskType.temperature:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (journal != null)
                            {
                                //notify the player
                                journal.Pulse();

                            }
                        }
                        break;
                    case TaskType.ownPet:
                        {
                            if (inventory.itemDatas[(int)ListType.Pets].Count > 0)
                            {
                                (someTask as TaskData).isFinished = true;
                                if (journal != null)
                                {
                                    //notify the player
                                    journal.Pulse();

                                }
                            }
                        }
                        break;
                    case TaskType.usePet:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (journal != null)
                            {
                                //notify the player
                                journal.Pulse();

                            }
                        }
                        break;
                    case TaskType.getSeedFromPet:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (journal != null)
                            {
                                //notify the player
                                journal.Pulse();

                            }
                        }
                        break;
                    */
                    default:
                        break;
                }
            }
        }


    }


    //handle delegate when player selling stuffs
    //when buying? not sure will include in this script or Inventory.cs
    //because need more info
    private void HandleDelegatesAfterTransaction(TransationType transaction, ItemData i_data, int amount)
    {
        //SELLING STUFF
        if(transaction == TransationType.ConfirmSell)
        {
            if(i_data.ItemType == ItemData.Type.Plant && QuestTracker.SellPlant != null)
            {
                QuestTracker.SellPlant(amount);
            }
            if (i_data.ItemType == ItemData.Type.DryHerb && QuestTracker.SellDryHerb != null)
            {
                QuestTracker.SellDryHerb(amount);
            }
            if (i_data.ItemType == ItemData.Type.Potion && QuestTracker.SellPotion != null)
            {
                QuestTracker.SellPotion(amount);
            }

        }
    }
}
