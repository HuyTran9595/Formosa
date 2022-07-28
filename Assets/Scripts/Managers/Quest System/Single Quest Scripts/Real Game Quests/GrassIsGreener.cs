using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo: change "ExampleQuest" to your quest name
//The file name is "ExampleQuest.cs" -> class name MUST BE "ExampleQuest"
public class GrassIsGreener : MonoBehaviour, SingleQuestInterface
{
    const int GRASS_ROOT = 0; //TODO: change variable name 
    const int DESERT_GRASS_ROOT = 1;
    const int FRAGRANT_ORCHID = 2;
    const int GIANT_JUNGLE_ORCHID = 3;

    public bool activated = false;

    Quest quest;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(GRASS_ROOT, amount);//update parameter name
        return amount;
    }
    int PlayerGatherDesertGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(DESERT_GRASS_ROOT, amount);//update parameter name
        return amount;
    }
    int PlayerGatherFragrantOrchid(int amount)
    {
        quest.UpdateQuestProgress(FRAGRANT_ORCHID, amount);//update parameter name
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
        QuestTracker.GrassRoot += PlayerGatherGrassRoot; //update function calls and update QuestTracker.something
        QuestTracker.DesertGrassRoot += PlayerGatherDesertGrassRoot;
        QuestTracker.FragrantOrchid += PlayerGatherFragrantOrchid;
        QuestTracker.GiantJungleOrchid += PlayerGatherGiantJungleOrchid;
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GrassRoot -= PlayerGatherGrassRoot; //update function calls and update QuestTracker.something
        QuestTracker.DesertGrassRoot -= PlayerGatherDesertGrassRoot;
        QuestTracker.FragrantOrchid -= PlayerGatherFragrantOrchid;
        QuestTracker.GiantJungleOrchid -= PlayerGatherGiantJungleOrchid;
    }
    public bool IsActivated()
    {
        return activated;
    }
}

