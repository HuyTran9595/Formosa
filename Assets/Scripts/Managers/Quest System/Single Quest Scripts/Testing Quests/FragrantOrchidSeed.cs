using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragrantOrchidSeed : MonoBehaviour, SingleQuestInterface
{
    const int FRAGRANT_ORCHID_SEED = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    public bool activated = false;
    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerGatherFragrantOrchidSeed(int amount)
    {
        quest.UpdateQuestProgress(FRAGRANT_ORCHID_SEED, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.FragrantOrchidSeed += PlayerGatherFragrantOrchidSeed;
    }

    public void Unsubscribe()
    {
        QuestTracker.FragrantOrchidSeed -= PlayerGatherFragrantOrchidSeed;
    }
    public bool IsActivated()
    {
        return activated;
    }
}

