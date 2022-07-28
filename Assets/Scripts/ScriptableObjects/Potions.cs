using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Potion", fileName = "New Potion", order = 3)]
public class Potions : ScriptableObject
{
    public int Unlocked;
    public float BrewTime;
    public int Multiplier;
    public List<ItemData> Components;
    public ItemData Produced;
}
