using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoLosesWhenElephantsFight : MonoBehaviour, SingleQuestInterface
{
    const int GOLD_GRASS_ROOT = 0;
    const int GOLD_DERSERT_GRASS_ROOT = 1;

    Quest quest;
    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGoldGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(GOLD_GRASS_ROOT, amount);//update parameter name
        return amount;
    }

    int PlayerGatherGoldDesertGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(GOLD_DERSERT_GRASS_ROOT, amount);//update parameter name
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
        QuestTracker.GoldGrassRoot += PlayerGatherGoldGrassRoot;
        QuestTracker.GoldDesertGrassRoot += PlayerGatherGoldDesertGrassRoot;

    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GoldGrassRoot -= PlayerGatherGoldGrassRoot;
        QuestTracker.GoldDesertGrassRoot -= PlayerGatherGoldDesertGrassRoot;
    }
    public bool IsActivated()
    {
        return activated;
    }
}