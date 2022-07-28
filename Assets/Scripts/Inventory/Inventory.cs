using System;
using System.Collections.Generic;
using TJayEnums;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{

    public bool I_Active = false;
    public bool PI_Active = false;

    public int Coins;

    //TJay stuff


    public ListType currentList = ListType.Seeds;
    public RowItem rowItemPrefab = null;
    public RowItem selectedRowItem = null;
    public Pet_AI pet = null;
    public PetData currentPet;
    private List<RowItem> availableRows = new List<RowItem>();

    [Header("Warehouse")]

    public List<ItemData>[] itemDatas; //Contain Seeds, Plants, Herbs, Potions, Recipes, Pets, PetFood
    //public List<PetData> pets = new List<PetData>();
    public List<ItemData> taskObjectives = new List<ItemData>();
    public List<int> initialTasks = new List<int>();
    //public List<PetFood> food = new List<PetFood>();


    [Header("UI")]
    public Text SelectedText = null;
    public Text SelectedShadowText = null;
    public Text SelectedForgroundText = null;
    public Text descriptionText = null;
    public Text CoinText;
    public GameObject InvPanel;
    public GameObject targetPanel = null;
    public GameObject SelectionPanel = null;
    public GameObject displayContent = null;
    public GameObject PetInv;
    public List<Button> selectedButtons = new List<Button>();
    public InventoryUI inventoryUI;


    [Header("Managers")]
    public StationManager station = null;
    public PlotManager greenhouseManager = null;
    public PlotManager labManager = null;
    public PlotManager magicManager = null;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Inventory.cs attached to " + gameObject.name);
        CoinText = GameObject.Find("CoinText").GetComponent<Text>();
        if (CoinText != null)
        {
            CoinText.text = "" + Coins.ToString();
        }
        for (int i = 0; i < initialTasks.Count; i++)
        {
            taskObjectives.Add(DatabaseScript.GetItem(initialTasks[i]));
        }

        itemDatas = new List<ItemData>[7];
        for (int i = 0; i < itemDatas.Length; i++)
        {
            itemDatas[i] = new List<ItemData>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            I_Active = !I_Active;
            InvPanel.SetActive(I_Active);
            if (PI_Active == true)
            {
                PI_Active = !PI_Active;
                PetInv.SetActive(PI_Active);
            }
        }

        Test();

        ///Debug
        //if (Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    ItemData item = new ItemData();

        //    item.ID = 1;
        //    item.Name = "Wheat Seed";
        //    item.ItemType = ItemData.Type.Seed;
        //    item.Price = 1;
        //    item.Icon = Resources.Load("Assets/Resources/Images/Wheat_Seed_Image.jpg") as Sprite;
        //    item.MaxHeld = 5;
        //    item.ProcessTime = 5f;
        //    AddItem(item);
        //}
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CoinUpdate(1000);
        }
    }

    public void InvButPress()
    {
        //I_Active = !I_Active;
        //InvPanel.SetActive(I_Active);
        //if (PI_Active == true)
        //{
        //    PI_Active = !PI_Active;
        //    PetInv.SetActive(PI_Active);
        //}
        if (targetPanel != null)
        {
            targetPanel.SetActive(!targetPanel.activeInHierarchy);
            if (station != null)
            {
                if (targetPanel.activeInHierarchy == true)
                {
                    station.currentMenu = PlayerMenu.GenericInventory;
                    currentList = ListType.Seeds;
                    UpdateButtons();
                }
                else
                {
                    station.currentMenu = PlayerMenu.None;
                }
            }
        }
        UpdateContent();
    }
    public void NxtButton()
    {
        PI_Active = !PI_Active;
        InvPanel.SetActive(!PI_Active);
        PetInv.SetActive(PI_Active);
    }
    public void AddPet(PetData p_data)
    {
       // Debug.Log(p_data.ItemName);
        if (p_data != null)
        {
            if (itemDatas[(int)ListType.Pets].Contains(p_data as ItemData))
            {
                Debug.LogError("Pet already Exist!");
                //PetData aPet = pets[pets.IndexOf(p_data)];
            }
            else
            {
                itemDatas[(int)ListType.Pets].Add(p_data);
                HandleDelegatesAfterAddingItem(p_data, 1);
            }
        }
    }

    public void AddItem(ItemData i_data, int amount = 1)
    {
        Debug.Log("adding " + amount.ToString() + " " + i_data.ItemName);
        if (i_data != null)
        {
            int type = (int)i_data.ItemType;
            List<ItemData> items = itemDatas[type];


            int index = items.IndexOf(i_data);
            HandleDelegatesAfterAddingItem(i_data, amount);
            if (index > -1)
            {
                ItemData tempData = items[index];
                if (tempData.CurrHeld < tempData.MaxHeld)
                {
                    tempData.CurrHeld += amount;
                }
                    

            }
            else
            {
                i_data.CurrHeld = amount;
                items.Add(i_data);
            }
        }
        #region old Code
        /*
            switch (i_data.ItemType)
            {
                case ItemData.Type.Seed:
                    {
                        if (seeds.Contains(i_data))
                        {
                            ItemData aSeed = seeds[seeds.IndexOf(i_data)];
                            if (aSeed.CurrHeld < aSeed.MaxHeld)
                            {
                                aSeed.CurrHeld++;
                            }

                        }
                        else
                        {
                            i_data.CurrHeld = 1;
                            seeds.Add(i_data);
                        }
                    }
                    break;
                case ItemData.Type.Plant:
                    {
                        if (plants.Contains(i_data))
                        {
                            ItemData aPlant = plants[plants.IndexOf(i_data)];
                            if (aPlant.CurrHeld < aPlant.MaxHeld)
                            {
                                aPlant.CurrHeld++;
                            }

                        }
                        else
                        {
                            i_data.CurrHeld = 1;
                            plants.Add(i_data);
                        }
                    }
                    break;

                case ItemData.Type.DryHerb:
                    {
                        if (herbs.Contains(i_data))
                        {
                            ItemData aHerb = herbs[herbs.IndexOf(i_data)];
                            if (aHerb.CurrHeld < aHerb.MaxHeld)
                            {
                                aHerb.CurrHeld++;
                            }

                        }
                        else
                        {
                            i_data.CurrHeld = 1;
                            herbs.Add(i_data);
                        }
                    }
                    break;
                case ItemData.Type.Potion:
                    {
                        if (potions.Contains(i_data))
                        {
                            ItemData aSeed = potions[potions.IndexOf(i_data)];
                            if (aSeed.CurrHeld < aSeed.MaxHeld)
                            {
                                aSeed.CurrHeld++;
                            }

                        }
                        else
                        {
                            i_data.CurrHeld = 1;
                            potions.Add(i_data);
                        }
                    }
                    break;
                case ItemData.Type.Recipe:
                    {
                        if (recipes.Contains(i_data))
                        {
                            ItemData aSeed = recipes[recipes.IndexOf(i_data)];
                            if (aSeed.CurrHeld < aSeed.MaxHeld)
                            {
                                aSeed.CurrHeld++;
                            }

                        }
                        else
                        {
                            i_data.CurrHeld = 1;
                            recipes.Add(i_data);
                        }
                    }
                    break;
                default:
                    break;
            }
        */
        #endregion 
    }
    public void AddFood(PetFood i_data, int amount = 1)
    {
        if (i_data != null)
        {
            int index = itemDatas[(int)ListType.PetFoods].IndexOf(i_data);
            if (index>-1)
            {
                PetFood someFood = itemDatas[(int)ListType.PetFoods][index] as PetFood;
                if (someFood.CurrHeld < someFood.MaxHeld)
                {
                    someFood.CurrHeld+= amount;
                    if (someFood.CurrHeld > someFood.MaxHeld)
                        someFood.CurrHeld = someFood.MaxHeld;
                }
            }
            else
            {
                i_data.CurrHeld = amount;
                itemDatas[(int)ListType.PetFoods].Add(i_data);
            }
        }

    }

    public void CoinUpdate(int amount)
    {
        Coins += amount;
        CoinText.text = "" + Coins.ToString();
        HandleDelegateForCoin(amount);
    }
    private void UpdateButtons()
    {
        int cur = (int)currentList;

        if (cur < selectedButtons.Count)
        {
            for (int i = 0; i < selectedButtons.Count; i++)
            {
                if (i == cur)
                {
                    selectedButtons[i].Select();
                    Image selectedImage = selectedButtons[i].gameObject.GetComponent<Image>();
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
                    Image selectedImage = selectedButtons[i].gameObject.GetComponent<Image>();
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
    }
    public void ChangeSelectedList(int newList)
    {
        currentList = (ListType)newList;
        UpdateButtons();
        UpdateContent();
    }
    public void ForceUpdateContent(GameObject newContent)
    {
        UpdateContent(false, newContent);
    }
    public void ForceUpdateContent(GameObject newContent, ListType newList)
    {
        currentList = newList;
        UpdateButtons();
        UpdateContent(false, newContent);
    }

    public void UpdateContent(bool hideSelector = false, GameObject targetContent = null)
    {
        if (targetContent == null)
        {
            targetContent = displayContent;
        }
        if (descriptionText != null)
        {
            descriptionText.text = "";
        }
        if (SelectionPanel != null)
        {
            if (hideSelector == true)
                SelectionPanel.SetActive(false);
            else
                SelectionPanel.SetActive(true);
        }
        if (targetContent != null)
        {
            if (rowItemPrefab != null)
            {
                if (targetContent != displayContent)
                {

                    for (int i = 0; i < displayContent.transform.childCount; i++)
                    {
                        RowItem possibleItem = displayContent.transform.GetChild(i).GetComponent<RowItem>();
                        if (possibleItem != null)
                        {
                            if (!availableRows.Contains(possibleItem))
                                availableRows.Add(possibleItem);

                            possibleItem.nameText.rectTransform.sizeDelta = new Vector2(508.3775f, 168.9934f);
                            if (possibleItem.myBackgroundImage)
                            {
                                possibleItem.myBackgroundImage.color = Color.white;
                            }

                            possibleItem.gameObject.SetActive(false);
                        }
                    }
                }

                {


                    for (int i = 0; i < targetContent.transform.childCount; i++)
                    {
                        RowItem possibleItem = targetContent.transform.GetChild(i).GetComponent<RowItem>();
                        if (possibleItem != null)
                        {
                            if (!availableRows.Contains(possibleItem))
                                availableRows.Add(possibleItem);


                            if (targetContent != displayContent)
                            {
                                possibleItem.nameText.rectTransform.sizeDelta = new Vector2(508.3775f, 168.9934f);
                            }
                            else
                            {
                                possibleItem.nameText.rectTransform.sizeDelta = new Vector2(326.4563f, 168.9934f);

                            }
                            if (possibleItem.myBackgroundImage)
                            {
                                possibleItem.myBackgroundImage.color = Color.white;
                            }

                            possibleItem.gameObject.SetActive(false);
                        }
                    }

                }

                if (station == null)
                {
                    UpdatedSelectedText(currentList.ToString());

                }
                else
                {
                    if (station.currentMenu == PlayerMenu.Selling)
                    {

                        UpdatedSelectedText("Selling");
                    }
                    else
                    {
                        UpdatedSelectedText(currentList.ToString());
                    }

                }

                bool foundOne = false;

                switch (currentList)
                {
                    case ListType.Seeds:
                    case ListType.Plants:
                    case ListType.Herbs:
                    case ListType.Potions:
                    case ListType.Recipes:
                        {
                            int type = (int)currentList;
                            List<ItemData> rowItemData = itemDatas[type];

                            if (rowItemData.Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {

                                for (int i = 0; i < rowItemData.Count; i++)
                                {
                                    if (rowItemData[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem seedItem = null;
                                        seedItem = GetNextRow(targetContent);
                                        if (seedItem != null)
                                        {

                                            seedItem.myIcon.sprite = rowItemData[i].Icon;

                                            seedItem.nameText.text = rowItemData[i].ItemName;
                                            seedItem.countText.text = "x " + rowItemData[i].CurrHeld;
                                            seedItem.myIcon.color = Color.white;
                                            seedItem.referenceData = rowItemData[i];
                                            seedItem.referencePet = null;
                                            seedItem.myInventory = this;
                                            seedItem.Setup();
                                            seedItem.gameObject.SetActive(true);


                                        }
                                    }

                                }

                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }
                            }
                            break;
                        }
                    case ListType.Pets:
                        {

                            if (itemDatas[(int)ListType.Pets].Count == 0)
                            {
                                RowItem dataPet = null;
                                dataPet = GetNextRow(targetContent);
                                if (dataPet != null)
                                {
                                    dataPet.LoadDefault();
                                    if (descriptionText != null)
                                    {
                                        descriptionText.text = "";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < itemDatas[(int)ListType.Pets].Count; i++)
                                {
                                    //if (magics[i].CurrHeld > 0)
                                    {

                                        RowItem dataPet = null;
                                        dataPet = GetNextRow(targetContent);

                                        if (dataPet != null)
                                        {
                                            dataPet.myIcon.sprite = itemDatas[(int)ListType.Pets][i].Icon;
                                            dataPet.nameText.text = itemDatas[(int)ListType.Pets][i].ItemName;
                                            dataPet.countText.text = "";// + pets[i].CurrHeld;
                                            dataPet.myIcon.color = Color.white;
                                            dataPet.referenceData = null;
                                            dataPet.referencePet = itemDatas[(int)ListType.Pets][i] as PetData;
                                            dataPet.myInventory = this;
                                            dataPet.Setup();
                                        }
                                    }
                                }

                                if (itemDatas[(int)ListType.Pets].Count == 0)
                                {

                                    GetDefault(targetContent);

                                }

                            }
                            break;
                        }
                    case ListType.Tasks:
                        {
                            if (taskObjectives.Count == 0)
                            {

                                GetDefault(targetContent);
                            }
                            else
                            {

                                for (int i = 0; i < taskObjectives.Count; i++)
                                {
                                    //if (recipes[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem dataTask = null;
                                        dataTask = GetNextRow(targetContent);
                                        dataTask.transform.SetSiblingIndex(i);

                                        if (targetContent != displayContent)
                                        {
                                            dataTask.nameText.rectTransform.sizeDelta = new Vector2(508.3775f, 168.9934f);
                                        }
                                        else
                                        {
                                            dataTask.nameText.rectTransform.sizeDelta = new Vector2(326.4563f, 168.9934f);

                                        }

                                        if (dataTask != null)
                                        {
                                            dataTask.LoadDefault();
                                            Debug.Log(taskObjectives[i].ToString());
                                            dataTask.nameText.text = taskObjectives[i].ItemName;

                                            dataTask.referenceData = taskObjectives[i];
                                            dataTask.referencePet = null;
                                            dataTask.myInventory = this;
                                            dataTask.Setup();

                                            if ((taskObjectives[i] as TaskData).isFinished)
                                            {
                                                dataTask.transform.SetSiblingIndex(0);
                                                if (dataTask.myBackgroundImage)
                                                {
                                                    dataTask.myBackgroundImage.color = Color.yellow;
                                                }
                                            }
                                            else
                                            {

                                            }

                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                if (foundOne == false)
                                {

                                    GetDefault(targetContent);
                                }

                            }

                            break;
                        }
                    case ListType.PetFoods:
                        {
                            if (itemDatas[(int)ListType.PetFoods].Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {
                                for (int i = 0; i < itemDatas[(int)ListType.PetFoods].Count; i++)
                                {
                                    //if (recipes[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem foodItem = null;
                                        if (availableRows.Count > 0)

                                            foodItem = GetNextRow(targetContent);
                                        if (foodItem != null)
                                        {
                                            foodItem.myIcon.sprite = itemDatas[(int)ListType.PetFoods][i].Icon;
                                            foodItem.nameText.text = itemDatas[(int)ListType.PetFoods][i].ItemName;
                                            foodItem.countText.text = "x " + itemDatas[(int)ListType.PetFoods][i].CurrHeld;
                                            foodItem.myIcon.color = Color.white;
                                            foodItem.referenceData = null;
                                            foodItem.myInventory = this;
                                            foodItem.referencePet = null;
                                            foodItem.referencePetFood = (itemDatas[(int)ListType.PetFoods][i] as PetFood);
                                            foodItem.Setup();
                                        }
                                    }
                                }
                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }

                            }
                            break;
                        }
                    default:
                        break;
                }

                



                #region OldCode
                /*
                switch (currentList)
                {
                    case ListType.Seeds:
                        {

                            if (seeds.Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {

                                for (int i = 0; i < seeds.Count; i++)
                                {
                                    if (seeds[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem seedItem = null;
                                        seedItem = GetNextRow(targetContent);
                                        if (seedItem != null)
                                        {

                                            seedItem.myIcon.sprite = seeds[i].Icon;

                                            seedItem.nameText.text = seeds[i].Name;
                                            seedItem.countText.text = "x " + seeds[i].CurrHeld;
                                            seedItem.myIcon.color = Color.white;
                                            seedItem.referenceData = seeds[i];
                                            seedItem.referencePet = null;
                                            seedItem.myInventory = this;
                                            seedItem.Setup();
                                            seedItem.gameObject.SetActive(true);


                                        }
                                    }

                                }

                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }
                            }
                        }
                        break;
                    case ListType.Plants:
                        {
                            if (plants.Count == 0)
                            {

                                GetDefault(targetContent);
                            }
                            else
                            {
                                for (int i = 0; i < plants.Count; i++)
                                {
                                    if (plants[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem plantItem = null;
                                        plantItem = GetNextRow(targetContent);
                                        if (plantItem != null)
                                        {


                                            plantItem.myIcon.sprite = plants[i].Icon;
                                            plantItem.nameText.text = plants[i].Name;
                                            plantItem.countText.text = "x " + plants[i].CurrHeld;
                                            plantItem.myIcon.color = Color.white;
                                            plantItem.referenceData = plants[i];
                                            plantItem.myInventory = this;
                                            plantItem.referencePet = null;
                                            plantItem.Setup();
                                            plantItem.gameObject.SetActive(true);


                                        }
                                    }
                                }

                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }

                            }
                        }
                        break;
                    case ListType.Herbs:
                        {
                            if (herbs.Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {
                                for (int i = 0; i < herbs.Count; i++)
                                {
                                    if (herbs[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem productItem = null;
                                        productItem = GetNextRow(targetContent);
                                        if (productItem != null)
                                        {


                                            productItem.myIcon.sprite = herbs[i].Icon;
                                            productItem.nameText.text = herbs[i].Name;
                                            productItem.countText.text = "x " + herbs[i].CurrHeld;
                                            productItem.myIcon.color = Color.white;
                                            productItem.referenceData = herbs[i];
                                            productItem.myInventory = this;
                                            productItem.referencePet = null;
                                            productItem.Setup();
                                            productItem.gameObject.SetActive(true);

                                        }
                                    }
                                }

                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }

                            }
                        }
                        break;
                    case ListType.Potions:
                        {
                            if (potions.Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {
                                for (int i = 0; i < potions.Count; i++)
                                {
                                    if (potions[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem magicItem = null;
                                        magicItem = GetNextRow(targetContent);
                                        if (magicItem != null)
                                        {
                                            magicItem.myIcon.sprite = potions[i].Icon;
                                            magicItem.nameText.text = potions[i].Name;
                                            magicItem.countText.text = "x " + potions[i].CurrHeld;
                                            magicItem.myIcon.color = Color.white;
                                            magicItem.referenceData = potions[i];
                                            magicItem.myInventory = this;
                                            magicItem.referencePet = null;
                                            magicItem.Setup();
                                            magicItem.gameObject.SetActive(true);
                                        }
                                    }
                                }

                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }

                            }
                        }
                        break;
                    case ListType.Recipes:
                        {
                            if (recipes.Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {
                                for (int i = 0; i < recipes.Count; i++)
                                {
                                    //if (recipes[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem recipeItem = null;
                                        if (availableRows.Count > 0)

                                            recipeItem = GetNextRow(targetContent);
                                        if (recipeItem != null)
                                        {
                                            recipeItem.myIcon.sprite = recipes[i].Icon;
                                            recipeItem.nameText.text = recipes[i].Name;
                                            recipeItem.countText.text = "x " + recipes[i].CurrHeld;
                                            recipeItem.myIcon.color = Color.white;
                                            recipeItem.referenceData = recipes[i];
                                            recipeItem.myInventory = this;
                                            recipeItem.referencePet = null;
                                            recipeItem.Setup();
                                        }
                                    }
                                }
                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }

                            }
                        }
                        break;
                    case ListType.Pets:
                        {

                            if (pets.Count == 0)
                            {
                                RowItem dataPet = null;
                                dataPet = GetNextRow(targetContent);
                                if (dataPet != null)
                                {
                                    dataPet.LoadDefault();
                                    if (descriptionText != null)
                                    {
                                        descriptionText.text = "";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < pets.Count; i++)
                                {
                                    //if (magics[i].CurrHeld > 0)
                                    {

                                        RowItem dataPet = null;
                                        dataPet = GetNextRow(targetContent);

                                        if (dataPet != null)
                                        {
                                            dataPet.myIcon.sprite = pets[i].Icon;
                                            dataPet.nameText.text = pets[i].Name;
                                            dataPet.countText.text = "";// + pets[i].CurrHeld;
                                            dataPet.myIcon.color = Color.white;
                                            dataPet.referenceData = null;
                                            dataPet.referencePet = pets[i];
                                            dataPet.myInventory = this;
                                            dataPet.Setup();
                                        }
                                    }
                                }

                                if (pets.Count == 0)
                                {

                                    GetDefault(targetContent);

                                }

                            }
                        }
                        break;
                    case ListType.Tasks:
                        {
                            if (taskObjectives.Count == 0)
                            {

                                GetDefault(targetContent);
                            }
                            else
                            {

                                for (int i = 0; i < taskObjectives.Count; i++)
                                {
                                    //if (recipes[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem dataTask = null;
                                        dataTask = GetNextRow(targetContent);
                                        dataTask.transform.SetSiblingIndex(i);

                                        if (targetContent != displayContent)
                                        {
                                            dataTask.nameText.rectTransform.sizeDelta = new Vector2(508.3775f, 168.9934f);
                                        }
                                        else
                                        {
                                            dataTask.nameText.rectTransform.sizeDelta = new Vector2(326.4563f, 168.9934f);

                                        }

                                        if (dataTask != null)
                                        {
                                            dataTask.LoadDefault();

                                            dataTask.nameText.text = taskObjectives[i].Name;

                                            dataTask.referenceData = taskObjectives[i];
                                            dataTask.referencePet = null;
                                            dataTask.myInventory = this;
                                            dataTask.Setup();

                                            if (taskObjectives[i].EvolutionID == 1)
                                            {
                                                dataTask.transform.SetSiblingIndex(0);
                                                if (dataTask.myBackgroundImage)
                                                {
                                                    dataTask.myBackgroundImage.color = Color.yellow;
                                                }
                                            }
                                            else
                                            {

                                            }

                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                if (foundOne == false)
                                {

                                    GetDefault(targetContent);
                                }

                            }

                        }
                        break;
                    case ListType.Food:
                        {
                            if (food.Count == 0)
                            {
                                GetDefault(targetContent);
                            }
                            else
                            {
                                for (int i = 0; i < food.Count; i++)
                                {
                                    //if (recipes[i].CurrHeld > 0 || station.currentMenu == PlayerMenu.ViewingJournal)
                                    {
                                        foundOne = true;
                                        RowItem foodItem = null;
                                        if (availableRows.Count > 0)

                                            foodItem = GetNextRow(targetContent);
                                        if (foodItem != null)
                                        {
                                            foodItem.myIcon.sprite = food[i].Icon;
                                            foodItem.nameText.text = food[i].Name;
                                            foodItem.countText.text = "x " + food[i].CurrHeld;
                                            foodItem.myIcon.color = Color.white;
                                            foodItem.referenceData = null;
                                            foodItem.myInventory = this;
                                            foodItem.referencePet = null;
                                            foodItem.referencePetFood = food[i];
                                            foodItem.Setup();
                                        }
                                    }
                                }
                                if (foundOne == false)
                                {
                                    GetDefault(targetContent);
                                }

                            }
                        }
                        break;

                }
                */
                #endregion



            }
        }
    }
    private void UpdatedSelectedText(string newText)
    {

        if (SelectedText != null && SelectedForgroundText != null && SelectedShadowText != null)
        {
            SelectedText.text = newText;
            SelectedShadowText.text = newText;
            SelectedForgroundText.text = newText;
        }
    }
    public RowItem GetNextRow(GameObject targetContent)
    {
        RowItem someItem = null;
        if (availableRows.Count > 0)
        {
            someItem = availableRows[0];
            availableRows.Remove(someItem);
        }
        else
        {
            someItem = Instantiate(rowItemPrefab, targetContent.transform).GetComponent<RowItem>();

        }
        if (someItem != null)
        {
            if (someItem.transform.parent != targetContent.transform)
                someItem.transform.SetParent(targetContent.transform);

            someItem.gameObject.SetActive(true);

        }
        return someItem;
    }
    public RowItem GetDefault(GameObject targetContent)
    {
        RowItem someItem = null;
        if (availableRows.Count > 0)
        {
            someItem = availableRows[0];
            availableRows.Remove(someItem);
        }
        else
        {
            someItem = Instantiate(rowItemPrefab, targetContent.transform).GetComponent<RowItem>();

        }
        if (someItem != null)
        {
            if (someItem.transform.parent != targetContent.transform)
                someItem.transform.SetParent(targetContent.transform);
            someItem.LoadDefault();
            someItem.referenceData = null;
            someItem.referencePet = null;
            someItem.myInventory = this;
            someItem.enabled = true;
            someItem.gameObject.SetActive(true);

        }

        return someItem;
    }

    public void UpdateTasks(TaskType operation = TaskType.buy)
    {
        RowItem fake = GetDefault(displayContent);
        fake.LoadDefault();
        fake.Setup();
        fake.UpdateTasks(operation);
        fake.gameObject.SetActive(false);

    }
    //Call from save load manager, to clear the inventory and load data
    public void ClearInventory()
    {
        CoinUpdate(0);
        foreach (var row in itemDatas)
        {
            row.Clear();
        }
        //seeds.Clear();
        //plants.Clear();
        //herbs.Clear();
        //potions.Clear();
        //recipes.Clear();
    }

    public bool CheckIfContain(ItemData itemData)
    {
        ItemData check = GetItemData(itemData);
        return check != null ? true : false; 
    }

    public bool RemoveItem(ItemData itemData, int amount = 1)
    {
        bool removed = false;
        if (itemData != null)
        {
            int type = (int)itemData.ItemType;
            int index = itemDatas[type].IndexOf(itemData);

            if (index > -1) //found
            {
                ItemData item = itemDatas[type][index];
                if (item.CurrHeld < amount)
                {
                    Debug.Log("Too few to remove");
                }   
                else if (item.CurrHeld == amount)
                {
                    Debug.Log("Removing " + amount + " " + itemData.name);
                    itemDatas[type].RemoveAt(index);
                    item.CurrHeld = 0;
                    
                    removed = true;
                }
                else
                {
                    item.CurrHeld -= amount;
                    removed = true;
                }
                    
            }
            else
                Debug.Log("Cant find the item");
        }
        else
            Debug.Log("Attempted to remove NULL item");
        return removed;
    }

    public ItemData GetItemData(ItemData checkItem)
    {
        int type = (int)checkItem.ItemType;
        foreach (var item in itemDatas[type])
        {
            if (item.ID == checkItem.ID)
                return item;
        }
        return null;
    }

    public ItemData GetItemData(int ID, ItemData.Tier tier = ItemData.Tier.None)
    {
        ItemData.Type type = ItemData.Type.Seed;
        bool contain = DatabaseScript.GetItemType(ID, ref type);

        if (!contain) return null; //invalid ID

        switch (type)
        {
            case ItemData.Type.Plant:
            case ItemData.Type.DryHerb:
                {
                    foreach (var item in itemDatas[(int)type])
                    {
                        if (item.ID == ID && item.tier == tier) return item;
                    }
                }
                break;
            default:
                {
                    foreach (var item in itemDatas[(int)type])
                    {
                        if (item.ID == ID) return item;
                    }
                }
                break;
        }
        return null;
    }

    public List<ItemData> GetAllRecipes()
    {
        return itemDatas[(int)ListType.Recipes];
    }


    //this function will handle every delegates.
    //because the list is long, the code takes a lot of lines, but the logic is the same. If the delegate is not null, 
    //we invoke it and pass in the amount as parameter
    //haven't figure out how to shorten this code yet. Since can't find a way to make a list of delegate
    private void HandleDelegatesAfterAddingItem(ItemData i_data, int amount)
    {
        //Buy plant/seed
        if(i_data.ItemType == ItemData.Type.Seed && QuestTracker.BuyPlant != null)
        {
            QuestTracker.BuyPlant(amount);
        }

        if(i_data.ItemName == "Fragrant Orchid Enhancer")
        {
            Debug.Log("this is fragrant orchid enhancer");
            if (QuestTracker.GatherFragrentEnhancer == null)
            {
                Debug.Log("gatherfragrentenhancer is null");
            }
            
        }

        #region RECIPE_AND_ENHANCER
        //Buy potion recipe
        if (i_data.ItemType == ItemData.Type.Recipe && QuestTracker.BuyPotionRecipe != null)
        {
            QuestTracker.BuyPotionRecipe(amount);
        }

        if(i_data.ItemName == " Grass Enhancer Recipe" && QuestTracker.BuyGrassEnhancerRecipe != null)
        {
            QuestTracker.BuyGrassEnhancerRecipe(amount);
        }
        if (i_data.ItemName == " Fragrent Enhancer Recipe" && QuestTracker.BuyFragrantEnhancerRecipe != null)
        {
            QuestTracker.BuyFragrantEnhancerRecipe(amount);
        }
        if (i_data.ItemName == " Desert Grass Enhancer Recipe" && QuestTracker.BuyDesertGrassEnhancerRecipe != null)
        {
            QuestTracker.BuyDesertGrassEnhancerRecipe(amount);
        }
        if (i_data.ItemName == " Giant Jungle Recipe" && QuestTracker.BuyGiantJungleRecipe != null)
        {
            Debug.Log("Triggered. Amount = " + amount );
            QuestTracker.BuyGiantJungleRecipe(amount);
        }



        if (i_data.ItemName == "Grass Root Enhancer" && QuestTracker.GatherGrassRootEnhancer != null)
        {
            QuestTracker.GatherGrassRootEnhancer(amount);
        }
        if (i_data.ItemName == "Fragrant Orchid Enhancer" && QuestTracker.GatherFragrentEnhancer != null)
        {
            QuestTracker.GatherFragrentEnhancer(amount);
        }
        if (i_data.ItemName == "Desert Grass Root Enhancer" && QuestTracker.GatherDesertGrassEnhancer != null)
        {
            QuestTracker.GatherDesertGrassEnhancer(amount);
        }
        if (i_data.ItemName == "Giant Jungle Orchid Enhancer" && QuestTracker.GatherGiantJungleOrchidEnhancer != null)
        {
            QuestTracker.GatherGiantJungleOrchidEnhancer(amount);
        }
        #endregion

        #region FLOWERS
        //FLOWERS
        //Grass root stuffs
        if (i_data.ItemName == "Grass Root Seed" && QuestTracker.GrassRootSeed != null)
        {
            QuestTracker.GrassRootSeed(amount);
        }
        if (i_data.ItemName == "Grass Root" && QuestTracker.GrassRoot != null)
        {
            QuestTracker.GrassRoot(amount);
        }
        if (i_data.ItemName == "Dried Grass Root" && QuestTracker.DriedGrassRoot != null)
        {
            QuestTracker.DriedGrassRoot(amount);
        }

        //Fragrant Orchid stuffs
        if (i_data.ItemName == "Fragrant Orchid Seed" && QuestTracker.FragrantOrchidSeed != null)
        {
            QuestTracker.FragrantOrchidSeed(amount);
        }
        if (i_data.ItemName == "Fragrant Orchid" && QuestTracker.FragrantOrchid != null)
        {
            QuestTracker.FragrantOrchid(amount);
        }
        if (i_data.ItemName == "Dried Fragrant Orchid" && QuestTracker.DriedFragrantOrchid != null)
        {
            QuestTracker.DriedFragrantOrchid(amount);
        }

        //Desert Grass Root Seed
        if (i_data.ItemName == "Desert Grass Root Seed" && QuestTracker.DesertGrassRootSeed != null)
        {
            QuestTracker.DesertGrassRootSeed(amount);
        }
        if (i_data.ItemName == "Desert Grass Root" && QuestTracker.DesertGrassRoot != null)
        {
            QuestTracker.DesertGrassRoot(amount);
        }
        if (i_data.ItemName == "Dried Desert Grass Root" && QuestTracker.DriedDesertGrassRoot != null)
        {
            QuestTracker.DriedDesertGrassRoot(amount);
        }


        //Giant Jungle Orchid stuffs
        if (i_data.ItemName == "Giant Jungle Orchid Seed" && QuestTracker.GiantJungleOrchidSeed != null)
        {
            QuestTracker.GiantJungleOrchidSeed(amount);
        }
        if (i_data.ItemName == "Giant Jungle Orchid " && QuestTracker.GiantJungleOrchid != null)
        {
            QuestTracker.GiantJungleOrchid(amount);
        }
        if (i_data.ItemName == "Dried Giant Jungle Orchid" && QuestTracker.DriedGiantJungleOrchid != null)
        {
            QuestTracker.DriedGiantJungleOrchid(amount);
        }


        //Thorny Jungle Vine
        if (i_data.ItemName == "Thorny Jungle Vine Seed" && QuestTracker.ThornyJungleVineSeed != null)
        {
            QuestTracker.ThornyJungleVineSeed(amount);
        }
        if (i_data.ItemName == "Thorny Jungle Vine" && QuestTracker.ThornyJungleVine != null)
        {
            QuestTracker.ThornyJungleVine(amount);
        }
        if (i_data.ItemName == "Dried Thorny Jungle Vine" && QuestTracker.DriedThornyJungleVine != null)
        {
            QuestTracker.DriedThornyJungleVine(amount);
        }


        //Strong Forest Herb
        if (i_data.ItemName == "Strong Forest Herb Seed" && QuestTracker.StrongForestHerbSeed != null)
        {
            QuestTracker.StrongForestHerbSeed(amount);
        }
        if (i_data.ItemName == "Strong Forest Herb" && QuestTracker.StrongForestHerb != null)
        {
            QuestTracker.StrongForestHerb(amount);
        }
        if (i_data.ItemName == "Dried Strong Forest Herb" && QuestTracker.DriedStrongForestHerb != null)
        {
            QuestTracker.DriedStrongForestHerb(amount);
        }


        //Glowing Oceanic Fungi
        if (i_data.ItemName == "Glowing Oceanic Fungi Seed" && QuestTracker.GlowingOceanicFungiSeed != null)
        {
            QuestTracker.GlowingOceanicFungiSeed(amount);
        }
        if (i_data.ItemName == "Glowing Oceanic Fungi" && QuestTracker.GlowingOceanicFungi != null)
        {
            QuestTracker.GlowingOceanicFungi(amount);
        }
        if (i_data.ItemName == "Dried Glowing Oceanic Fungi" && QuestTracker.DriedGlowingOceanicFungi != null)
        {
            QuestTracker.DriedGlowingOceanicFungi(amount);
        }


        //Pulsating Cave Moss
        if (i_data.ItemName == "Pulsating Cave Moss Seed" && QuestTracker.PulsatingCaveMossSeed != null)
        {
            QuestTracker.PulsatingCaveMossSeed(amount);
        }
        if (i_data.ItemName == "Pulsating Cave Moss" && QuestTracker.PulsatingCaveMoss != null)
        {
            QuestTracker.PulsatingCaveMoss(amount);
        }
        if (i_data.ItemName == "Dried Pulsating Cave Moss" && QuestTracker.DriedPulsatingCaveMoss != null)
        {
            QuestTracker.DriedPulsatingCaveMoss(amount);
        }


        //Carnivorous Cavern Vine
        if (i_data.ItemName == "Carnivorous Cavern Vine Seed" && QuestTracker.CarnivorousCavernVineSeed != null)
        {
            QuestTracker.CarnivorousCavernVineSeed(amount);
        }
        if (i_data.ItemName == "Carnivorous Cavern Vine" && QuestTracker.CarnivorousCavernVine != null)
        {
            QuestTracker.CarnivorousCavernVine(amount);
        }
        if (i_data.ItemName == "Dried Carnivorus Cavern Vine" && QuestTracker.DriedCarnivorousCavernVine != null)
        {
            QuestTracker.DriedCarnivorousCavernVine(amount);
        }


        //Fiery Desert Moss
        if (i_data.ItemName == "Fiery Desert Moss Seed" && QuestTracker.FieryDesertMossSeed != null)
        {
            QuestTracker.FieryDesertMossSeed(amount);
        }
        if (i_data.ItemName == "Fiery Desert Moss" && QuestTracker.FieryDesertMoss != null)
        {
            QuestTracker.FieryDesertMoss(amount);
        }
        if (i_data.ItemName == "Dried Fiery Desert Moss" && QuestTracker.DriedFieryDesertMoss != null)
        {
            QuestTracker.DriedFieryDesertMoss(amount);
        }


        #endregion



        //PLANT GRADE/TIER
        if (i_data.tier == ItemData.Tier.Bronze && QuestTracker.BronzeTier != null)
        {
            QuestTracker.BronzeTier(amount);
        }
        if (i_data.tier == ItemData.Tier.Silver && QuestTracker.SilverTier != null)
        {
            QuestTracker.SilverTier(amount);
        }
        if (i_data.tier == ItemData.Tier.Gold && QuestTracker.GoldTier != null)
        {
            QuestTracker.GoldTier(amount);
        }


        //BUY PET
        if (i_data.ItemName == "Dog" && QuestTracker.BuyDog != null)
        {
            QuestTracker.BuyDog(1);//we can only buy 1
        }
        if (i_data.ItemName == "Cat" && QuestTracker.BuyCat != null)
        {
            QuestTracker.BuyCat(1);//we can only buy 1
        }
        if (i_data.ItemName == "Horse" && QuestTracker.BuyHorse != null)
        {
            QuestTracker.BuyHorse(1);//we can only buy 1
        }
        if (i_data.ItemName == "Fox" && QuestTracker.BuyFox != null)
        {
            QuestTracker.BuyFox(1);//we can only buy 1
        }
        if (i_data.ItemName == "Turtle" && QuestTracker.BuyTurtle != null)
        {
            QuestTracker.BuyTurtle(1);//we can only buy 1
        }
        if (i_data.ItemName == "Rabbit" && QuestTracker.BuyRabit != null)
        {
            QuestTracker.BuyRabit(1);//we can only buy 1
        }



        //BUY AND SELL FROM STORE: buying can be checked in inventory.cs after the player bought it
        //but SELLING can be only checked in TransactionManager.cs (ConfirmTransaction function) 



        #region QUEST_SPECIFICS
        //quest 3 Silver Strong Forest Herb
        if (i_data.ItemName == "Strong Forest Herb" && i_data.tier == ItemData.Tier.Silver && 
            QuestTracker.SilverStrongForestHerb != null)
        {
            QuestTracker.SilverStrongForestHerb(amount);
        }


        //Quest 4: Gold Grass Root
        if (i_data.ItemName == "Grass Root" && i_data.tier == ItemData.Tier.Gold &&
            QuestTracker.GoldGrassRoot != null)
        {
            QuestTracker.GoldGrassRoot(amount);
        }
        //Quest 4: Gold Desert Grass Root
        if (i_data.ItemName == "Desert Grass Root" && i_data.tier == ItemData.Tier.Gold &&
            QuestTracker.GoldDesertGrassRoot != null)
        {
            QuestTracker.GoldDesertGrassRoot(amount);
        }


        //Quest 5: Gold Glowing Oceanic Fungi
        if (i_data.ItemName == "Glowing Oceanic Fungi" && i_data.tier == ItemData.Tier.Gold &&
    QuestTracker.GoldGlowingOceanicFungi != null)
        {
            QuestTracker.GoldGlowingOceanicFungi(amount);
        }
        //Quest 5: Silver Glowing Oceanic Fungi
        if (i_data.ItemName == "Glowing Oceanic Fungi" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverGlowingOceanicFungi != null)
        {
            QuestTracker.SilverGlowingOceanicFungi(amount);
        }


        //Quest 6: Silver Thorny Jungle Vine
        if (i_data.ItemName == "Thorny Jungle Vine" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverThornyJungleVine != null)
        {
            QuestTracker.SilverThornyJungleVine(amount);
        }

        //Quest 6: Silver Carnivorous Cavern Vine
        if (i_data.ItemName == "Carnivorous Cavern Vine" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverCarnivorousCavernVine != null)
        {
            QuestTracker.SilverCarnivorousCavernVine(amount);
        }

        //Quest 6: Silver Pulsating Cave Moss
        if (i_data.ItemName == "Pulsating Cave Moss" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverPulsatingCaveMoss != null)
        {
            QuestTracker.SilverPulsatingCaveMoss(amount);
        }


        //Quest 8: Gold Fragrant Orchid
        if (i_data.ItemName == "Fragrant Orchid" && i_data.tier == ItemData.Tier.Gold &&
    QuestTracker.GoldFragrantOrchid != null)
        {
            QuestTracker.GoldFragrantOrchid(amount);
        }


        //Quest 11: Gold Pulsating Cave Moss
        if (i_data.ItemName == "Pulsating Cave Moss" && i_data.tier == ItemData.Tier.Gold &&
    QuestTracker.GoldPulsatingCaveMoss != null)
        {
            QuestTracker.GoldPulsatingCaveMoss(amount);
        }


        //Quest 12: Silver Grass Root
        if (i_data.ItemName == "Grass Root" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverGrassRoot != null)
        {
            QuestTracker.SilverGrassRoot(amount);
        }
        //Quest 12: Silver Desert Grass Root
        if (i_data.ItemName == "Desert Grass Root" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverDesertGrassRoot != null)
        {
            QuestTracker.SilverDesertGrassRoot(amount);
        }



        //Quest 13: Gold Giant Jungle Orchid
        if (i_data.ItemName == "Giant Jungle Orchid " && i_data.tier == ItemData.Tier.Gold &&
    QuestTracker.GoldGiantJungleOrchid != null)
        {
            QuestTracker.GoldGiantJungleOrchid(amount);
        }


        //Quest 14: Gold Strong Forest Herb
        if (i_data.ItemName == "Strong Forest Herb" && i_data.tier == ItemData.Tier.Gold &&
    QuestTracker.GoldStrongForestHerb != null)
        {
            QuestTracker.GoldStrongForestHerb(amount);
        }
        //Quest 14: Silver Giant Jungle Orchid
        if (i_data.ItemName == "Giant Jungle Orchid " && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverGiantJungleOrchid != null)
        {
            QuestTracker.SilverGiantJungleOrchid(amount);
        }


        //Quest 15: Silver Fragrant Orchid
        if (i_data.ItemName == "Fragrant Orchid" && i_data.tier == ItemData.Tier.Silver &&
    QuestTracker.SilverFragrantOrchid != null)
        {
            QuestTracker.SilverFragrantOrchid(amount);
        }


        //Quest 18: Gold Carnivorous Cavern Vine
        if (i_data.ItemName == "Carnivorous Cavern Vine" && i_data.tier == ItemData.Tier.Gold &&
    QuestTracker.GoldCarnivorousCavernVine != null)
        {
            QuestTracker.GoldCarnivorousCavernVine(amount);
        }
        #endregion
    }

    //amount is the amount of coins earned
    private void HandleDelegateForCoin(int amount)
    {
        if(QuestTracker.EarnCoin != null)
        {
            QuestTracker.EarnCoin(amount);
        }

        if(QuestTracker.TotalCoin != null)
        {
            QuestTracker.TotalCoin(Coins);
        }
    }


}
