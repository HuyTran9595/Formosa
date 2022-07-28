using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatCouldGoWrong : MonoBehaviour, SingleQuestInterface
{
    const int SILVER_THORNY_JUNGLE_VINE = 0;
    const int SILVER_CARNIVOROUS_VINE = 1;
    const int SILVER_PULSATING_CAVE_MOSS = 2;
    Quest quest;
    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherSilverThornyJungleVine(int amount)
    {
        quest.UpdateQuestProgress(SILVER_THORNY_JUNGLE_VINE, amount);//update parameter name
        return amount;
    }
    int PlayerGatherSilverCarnivorousVine(int amount)
    {
        quest.UpdateQuestProgress(SILVER_CARNIVOROUS_VINE, amount);//update parameter name
        return amount;
    }
    int PlayerGatherSilverPulsatingCaveMoss(int amount)
    {
        quest.UpdateQuestProgress(SILVER_PULSATING_CAVE_MOSS, amount);//update parameter name
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
        QuestTracker.SilverThornyJungleVine += PlayerGatherSilverThornyJungleVine;
        QuestTracker.SilverCarnivorousCavernVine += PlayerGatherSilverCarnivorousVine;
        QuestTracker.SilverPulsatingCaveMoss += PlayerGatherSilverPulsatingCaveMoss;
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.SilverThornyJungleVine -= PlayerGatherSilverThornyJungleVine;
        QuestTracker.SilverCarnivorousCavernVine -= PlayerGatherSilverCarnivorousVine;
        QuestTracker.SilverPulsatingCaveMoss -= PlayerGatherSilverPulsatingCaveMoss;
    }
    public bool IsActivated()
    {
        return activated;
    }

}
