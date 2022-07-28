using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TJayEnums;
public class Shop : MonoBehaviour
{
    [Header("Seeds")]
    public GameObject StoreButton;
    public List<int> SoldItems;
    [Header("Pets")]
    public List<PetData> PetSold;

    public List<int> SoldPetFood;
    public GameObject PetSlot;

    //TJay stuff
    public List<ItemData> items = new List<ItemData>();
    public List<PetData> pets = new List<PetData>();
    public List<PetFood> petFood = new List<PetFood>();

    public GameObject displayContent = null;
    public RowItem rowItemPrefab = null;
    [SerializeField]
    private ListType currentList = ListType.Seeds;
    public Text descriptionText = null;
    public RowItem selectedRowItem = null;
    private Queue<RowItem> availableRows = new Queue<RowItem>();
    public GameObject targetPanel = null;
    public Text SelectedText = null;
    public Text SelectedShadowText = null;
    public Text SelectedForgroundText = null;
    public GameObject SelectionPanel = null;
    public StationManager station = null;
    public List<Button> selectedButtons = new List<Button>();

    int index = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (SoldItems.Count > 0)
        {
            foreach (int Data in SoldItems)
            {
                items.Add(DatabaseScript.GetItem(Data));
                //Create button
                //GameObject SB = Instantiate(StoreButton, gameObject.transform);
                //SB.GetComponent<Store_Slot>().HeldItem = DatabaseScript.GetItem(Data.ID);
            }
        }

        if (PetSold.Count > 0)
        {
            foreach (PetData P_Data in PetSold)
            {
                //  GameObject PS = Instantiate(PetSlot, gameObject.transform);
                //  PS.GetComponent<Pet_Shop_Slot>().PetStuff = P_Data;
                pets.Add(P_Data);
            }
        }

