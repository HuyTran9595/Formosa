using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Holder_Script : MonoBehaviour
{
    public List<PetData> Collection;
    public GameObject PetSlot;
    public List<GameObject> Slots;
    public void OnEnable()
    {
        foreach(PetData P_Data in Collection)
        {          
            if (Slots.Count <= 0)
            {
                GameObject Temp = Instantiate(PetSlot, gameObject.transform);
                Temp.GetComponent<Pet_Slots>().Stats = P_Data;
                Slots.Add(Temp);
            }
            int ColNum = Collection.IndexOf(P_Data);
            if (Slots[ColNum].GetComponent<Pet_Slots>().Stats != P_Data)
            {
                CreateSlot(P_Data);
            }
        }
    }
    public void CreateSlot(PetData SlotPet)
    {
        GameObject Temp = Instantiate(PetSlot, gameObject.transform);
        Temp.GetComponent<Pet_Slots>().Stats = SlotPet;
        Slots.Add(Temp);
    }
}
