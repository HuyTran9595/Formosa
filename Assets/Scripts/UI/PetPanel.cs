using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetPanel : MonoBehaviour
{
    Pet_AI petAI;
    Pet_AI.PetStat petStat;

    //Affection Meter
    Text AffectionText;
    //Hunger Meter
    Slider hungerSlider;
    Text HungerText;
    //Search timer
    Text remainSearchTime;

    // Start is called before the first frame update
    void Start()
    {
        petAI = FindObjectOfType<Pet_AI>();
        AffectionText = transform.Find("Affection").GetComponent<Text>();
        HungerText = transform.Find("Hunger").transform.Find("RemainTime").GetComponent<Text>();
        remainSearchTime = transform.Find("Search").transform.Find("RemainTime").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        petStat = petAI.petStat;

        AffectionText.text = "Affection Level : " + petStat.AffectionLv.ToString() + " Progression : " + petStat.AffectionExp.ToString() + "/10";

        if(petStat.Hunger < 0)
        {
            HungerText.text = "Your pet is hungry now!";
        }
        else
        {
            HungerText.text = "Your pet won't hungry for : " + TimeToString(petStat.Hunger);
        }

        if (petStat.SearchRemainTime < 0)
        {
            remainSearchTime.text = "Ready to Collect!";
        }
        else
        {
            remainSearchTime.text = "Remain time : " + TimeToString(petStat.SearchRemainTime);
        }
    }

    string TimeToString(float time)
    {
        string output = "";
        int digit = 0;
        if(time > 3600) //more than 1 hour
        {
            digit = (int)(time / 3600f);
            time -= digit * 3600;
            output += digit.ToString() + " h ";
        }
        if (time > 60) // 1 minute
        {
            digit = (int)(time / 60f);
            time -= digit * 60;
            output += digit.ToString() + " m ";
        }
        output += ((int)time).ToString() + " s ";
        return output;
    }

}
