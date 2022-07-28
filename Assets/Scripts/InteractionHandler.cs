using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TJayEnums;
public class InteractionHandler : MonoBehaviour
{
    public Tiles targetTile = null;
    public ItemData targetData = null;
    public UnityEngine.UI.Text displayText = null;
    public StationManager station = null;
    public Inventory playerInventory = null;
    private void Start()
    {
        if (station == null)
        {
            station = GameObject.FindObjectOfType<StationManager>();
        }
        if (playerInventory == null)
        {
            playerInventory = GameObject.FindObjectOfType<Inventory>();
        }
        gameObject.SetActive(false);
    }

    public void LoadData(Tiles newTile)
    {
        //  targetData = newData;
        targetTile = newTile;
        if (displayText != null)
        {
            switch (newTile.type)
            {
                case Tiles.TileType.greenHouse:
                    {
                        displayText.text = "Plant";
                    }
                    break;
                case Tiles.TileType.lab:
                    {

                        displayText.text = "Lab";
                    }
                    break;
                case Tiles.TileType.magicStore:
                    {
                        displayText.text = "Convert";
                    }
                    break;

            }
        }

        if (station)
        {
            station.selectedTile = newTile;
            station.TEMPERATURE = newTile.CurrentTemperature;
            station.tempText.text = "" + station.TEMPERATURE;

            if (station.menuSettings != null)
                station.menuSettings.PlayOpenSound();

        }


    }

    public void LoadInventoryList()
    {
        if (targetTile != null && playerInventory != null && station != null)
        {
            if (playerInventory.targetPanel.activeInHierarchy == false || station.currentMenu == PlayerMenu.GenericInventory)
            {
                
                switch (targetTile.type)
                {
                    case Tiles.TileType.greenHouse:
                        {
                            playerInventory.targetPanel.SetActive(true);
                            //station.currentMenu = PlayerMenu.SelectSeed;
                            //playerInventory.ChangeSelectedList((int)ListType.Seeds);
                            //playerInventory.UpdateContent(true);
                            playerInventory.inventoryUI.setUsage(InventoryUI.InventoryUsage.SelectSeed);
                            playerInventory.inventoryUI.HardChangeList(InventoryUI.ListT.Seeds);
                            playerInventory.inventoryUI.UpdateContent();
                            playerInventory.inventoryUI.ToggleSelectionBar(false);
                        }
                        break;
                    case Tiles.TileType.lab:
                        {
                            playerInventory.targetPanel.SetActive(true);
                            //station.currentMenu = PlayerMenu.SelectPlant;
                            //playerInventory.inventoryUI.setUsage(InventoryUI.InventoryUsage.SelectPlant);
                            //playerInventory.ChangeSelectedList((int)ListType.Plants);
                            //playerInventory.UpdateContent(true);

                            playerInventory.inventoryUI.setUsage(InventoryUI.InventoryUsage.SelectPlant);
                            playerInventory.inventoryUI.HardChangeList(InventoryUI.ListT.Plants);
                            playerInventory.inventoryUI.UpdateContent();
                            playerInventory.inventoryUI.ToggleSelectionBar(false);
                        }
                        break;
                    case Tiles.TileType.magicStore:
                        {
                            //station.currentMenu = PlayerMenu.SelectHerb;
                            //playerInventory.inventoryUI.setUsage(InventoryUI.InventoryUsage.SelectHerb);
                            //playerInventory.ChangeSelectedList((int)ListType.Recipes);
                            //playerInventory.UpdateContent(true);

                            //I Dont need these
                            //playerInventory.inventoryUI.setUsage(InventoryUI.InventoryUsage.SelectHerb);
                            //playerInventory.inventoryUI.HardChangeList(InventoryUI.ListT.Recipes);
                            //playerInventory.inventoryUI.UpdateContent();
                            //playerInventory.inventoryUI.ToggleSelectionBar(false);

                            playerInventory.inventoryUI.setUsage(InventoryUI.InventoryUsage.SelectPotion);
                            PotionMenu pm = FindObjectOfType<PotionMenu>();
                            if (pm != null)
                            {
                                pm.StartMenu();
                            }
                        }
                        break;
                }
            }
            else
            {
                playerInventory.InvButPress();
            }
        }

        if(station != null)
        {
            if (station.tempPanel.activeInHierarchy == true)
                station.ToggleTemperaturePanel();

            if (station.menuSettings != null)
                station.menuSettings.PlayOpenSound();
     
        }
    }

    public void SetInactive()
    {
        if (station)
        {
            if (station.storePanel != null)
            {
                //if (station.storePanel.gameObject.activeInHierarchy == true)
                    station.storePanel.gameObject.SetActive(false);
            }
        }
        if (station != null)
        {
    
            if (station.menuSettings != null)
                station.menuSettings.PlayCloseSound();

        }
        gameObject.SetActive(false);
    }
}
