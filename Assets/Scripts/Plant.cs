using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Plant : MonoBehaviour, NotiInterface
{
    [Header("UI")]
    public Transform canvas;
    public Image Fill;
    public Text progress;
    public Text remainingText;

    [Header("Config")]
    public string plantName;
    public int plantID = 0;
    public int ProductID;
    public float harvestTime;
    public float timePassed;
    public float plantGrowSpeed = 1;
    public ItemData Crop;
    public TJayEnums.Genus genus = TJayEnums.Genus.unknown;
    public Tiles currentTile = null;
    [SerializeField] bool growing;
    bool readyToHarvest;
    public int currentIdealTemp;
    public Sprite plantIcon;

    //Managers
    TimerManager tm;
    private Inventory playerInventory;
    private PlayerController player;

    //Private var
    SkinnedMeshRenderer smr;
    [SerializeField] private float calcHarvestTime;
    // private Level level;
    public bool morethanHalfway = false;


    //notifications
    UI_Notification_Manager ui_notification_manager;

    // Start is called before the first frame update
    void Start()
    {
        //timer = harvestTime;
        growing = true;
        readyToHarvest = false;
        playerInventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<PlayerController>();
        //level = FindObjectOfType<Level>();
        smr = GetComponent<SkinnedMeshRenderer>();
        // Crop = DatabaseScript.GetItem(itemId);
        tm = FindObjectOfType<TimerManager>();
        if (!tm)
            Debug.LogWarning("TimerManager is not set up yet!");
        else
            tm.PlantsGrowTimeEvent += ChangePlantGrowSpeed;

        SaveLoadManager.Register(this.gameObject);

        //get UI notification manager from the scene
        GameObject[] ui_Notification_Managers = GameObject.FindGameObjectsWithTag("UI_Notification_Manager");
        ui_notification_manager = ui_Notification_Managers[0].GetComponent<UI_Notification_Manager>();
        if (ui_notification_manager == null)
        {
            Debug.LogError("could not find UI_Nofication_manager component");
        }
    }
    private void Awake()
    {
        // NotoficationsManager.SendNotofication("Test", "Plant is ready to harvest", 1);

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(smr.GetBlendShapeWeight(0).ToString());

        canvas.LookAt(Camera.main.transform);
        if (growing)
        {
            CalculateHarvestTime();
            timePassed += Time.deltaTime; //plantGrowSpeed * Time.deltaTime * GetIdealCalc();
            float percent = timePassed / calcHarvestTime;
            Fill.fillAmount = percent;

            if (morethanHalfway == false)
            {
                if (percent >= 0.5f)
                {
                    morethanHalfway = true;
                    ConvertToHalfwayModel();
                }
            }

            Fill.color = Color.Lerp(Color.yellow, Color.green, percent);
            if (timePassed > calcHarvestTime)
            {
                percent = 1f;
                growing = false;
                readyToHarvest = true;
                Fill.color = Color.cyan;
                ConvertToFullModel();

                //old notification
                //NotoficationsManager.SendNotofication("Time Finished!", Crop.Name + " is ready", 1);
                //InGameNotiManager.Instance.NewNotification_Plant(plantName, this.gameObject);

                //new notification
                String message = plantName + " is ready to harvest";
                CallNotification(message);
            }
            if (smr != null)
            {
                smr.SetBlendShapeWeight(0, percent * 100f);
            }
            // progress.text = (percent * 100).ToString("00") + "%";

            DisplayTime();
        }
    }
    private float CalculateHarvestTime()
    {
        float returnFloat = 1.0f;
        if (currentTile != null && Crop.ItemType == ItemData.Type.Seed)
        {
            if ((Crop as SeedData).IdealTemperature == currentTile.CurrentTemperature)
            {
                returnFloat = 0.5f;
            }
            else
            {
                int temp = Mathf.Abs((Crop as SeedData).IdealTemperature - currentTile.CurrentTemperature);
                if (temp < 10)
                {
                    returnFloat = 1.0f;
                }
                else if (temp >= 10)
                {
                    returnFloat = 2.0f;
                }
            }
        }
        float reduction = 0.0f;

        switch (genus)
        {
            case TJayEnums.Genus.unknown:
                break;
            case TJayEnums.Genus.grass:
                {
                    if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.accelGrass))
                    {
                        reduction = 60;
                    }
                }
                break;
            case TJayEnums.Genus.flower:
                {
                    if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.accelFlower))
                    {
                        reduction = 60;
                    }
                }
                break;
            case TJayEnums.Genus.vine:
                {
                    if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.accelVine))
                    {
                        reduction = 60;
                    }
                }
                break;
            case TJayEnums.Genus.herb:
                {
                    if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.accelHerb))
                    {
                        reduction = 60;
                    }
                }
                break;
            case TJayEnums.Genus.fungi:
                {
                    if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.accelFungi))
                    {
                        reduction = 60;
                    }
                }
                break;
            case TJayEnums.Genus.moss:
                {
                    if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.accelMoss))
                    {
                        reduction = 60;
                    }
                }
                break;
        }

        calcHarvestTime = (harvestTime * returnFloat) - reduction;
        return returnFloat;
    }

    private void DisplayTime()
    {
        float realTime = calcHarvestTime - timePassed;
        float minutes = Mathf.FloorToInt(realTime / 60);
        float seconds = Mathf.FloorToInt(realTime % 60);
        if (remainingText != null)
        {
            if (minutes > 0)
            {
                remainingText.text = string.Format("{0:00}m:{1:00}m", minutes, seconds);
                progress.text = string.Format("{0:00}m:{1:00}m", minutes, seconds);
                //"Remain : " + (harvestTime - timeRemaining).ToString("0.0") + "s";

            }
            else
            {
                remainingText.text = string.Format("{00}:{10}s", minutes, seconds);
                progress.text = string.Format("{00}:{10}s", minutes, seconds);

            }

        }
    }

    public void Harvest()
    {
        if (readyToHarvest)
        {
            if (playerInventory != null)
            {
                if (Crop != null)
                {
                    if (ProductID >= 0)
                    {
                        playerInventory.UpdateTasks(TJayEnums.TaskType.harvest);
                        //cancel current notification
                        OnNotificationQuit();

                        int getAmount = 1;
                        if (PotionEffectManager.Instance.ActiveEffects.Contains(TJayEnums.PotionType.specificMulti))
                        {
                            getAmount = PotionEffectManager.Instance.GetBoostSeedAmount(Crop.ID);
                        }

                        for (int i = 0; i < getAmount; i++)
                        {
                            ItemData.Tier tier = RandomizedTier();
                            Debug.Log(tier);
                            ItemData item = DatabaseScript.GetItem(ProductID, tier);
                            playerInventory.AddItem(item);
                        }
                    }

                }

                //Destroy the plant
                //Don't move this part, it won't fix the harvesting bug, and it will cause more bug when using potion. 
                if (player != null)
                {
                    player.seedPool.Add(this.gameObject);
                    player.GrantXP((Crop.ID + 1) * 10);
                    gameObject.SetActive(false);
                    SaveLoadManager.UnRegister(this.gameObject);
                }

            }

            timePassed = 0;
            growing = true;
            currentTile.currentPlant = null;
            currentTile = null;
            readyToHarvest = false;
        }
    }

    private bool IsPointerOverUIObject()
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
        if (readyToHarvest)
        {
            Harvest();
            if (player != null)
            {
                if (player.handler != null && currentTile != null)
                {
                    player.handler.gameObject.SetActive(false);

                    player.temperatureButton.SetActive(false);


                }
            }
        }
        else
        {
            if (player != null)
            {
                if (player.tilePanel != null)
                {
                    player.tilePanel.clickedTile(currentTile);
                }
            }


            /*
            if (player != null)
            {
                if (player.handler != null && currentTile != null)
                {
                    player.handler.LoadData(currentTile);
                    player.handler.gameObject.SetActive(false);


                    if (player.temperatureButton != null)
                    {
                        player.temperatureButton.SetActive(true);
                    }

                }
            }
            */

        }

    }

    void ChangePlantGrowSpeed(float v)
    {
        plantGrowSpeed = v;
    }

    private void OnApplicationQuit()
    {
        NotoficationsManager.SendNotofication("Test", "Plant is ready to harvest", harvestTime - timePassed);
    }

    public float TimeRemain()
    {
        return calcHarvestTime - timePassed;
    }

    private void ConvertToHalfwayModel()
    {
        //Remove the old model
        Destroy(transform.GetChild(2).gameObject);
        GameObject model = ModelManager.Instance.GetModelMesh(genus, 1);
        Instantiate(model, transform.position, model.transform.rotation, transform);
        ParticleSystem sys = GetComponentInChildren<ParticleSystem>();

        if (sys != null)
        {
            sys.Play();
        }
    }

    private void ConvertToFullModel()
    {
        Destroy(transform.GetChild(2).gameObject);
        GameObject model = ModelManager.Instance.GetModelMesh(genus, 2);
        Instantiate(model, transform.position, model.transform.rotation, transform);

        ParticleSystem sys = GetComponentInChildren<ParticleSystem>();

        if (sys != null)
        {
            sys.Play();
        }
    }


    //deadling with notification
    public void CallNotification(String message)
    {
        //uncomment this to work with Andy's code
        //InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti(message, gameObject));

        ui_notification_manager.CreateNewNotiButton(OnNotificationClick, message, GetInstanceID());


    }

    //when the player click the notification, do something? currently do nothing
    public void OnNotificationClick()
    {
        //do nothing currently
    }

    //when the game ends, we should remove the notification 
    public void OnNotificationQuit()
    {
        bool removed = ui_notification_manager.RemoveNotiButton(GetInstanceID());
        if (!removed)
        {
            Debug.LogError("cannot remove notification. Something is wrong.");
        }
    }

    ItemData.Tier RandomizedTier()
    {
       
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 10)
            return ItemData.Tier.Gold;
        else if (rand < 30)
            return ItemData.Tier.Silver;
        else
            return ItemData.Tier.Bronze;
    }
}
