using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //public Button Button;

    [Header("Inventory Slot Prefab")]
    public GameObject inventoryItemPrefab;
    Utilities.ObjectPool<GameObject> buttonPool = new Utilities.ObjectPool<GameObject>();
    public enum ListT
    {
        Seeds,
        Plants,
        Herbs,
        Potions,
        Recipes,
        Pets,
        Food,
        All
    }
    ListT currList = ListT.All;

    public enum InventoryUsage
    {
        Normal,
        SelectSeed,
        SelectPlant,
        SelectHerb,
        Selling,
        SelectPotion
    }

    InventoryUsage usage;
    InventoryItem currentButton;

    ItemData testItem;
    [Header("Debug")]
    public List<ItemData> fakeInv = new List<ItemData>();

    //Public
    [Header("Inventory")]
    public Inventory playerInventory;
    [Header("UI")]
    public List<Button> topBarButtons = new List<Button>();
    public List<Text> selectedText = new List<Text>();
    public Text descriptionText = null;
    public Transform content;
    public GameObject EmptySign;
    public GameObject selectionBar;
    public GameObject SellingPriceTag;
    Text SellingPriceTagText;
    [Header("Misc")]
    public TransactionManager transactionManager;
    public InteractionHandler handler;
    public TilePanel tilePanel;

    //Private
    List<GameObject> buttonShowing = new List<GameObject>();

    bool isLock; //if locked, player will not be able to swap tabs

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        if (SellingPriceTag != null)
        {
            SellingPriceTagText = SellingPriceTag.transform.GetComponentInChildren<Text>();
        }
        else
            Debug.Log("Missing Price Tag");
    }
    int count = 0;

    // Update is called once per frame
    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testItem = DatabaseScript.GetItem(count);
            count++;
            ItemData newItem;
            testItem.Copy(out newItem);
            newItem.CurrHeld = count;
            playerInventory.AddItem(newItem);
            fakeInv.Add(newItem);
            UpdateContent();
        }
    }

    void updateInfo()
    {
        Debug.Log("Update");
    }
    /// <summary>
    /// Change the list to set number. If current list equal to the number, it will reset to All
    /// </summary>
    /// <param name="num"></param>
    public void ChangeList(int num)
    {
        if(!isLock)
        {
            if ((int)currList == num)
                currList = ListT.All;
            else
                currList = (ListT)num;
        }
        UpdateSelectionBarButtons();
        UpdateContent();
    }
    /// <summary>
    /// Change the list to set number no matter what. 
    /// </summary>
    /// <param name="num"></param>
    public void HardChangeList(int num)
    {
        currList = (ListT)num;
        UpdateSelectionBarButtons();
        UpdateContent();
    }

    public void ChangeList(ListT listT)
    {
        ChangeList((int)listT);
    }
    public void HardChangeList(ListT listT)
    {
        HardChangeList((int)listT);
    }

    public void setLocked(bool _lock) => isLock = _lock;

    public void setUsage(InventoryUsage usage) => this.usage = usage;

    /// <summary>
    /// Called when any button is pressed in the inventory UI
    /// Called by Inventory Item
    /// </summary>
    /// <param name="itemButton"></param>
    public void ButtonIsClicked(InventoryItem itemButton)
    {
        currentButton = itemButton;

        switch (usage)
        {
            case InventoryUsage.Normal:
                {
                    //Update the description box

                    if(itemButton.ItemData is PetData)
                    {
                        //Release the pet
                        if(playerInventory.pet != null)
                        {
                            if(playerInventory.currentPet != itemButton.ItemData)
                            {
                                playerInventory.pet.petData = itemButton.ItemData as PetData;
                                playerInventory.pet.PetChange();

                            }
                        }
                    }

                    else if (itemButton.ItemData is PotionData)
                    {
                        (itemButton.ItemData as PotionData).Duration *= 60.0f;
                        PotionEffectManager.Instance.ActivePotions.Add(itemButton.ItemData);
                        PotionEffectManager.Instance.ActiveEffects.Add((itemButton.ItemData as PotionData).PotionType);
                        itemButton.ItemData.Price = 0;
                        playerInventory.RemoveItem(itemButton.ItemData);
                    }



                    descriptionText.text = itemButton.ItemData.Description;
                    break;
                }
            case InventoryUsage.SelectSeed:
                {
                    if (tilePanel != null)
                    {
                        if (playerInventory.CheckIfContain(itemButton.ItemData))
                        {
                            handler.targetTile.PlantSeed(itemButton.ItemData);
                            playerInventory.RemoveItem(itemButton.ItemData);
                            tilePanel.refreshTile();
                            UpdateContent();
                            ToggleSelectionBar();
                            gameObject.SetActive(false);
                        }
                        else
                            Debug.Log("Inventory cant find item");
                    }
                    else
                        Debug.Log("No tile panel");
                    break;
                }
            case InventoryUsage.SelectPlant:
                {
                    if (tilePanel != null)
                    {
                        if (playerInventory.CheckIfContain(itemButton.ItemData))
                        {
                            handler.targetTile.ConvertPlant(itemButton.ItemData);
                            playerInventory.RemoveItem(itemButton.ItemData);
                            tilePanel.refreshTile();
                            UpdateContent();
                            ToggleSelectionBar();
                            gameObject.SetActive(false);
                        }
                        else
                            Debug.Log("Inventory cant find item");
                    }
                    else
                        Debug.Log("No tile panel");
                    break;
                }
            case InventoryUsage.SelectHerb:
                {
                    if (tilePanel != null)
                    {
                        if (playerInventory.CheckIfContain(itemButton.ItemData))
                        {
                            PotionMenu pm = FindObjectOfType<PotionMenu>();
                            if(pm != null)
                            {
                                pm.setCurrentPotion(itemButton.ItemData as RecipeData);
                            }
                        }
                        else
                            Debug.Log("Inventory cant find item");
                    }
                    else
                        Debug.Log("No tile panel");
                    break;
                }
            case InventoryUsage.Selling:
                transactionManager.AttemptSell(itemButton.ItemData);
                break;
            case InventoryUsage.SelectPotion:
                {

                    break;
                }
            default:
                Debug.Log("I dont know what to do");
                break;
        }
    }

    /// <summary>
    /// Update the content of the inventory table
    /// </summary>
    public void UpdateContent()
    {
        foreach (var item in selectedText)
        {
            item.text = "";
        }
        descriptionText.text = "";
        //Remove whatever showing in the list;
        foreach (var button in buttonShowing)
        {
            button.SetActive(false);
            button.transform.SetParent(null);
            buttonPool.release(button);
        }
        buttonShowing.Clear();

        //Create the list I need to show
        //TODO : Change to follow the pattern at the bottom

        List<ItemData> printList = new List<ItemData>();

        if (currList == ListT.All)
        {
            for (int i = (int)ListT.Seeds; i < (int)ListT.All; i++)
            {
                foreach (var item in playerInventory.itemDatas[i])
                {
                    printList.Add(item);
                }
            }
        }
        else
        {
            foreach (var item in playerInventory.itemDatas[(int)currList])
            {
                printList.Add(item);
            }
        }

        //Print out all the items
        int itemCount = printList.Count;
        if (itemCount == 0)
            EmptySign.SetActive(true);
        else
        {
            EmptySign.SetActive(false);
            for (int i = 0; i < itemCount; i++)
            {
                ItemData data = printList[i];
                var temp = buttonPool.getNewItem(inventoryItemPrefab);
                InventoryItem item = temp.GetComponent<InventoryItem>();
                item.ItemData = data;
                item.index = i;
                item.activate(true);
                item.SetInventoryUI(this);
                temp.transform.SetParent(content);
                buttonShowing.Add(temp);
            }
        }

        UpdateLayout();
    }

    /// <summary>
    /// Update the buttons on the top selection bars. 
    /// Either highlight the selected and dim the rest,
    /// or highlight all the icon when showing all items
    /// </summary>
    void UpdateSelectionBarButtons()
    {
        int cur = (int)currList;

        switch (currList)
        {
            case ListT.All:
                {
                    for (int i = 0; i < topBarButtons.Count; i++)
                    {
                        //topBarButtons[i].Select();
                        Image selectedImage = topBarButtons[i].gameObject.GetComponent<Image>();
                        if (selectedImage != null)
                        {
                            selectedImage.color = Color.white;

                            Image childImage = selectedImage.transform.GetChild(0).gameObject.GetComponent<Image>();
                            if (childImage != null)
                            {
                                childImage.color = Color.white;
                            }

                        }
                    }
                    break;
                }
            case ListT.Seeds:
            case ListT.Plants:
            case ListT.Herbs:
            case ListT.Potions:
            case ListT.Recipes:
            case ListT.Pets:
            case ListT.Food:
                {
                    if (cur < topBarButtons.Count)
                    {
                        for (int i = 0; i < topBarButtons.Count; i++)
                        {
                            if (i == cur)
                            {
                                topBarButtons[i].Select();
                                Image selectedImage = topBarButtons[i].gameObject.GetComponent<Image>();
                                if (selectedImage != null)
                                {
                                    selectedImage.color = Color.white;

                                    Image childImage = selectedImage.transform.GetChild(0).gameObject.GetComponent<Image>();
                                    if (childImage != null)
                                    {
                                        childImage.color = Color.white;
                                    }

                                }
                            }
                            else
                            {
                                Color transparent = new Color(1, 1, 1, 0.25f);
                                Image selectedImage = topBarButtons[i].gameObject.GetComponent<Image>();
                                if (selectedImage != null)
                                {
                                    selectedImage.color = transparent;

                                    Image childImage = selectedImage.transform.GetChild(0).gameObject.GetComponent<Image>();
                                    if (childImage != null)
                                    {
                                        childImage.color = transparent;
                                    }

                                }
                            }
                        }
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void PointerEnterTheButton(InventoryItem itemButton)
    {
        ItemData theItem = itemButton.ItemData;
        foreach (var item in selectedText)
        {
            item.text = theItem.ItemName;
        }


        descriptionText.text = theItem.Description;

        if (usage == InventoryUsage.Selling)
        {
            SellingPriceTagText.text = "Selling Price : " + theItem.Price.ToString();
            if(theItem is PetData)
            {
                SellingPriceTagText.text = "Priceless!";
            }
        }
    }

    //void setFunctions(InventoryItem item)
    //{
    //    item.setFunction(updateInfoBox);
    //    if (usage == InventoryUsage.Selling)
    //        item.setFunction(SellingItem);
    //}

    bool updateInfoBox(int index, ItemData data)
    {
        if (index < 0 || index >= buttonShowing.Count)
            return false;

        foreach (var item in selectedText)
        {
            item.text = data.ItemName;
        }
        descriptionText.text = data.Description;
        return true;
    }

    public void ToggleSelectionBar(bool b = true)
    {
        selectionBar.SetActive(b);
    }

    public void UpdateLayout()
    {
        switch (usage)
        {
            case InventoryUsage.Selling:
                {
                    SellingPriceTag.SetActive(true);
                    SellingPriceTagText.text = "";
                    break;
                }
            default:
                {
                    SellingPriceTag.SetActive(false);
                }
                break;
        }
    }

    ///System design
    ///
    ///Each item in the inventory will be created as single button(Icon) in the Viewport
    ///When add new unique item into inventory, add the icon
    ///When remove new unique item, remove icon as well
    ///When showing the list of item, simply set active or inactive the icon
    ///It will save time on changing UI
}