        if (SoldPetFood.Count > 0)
        {
            foreach (int Food in SoldPetFood)
            {
                petFood.Add(DatabaseScript.GetPetFood(Food));
            }
        }
    }

    void Start()
    {
        gameObject.SetActive(false); //Added Awake so Level.cs can find the shop before inactive
    }

    //private void OnEnable()
    //{

    //   ChangeSelectedList(0);
    //   UpdateContent();
    //}

    public void ChangeSelectedList(int newList)
    {
        if (newList == -1)
            newList = (int)currentList;

        currentList = (ListType)newList;
        UpdateContent();

     

        if (station != null)
        {
            station.SetCurrentMenu(7);
        }
        UpdateButtons();
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
                    Color transparent = new Color(1, 1, 1, 0.5f);
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
    public void UpdateContent(bool hideSelector = false)
    {
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
        if (displayContent != null)
        {
            if (rowItemPrefab != null)
            {
                availableRows.Clear();
                for (int i = 0; i < displayContent.transform.childCount; i++)
                {
                    RowItem possibleItem = displayContent.transform.GetChild(i).GetComponent<RowItem>();
                    if (possibleItem != null)
                    {
                        possibleItem.gameObject.SetActive(false);
                        availableRows.Enqueue(possibleItem);
                    }
                }
                UpdatedSelectedText("Buying: " + currentList.ToString());
                switch (currentList)
                {
                    case ListType.Seeds:
                        {
                            if (items.Count == 0)
                            {
                                RowItem seedItem = GetNewRowItem();
                                if (seedItem != null)
                                {

                                    seedItem.gameObject.SetActive(true);
                                    seedItem.LoadDefault();
                                    if (descriptionText != null)
                                    {
                                        descriptionText.text = "";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < items.Count; i++)
                                {
                                    if (items[i].ItemType == ItemData.Type.Seed)
                                    {

                                        RowItem seedItem = GetNewRowItem();
                                        if (seedItem != null)
                                        {
                                            seedItem.myIcon.sprite = items[i].Icon;
                                            seedItem.nameText.text = items[i].ItemName;
                                            seedItem.countText.text = "" + items[i].Price + " ST";
                                            seedItem.myIcon.color = Color.white;
                                            seedItem.referenceData = items[i];
                                            if (seedItem.myInventory == null)
                                            {
                                                seedItem.myInventory = GameObject.FindObjectOfType<Inventory>();
                                            }
                                            seedItem.Setup();
                                            seedItem.transform.SetSiblingIndex(i);
                                        }
                                    }
                                }

                            }
                        }
                        break;
                    case ListType.Recipes:
                        {
                            if (items.Count == 0)
                            {
                                RowItem recipeItem = GetNewRowItem();
                            
                                if (recipeItem != null)
                                {

                                    recipeItem.gameObject.SetActive(true);
                                    recipeItem.LoadDefault();
                                    if (descriptionText != null)
                                    {
                                        descriptionText.text = "";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < items.Count; i++)
                                {
                                    if (items[i].ItemType == ItemData.Type.Recipe || items[i].ItemType == ItemData.Type.Potion)
                                    {

                                        RowItem seedItem = GetNewRowItem();
                                        if (seedItem != null)
                                        {
                                            seedItem.myIcon.sprite = items[i].Icon;
                                            seedItem.nameText.text = items[i].ItemName;
                                            seedItem.countText.text = "" + items[i].Price + " ST";
                                            seedItem.myIcon.color = Color.white;
                                            seedItem.referenceData = items[i];
                                            if (seedItem.myInventory == null)
                                            {
                                                seedItem.myInventory = GameObject.FindObjectOfType<Inventory>();
                                            }
                                            seedItem.Setup();
                                            seedItem.transform.SetSiblingIndex(i);
                                        }
                                    }
                                }

                            }
                        }
                        break;

                    case ListType.Pets:
                        {
                            if (pets.Count == 0)
                            {
                                RowItem dataPet = GetNewRowItem();
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

                                        RowItem dataPet = GetNewRowItem();
                                        if (dataPet != null)
                                        {
                                            dataPet.myIcon.sprite = pets[i].Icon;
                                            dataPet.nameText.text = pets[i].ItemName;
                                            dataPet.countText.text = "" + pets[i].Price + " ST";
                                            dataPet.myIcon.color = Color.white;
                                            dataPet.referencePet = pets[i];
                                            if (dataPet.myInventory == null)
                                            {
                                                dataPet.myInventory = GameObject.FindObjectOfType<Inventory>();
                                            }
                                            dataPet.Setup();
                                        }
                                    }
                                }

                            }
                        }
                        break;
                    case ListType.PetFoods:
                        {
                            if (petFood.Count == 0)
                            {
                                RowItem dataFood = GetNewRowItem();
                                if (dataFood != null)
                                {
                                    dataFood.LoadDefault();
                                    if (descriptionText != null)
                                    {
                                        descriptionText.text = "";
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < petFood.Count; i++)
                                {
                                    //if (magics[i].CurrHeld > 0)
                                    {
                                        RowItem dataFood = GetNewRowItem();
        
                                        if (dataFood != null)
                                        {
                                            dataFood.myIcon.sprite = petFood[i].Icon;
                                            dataFood.nameText.text = petFood[i].ItemName;
                                            dataFood.countText.text = "" + petFood[i].Price + " ST";
                                            dataFood.myIcon.color = Color.white;
                                            dataFood.referencePetFood = petFood[i];

                                            if (dataFood.myInventory == null)
                                            {
                                                dataFood.myInventory = GameObject.FindObjectOfType<Inventory>();
                                            }
                                            dataFood.Setup();
                                        }
                                    }
                                }

                            }
                        }
                        break;
                    default:
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                if (items[i].ItemType != ItemData.Type.Seed && items[i].ItemType != ItemData.Type.Recipe)
                                {

                                    RowItem seedItem = GetNewRowItem();
                                    if (seedItem != null)
                                    {
                                        seedItem.gameObject.SetActive(true);
                                        seedItem.myIcon.sprite = items[i].Icon;
                                        seedItem.nameText.text = items[i].ItemName;
                                        seedItem.countText.text = "" + items[i].Price + " ST";
                                        seedItem.myIcon.color = Color.white;
                                        seedItem.referenceData = items[i];
                                        seedItem.referencePet = null;
                                        if (seedItem.myInventory == null)
                                        {
                                            seedItem.myInventory = GameObject.FindObjectOfType<Inventory>();
                                        }
                                        seedItem.Setup();
                                        seedItem.transform.SetSiblingIndex(i);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("Can't find prefab");
            }
        }
        else
        {
            Debug.Log("Can't find content");
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

    public void SoldPet(PetData pet)
    {
        int index = pets.IndexOf(pet);
        if (index > -1) // found
        {
            pets.RemoveAt(index);
            UpdateContent();
        }
        else
        {
            Debug.Log("Cant find the pet");
        }
    }



    RowItem GetNewRowItem()
    {
        RowItem result = null;
        if (availableRows.Count > 0)
        {
            result = availableRows.Dequeue();
            result.CleanAllData();
            result.gameObject.SetActive(true);
            result.transform.SetParent(null);
            result.transform.SetParent(displayContent.transform);
        }
        else
        {
            result = Instantiate(rowItemPrefab, displayContent.transform).GetComponent<RowItem>();
            result.name = index.ToString();
            index++;
        }

        if (result == null)
            Debug.Log("Its null");
        return result;
    }
}
