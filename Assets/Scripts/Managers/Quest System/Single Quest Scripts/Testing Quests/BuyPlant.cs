using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPlant : MonoBehaviour, SingleQuestInterface
{
    const int BUY_PLANT = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerBuyPlant(int amount)
    {
        quest.UpdateQuestProgress(BUY_PLANT, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.BuyPlant += PlayerBuyPlant;
    }

    public void Unsubscribe()
    {
        QuestTracker.BuyPlant -= PlayerBuyPlant;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

