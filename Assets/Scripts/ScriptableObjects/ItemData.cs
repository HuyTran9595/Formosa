using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item Data", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    public const float SilverBonus = 1.1f;
    public const float GoldBonus = 1.25f; 
    public int ID;
    public string ItemName;
    public Type ItemType;
    public int Price;
    public string Description = "";
    public Sprite Icon;
    //public MeshFilter targetMesh;
    public int MaxHeld;
    public int unlock;
    public int CurrHeld = 0;
    public Tier tier;
    public enum Type { 
        Seed,
        Plant,
        DryHerb,
        Potion,
        Recipe,
        Pet,
        PetFood,
        Task
    };

    public enum Tier
    {
        None = -1,

        Bronze,
        Silver,
        Gold,

        Tier1,      //B + S
        Tier2,      //S + S or B + G
        Tier3,      //S + G
        Tier4,       //G + G
        //B = Bronze, S = Silver, G = Gold
        //Tier of the ingredient
    }

    public virtual void Copy(out ItemData des)
    {
        des = new ItemData();
        des.ID = this.ID;
        des.ItemName = this.ItemName;
        des.ItemType = this.ItemType;
        des.Price = this.Price;
        des.Description = this.Description;
        des.Icon = this.Icon;
        //des.targetMesh = this.targetMesh;
        des.MaxHeld = this.MaxHeld;
        des.unlock = this.unlock;
        des.CurrHeld = this.CurrHeld;
        des.tier = this.tier;
    }
}
