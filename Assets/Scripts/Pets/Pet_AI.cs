using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Pet_AI : MonoBehaviour, NotiInterface
{
    public PetData petData;
    public bool isSpawn = false;

    //Affection
    List<PAD_Config> PossibleSeedPerLvList = new List<PAD_Config>();
    public int AffectionLv;
    public int AffectionExp;

    [Header("Searching")]
    [Tooltip("How fast pet finds Seed")]
    public float FindClock;
    float Reset;
    public bool IsReadyToHarvest = false;
    ItemData crop;
    //[Tooltip("Seeds that can be found")]
    //public List<int> ToFind;
    List<KeyValuePair<int, float>> currAffToFind = new List<KeyValuePair<int, float>>();
    int nextAffLvIndex = 1;

    [Header("Hunger")]
    public float Hunger;
    public bool isHungry = false;

    [Header("Petting")]
    public float pettingTimer = 300f;
    public bool IsReadyToPet = false;

    public Transform PetStartPos;
    Inventory PInv;

    [Header("Unity Config")]
    public GameObject petButton;
    public Canvas canvas;
    public Image seedImage;
    public Image foodImage;
    public Image heartImage;
    public Image petImage;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Animator anim;
    State currState;
    public GameObject FeedPanel;
    public List<GameObject> FoodButtonList = new List<GameObject>();
    PetFood[] petFoodList = new PetFood[3];

    private MeshCollider meshCollider;


    //notification handling
    UI_Notification_Manager ui_notification_manager;




    struct PAD_Config
    {
        public int AffectionLv;
        public List<KeyValuePair<int, float>> possibleSeed;
    }

    //AI stat package
    public struct PetStat
    {
        public float Hunger;
        public int AffectionLv;
        public int AffectionExp;
        public float SearchRemainTime;
    }
    public PetStat petStat;

    public void SetState(State state)
    {
        currState = state;
        StartCoroutine(currState.start());
    }

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        PInv = FindObjectOfType<Inventory>();
        Reset = FindClock;

        ReadPetAffection();
        currAffToFind = PossibleSeedPerLvList[nextAffLvIndex - 1].possibleSeed;

        Transform temp = transform.GetChild(0).Find("FeedPanel");
        int count = temp.childCount;
        for (int i = 0; i < count; i++)
        {
            FoodButtonList.Add(temp.GetChild(i).gameObject);
        }
        //transform.GetChild
        //StartCoroutine(Debugging());

        //get the UI_Nofication_manager component
        GameObject[] ui_Notification_Managers = GameObject.FindGameObjectsWithTag("UI_Notification_Manager");
        ui_notification_manager = ui_Notification_Managers[0].GetComponent<UI_Notification_Manager>();
        if (ui_notification_manager == null)
        {
            Debug.LogError("could not find UI_Nofication_manager component");
        }

        meshCollider = GetComponent<MeshCollider>();
        if(meshCollider is null)
        {
            Debug.Log("Missing Mesh Collider in " + name);
        }
        meshCollider.enabled = false;
    }
    public void Update()
    {
        if (isSpawn)
        {

            canvas.transform.LookAt(Camera.main.transform);
            if (agent.hasPath)
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    Debug.Log("pet is stucked");
                    agent.ResetPath();
                    currState.onArrive();
                }
            }

            if (Hunger < 0)
            {
                isHungry = true;
            }
            else
            {
                Hunger -= Time.deltaTime;

                if (FindClock < 0)
                {
                    IsReadyToHarvest = true;
                }
                else
                {
                    FindClock -= Time.deltaTime;
                }

                if(pettingTimer < 0)
                {
                    IsReadyToPet = true;
                }
                else
                {
                    pettingTimer -= Time.deltaTime;
                }
            }
            petStat.SearchRemainTime = FindClock;
            petStat.Hunger = Hunger;
        }

    }
    public void OnMouseDown()
    {
        if (currState != null)
            currState.onClick();
    }

    public void PetChange()
    {
        meshCollider.enabled = true;
        petButton.SetActive(true);
        petButton.transform.Find("Image").GetComponent<Image>().sprite = petData.Icon;
        gameObject.transform.position = PetStartPos.position;
        FindClock = petData.FindTimer;
        //for (int i = 0; i < petData.Findable.Count; i++)
        //{
        //    Debug.Log(i);
        //    ItemData I_Data = DatabaseScript.GetItem(petData.Findable[i]);
        //    int TempS = petData.Findable.IndexOf(I_Data.ID);
        //    ToFind[TempS] = petData.Findable[TempS];
        //}
        Reset = FindClock;
        if (gameObject.GetComponentInChildren<Transform>())
        {
            Debug.Log(gameObject.GetComponentInChildren<Transform>());
            if (gameObject.transform.childCount > 1)
            {
                Destroy(gameObject.transform.GetChild(1).gameObject);
            }
        }
        GameObject newPetModel = Instantiate(petData.Model, gameObject.transform);
        Debug.Log("Instantiate pet in " + gameObject.transform.position.ToString());

        //Begin of FSM
        anim = newPetModel.GetComponent<Animator>();
        isSpawn = true;
        SetState(new FSM_Idle(this));
    }
    public void RandMove()
    {
        while (true)
        {
            //agent.SetDestination(PetStartPos.position);
            float wander = 10f;
            Vector2 rand = UnityEngine.Random.insideUnitCircle;
            Vector3 randPoint = new Vector3(rand.x, 0, rand.y);
            randPoint *= wander;
            randPoint += transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randPoint, out hit, wander, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                break;
            }
            else
                Debug.Log("HI");
        }

    }

    public void BackToHome()
    {
        agent.SetDestination(PetStartPos.position);
    }

    public void Harvest()
    {
        Debug.Log("Player get seed from pet!");
        if (crop != null)
        {
            PInv.AddItem(crop);
            crop = null;
        }
        else
            Debug.LogError("Error");
        IsReadyToHarvest = false;
        seedImage.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        FindClock = Reset;
        PInv.UpdateTasks(TJayEnums.TaskType.getSeedFromPet);
    }

    public void ShowHarvestIcon()
    {
        ItemData itemData = DatabaseScript.GetItem(GetRandSeed());
        crop = itemData;
        Debug.Log("Seed ID : " + crop.ID);
        seedImage.sprite = itemData.Icon;
        seedImage.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
        InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti("Your pet found a new plant!", gameObject));
    }

    public void ShowHungryIcon()
    {
        foodImage.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);

        //old noti
        //InGameNotiManager.Instance.NewNotification_Pet_Hungry(gameObject);

        //call notification hungry
        String message = "Your pet is hungry.";
        CallNotification(message);
    }

    public void ShowPettingIcon()
    {
        petImage.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
    }

    public void Petting()
    {
        petImage.gameObject.SetActive(false);
        pettingTimer = 300f;
        IsReadyToPet = false;
        heartImage.GetComponent<Animation>().Play();
        AffectionUp(1);
    }

    //public void Feed()
    //{
    //    foodImage.gameObject.SetActive(false);
    //    heartImage.GetComponent<Animation>().Play();
    //    //canvas.gameObject.SetActive(false);
    //    Hunger = HungerMax;
    //    isHungry = false;
    //    Affection++;
    //    petStat.Affection = Affection;
    //}

    public void Feed(int type)
    {
        foodImage.gameObject.SetActive(false);
        heartImage.GetComponent<Animation>().Play();
        switch (type)
        {
            case -1: //Cheap
                {
                    Hunger = 90f;
                    break;
                }
            case 0: //Premium
            case 1:
            case 2:
                {
                    PetFood pf = petFoodList[type];
                    Hunger += pf.Duration * 60f;
                    AffectionUp(pf.Affection);
                    pf.CurrHeld--;
                    break;
                }
            default:
                break;
        }
        isHungry = false;
        petStat.AffectionLv = AffectionLv;
        petStat.AffectionExp = AffectionExp;
        FeedPanel.SetActive(false);
        SetState(new FSM_RandomWalk(this));

        //pet is not hungry anymore?
        //if so clear notification
        OnNotificationQuit();

        //Handle quest
        HandleDelegateFeeding();


    }

    public void ShowFeedPanel()
    {
        FeedPanel.SetActive(true);
        List<ItemData> foodList = PInv.itemDatas[(int)TJayEnums.ListType.PetFoods];
        bool[] have = new bool[4];
        foreach (var food in foodList)
        {
            if (food.CurrHeld > 0)
            {
                have[food.ID + 1] = true;
                petFoodList[food.ID] = food as PetFood;
            }
        }
        for (int i = 1; i < 4; i++)
        {
            if (have[i])
            {
                Image image = FoodButtonList[i].GetComponent<Image>();
                Color c = image.color;
                c.g = 1f;
                image.color = c;

                Button button = FoodButtonList[i].GetComponent<Button>();
                button.interactable = true;
            }
            else
            {
                Image image = FoodButtonList[i].GetComponent<Image>();
                Color c = image.color;
                c.g = 0.25f;
                image.color = c;

                Button button = FoodButtonList[i].GetComponent<Button>();
                button.interactable = false;
            }
        }
    }

    void ReadPetAffection()
    {
        string pa = "PetAffData";
        TextAsset ta = Resources.Load(pa) as TextAsset; //Load the csv
        if (ta)
        {
            string wholeText = ta.text;
            string[] rows = wholeText.Split('\n'); //Split by row
            for (int i = 1; i < rows.Length; i++)
            {
                string row = rows[i];
                if (row.Length > 0)
                {
                    string[] rowData = row.Split(','); //Split by ,
                    PAD_Config temp2 = new PAD_Config();
                    temp2.possibleSeed = new List<KeyValuePair<int, float>>();
                    temp2.AffectionLv = Int32.Parse(rowData[0]);
                    for (int j = 1; j < rowData.Length; j += 2)
                    {
                        if (rowData[j].Length > 0)
                        {
                            KeyValuePair<int, float> temp = new KeyValuePair<int, float>(Int32.Parse(rowData[j]), Single.Parse(rowData[j + 1]));
                            temp2.possibleSeed.Add(temp);
                        }
                        else
                            break;
                    }
                    PossibleSeedPerLvList.Add(temp2);
                }
            }
        }
    }

    int GetRandSeed() //Get an ID of random seed 
    {
        float sum = 0;
        Queue<KeyValuePair<int, float>> seedQ = new Queue<KeyValuePair<int, float>>();
        foreach (var item in currAffToFind)
        {
            sum += item.Value;
            seedQ.Enqueue(item);
        }
        float v = UnityEngine.Random.Range(0, sum);
        while (seedQ.Count != 0)
        {
            KeyValuePair<int, float> temp = seedQ.Dequeue();
            v -= temp.Value;
            if (v <= 0)
                return temp.Key;
        }
        Debug.Log("You shouldn't see this message. If yes, well...");
        return int.MinValue;
    }

    void AffectionUp(int amount)
    {
        AffectionExp += amount;
        if (AffectionExp >= 10)
        {
            AffectionExp -= 10;
            AffectionLv++;
        }
        if (nextAffLvIndex < PossibleSeedPerLvList.Count && AffectionLv >= PossibleSeedPerLvList[nextAffLvIndex].AffectionLv)
        {
            currAffToFind = PossibleSeedPerLvList[nextAffLvIndex].possibleSeed;
            nextAffLvIndex++;
        }
    }

    IEnumerator Debugging()
    {
        int test = 1000;
        while (test > 0)
        {
            Debug.Log(GetRandSeed());
            yield return new WaitForSeconds(0.05f);
            test--;
        }
    }



    /// <summary>
    /// call this function to signify new notification to the game
    /// </summary>
    public void CallNotification(String message)
    {
        //uncomment this to work with Andy's code
        //InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti(message, gameObject));

        ui_notification_manager.CreateNewNotiButton(OnNotificationClick, message, GetInstanceID());


    }

    //when the player click the notification, the game should pops up
    public void OnNotificationClick()
    {
        //do nothing
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

    //signify the quest tracker that player just feed the pet.
    private void HandleDelegateFeeding()
    {
        Debug.Log(petData.ItemName);

        if (QuestTracker.FeedAnyPet != null)
        {
            QuestTracker.FeedAnyPet(1);
        }

        if(petData.ItemName == "Dog" && QuestTracker.FeedDog != null)
        {
            QuestTracker.FeedDog(1);
        }
        if (petData.ItemName == "Cat" && QuestTracker.FeedCat != null)
        {
            QuestTracker.FeedCat(1);
        }
        if (petData.ItemName == "Rabbit" && QuestTracker.FeedRabbit != null)
        {
            QuestTracker.FeedRabbit(1);
        }
        if (petData.ItemName == "Horse" && QuestTracker.FeedHorse != null)
        {
            QuestTracker.FeedHorse(1);
        }
        if (petData.ItemName == "Fox" && QuestTracker.FeedFox != null)
        {
            QuestTracker.FeedFox(1);
        }
        if (petData.ItemName == "Turtle" && QuestTracker.FeedTurtle != null)
        {
            QuestTracker.FeedTurtle(1);
        }




    }
    
}
