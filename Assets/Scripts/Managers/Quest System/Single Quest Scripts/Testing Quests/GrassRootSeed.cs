using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRootSeed : MonoBehaviour, SingleQuestInterface
{
    const int GRASS_ROOT_SEED = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerGatherGrassRootSeed(int amount)
    {
        quest.UpdateQuestProgress(GRASS_ROOT_SEED, amount);
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.GrassRootSeed += PlayerGatherGrassRootSeed;
    }

    public void Unsubscribe()
    {
        QuestTracker.GrassRootSeed -= PlayerGatherGrassRootSeed;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

