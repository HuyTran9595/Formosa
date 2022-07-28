using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PotionMenu : MonoBehaviour
{
    //public Image firstItem = null;
    //public Image secondItem = null;
    //public Image newPotion = null;
    //public Text potionDescription = null;
    //public Text firstAmount = null;
    //public Text secondAmount = null;
    ItemData first = null;
    ItemData second = null;
    ItemData pot = null;
    ItemData rec = null;
    public InteractionHandler theHandler = null;

    [Header("General")]
    public GameObject mainPanel = null;
    public GameObject recipeListPanel = null;
    public GameObject craftingPanel = null;
    public Inventory inventory = null;
    public RecipeData currentPotionRecipe = null;
    public PotionData currentPotionResult = null;
    public Transform recipeListContent = null;
    public Color highlightColor;
    public Color unhighlightColor;

    public GameObject emptyRecipeSign = null;
    [Header("Inventory Slot Prefab")]
    public GameObject inventoryItemPrefab;
    Utilities.ObjectPool<GameObject> buttonPool = new Utilities.ObjectPool<GameObject>();
    List<GameObject> buttonShowing = new List<GameObject>();

    [Header("First Ingredient")]
    public Image firstItemImage = null;
    public Text firstAmountText = null;
    public ItemData firstItemData;
    public GameObject firstItemTierList = null;
    List<Image> firstItemTierButtons = new List<Image>();
    int firstCurrentTier = 0;
    int firstCurrentHold;

    [Header("Second Ingredient")]
    public Image secondItemImage = null;
    public Text secondAmountText = null;
    public ItemData secondItemData;
    public GameObject secondItemTierList = null;
    List<Image> secondItemTierButtons = new List<Image>();
    int secondCurrentTier = 0;
    int secondCurrentHold;

    [Header("Product")]
    public Text[] potionName = new Text[3];
    public Text potionDescriptionText = null;
    public Image newPotionImage = null;
    public Text potionAmount = null;
    public Text potionTier = null;
    public Text potionMaxAmount = null;
    public int maxPossibleAmount = 0;
    public int currentPotionAmount = 0;



    private void Start()
    {
        if (firstItemTierList != null)
        {
            for (int i = 0; i < firstItemTierList.transform.childCount; i++)
            {
                firstItemTierButtons.Add(firstItemTierList.transform.GetChild(i).GetComponent<Image>());
            }
        }
        else
            Debug.Log("Error");

        if (secondItemTierList != null)
        {
            for (int i = 0; i < secondItemTierList.transform.childCount; i++)
            {
                secondItemTierButtons.Add(secondItemTierList.transform.GetChild(i).GetComponent<Image>());
            }
        }
        else
            Debug.Log("Error");
    }


    /*
    public void Craft()
    {
        if (first != null && second != null)
        {
            if (firstItem != null && secondItem != null)
            {
                if (first == second)
                {
                    List<ItemData> row = inventory.itemDatas[(int)TJayEnums.ListType.Herbs];
                    int index = row.IndexOf(first);

                    if(index > -1)
                    { 
                        if (row[index].CurrHeld >= 2)
                        {
                            row[index].CurrHeld -= 2;
                            if (theHandler == null)
                            {
                                theHandler = GameObject.FindObjectOfType<InteractionHandler>();
                            }

                            if (theHandler != null)
                            {
                                if (theHandler.targetTile != null && rec != null)
                                {
                                    theHandler.targetTile.ConvertPlant(rec);
                                    inventory.UpdateTasks(TJayEnums.TaskType.magic);

                                }
                            }
                            ToggleMenu();
                        }

                    }
                    else
                    {
                        //TODO
                    }
                }
            }
        }
    }
    */
    public void Cancel()
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (mainPanel != null)
        {
            mainPanel.SetActive(!mainPanel.activeInHierarchy);
        }
    }

    /*
    public void LoadData(ItemData recipe)
    {
        rec = recipe;
        first = null;
        second = null;
        pot = null;
        if (inventory == null)
        {
            inventory = GameObject.FindObjectOfType<Inventory>();
        }
        if (inventory != null)
        {

            if (recipe != null)
            {
                first = DatabaseScript.GetItem((recipe as RecipeData).FirstIngredient);
                second = DatabaseScript.GetItem((recipe as RecipeData).SecondIngredient);
                pot = DatabaseScript.GetItem((recipe as RecipeData).ProductID);
                if (first != null && second != null)
                {
                    if (firstItem != null && secondItem != null)
                    {
                        firstItem.sprite = first.Icon;
                        secondItem.sprite = second.Icon;
                        if (firstAmount != null && secondAmount != null)
                        {
                            List<ItemData> row = inventory.itemDatas[(int)TJayEnums.ListType.Herbs];
                            int index = row.IndexOf(first);
                            if(index > -1)
                            {
                                firstAmount.text = "Amount: " + row[index].CurrHeld;
                            }
                            else
                            {
                                firstAmount.text = "Amount: 0";
                            }
                            index = row.IndexOf(second);
                            if (index > -1)
                            {
                                secondAmount.text = "Amount: " + row[index].CurrHeld;
                            }
                            else
                            {
                                secondAmount.text = "Amount: 0";
                            }
                        }
                    }
                }
                if (pot != null && newPotion != null)
                {
                    newPotion.sprite = pot.Icon;
                    if (potionDescription != null)
                    {
                        potionDescription.text = pot.Description;
                    }
                }
            }
        }
    }
    */

    public void StartMenu()
    {
        mainPanel.gameObject.SetActive(true);
        craftingPanel.gameObject.SetActive(false);
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();

        setupRecipeList();

    }

    public void setCurrentPotion(RecipeData curr)
    {
        currentPotionRecipe = curr;
    }

    void setupCraftPanel()
    {
        if (currentPotionRecipe != null)
        {
            craftingPanel.SetActive(true);
            firstItemData = DatabaseScript.GetItem(currentPotionRecipe.FirstIngredient);
            secondItemData = DatabaseScript.GetItem(currentPotionRecipe.SecondIngredient);

            updateFirstIngredientUI();
            updateSecondIngredientUI();

            getMaxAmount();
            currentPotionAmount = 0;
            setCurrentAmountText();
        }
        else
            Debug.Log("Current potion is null");
    }

    void updateFirstIngredientUI()
    {
        firstItemImage.sprite = firstItemData.Icon;
        setFirstTier(0);

        if (firstItemData is DryHerbData || firstItemData is PlantData)
        {
            firstItemTierList.SetActive(true);
        }
        else
            firstItemTierList.SetActive(false);

    }

    void updateSecondIngredientUI()
    {
        secondItemImage.sprite = secondItemData.Icon;
        setSecondTier(0);

        if (secondItemData is DryHerbData || secondItemData is PlantData)
        {
            secondItemTierList.SetActive(true);
        }
        else
            secondItemTierList.SetActive(false);
    }

    void setFirstIngCount()
    {
        ItemData item = inventory.GetItemData(firstItemData.ID, (ItemData.Tier)firstCurrentTier);
        if (item != null)
        {
            firstAmountText.text = "Amount : " + item.CurrHeld.ToString();
            firstCurrentHold = item.CurrHeld;
        }
        else
        {
            firstAmountText.text = "Amount : 0";
            firstCurrentHold = 0;
        }
    }

    void setSecondIngCount()
    {
        ItemData item = inventory.GetItemData(secondItemData.ID, (ItemData.Tier)secondCurrentTier);
        if (item != null)
        {
            secondAmountText.text = "Amount : " + item.CurrHeld.ToString();
            secondCurrentHold = item.CurrHeld;
        }
        else
        {
            secondAmountText.text = "Amount : 0";
            secondCurrentHold = 0;
        }
    }

    void setFirstTierListIcon()
    {
        for (int i = 0; i < firstItemTierButtons.Count; i++)
        {
            if (i == firstCurrentTier)
                firstItemTierButtons[i].color = highlightColor;
            else
                firstItemTierButtons[i].color = unhighlightColor;
        }
    }
    void setSecondTierListIcon()
    {
        for (int i = 0; i < secondItemTierButtons.Count; i++)
        {
            if (i == secondCurrentTier)
                secondItemTierButtons[i].color = highlightColor;
            else
                secondItemTierButtons[i].color = unhighlightColor;
        }
    }
    void setProductName(string name)
    {
        for (int i = 0; i < potionName.Length; i++)
        {
            potionName[i].text = name;
        }
    }

    void setDescription(string des)
    {
        potionDescriptionText.text = des;
    }

    void setupRecipeList()
    {
        List<ItemData> recipes = inventory.GetAllRecipes();

        //Clean the list of the button first
        foreach (var button in buttonShowing)
        {
            button.SetActive(false);
            button.transform.SetParent(null);
            buttonPool.release(button);
        }
        buttonShowing.Clear();

        if (recipes.Count == 0)
        {
            emptyRecipeSign.SetActive(true);
        }
        else
        {
            emptyRecipeSign.SetActive(false);
            int recipeCount = recipes.Count;

            for (int i = 0; i < recipeCount; i++)
            {
                ItemData data = recipes[i];
                var temp = buttonPool.getNewItem(inventoryItemPrefab);
                InventoryItem item = temp.GetComponent<InventoryItem>();
                item.ItemData = data;
                item.index = i;
                item.activate(true);
                item.SetPotionMenu(this);
                temp.transform.SetParent(recipeListContent);
                buttonShowing.Add(temp);
            }
        }

    }

    void setProductTier(ItemData.Tier _tier)
    {
        string str = "";
        switch (_tier)
        {
            case ItemData.Tier.None:
                str = "I";
                break;
            case ItemData.Tier.Tier1:
                str = "II";
                break;
            case ItemData.Tier.Tier2:
                str = "III";
                break;
            case ItemData.Tier.Tier3:
                str = "IV";
                break;
            case ItemData.Tier.Tier4:
                str = "V";
                break;
            default:
                break;
        }
        potionTier.text = str;
    }

    void setupProduct(int ID, ItemData.Tier _tier = ItemData.Tier.None)
    {
        currentPotionResult = DatabaseScript.GetItem(ID, _tier) as PotionData;
        setProductTier(_tier);
    }

    public void OnRecipeButtonClick(RecipeData recipeData)
    {
        if (recipeData == null) Debug.LogError("No recipe Data");

        currentPotionRecipe = recipeData;
        setupProduct(currentPotionRecipe.ProductID);

        setupCraftPanel();
        setProductName(currentPotionResult.ItemName);
        setDescription(currentPotionResult.Description);
        ///Set up current recipe
        ///then set up the UI
        ///     Name
        ///     First ingredient
        ///     Second ingredient
        ///     Max amount can craft
    }

    public void Craft()
    {
        if(currentPotionAmount>0)
        {
            inventory.RemoveItem(DatabaseScript.GetItem(firstItemData.ID, (ItemData.Tier)firstCurrentTier), currentPotionAmount);
            inventory.RemoveItem(DatabaseScript.GetItem(secondItemData.ID, (ItemData.Tier)secondCurrentTier), currentPotionAmount);

            inventory.AddItem(currentPotionResult, currentPotionAmount);

            updateFirstIngredientUI();
            updateSecondIngredientUI();

            getMaxAmount();
            currentPotionAmount = 0;
            setCurrentAmountText();
        }
    }

    public void OnTierButtonClick(int num)
    {
        /// 0 1 2 for left
        /// 3 4 5 for right
        if (num < 3)
        {
            setFirstTier(num);
        }
        else
        {
            setSecondTier(num - 3);
        }

        setupProduct(currentPotionRecipe.ProductID, (ItemData.Tier)(firstCurrentTier + secondCurrentTier == 0 ? -1: 2 + firstCurrentTier + secondCurrentTier));
        getMaxAmount();
    }

    void setFirstTier(int tier)
    {
        firstCurrentTier = tier;
        setFirstTierListIcon();
        setFirstIngCount();
    }

    void setSecondTier(int tier)
    {
        secondCurrentTier = tier;
        setSecondTierListIcon();
        setSecondIngCount();
    }
    
    void getMaxAmount()
    {
        if (firstItemData.ID == secondItemData.ID && firstCurrentTier == secondCurrentTier)
        {
            maxPossibleAmount = firstCurrentHold / 2;
        }
        else
        {
            maxPossibleAmount = Mathf.Min(firstCurrentHold, secondCurrentHold);
        }

        potionMaxAmount.text = "Max : " + maxPossibleAmount.ToString();
        currentPotionAmount = 0;
        setCurrentAmountText();
    }

    public void Increment()
    {
        if(currentPotionAmount<maxPossibleAmount)
        {
            currentPotionAmount++;
            setCurrentAmountText();
        }
    }

    public void Decrement()
    {
        if(currentPotionAmount>0)
        {
            currentPotionAmount--;
            setCurrentAmountText();
        }
    }

    void setCurrentAmountText()
    {
        potionAmount.text = currentPotionAmount.ToString();
    }
}

///When player click the potion tile
///Set up the recipe list first
///then when click on recipe
///change the right panel