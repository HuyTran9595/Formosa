using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitForAKing : MonoBehaviour, SingleQuestInterface
{
    const int GOLD_CARNIVOROUS_CAVERN_VINE = 0;

    Quest quest;
    public bool activated = false;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGoldCarnivorousCavernVine(int amount)
    {
        quest.UpdateQuestProgress(GOLD_CARNIVOROUS_CAVERN_VINE, amount);//update parameter name
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
        QuestTracker.GoldCarnivorousCavernVine += PlayerGatherGoldCarnivorousCavernVine;


    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GoldCarnivorousCavernVine -= PlayerGatherGoldCarnivorousCavernVine;
    }
    public bool IsActivated()
    {
        return activated;
    }

}
