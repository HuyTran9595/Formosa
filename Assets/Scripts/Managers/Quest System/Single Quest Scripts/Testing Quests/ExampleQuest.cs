using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Todo: change "ExampleQuest" to your quest name
//The file name is "ExampleQuest.cs" -> class name MUST BE "ExampleQuest"
public class ExampleQuest : MonoBehaviour, SingleQuestInterface
{
    const int EXAMPLE_QUEST = 0; //TODO: change variable name 
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerExampleQuest(int amount)
    {
        quest.UpdateQuestProgress(EXAMPLE_QUEST, amount);//update parameter name
        return amount;
    }

    public void Subscribe()
    {
        QuestTracker.ExampleQuest += PlayerExampleQuest; //update function calls and update QuestTracker.something
    }

    public void Unsubscribe()
    {
        QuestTracker.ExampleQuest -= PlayerExampleQuest;//update function calls and update QuestTracker.something
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

