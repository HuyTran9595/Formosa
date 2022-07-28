﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASouperRecovery : MonoBehaviour, SingleQuestInterface
{
    const int STRONG_FOREST_HERB = 0;
    const int SILVER_GLOWING_OCEANIC_FUNGI = 1;
    const int GOLD_PULSATING_CAVE_MOSS = 2;
    Quest quest;

    public bool activated = false;
    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherStrongForestHerb(int amount)
    {
        quest.UpdateQuestProgress(STRONG_FOREST_HERB, amount);//update parameter name
        return amount;
    }

    int PlayerGatherSilverGlowingOceanicFungi(int amount)
    {
        quest.UpdateQuestProgress(SILVER_GLOWING_OCEANIC_FUNGI, amount);//update parameter name
        return amount;
    }

    int PlayerGatherGoldPulsatingCaveMoss(int amount)
    {
        quest.UpdateQuestProgress(GOLD_PULSATING_CAVE_MOSS, amount);//update parameter name
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
        QuestTracker.StrongForestHerb += PlayerGatherStrongForestHerb;
        QuestTracker.SilverGlowingOceanicFungi += PlayerGatherSilverGlowingOceanicFungi;
        QuestTracker.GoldPulsatingCaveMoss += PlayerGatherGoldPulsatingCaveMoss;

    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.StrongForestHerb -= PlayerGatherStrongForestHerb;
        QuestTracker.SilverGlowingOceanicFungi -= PlayerGatherSilverGlowingOceanicFungi;
        QuestTracker.GoldPulsatingCaveMoss -= PlayerGatherGoldPulsatingCaveMoss;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
