using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellsLikeTrouble : MonoBehaviour, SingleQuestInterface
{
    const int FRAGRENT_ORCHID_ENHANCER = 0;
    const int GIANT_JUNGLE_ORCHID_ENHANCER = 1;
    const int THORNY_JUNGLE_VINE = 2;
    Quest quest;

    public bool activated = false;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherFragrentEnhancer(int amount)
    {
        quest.UpdateQuestProgress(FRAGRENT_ORCHID_ENHANCER, amount);//update parameter name
        return amount;
    }
    int PlayerGatherGiantJungleOrchidEnhancer(int amount)
    {
        quest.UpdateQuestProgress(GIANT_JUNGLE_ORCHID_ENHANCER, amount);//update parameter name
        return amount;
    }

    int PlayerGatherThornyJungleVine(int amount)
    {
        quest.UpdateQuestProgress(THORNY_JUNGLE_VINE, amount);//update parameter name
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

        //Debug.Log("Smell like trouble subscribe called");
        QuestTracker.GatherFragrentEnhancer += PlayerGatherFragrentEnhancer; //update function calls and update QuestTracker.something
        QuestTracker.GatherGiantJungleOrchidEnhancer += PlayerGatherGiantJungleOrchidEnhancer;
        QuestTracker.ThornyJungleVine += PlayerGatherThornyJungleVine;
        //if(QuestTracker.GatherFragrentEnhancer != null)
        //{
        //    Debug.Log("Gather gragrent enhancer is not null");
        //}
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GatherFragrentEnhancer -= PlayerGatherFragrentEnhancer;//update function calls and update QuestTracker.something
        QuestTracker.GatherGiantJungleOrchidEnhancer -= PlayerGatherGiantJungleOrchidEnhancer;
        QuestTracker.ThornyJungleVine -= PlayerGatherThornyJungleVine;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
