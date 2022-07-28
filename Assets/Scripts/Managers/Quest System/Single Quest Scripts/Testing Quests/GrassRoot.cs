using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRoot : MonoBehaviour, SingleQuestInterface
{
    const int GRASS_ROOT = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerGatherGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(GRASS_ROOT, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.GrassRoot += PlayerGatherGrassRoot;
    }

    public void Unsubscribe()
    {
        QuestTracker.GrassRoot -= PlayerGatherGrassRoot;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

