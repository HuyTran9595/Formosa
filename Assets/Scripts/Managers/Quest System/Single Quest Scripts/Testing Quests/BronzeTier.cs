using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeTier : MonoBehaviour, SingleQuestInterface
{
    const int BRONZE_TIER = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerGatherBronzeTier(int amount)
    {
        quest.UpdateQuestProgress(BRONZE_TIER, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.BronzeTier += PlayerGatherBronzeTier;
    }

    public void Unsubscribe()
    {
        QuestTracker.BronzeTier -= PlayerGatherBronzeTier;
    }

    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

