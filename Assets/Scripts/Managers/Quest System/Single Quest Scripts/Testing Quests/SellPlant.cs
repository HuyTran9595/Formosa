using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellPlant : MonoBehaviour, SingleQuestInterface
{
    const int SELL_PLANT = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerSellPlant(int amount)
    {
        quest.UpdateQuestProgress(SELL_PLANT, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.SellPlant += PlayerSellPlant;
    }

    public void Unsubscribe()
    {
        QuestTracker.SellPlant -= PlayerSellPlant;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

