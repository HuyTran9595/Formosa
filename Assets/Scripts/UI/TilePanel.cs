using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TilePanel : MonoBehaviour
{
    Tiles currentTile = null;

    //Handler
    GameObject HandlerButton;
    InteractionHandler handler;

    //PlantPanel
    GameObject plantPanel;
    Text plantName;
    Text RemainTime;
    Image plantIcon;

    //Temp
    GameObject tempPanel;
    Text currTemp;
    GameObject tempBut;

    // Start is called before the first frame update
    void Start()
    {
        HandlerButton = transform.Find("InteractButton").gameObject;
        handler = HandlerButton.GetComponent<InteractionHandler>();

        plantPanel = transform.Find("Plant Info").gameObject;
        plantName = plantPanel.transform.Find("Plant Name").GetComponent<Text>();
        RemainTime = plantPanel.transform.Find("Remain Time").GetComponent<Text>();
        plantIcon = plantPanel.transform.Find("Plant Icon").GetComponent<Image>();

        tempPanel = transform.Find("Temp").gameObject;
        currTemp = tempPanel.transform.Find("Curr Temp").transform.Find("Text").GetComponent<Text>();
        tempBut = tempPanel.transform.Find("temperature button").gameObject;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currTemp.text = currentTile.CurrentTemperature.ToString();
        if(currentTile.currentPlant!=null)
        {
            float remainTime = currentTile.currentPlant.TimeRemain();
            if (remainTime <= 0)
            {
                RemainTime.text = "Ready to Harvest!";
            }
            else
            {
                RemainTime.text = "Remain Time : " + Mathf.CeilToInt(remainTime) + "s";

            }
        }
    }

    public void clickedTile(Tiles tile)
    {
        if (currentTile!= null && currentTile != tile)
            currentTile.changeColor(false);

        gameObject.SetActive(true);
        currentTile = tile;
        handler.LoadData(tile);   ///
        currentTile.changeColor(true);
        //Temp setting
        if (tile.type == Tiles.TileType.greenHouse)
        {
            tempPanel.SetActive(true);
            tempBut.SetActive(true);
        }
        else
            tempPanel.SetActive(false);
        if (tile.currentPlant == null) // No plant
        {
            plantPanel.SetActive(false);
            HandlerButton.SetActive(true); 
        }
        else  // Planting
        {
            HandlerButton.SetActive(false);
            plantPanel.SetActive(true);

            plantName.text = tile.currentPlant.plantName;
            plantIcon.sprite = tile.currentPlant.plantIcon;

            
        }
    }

    public void refreshTile()
    {
        if(currentTile !=null)
        {
            if (currentTile.type == Tiles.TileType.greenHouse)
            {
                tempPanel.SetActive(true);
                tempBut.SetActive(true);
            }
            else
                tempPanel.SetActive(false);
            if (currentTile.currentPlant == null) // No plant
            {
                plantPanel.SetActive(false);
                HandlerButton.SetActive(true);
            }
            else  // Planting
            {
                HandlerButton.SetActive(false);
                plantPanel.SetActive(true);

                plantName.text = currentTile.currentPlant.plantName;
                plantIcon.sprite = currentTile.currentPlant.plantIcon;
                HandleDelegateWhenPlanting();
            }
        }
    }

    public void setInactive()
    {
        if (currentTile != null)
            currentTile.changeColor(false);
        gameObject.SetActive(false);
    }

    private void HandleDelegateWhenPlanting()
    {
        if(QuestTracker.PlantingTemperature != null)
        {
            QuestTracker.PlantingTemperature(currentTile.CurrentTemperature);
        }
    }
}
