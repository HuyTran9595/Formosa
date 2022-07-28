using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromTheHorsesMouth : MonoBehaviour, SingleQuestInterface
{
    const int SILVER_GRASS_ROOT = 0;
    const int SILVER_DESERT_GRASS_ROOT = 1;
    const int GIANT_JUNGLE_ORCHID = 2;
    Quest quest;
    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherSilverGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(SILVER_GRASS_ROOT, amount);//update parameter name
        return amount;
    }
    int PlayerGatherSilverDesertGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(SILVER_DESERT_GRASS_ROOT, amount);//update parameter name
        return amount;
    }
    int PlayerGatherGiantJungleOrchid(int amount)
    {
        quest.UpdateQuestProgress(GIANT_JUNGLE_ORCHID, amount);//update parameter name
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
        QuestTracker.SilverGrassRoot += PlayerGatherSilverGrassRoot;
        QuestTracker.SilverDesertGrassRoot += PlayerGatherSilverDesertGrassRoot;
        QuestTracker.GiantJungleOrchid += PlayerGatherGiantJungleOrchid;
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.SilverGrassRoot -= PlayerGatherSilverGrassRoot;
        QuestTracker.SilverDesertGrassRoot -= PlayerGatherSilverDesertGrassRoot;
        QuestTracker.GiantJungleOrchid -= PlayerGatherGiantJungleOrchid;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
