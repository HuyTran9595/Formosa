using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler
{
    InventoryUI IUI;
    PotionMenu PUI;
    Button button;
    public ItemData ItemData { get; set; }
    public int index { get; set; }

    public Image item_icon;
    public TextMeshProUGUI amount_text;
    public Image tier_icon;

    public List<Sprite> TierIcons;
    public TextMeshProUGUI tier_text;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => clicked());
    }

    public Button GetButton() => button;

    public void SetInventoryUI(InventoryUI _UI) => IUI = _UI;
    public void SetPotionMenu(PotionMenu potionMenu) => PUI = potionMenu;

    public void clicked()
    {
        if (IUI)
            IUI.ButtonIsClicked(this);
        else if (PUI)
            PUI.OnRecipeButtonClick(this.ItemData as RecipeData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IUI)
            IUI.PointerEnterTheButton(this); 
    }

    public void activate(bool b)
    {
        if (b)
        {
            UpdateIcon();
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }

    public void UpdateIcon()
    {
        item_icon.sprite = ItemData.Icon;
        if (ItemData is PetData || ItemData is RecipeData)
            amount_text.gameObject.SetActive(false);
        else
        {
            amount_text.gameObject.SetActive(true);
            amount_text.SetText(ItemData.CurrHeld.ToString());
        }

        if (ItemData is DryHerbData || ItemData is PlantData)
        {
            tier_icon.gameObject.SetActive(true);
            if      (ItemData.tier == ItemData.Tier.Bronze)  tier_icon.sprite = TierIcons[0];
            else if (ItemData.tier == ItemData.Tier.Silver)  tier_icon.sprite = TierIcons[1];
            else if (ItemData.tier == ItemData.Tier.Gold)    tier_icon.sprite = TierIcons[2];
        }
        else
            tier_icon.gameObject.SetActive(false);

        if(ItemData is PotionData)
        {
            tier_text.gameObject.SetActive(true);
            if      (ItemData.tier == ItemData.Tier.None)  tier_text.SetText("I");
            else if (ItemData.tier == ItemData.Tier.Tier1) tier_text.SetText("II");
            else if (ItemData.tier == ItemData.Tier.Tier2) tier_text.SetText("III");
            else if (ItemData.tier == ItemData.Tier.Tier3) tier_text.SetText("IV");
            else if (ItemData.tier == ItemData.Tier.Tier4) tier_text.SetText("V");
        }
        else
            tier_text.gameObject.SetActive(false);
    }


    //public void setFunction(Func<int, ItemData,bool> function)
    //{
    //    button.onClick.AddListener(() => { function(index, ItemData); });
    //}

    //public void setFunction(Func<ItemData,bool> function)
    //{
    //    button.onClick.AddListener(() => { function(ItemData); });
    //}

    //public void removeFunction(Func<int, ItemData, bool> function)
    //{
    //    button.onClick.RemoveListener(() => { function(index, ItemData); });
    //}
    //public void removeFunction(Func<ItemData, bool> function)
    //{
    //    button.onClick.RemoveListener(() => { function(ItemData); });
    //}

    //public void removeFunction()
    //{
    //    button.onClick.RemoveAllListeners();
    //}


}
