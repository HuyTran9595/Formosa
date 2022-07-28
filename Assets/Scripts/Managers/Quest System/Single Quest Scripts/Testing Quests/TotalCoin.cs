using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalCoin : MonoBehaviour, SingleQuestInterface
{
    const int TOTAL_COIN = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerTotalCoin(int coins)
    {
        quest.UpdateQuestProgress(TOTAL_COIN, coins, false);
        return coins;
    }

    public void Subscribe()
    {
        QuestTracker.TotalCoin += PlayerTotalCoin;
    }

    public void Unsubscribe()
    {
        QuestTracker.TotalCoin -= PlayerTotalCoin;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

