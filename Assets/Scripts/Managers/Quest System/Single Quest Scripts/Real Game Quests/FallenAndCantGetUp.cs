using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenAndCantGetUp : MonoBehaviour, SingleQuestInterface
{
    const int SILVER_STRONG_FOREST_HERB = 0;
    const int GRASS_ROOT_ENHANCER = 1;
    const int DESERT_GRASS_ROOT = 2;
    Quest quest;

    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherSilverStrongForestHerb(int amount)
    {
        quest.UpdateQuestProgress(SILVER_STRONG_FOREST_HERB, amount);//update parameter name
        return amount;
    }

    int PlayerGatherGrassRootEnhancer(int amount)
    {
        quest.UpdateQuestProgress(GRASS_ROOT_ENHANCER, amount);//update parameter name
        return amount;
    }

    int PlayerGatherDesertGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(DESERT_GRASS_ROOT, amount);//update parameter name
        return amount;
    }



    public void Subscribe()
    {
        if (activated)
        {
            Debug.Log("Quest already activated.");
            return;
        }
        activated = true;
        //Debug.Log("subscribe called");
        QuestTracker.SilverStrongForestHerb += PlayerGatherSilverStrongForestHerb;
        QuestTracker.GatherGrassRootEnhancer += PlayerGatherGrassRootEnhancer;
        QuestTracker.DesertGrassRoot += PlayerGatherDesertGrassRoot;

    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.SilverStrongForestHerb -= PlayerGatherSilverStrongForestHerb;
        QuestTracker.GatherGrassRootEnhancer -= PlayerGatherDesertGrassRoot;
        QuestTracker.DesertGrassRoot -= PlayerGatherDesertGrassRoot;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
