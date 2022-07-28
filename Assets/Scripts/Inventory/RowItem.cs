using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TJayEnums;
using UnityEngine.EventSystems;

public class RowItem : MonoBehaviour, IPointerEnterHandler
{
    public Image myIcon = null;
    public Text nameText = null;
    public Text countText = null;
    public ItemData referenceData = null;
    public PetData referencePet = null;
    public PetFood referencePetFood = null;
    public Inventory myInventory = null;
    public InteractionHandler theHandler = null;
    public TilePanel tilePanel = null;
    public Button myButton = null;
    public bool isSetup = false;
    private Shop theShop = null;
    private Journal theJournal = null;
    public Image myBackgroundImage = null;

    public TransactionManager TM;
    private void Start()
    {
        myBackgroundImage = GetComponent<Image>();
        TM = FindObjectOfType<TransactionManager>();
    }
    public void Setup()
    {
        if (!isSetup)
        {
            if (myButton == null)
            {
                myButton = GetComponent<Button>();
            }
            if (myButton != null)
            {
                myButton.onClick.AddListener(PressedItem);
            }
            isSetup = true;
        }
    }
    private Color transparent = new Color(0, 0, 0, 0);
    public void LoadDefault()
    {
        if (myIcon != null)
        {
            myIcon.color = transparent;
        }
        if (nameText != null)
        {
            nameText.text = "No items to display";
        }
        if (countText != null)
        {
            countText.text = "";
        }

    }


