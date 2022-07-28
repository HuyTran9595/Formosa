using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineInsideTheJungle : MonoBehaviour, SingleQuestInterface
{
    const int CARNIVOROUS_CAVERN_VINE = 0;
    const int GOLD_GIANT_JUNGLE_ORCHID = 1;

    Quest quest;
    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherCarnivorousCavernVine(int amount)
    {
        quest.UpdateQuestProgress(CARNIVOROUS_CAVERN_VINE, amount);//update parameter name
        return amount;
    }

    int PlayerGatherGoldGiantJungleOrchid(int amount)
    {
        quest.UpdateQuestProgress(GOLD_GIANT_JUNGLE_ORCHID, amount);//update parameter name
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
        QuestTracker.CarnivorousCavernVine += PlayerGatherCarnivorousCavernVine;
        QuestTracker.GoldGiantJungleOrchid += PlayerGatherGoldGiantJungleOrchid;

    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.CarnivorousCavernVine -= PlayerGatherCarnivorousCavernVine;
        QuestTracker.GoldGiantJungleOrchid -= PlayerGatherGoldGiantJungleOrchid;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
