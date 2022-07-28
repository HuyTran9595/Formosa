using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magic_Shop : MonoBehaviour
{
    public Inventory PInv;
    public bool Active = false;
    public GameObject MSM_Hold;
    public bool Pro = true;
    public GameObject Slots; //These are the slots that are created when the menus are opened
    [Header("Products")]
    public List<Products> Pro_Recipes; //All the Produce Recipes
    public GameObject ProHolder; //Transform position for the ProSlots
    public List<GameObject> ProSlots; //Produce Slots
    public GameObject ProducePanel;
    [Header("Potions")]
    public List<Potions> Pot_Recipes; //All the Potion Recipes
    public GameObject PotHolder; //Transform position for the PotSlots
    public List<GameObject> PotSlots; //Potion Slots
    public GameObject PotionPanel;

    void Start()
    {
        Active = false;
        PotionPanel.SetActive(!Pro);
        ProducePanel.SetActive(Pro);
        PInv = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NextButton()
    {
        Pro = !Pro;
        PotionPanel.SetActive(!Pro);
        ProducePanel.SetActive(Pro);
        if (Pro == true)
        {
            for (int i = 0; i < Pro_Recipes.Count; i++)
            {
                if (ProSlots[i].GetComponent<Recipe_Slots>().ProStat != Pro_Recipes[i])
                {
                    GameObject Temper = Instantiate(Slots, ProHolder.transform, false);
                    Temper.GetComponent<Recipe_Slots>().ProStat = Pro_Recipes[i];
                    ProSlots.Add(Temper);
                }
            }
        }
        else
        {
            if (PotSlots.Count <= 0)
            {
                for (int i = 0; i < Pot_Recipes.Count; i++)
                {
                    GameObject Temper = Instantiate(Slots, PotHolder.transform, false);
                    Temper.GetComponent<Recipe_Slots>().PotStats = Pot_Recipes[i];
                    PotSlots.Add(Temper);
                }
            }
            else
            {
                for (int i = 0; i < Pot_Recipes.Count; i++)
                {
                    if (PotSlots[i].GetComponent<Recipe_Slots>().PotStats != Pot_Recipes[i])
                    {
                        GameObject Temper = Instantiate(Slots, PotHolder.transform, false);
                        Temper.GetComponent<Recipe_Slots>().PotStats = Pot_Recipes[i];
                        PotSlots.Add(Temper);
                    }
                }
            }
        }
    }
    public void SetStore()
    {
        Active = !Active;
        MSM_Hold.SetActive(Active);
        Pro = true;
        if (ProSlots.Count <= 0)
        {
            for (int i = 0; i < Pro_Recipes.Count; i++)
            {
                GameObject Temper = Instantiate(Slots, ProHolder.transform, false);
                Temper.GetComponent<Recipe_Slots>().ProStat = Pro_Recipes[i];
                ProSlots.Add(Temper);
            }
        }
    }
}
