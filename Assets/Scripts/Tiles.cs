using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tiles : MonoBehaviour
{

    public bool isSelected = false;
    public Color original;
    public Color clicked;
    public GameObject plant;
    public Inventory inventory;
    private PlayerController player;
    [SerializeField]
    public TileType type = TileType.greenHouse;
    public int CurrentTemperature = 20;
    public Plant currentPlant = null;
    public int ID;


    private Renderer renderer;

    public enum TileType
    {
        greenHouse,
        lab,
        magicStore
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        renderer = GetComponent<Renderer>();
        original = renderer.material.color;
    }



    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public void OnMouseDown()
    {

        if (IsPointerOverUIObject()) return;

        if (player != null)
        {
            if (player.tilePanel != null)
            {
                player.tilePanel.clickedTile(this);
            }
        }

        /*
        if (player != null)
        {
            if (player.handler != null)
            {
                player.handler.gameObject.SetActive(true);
                player.handler.LoadData(this);
                if (type == TileType.greenHouse)
                {

                    if (player.temperatureButton != null)
                    {
                        player.temperatureButton.SetActive(true);
                    }
                }
            }
        }
        */




        //Ray ray;

        //if (Input.touchCount > 0)
        //    ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        //else
        //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.transform.gameObject.tag == "Selectable")
        //    {
        //    }
        //}
    }
    public void PlantSeed(ItemData someSeed)
    {
        SeedData theSeed = someSeed as SeedData;
        if (currentPlant == null)
        {

            GameObject newSeed = GetSeed(transform.position);

            if (newSeed != null)
            {
                Plant aPlant = newSeed.GetComponent<Plant>();

                if (aPlant != null)
                {
                    aPlant.Crop = theSeed;
                    aPlant.harvestTime = theSeed.ProcessTime * 60;
                    aPlant.currentIdealTemp = theSeed.IdealTemperature;
                    aPlant.currentTile = this;
                    aPlant.plantIcon = theSeed.Icon;
                    aPlant.plantID = theSeed.ID;
                    aPlant.ProductID = theSeed.ProductID;
                    aPlant.plantName = theSeed.ItemName;
                    aPlant.morethanHalfway = false;
                    aPlant.genus = theSeed.genus;
                    currentPlant = aPlant;

                    if (theSeed.ID == 0)
                    {

                        inventory.UpdateTasks(TJayEnums.TaskType.plant);
                    }


                    if (aPlant.currentIdealTemp == this.CurrentTemperature && theSeed.ID > 0)
                    {
                        inventory.UpdateTasks(TJayEnums.TaskType.temperature);
                    }

                    GameObject model = ModelManager.Instance.GetModelMesh(theSeed.genus, 0);
                    Instantiate(model, aPlant.transform.position, model.transform.rotation, aPlant.transform);
                }

            }
            newSeed.SetActive(true);
        }
    }

    public void ConvertPlant(ItemData somePlant)
    {
        PlantData thePlant = somePlant as PlantData;
        if (currentPlant == null)
        {

            GameObject newPlant = GetSeed(transform.position);

            if (newPlant != null)
            {
                Plant aPlant = newPlant.GetComponent<Plant>();

                if (aPlant != null)
                {
                    aPlant.Crop = thePlant;
                    aPlant.harvestTime = thePlant.ProcessTime * 60;
                    aPlant.currentTile = this;
                    aPlant.plantIcon = thePlant.Icon;
                    aPlant.plantID = thePlant.ID;
                    aPlant.ProductID = thePlant.ProductID;
                    aPlant.plantName = thePlant.ItemName;
                    aPlant.genus = thePlant.genus;
                    aPlant.morethanHalfway = false;
                    //aPlant.Crop.targetMesh 

                    GameObject model = ModelManager.Instance.GetModelMesh(thePlant.genus, 0);
                    Instantiate(model, aPlant.transform.position, model.transform.rotation, aPlant.transform);
                }

            }
            newPlant.SetActive(true);
        }
    }

    /*public void oldFunction()
    {
        Ray ray;

        if (Input.touchCount > 0)
            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
        else
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Selectable")
            {
                if (type == TileType.greenHouse)
                {
                    for (int i = 0; i < inventory.Slots.Length; i++)
                    {
                        InvSlot curSlot = inventory.Slots[i].GetComponent<InvSlot>();
                        if (curSlot.ItemHeld != null)
                        {
                            if (curSlot.ItemHeld.ItemType == ItemData.Type.Seed)
                            {

                                if (inventory.Slots[i].GetComponent<InvSlot>().CurHeld > 0)
                                {

                                    GameObject newPlant = GetSeed(hit.transform.position);

                                    if (newPlant != null)
                                    {
                                        Plant aPlant = newPlant.GetComponent<Plant>();

                                        if (aPlant != null)
                                        {
                                            aPlant.Crop.ItemType = ItemData.Type.Seed;
                                            //aPlant.Crop.Model = null;
                                        }
                                        newPlant.GetComponent<Plant>().Crop.ItemType = ItemData.Type.Seed;
                                    }

                                    inventory.Slots[i].GetComponent<InvSlot>().CurHeld--;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Debug.Log("No Seeds");
                        }
                    }
                }
                else if (type == TileType.lab)
                {

                    for (int i = 0; i < inventory.Slots.Length; i++)
                    {
                        InvSlot curSlot = inventory.Slots[i].GetComponent<InvSlot>();
                        if (curSlot.ItemHeld != null)
                        {
                            if (curSlot.ItemHeld.ItemType == ItemData.Type.Plant)
                            {
                                Debug.Log("found " + curSlot.CurHeld + " crop in slot " + i);

                                if (curSlot.CurHeld > 0)
                                {
                                    GameObject somePlant = GetSeed(hit.transform.position);

                                    if (somePlant != null)
                                    {
                                        Plant aPlant = somePlant.GetComponent<Plant>();

                                        if (aPlant != null)
                                        {
                                            aPlant.Crop.ItemType = ItemData.Type.Plant;
                                            //  aPlant.Crop.Model = null;
                                        }

                                        somePlant.SetActive(true);

                                        inventory.Slots[i].GetComponent<InvSlot>().CurHeld--;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log("No Seeds");
                }
            }

        }
    }
    */

    public GameObject GetSeed(Vector3 spawnLocation)
    {
        GameObject newPlant = null;
        if (player != null)
        {
            if (player.seedPool.Count > 0)
            {
                newPlant = player.seedPool[0];
                player.seedPool.Remove(player.seedPool[0]);
            }
            else
            {
                newPlant = Instantiate(plant, spawnLocation, Quaternion.identity);
            }
        }
        else
        {
            newPlant = Instantiate(plant, spawnLocation, Quaternion.identity);
        }
        if (newPlant != null)
        {
            newPlant.transform.position = spawnLocation;
            GameObject plantPool = GameObject.Find("PlantPool");
            if (!plantPool)
            {
                plantPool = Instantiate(new GameObject());
                plantPool.name = "PlantPool";
            }
            newPlant.transform.parent = plantPool.transform;
            if (newPlant.transform.childCount > 2)
                Destroy(newPlant.transform.GetChild(2).gameObject);

        }
        return newPlant;
    }


    public void LoadPlant(Plant pNewPlant)
    {
        if (currentPlant)
            Destroy(currentPlant.gameObject);
        GameObject newplant = GetSeed(transform.position);
        Plant tPlant = newplant.GetComponent<Plant>();
        tPlant.harvestTime = pNewPlant.harvestTime;
        tPlant.Crop = pNewPlant.Crop;
        tPlant.timePassed = pNewPlant.timePassed;
        tPlant.currentTile = this;
        tPlant.plantIcon = pNewPlant.plantIcon;
        tPlant.plantID = pNewPlant.plantID;
        ItemData data = DatabaseScript.GetItem(tPlant.plantID);
        tPlant.plantIcon = data.Icon;
        tPlant.plantName = data.ItemName;
        currentPlant = tPlant;
    }

    public void changeColor(bool isClicked)
    {
        if (isClicked)
        {
            renderer.material.color = clicked;
        }
        else
        {
            renderer.material.color = original;
        }
    }
}




