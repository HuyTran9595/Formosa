using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour, SingleQuestInterface
{
    const int LEVEL_UP = 0; //first thing we track is leveling up
    Quest quest;
    public bool activated = false;
    private void Start()
    {
        //Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }


    //assumming level up 1 at a time
    //upgrade later
    int PlayerLevelUp(int currentLevel)
    {
        quest.UpdateQuestProgress(LEVEL_UP, 1);
        return 1;
    }

    //subsribe to the appropriate tracker
    public void Subscribe()
    {
        if (activated)
        {
            Debug.Log("Quest already activated.");
            return;
        }
        activated = true;
        QuestTracker.LevelUp += PlayerLevelUp;
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.LevelUp -= PlayerLevelUp;

    }
    public bool IsActivated()
    {
        return activated;
    }
}
