using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDog : MonoBehaviour, SingleQuestInterface
{
    const int BUY_DOG = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerBuyDog(int amount)
    {
        quest.UpdateQuestProgress(BUY_DOG, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.BuyDog += PlayerBuyDog;
    }

    public void Unsubscribe()
    {
        QuestTracker.BuyDog -= PlayerBuyDog;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

