using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANobleEndeavor : MonoBehaviour, SingleQuestInterface
{
    const int GOLD_FRAGRANT_ORCHID = 0;

    Quest quest;
    public bool activated = false;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGoldFragrantOrchid(int amount)
    {
        quest.UpdateQuestProgress(GOLD_FRAGRANT_ORCHID, amount);//update parameter name
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
        QuestTracker.GoldFragrantOrchid += PlayerGatherGoldFragrantOrchid;


    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GoldFragrantOrchid -= PlayerGatherGoldFragrantOrchid;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
