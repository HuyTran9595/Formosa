using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetFood : ItemData
{
    //32378-meat-on-bone-icon
    public PetFoodQuality Quality;
    public float Duration;
    public int Affection;
    public enum PetFoodQuality
    {
        Basic,
        Standard,
        Premium
    };

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is PetFood)
        {
            PetFood d = (PetFood)des;
            d.Quality = this.Quality;
            d.Duration = this.Duration;
            d.Affection = this.Affection;
        }
    }
}