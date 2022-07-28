using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonScents : MonoBehaviour, SingleQuestInterface
{
    const int SILVER_FRAGRANT_ORCHID = 0;
    const int GIANT_JUNGLE_ORCHID = 1;
    const int SILVER_THORNY_JUNGLE_VINE = 2;

    Quest quest;

    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherSilverFragrantOrchid(int amount)
    {
        quest.UpdateQuestProgress(SILVER_FRAGRANT_ORCHID, amount);//update parameter name
        return amount;
    }
    int PlayerGatherGiantJungleOrchid(int amount)
    {
        quest.UpdateQuestProgress(GIANT_JUNGLE_ORCHID, amount);//update parameter name
        return amount;
    }
    int PlayerGatherSilverThornyJungleVine(int amount)
    {
        quest.UpdateQuestProgress(SILVER_THORNY_JUNGLE_VINE, amount);//update parameter name
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
        QuestTracker.SilverFragrantOrchid += PlayerGatherSilverFragrantOrchid;
        QuestTracker.GiantJungleOrchid += PlayerGatherGiantJungleOrchid;
        QuestTracker.SilverThornyJungleVine += PlayerGatherSilverThornyJungleVine;


    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.SilverFragrantOrchid -= PlayerGatherSilverFragrantOrchid;
        QuestTracker.GiantJungleOrchid -= PlayerGatherGiantJungleOrchid;
        QuestTracker.SilverThornyJungleVine -= PlayerGatherSilverThornyJungleVine;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
