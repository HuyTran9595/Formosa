using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Product", menuName = "Item/Product", order = 2)]
public class Products : ScriptableObject
{
    public int Unlocked;
    public float ProcessTime;
    public List<ItemData> Components;
    public ItemData Produced;
}
