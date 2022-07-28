using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnCoin : MonoBehaviour, SingleQuestInterface
{
    const int EARN_COIN = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerEarnCoin(int amount)
    {
        quest.UpdateQuestProgress(EARN_COIN, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.EarnCoin += PlayerEarnCoin;
    }

    public void Unsubscribe()
    {
        QuestTracker.EarnCoin -= PlayerEarnCoin;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