    public void PressedItem()
    {
        if (myInventory != null && (referenceData != null || referencePet != null || referencePetFood != null) && myInventory.descriptionText != null)
        {
            if (referenceData != null)
                myInventory.descriptionText.text = referenceData.Description;

            if (referencePet != null)
                myInventory.descriptionText.text = "It's a " + referencePet.ItemName;

            if (referencePetFood != null)
                myInventory.descriptionText.text = "Premium " + referencePetFood.ItemName;
            if (myInventory.station != null)
            {
                if (theHandler == null)
                {
                    theHandler = GameObject.FindObjectOfType<InteractionHandler>();
                }

                if (tilePanel == null)
                    tilePanel = GameObject.FindObjectOfType<TilePanel>();


                switch (myInventory.station.currentMenu)
                {
                    case PlayerMenu.None:
                        break;
                    case PlayerMenu.GenericInventory:
                        {
                            if (referencePet != null)
                            {
                                if (myInventory.pet != null)
                                {
                                    if (myInventory.currentPet != referencePet)
                                    {


                                        myInventory.pet.petData = referencePet;
                                        myInventory.pet.PetChange();
                                        UpdateTasks(TaskType.usePet);

                                    }
                                }
                            }
                            else if (referenceData != null && referenceData.ItemType == ItemData.Type.Potion)
                            {
                                (referenceData as PotionData).Duration *= 60.0f;
                                PotionEffectManager.Instance.ActivePotions.Add(referenceData);
                                PotionEffectManager.Instance.ActiveEffects.Add((referenceData as PotionData).PotionType);
                                referenceData.Price = 0;
                                SellMe();
                            }
                        }
                        break;
                    case PlayerMenu.GenericMenu:
                        break;
                    case PlayerMenu.SelectSeed:
                        {
                            if (tilePanel != null)
                            {
                                List<ItemData> row = myInventory.itemDatas[(int)ListType.Seeds];

                                int index = row.IndexOf(referenceData);
                                if (index > -1)
                                {
                                    if (row[index].CurrHeld > 0)
                                    {
                                        if (theHandler.targetTile != null)
                                        {
                                            theHandler.targetTile.PlantSeed(referenceData);
                                        }
                                        myInventory.InvButPress();
                                        row[index].CurrHeld--;
                                    }
                                    tilePanel.refreshTile();
                                }
                            }
                        }
                        break;
                    case PlayerMenu.SelectPlant:
                        if (tilePanel != null)
                        {
                            List<ItemData> row = myInventory.itemDatas[(int)ListType.Plants];

                            int index = row.IndexOf(referenceData);
                            if (index > -1)
                            {
                                if (row[index].CurrHeld > 0)
                                {

                                    row[index].CurrHeld--;
                                    if (theHandler.targetTile != null)
                                    {
                                        theHandler.targetTile.ConvertPlant(referenceData);

                                        UpdateTasks(TJayEnums.TaskType.lab);

                                    }
                                    myInventory.InvButPress();
                                }
                            }
                            tilePanel.refreshTile();
                        }
                        break;
                    case PlayerMenu.SelectHerb:
                        {//selecting recipe for now
                            if (theHandler != null)
                            {
                                List<ItemData> row = myInventory.itemDatas[(int)ListType.Recipes];
                                if (row.Contains(referenceData))
                                {
                                    PotionMenu pm = GameObject.FindObjectOfType<PotionMenu>();
                                    if (pm != null)
                                    {
                                        //pm.LoadData(referenceData);
                                        //pm.ToggleMenu();
                                    }
                                    //int index = myInventory.plants.IndexOf(referenceData);
                                    //if (myInventory.plants[index].CurrHeld > 0)
                                    //{

                                    //    myInventory.plants[index].CurrHeld--;
                                    //    if (theHandler.targetTile != null)
                                    //    {
                                    //        theHandler.targetTile.ConvertPlant(referenceData);
                                    //    }
                                    //    myInventory.InvButPress();
                                    //}
                                }
                                // theHandler.gameObject.SetActive(false);
                            }
                        }
                        break;
                    case PlayerMenu.SelectPotion:
                        break;
                    case PlayerMenu.Buying:
                        {

                            if (theShop != null)
                            {
                                if (referenceData != null)
                                {
                                    if (TM == null)
                                    {
                                        Debug.Log("TM is null");
                                    }
                                    TM.AttemptBuy(referenceData);
                                    //if (myInventory.Coins >= referenceData.Price)
                                    //{
                                    //    myInventory.station.AttemptBuy(this);
                                    //}

                                }
                                else if (referencePet != null)
                                {
                                    TM.AttemptBuy(referencePet);
                                    //if (myInventory.Coins >= referencePet.Price)
                                    //{
                                    //    myInventory.station.AttemptBuy(this);
                                    //}

                                }
                                else if (referencePetFood != null)
                                {
                                    TM.AttemptBuy(referencePetFood);
                                    //if (myInventory.Coins >= referencePetFood.Price)
                                    //{
                                    //    myInventory.station.AttemptBuy(this);
                                    //}

                                }

                            }

                        }
                        break;
                    case PlayerMenu.Selling:
                        {
                            Debug.Log("Check");
                            /*
                            if (referenceData != null)
                            {

                                if (referenceData.CurrHeld > 0)
                                {
                                    myInventory.UpdateContent();
                                    myInventory.station.AttemptSell(this);
                                }

                            }
                            */
                        }
                        break;
                }
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myInventory != null && (referenceData != null || referencePet != null) && myInventory.descriptionText != null)
        {

            if (myInventory.station != null)
            {
                if (myInventory.station.currentMenu == PlayerMenu.Buying)
                {
                    if (theShop == null)
                    {
                        theShop = GameObject.FindObjectOfType<Shop>();
                    }
                    if (theShop != null)
                    {
                        if (theShop.descriptionText != null)
                        {
                            if (referenceData != null)
                                theShop.descriptionText.text = referenceData.Description;

                            if (referencePet != null)
                                theShop.descriptionText.text = "It's a " + referencePet.ItemName;
                        }
                    }
                }
                else if (myInventory.station.currentMenu == PlayerMenu.ViewingJournal)
                {
                    if (theJournal == null)
                    {
                        theJournal = GameObject.FindObjectOfType<Journal>();
                    }

                    if (theJournal != null)
                    {
                        theJournal.selectedItem = this;
                        if (theJournal.claimButton != null)
                        {
                            theJournal.claimButton.gameObject.SetActive(false);
                        }
                        if (referenceData != null)
                        {
                            if (referenceData.ItemType == ItemData.Type.Task)
                            {
                                theJournal.selectedName.text = referenceData.ItemName;
                                theJournal.selectedGenus.text = "Task Type: " + (referenceData as TaskData).TaskType;
                                theJournal.selectedTemp.text = "";//"Ideal Temp: " + referenceData.IdealTemperature;

                                theJournal.selectedGrowthTime.text = "Reward: " + (referenceData.Price) + " EXP!";
                                theJournal.selectedDescription.text = "Requirements: \n" + referenceData.Description;
                                if (theJournal.claimButton != null && (referenceData as TaskData).isFinished)
                                {
                                    theJournal.claimButton.gameObject.SetActive(true);
                                }
                            }
                            else
                            {


                                theJournal.selectedName.text = referenceData.ItemName;
                                if (referenceData is SeedData) theJournal.selectedGenus.text = "Genus: " + (referenceData as SeedData).genus.ToString().ToUpper();
                                if (referenceData is PlantData) theJournal.selectedGenus.text = "Genus: " + (referenceData as PlantData).genus.ToString().ToUpper();
                                if (referenceData is DryHerbData) theJournal.selectedGenus.text = "Genus: " + (referenceData as DryHerbData).genus.ToString().ToUpper();
                                if (referenceData.ItemType == ItemData.Type.Seed)
                                    theJournal.selectedTemp.text = "Ideal Temp: " + (referenceData as SeedData).IdealTemperature;
                                else
                                    theJournal.selectedTemp.text = "Sell Price: " + referenceData.Price;
                                if (referenceData is SeedData) theJournal.selectedGrowthTime.text = "Avg Process Time: " + ((referenceData as SeedData).ProcessTime * 60) + "s";
                                if (referenceData is PlantData) theJournal.selectedGrowthTime.text = "Avg Process Time: " + ((referenceData as PlantData).ProcessTime * 60) + "s";
                                theJournal.selectedDescription.text = referenceData.Description;
                            }
                        }
                        else if (referencePet != null)
                        {
                            theJournal.selectedName.text = referencePet.ItemName;
                            theJournal.selectedGenus.text = "Finds: ";
                            for (int i = 0; i < referencePet.Findable.Count; i++)
                            {
                                //ItemData temp = DatabaseScript.GetItem(referencePet.Findable[i].ID);
                                theJournal.selectedGenus.text += DatabaseScript.GetItem(referencePet.Findable[i]).ItemName;
                                if (i + 1 < referencePet.Findable.Count)
                                    theJournal.selectedGenus.text += ", ";
                            }
                            theJournal.selectedTemp.text = "Price: " + referencePet.Price;
                            theJournal.selectedGrowthTime.text = "Avg time to find seeds:" + referencePet.FindTimer + "s"; ;
                            theJournal.selectedDescription.text = "It's a " + referencePet.ItemName;
                        }
                        else
                        {
                            theJournal.ShowNone();
                        }
                    }
                }
                else
                {
                    if (referenceData != null)
                        myInventory.descriptionText.text = referenceData.Description;

                    if (referencePet != null)
                        myInventory.descriptionText.text = "It's a " + referencePet.ItemName;
                }
            }
        }
        else
        {
            if (myInventory != null)
            {
                if (myInventory.station.currentMenu == PlayerMenu.ViewingJournal)
                {
                    if (theJournal == null)
                    {
                        theJournal = GameObject.FindObjectOfType<Journal>();
                    }
                    if (theJournal != null)
                    {
                        theJournal.ShowNone();

                    }
                }
            }
        }
    }

    public void BuyMe(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            if (referenceData != null)
            {

                if (myInventory.Coins >= referenceData.Price && referenceData.CurrHeld < referenceData.MaxHeld)
                {
                    myInventory.CoinUpdate(-referenceData.Price);
                    myInventory.AddItem(DatabaseScript.GetItem(referenceData.ID));
                    UpdateTasks(TaskType.buy);
                    if (referenceData.ItemType == ItemData.Type.Recipe)
                    {
                        if (theShop != null)
                        {
                            if (theShop.items.Contains(referenceData))
                            {
                                theShop.items.Remove(referenceData);
                                theShop.UpdateContent();
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Player has attempted to buy but doesnt have enough coins");
                    break;
                }
            }
            else if (referencePetFood != null)
            {

                if (myInventory.Coins >= referencePetFood.Price && referencePetFood.CurrHeld < referencePetFood.MaxHeld)
                {
                    myInventory.CoinUpdate(-referencePetFood.Price);
                    myInventory.AddFood(DatabaseScript.GetPetFood(referencePetFood.ID));

                }
                else
                {
                    Debug.Log("Player has attempted to buy but doesnt have enough coins");
                    break;
                }
            }
            else if (referencePet != null)
            {
                if (myInventory.Coins >= referencePet.Price)
                {
                    myInventory.CoinUpdate(-referencePet.Price);
                    myInventory.AddPet(referencePet);
                    // myInventory.AddItem(DatabaseScript.GetItem(referencePet.ID));
                    if (theShop != null)
                    {
                        if (theShop.pets.Contains(referencePet))
                        {
                            theShop.pets.Remove(referencePet);
                            theShop.UpdateContent();
                        }
                    }
                    UpdateTasks(TaskType.ownPet);

                }
                else
                {
                    Debug.Log("Player has attempted to buy but doesnt have enough coins");
                    break;
                }
            }
        }
    }

    public void SellMe(int count = 1)
    {
        UpdateTasks(TaskType.sell);
        for (int i = 0; i < count; i++)
        {
            int type = (int)referenceData.ItemType;
            List<ItemData> row = myInventory.itemDatas[type];
            int index = row.IndexOf(referenceData);

            if (index > -1)
            {
                if (row[index].CurrHeld > 0)
                {
                    myInventory.CoinUpdate(referenceData.Price);
                    row[index].CurrHeld--;
                    countText.text = "x " + row[index].CurrHeld;
                }
            }
            else
            {
                Debug.Log("Player tried to sell item but inventory couldnt find it");
            }

            #region oldCode
            /*
            if (myInventory.seeds.Contains(referenceData) && myInventory.seeds[myInventory.seeds.IndexOf(referenceData)].CurrHeld > 0)
            {
                myInventory.Coins += referenceData.Price;
                myInventory.CoinUpdate();
                myInventory.seeds[myInventory.seeds.IndexOf(referenceData)].CurrHeld--;
                countText.text = "x " + myInventory.seeds[myInventory.seeds.IndexOf(referenceData)].CurrHeld;
            }
            else if (myInventory.plants.Contains(referenceData) && myInventory.plants[myInventory.plants.IndexOf(referenceData)].CurrHeld > 0)
            {
                myInventory.Coins += referenceData.Price;
                myInventory.CoinUpdate();
                myInventory.plants[myInventory.plants.IndexOf(referenceData)].CurrHeld--;
                countText.text = "x " + myInventory.plants[myInventory.plants.IndexOf(referenceData)].CurrHeld;
            }
            else if (myInventory.herbs.Contains(referenceData) && myInventory.herbs[myInventory.herbs.IndexOf(referenceData)].CurrHeld > 0)
            {
                myInventory.Coins += referenceData.Price;
                myInventory.CoinUpdate();
                myInventory.herbs[myInventory.herbs.IndexOf(referenceData)].CurrHeld--;
                countText.text = "x " + myInventory.herbs[myInventory.herbs.IndexOf(referenceData)].CurrHeld;
            }
            else if (myInventory.potions.Contains(referenceData) && myInventory.potions[myInventory.potions.IndexOf(referenceData)].CurrHeld > 0)
            {
                myInventory.Coins += referenceData.Price;
                myInventory.CoinUpdate();
                myInventory.potions[myInventory.potions.IndexOf(referenceData)].CurrHeld--;
                countText.text = "x " + myInventory.potions[myInventory.potions.IndexOf(referenceData)].CurrHeld;
            }
            else
            {
                Debug.Log("Player tried to sell item but inventory couldnt find it");

            }
            */
            #endregion
        }
    }

    public void UpdateTasks(TaskType operation = TaskType.buy)
    {
        if (theJournal == null)
        {
            theJournal = GameObject.FindObjectOfType<Journal>();
        }

        for (int i = 0; i < myInventory.taskObjectives.Count; i++)
        {
            ItemData someTask = myInventory.taskObjectives[i];

            TaskType ttype = (someTask as TaskData).TaskType;
            if (operation == ttype)
            {

                switch (ttype)
                {
                    case TaskType.buy:
                        {

                            //currently storing item id for tasks in ideal temperature
                            //checking to make sure player bought the specified item
                            if (referenceData != null)
                            {

                                if ((someTask as TaskData).IDForPlant == referenceData.ID)
                                {
                                    //evolution id being used to show wether or not a task has been completed
                                    (someTask as TaskData).isFinished = true;
                                    if (theJournal != null)
                                    {
                                        //notify the player
                                        theJournal.Pulse();

                                    }
                                }
                            }
                        }
                        break;
                    case TaskType.sell:
                        {
                            if ((someTask as TaskData).IDForPlant == referenceData.ID)
                            {
                                //evolution id being used to show wether or not a task has been completed
                                (someTask as TaskData).isFinished = true;
                                if (theJournal != null)
                                {
                                    //notify the player
                                    theJournal.Pulse();

                                }
                            }
                        }
                        break;
                    case TaskType.plant:
                        {
                            //Huy editted this part
                            //if ((someTask as TaskData).IDForPlant == referenceData.ID)
                            (someTask as TaskData).isFinished = true;
                            if (theJournal != null)
                            {
                                //notify the player
                                theJournal.Pulse();

                            }

                        }
                        break;
                    case TaskType.lab:
                        {

                            //currently storing item id for tasks in ideal temperature

                            if ((someTask as TaskData).IDForPlant == referenceData.ID)
                            {

                                (someTask as TaskData).isFinished = true;
                                if (theJournal != null)
                                {
                                    //notify the player
                                    theJournal.Pulse();

                                }
                            }
                        }
                        break;
                    case TaskType.magic:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (theJournal != null)
                            {
                                //notify the player
                                theJournal.Pulse();

                            }
                        }
                        break;
                    case TaskType.harvest:
                        {
                            //Huy editted this part
                            //if ((someTask as TaskData).IDForPlant == referenceData.ID)
                            (someTask as TaskData).isFinished = true;
                            if (theJournal != null)
                            {
                                //notify the player
                                theJournal.Pulse();

                            }






                        }
                        break;
                    case TaskType.temperature:
                        {
                            //Huy editted this part
                            //if ((someTask as TaskData).IDForPlant == referenceData.ID)
                            (someTask as TaskData).isFinished = true;
                            if (theJournal != null)
                            {
                                //notify the player
                                theJournal.Pulse();

                            }



                        }
                        break;
                    case TaskType.ownPet:
                        {
                            if (myInventory.itemDatas[(int)ListType.Pets].Count > 0)
                            {
                                (someTask as TaskData).isFinished = true;
                                if (theJournal != null)
                                {
                                    //notify the player
                                    theJournal.Pulse();

                                }
                            }
                        }
                        break;
                    case TaskType.usePet:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (theJournal != null)
                            {
                                //notify the player
                                theJournal.Pulse();

                            }
                        }
                        break;
                    case TaskType.getSeedFromPet:
                        {
                            (someTask as TaskData).isFinished = true;
                            if (theJournal != null)
                            {
                                //notify the player
                                theJournal.Pulse();

                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }


    }

    public void CleanAllData()
    {
        referenceData = null;
        referencePet = null;
        referencePetFood = null;
        nameText.text = "";
        countText.text = "";
        myIcon.sprite = null;
    }
}
