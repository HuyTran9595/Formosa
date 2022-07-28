using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet_Slots : MonoBehaviour
{
    public PetData Stats;
    Pet_AI Pet;
    public Image PetIcon;
    public int ID;
    Pet_Holder_Script SpawnPoint;
    public void Start()
    {
        Pet = FindObjectOfType<Pet_AI>();
        SpawnPoint = FindObjectOfType<Pet_Holder_Script>();
        ID = Stats.ID;
    }
    public void Update()
    {
        if(Stats != null)
        {
            //Change Image to PetData Icon.
            PetIcon.sprite = Stats.Icon;
        }
    }
    public void OnMouseDown()
    {
        if (Stats != Pet.petData)
        {
            Debug.Log(Stats.ToString());
            Pet.petData = Stats;
            Pet.PetChange();
        }
    }
}
