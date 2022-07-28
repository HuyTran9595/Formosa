using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragrantOrchid : MonoBehaviour, SingleQuestInterface
{
    const int FRAGRANT_ORCHID = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;
    public bool activated = false;
    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerGatherFragrantOrchid(int amount)
    {
        quest.UpdateQuestProgress(FRAGRANT_ORCHID, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.FragrantOrchid += PlayerGatherFragrantOrchid;
    }

    public void Unsubscribe()
    {
        QuestTracker.FragrantOrchid -= PlayerGatherFragrantOrchid;
    }
    public bool IsActivated()
    {
        return activated;
    }
}

