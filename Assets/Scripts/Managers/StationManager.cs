using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TJayEnums;
/// <summary>
/// This class will be updated with any variables the station needs to keep track of.
/// </summary>
/// 
namespace TJayEnums
{

    public enum PlayerMenu
    {
        None,
        GenericInventory,
        GenericMenu,
        SelectSeed,
        SelectPlant,
        SelectHerb,
        SelectPotion,
        Buying,
        Selling,
        SelectRecipe,
        ConfirmBuy,
        FinishDeal,
        ConfirmSell,
        FinishSale,
        ConfirmGeneric,
        FinishGeneric,
        ViewingJournal

    }

    public enum ListType
    {
        Seeds,
        Plants,
        Herbs,
        Potions,
        Recipes,
        Pets,
        PetFoods,
        Tasks
    }

    public enum UnlockType
    {
        item,
        plot,
        lab,
        magic,
        task
    }

    public enum TaskType
    {
        buy,
        sell,
        plant,
        lab,
        magic,
        harvest,
        temperature,
        ownPet,
        usePet,
        getSeedFromPet

    }

    public enum Biome
    {
        unknown,
        desert,
        jungle,
        forrest,
        ocean,
        cave,
        artic
    }

    public enum Genus
    {
        unknown = -1,
        grass,
        flower,
        vine,
        herb,
        fungi,
        moss
    }

    public enum PotionType
    {
        unknown,
        specificMulti,
        genericMulti,
        accelGrass,
        accelFlower,
        accelVine,
        accelHerb,
        accelFungi,
        accelMoss
    }

}
public class StationManager : MonoBehaviour
{
    private float stationTemperature = 5.0f;
    public Text tempText;
    public GameObject tempPanel = null;
    public GameObject pausePanel = null;
    public GameObject inventoryPanel = null;
    public GameObject journalPanel = null;
    public GameObject storePanel = null;
    public Shop actualShop = null;
    public Tiles selectedTile = null;

    public BuySellButtons displayText;
    public BuySellButtons yesPrompt;
    public BuySellButtons noPrompt;
    public BuySellButtons amountToBuyObject;
    public int howMany = 0;
    public RowItem currentItem = null;
    public PlayerMenu currentMenu = PlayerMenu.None;
    public Button selectedButton;
    public Journal playerJournal;
    public Inventory playerInventory;
    public MenuSettings menuSettings = null;

    public InventoryUI InventoryUI;
    private void Awake()
    {
        if (tempText != null)
        {
            tempText.text = TEMPERATURE.ToString();
        }

        if (tempPanel != null)
        {
            tempPanel.gameObject.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.gameObject.SetActive(false);
        }
        if (playerJournal == null)
        {
            playerJournal = GameObject.FindObjectOfType<Journal>();
        }
        if (playerInventory == null)
        {
            playerInventory = GameObject.FindObjectOfType<Inventory>();
        }
        if (menuSettings == null)
        {
            menuSettings = GameObject.FindObjectOfType<MenuSettings>();
        }

        if (inventoryPanel != null)
            InventoryUI = inventoryPanel.GetComponent<InventoryUI>();
        else
            Debug.Log("No Inventory Panel!");
    }

    public void SetCurrentMenu(int newMenu)
    {
        currentMenu = (PlayerMenu)newMenu;
    }

    public float TEMPERATURE
    {
        get { return stationTemperature; }
        set { stationTemperature = value; }
    }

    public void IncreaseTemperature(int amount = 1)
    {
        if (selectedTile != null)
        {
            selectedTile.CurrentTemperature += amount;
            TEMPERATURE = selectedTile.CurrentTemperature;
        }
        if (tempText != null)
        {
            tempText.text = TEMPERATURE.ToString();
        }
    }

    public void DecreaseTemperature(int amount = 1)
    {
        if (selectedTile != null)
        {
            selectedTile.CurrentTemperature -= amount;
            TEMPERATURE = selectedTile.CurrentTemperature;
        }
        if (tempText != null)
        {
            tempText.text = TEMPERATURE.ToString();
        }
    }

    public void ToggleTemperaturePanel()
    {
        if (tempPanel != null)
        {
            tempPanel.SetActive(!tempPanel.activeInHierarchy);

            if (tempPanel.activeInHierarchy == true)
            {
                if (inventoryPanel != null)
                {
                    if (inventoryPanel.activeInHierarchy == true)
                    {
                        inventoryPanel.SetActive(false);
                    }
                }
                if (menuSettings != null)
                    menuSettings.PlayOpenSound();
            }
            else
            {

                if (menuSettings != null)
                    menuSettings.PlayCloseSound();
            }
        }
    }

