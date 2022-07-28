using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo: change "ExampleQuest" to your quest name
//The file name is "ExampleQuest.cs" -> class name MUST BE "ExampleQuest"
public class DesertGrassRootSeed : MonoBehaviour, SingleQuestInterface
{
    const int DESERT_GRASS_ROOT_SEED = 0; //TODO: change variable name 
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGrassRootSeed(int amount)
    {
        quest.UpdateQuestProgress(DESERT_GRASS_ROOT_SEED, amount);//update parameter name
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.DesertGrassRootSeed += PlayerGatherGrassRootSeed; //update function calls and update QuestTracker.something
    }

    public void Unsubscribe()
    {
        QuestTracker.DesertGrassRootSeed -= PlayerGatherGrassRootSeed;//update function calls and update QuestTracker.something
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