    public void ToggleStorePanel()
    {
        if (journalPanel.activeInHierarchy == true)
            journalPanel.SetActive(false);
        if (storePanel != null)
        {
            storePanel.SetActive(!storePanel.activeInHierarchy);
            if (storePanel.activeInHierarchy == true)
            {
                currentMenu = PlayerMenu.Buying;
                if (menuSettings != null)
                    menuSettings.PlayOpenSound();
            }
            else
            {
                currentMenu = PlayerMenu.None;
                CloseConfirmCancelInShop();
                if (menuSettings != null)
                    menuSettings.PlayCloseSound();
            }
            if (actualShop != null)
            {
                actualShop.ChangeSelectedList(0);
                actualShop.UpdateContent();
            }

            if (inventoryPanel != null)
            {
                if (inventoryPanel.activeInHierarchy == true)
                    inventoryPanel.SetActive(false);
            }
        }
    }

    public void TogglePausePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(!pausePanel.activeInHierarchy);

        }
        if (inventoryPanel.activeInHierarchy == true)
            inventoryPanel.SetActive(false);
        if (journalPanel.activeInHierarchy == true)
            journalPanel.SetActive(false);
        if (storePanel.activeInHierarchy == true)
        {
            ToggleStorePanel();
        }

        if (pausePanel.activeInHierarchy == true)
        {

            if (menuSettings != null)
                menuSettings.PlayOpenSound();
        }
        else
        {

            if (menuSettings != null)
                menuSettings.PlayCloseSound();
        }
    }
    public void ToggleJournalPanel()
    {
        if (storePanel != null)
        {
            if (storePanel.activeInHierarchy == true)
            {
                ToggleStorePanel();
            }
        }
        if (inventoryPanel != null)
        {
            if (inventoryPanel.activeInHierarchy == true)
                inventoryPanel.SetActive(false);

            journalPanel.SetActive(!journalPanel.activeInHierarchy);
            if (journalPanel.activeInHierarchy == true)
            {
                currentMenu = PlayerMenu.ViewingJournal;
                if (playerInventory != null && playerJournal != null)
                {
                    playerJournal.ForceUpdateContent((int)ListType.Tasks);
                }
                if (menuSettings != null)
                    menuSettings.PlayOpenSound();
            }
            else
            {
                currentMenu = PlayerMenu.None;
                if (menuSettings != null)
                    menuSettings.PlayCloseSound();
            }
        }

        if (tempPanel != null)
        {
            tempPanel.SetActive(false);

        }

    }
    public void ToggleSellPanel()
    {
        if (storePanel != null)
        {
            if (storePanel.activeInHierarchy == true)
            {
                ToggleStorePanel();
            }
        }
        if (journalPanel.activeInHierarchy == true)
            journalPanel.SetActive(false);
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
            if (inventoryPanel.activeInHierarchy == true)
            {
                currentMenu = PlayerMenu.Selling;
                if (playerInventory.currentList == ListType.Tasks)
                    playerInventory.currentList = ListType.Seeds;
                //playerInventory.ChangeSelectedList((int)playerInventory.currentList);
                InventoryUI.setUsage(InventoryUI.InventoryUsage.Selling);
                InventoryUI.ChangeList(InventoryUI.ListT.All);
                InventoryUI.ToggleSelectionBar();
                InventoryUI.UpdateContent();
                if (menuSettings != null)
                    menuSettings.PlayOpenSound();
            }
            else
            {
                InventoryUI.setUsage(InventoryUI.InventoryUsage.Normal);
                currentMenu = PlayerMenu.None;
                if (menuSettings != null)
                    menuSettings.PlayCloseSound();
            }
        }
        if (tempPanel != null)
        {
            tempPanel.SetActive(false);

        }

    }
    public void ToggleInventoryPanel()
    {
        if (storePanel != null)
        {
            if (storePanel.activeInHierarchy == true)
            {
                ToggleStorePanel();
            }
        }
        if (playerInventory != null)
        {

            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
                if (inventoryPanel.activeInHierarchy == true)
                {
                    currentMenu = PlayerMenu.GenericInventory;
                    //playerInventory.ChangeSelectedList((int)ListType.Seeds);
                    InventoryUI.setUsage(InventoryUI.InventoryUsage.Normal);
                    InventoryUI.HardChangeList(InventoryUI.ListT.All);
                    InventoryUI.ToggleSelectionBar();

                    if (menuSettings != null)
                        menuSettings.PlayOpenSound();
                }
                else
                {
                    currentMenu = PlayerMenu.None;

                    if (menuSettings != null)
                        menuSettings.PlayCloseSound();
                }

            }
        }
        if (journalPanel.activeInHierarchy == true)
        {
            journalPanel.SetActive(false);

        }
        if (tempPanel != null)
        {
            tempPanel.SetActive(false);

        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTemperaturePanel();
        }
    }

    public void TogglePanel(GameObject somePanel)
    {
        if (somePanel != null)
        {
            somePanel.SetActive(!somePanel.activeInHierarchy);

            if (somePanel.activeInHierarchy == true)
            {

                if (menuSettings != null)
                    menuSettings.PlayOpenSound();
            }
            else
            {

                if (menuSettings != null)
                    menuSettings.PlayCloseSound();
            }
        }
    }

    /* 
    public void AttemptBuy(RowItem someItem)
    {
        currentItem = someItem;
        if (currentItem != null)
        {

            if (displayText != null)
            {
                displayText.MakeActive();
                howMany = 1;
                string dataName = "";
                string finalString = "So you wanted to buy a ";

                if (currentItem.referenceData != null)
                {
                    dataName = currentItem.referenceData.Name;
                    finalString += "" + dataName + "? \nHow many would you like?";
                    if (amountToBuyObject != null)
                    {
                        amountToBuyObject.MakeActive();
                        amountToBuyObject.SetText("1");
                    }
                }
                else if (currentItem.referencePet != null)
                {
                    dataName = currentItem.referencePet.Name;
                    finalString += "" + dataName + "?";
                    if (amountToBuyObject != null)
                    {
                        amountToBuyObject.gameObject.SetActive(false);
                    }
                }
                else if (currentItem.referencePetFood != null)
                {
                    dataName = currentItem.referencePetFood.Name;
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


            SetCurrentMenu((int)PlayerMenu.ConfirmBuy);

        }

        if (menuSettings != null)
            menuSettings.PlayOpenSound();
    }

    public void AttemptSell(RowItem someItem)
    {
        currentItem = someItem;
        if (currentItem != null)
        {
            if (displayText != null)
            {
                displayText.MakeActive();
                howMany = 1;
                string dataName = "";
                string finalString = "So you wanted to sell your ";

                if (currentItem.referenceData != null)
                {
                    dataName = currentItem.referenceData.Name;
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


            SetCurrentMenu((int)PlayerMenu.ConfirmSell);

        }

        if (menuSettings != null)
            menuSettings.PlayCloseSound();
    }

    public void AttemptSell(ItemData itemData)
    {
        if (itemData != null)
        {
            if (displayText != null)
            {
                displayText.MakeActive();
                howMany = 1;
                string dataName = "";
                string finalString = "So you wanted to sell your ";

                if (itemData != null)
                {
                    dataName = itemData.Name;
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


            SetCurrentMenu((int)PlayerMenu.ConfirmSell);

        }

        if (menuSettings != null)
            menuSettings.PlayCloseSound();
    }

    public void BuyConfrim()
    {
        if (currentItem != null)
        {
            if (currentItem.referenceData != null)
            {
                if (currentMenu == PlayerMenu.ConfirmBuy)
                {
                    if (displayText != null)
                    {
                        displayText.SetText("So you want to buy " + howMany + " " + currentItem.referenceData.Name + "? \n That will cost " + (currentItem.referenceData.Price * howMany) + ". Is that ok?");
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

                    SetCurrentMenu((int)PlayerMenu.FinishDeal);
                }
                else if (currentItem.referencePet != null)
                {
                    currentItem.BuyMe();
                    currentItem = null;
                    CloseConfirmCancelInShop();
                }
                else if (currentMenu == PlayerMenu.FinishDeal)
                {
                    currentItem.BuyMe(howMany);
                    currentItem = null;
                    CloseConfirmCancelInShop();
                }
                else if (currentMenu == PlayerMenu.ConfirmSell)
                {
                    if (displayText != null)
                    {
                        displayText.SetText("So you want to sell " + howMany + " " + currentItem.referenceData.Name + "? \nI'll pay you " + (currentItem.referenceData.Price * howMany) + ". Is that ok?");
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

                    SetCurrentMenu((int)PlayerMenu.FinishSale);
                }
                else if (currentMenu == PlayerMenu.FinishSale)
                {
                    currentItem.SellMe(howMany);
                    currentItem = null;
                    CloseConfirmCancelInShop();
                    playerInventory.ChangeSelectedList((int)playerInventory.currentList);
                    SetCurrentMenu((int)PlayerMenu.Selling);

                }
            }
            else if (currentItem.referencePet != null)
            {
                currentItem.BuyMe();
                currentItem = null;
                CloseConfirmCancelInShop();
            }
            else if (currentItem.referencePetFood != null)
            {

                if (currentMenu == PlayerMenu.ConfirmBuy)
                {
                    if (displayText != null)
                    {
                        displayText.SetText("So you want to buy " + howMany + " " + currentItem.referencePetFood.Name + "? \n That will cost " + (currentItem.referencePetFood.Price * howMany) + ". Is that ok?");
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

                    SetCurrentMenu((int)PlayerMenu.FinishDeal);
                }
                else if (currentItem.referencePet != null)
                {
                    currentItem.BuyMe();
                    currentItem = null;
                    CloseConfirmCancelInShop();
                }
                else if (currentMenu == PlayerMenu.FinishDeal)
                {
                    currentItem.BuyMe(howMany);
                    currentItem = null;
                    CloseConfirmCancelInShop();
                }
                else if (currentMenu == PlayerMenu.ConfirmSell)
                {
                    if (displayText != null)
                    {
                        displayText.SetText("So you want to sell " + howMany + " " + currentItem.referencePetFood.Name + "? \nI'll pay you " + (currentItem.referencePetFood.Price * howMany) + ". Is that ok?");
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

                    SetCurrentMenu((int)PlayerMenu.FinishSale);
                }
                else if (currentMenu == PlayerMenu.FinishSale)
                {
                    currentItem.SellMe(howMany);
                    currentItem = null;
                    CloseConfirmCancelInShop();
                    playerInventory.ChangeSelectedList((int)playerInventory.currentList);
                    SetCurrentMenu((int)PlayerMenu.Selling);

                }

            }

        }

        if (menuSettings != null)
            menuSettings.PlayOpenSound();
    }
    public void NoCancel()
    {
        Debug.Log("Check");
        if (currentMenu == PlayerMenu.ConfirmBuy)
        {
            currentItem = null;
            CloseConfirmCancelInShop();
        }
        else if (currentMenu == PlayerMenu.FinishDeal)
        {
            AttemptBuy(currentItem);
        }
        else if (currentMenu == PlayerMenu.ConfirmSell)
        {
            currentItem = null;
            CloseConfirmCancelInShop();
        }
        else if (currentMenu == PlayerMenu.FinishSale)
        {
            AttemptSell(currentItem);
        }
        if (menuSettings != null)
            menuSettings.PlayCloseSound();
    }
    public void IncreaseAmountBuy()
    {
        if (currentItem != null)
        {
            if (currentItem.referenceData != null)
            {
                if (currentMenu == PlayerMenu.ConfirmBuy)
                {

                    int price = 0;
                    price = currentItem.referenceData.Price;

                    if ((howMany + 1) * price <= currentItem.myInventory.Coins && (howMany + 1) + currentItem.referenceData.CurrHeld <= currentItem.referenceData.MaxHeld)
                    {
                        howMany++;
                        amountToBuyObject.SetText("" + howMany);
                    }
                }
                else if (currentMenu == PlayerMenu.ConfirmSell)
                {
                    if ((howMany + 1) <= currentItem.referenceData.CurrHeld)
                    {
                        howMany++;
                        amountToBuyObject.SetText("" + howMany);
                    }
                }
            }
            else if (currentItem.referencePetFood != null)
            {
                if (currentMenu == PlayerMenu.ConfirmBuy)
                {

                    int price = 0;
                    price = currentItem.referencePetFood.Price;

                    if ((howMany + 1) * price <= currentItem.myInventory.Coins && (howMany + 1) + currentItem.referencePetFood.CurrHeld <= currentItem.referencePetFood.MaxHeld)
                    {
                        howMany++;
                        amountToBuyObject.SetText("" + howMany);
                    }
                }
                else if (currentMenu == PlayerMenu.ConfirmSell)
                {
                    if ((howMany + 1) <= currentItem.referencePetFood.CurrHeld)
                    {
                        howMany++;
                        amountToBuyObject.SetText("" + howMany);
                    }
                }
            }
        }
    }


    public void DecreaseAmountBuy()
    {

        if (howMany > 1)
        {
            howMany--;
            amountToBuyObject.SetText("" + howMany);

        }
    }
    */
    private void CloseConfirmCancelInShop()
    {
        if (actualShop != null)
        {

            actualShop.ChangeSelectedList(-1);
            if (amountToBuyObject != null)
            {
                amountToBuyObject.gameObject.SetActive(false);
            }
            if (displayText != null)
            {
                displayText.gameObject.SetActive(false);
            }

            if (yesPrompt != null)
            {
                yesPrompt.gameObject.SetActive(false);
            }

            if (noPrompt != null)
            {
                noPrompt.gameObject.SetActive(false);
            }
        }
    }


}

