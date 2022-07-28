﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANearMoss : MonoBehaviour, SingleQuestInterface
{
    const int SILVER_PULSATING_CAVE_MOSS = 0;
    const int SILVER_DESERT_GRASS_ROOT = 1;
    const int GOLD_PULSATING_CAVE_MOSS = 2;
    const int GOLD_DESERT_GRASS_ROOT = 3;

    Quest quest;
    public bool activated = false;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherSilverPulsatingCaveMoss(int amount)
    {
        quest.UpdateQuestProgress(SILVER_PULSATING_CAVE_MOSS, amount);//update parameter name
        return amount;
    }
    int PlayerGatherSilverDesertGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(SILVER_DESERT_GRASS_ROOT, amount);//update parameter name
        return amount;
    }
    int PlayerGatherGoldPulsatingCaveMoss(int amount)
    {
        quest.UpdateQuestProgress(GOLD_PULSATING_CAVE_MOSS, amount);//update parameter name
        return amount;
    }
    int PlayerGatherGoldDesertGrassRoot(int amount)
    {
        quest.UpdateQuestProgress(GOLD_DESERT_GRASS_ROOT, amount);//update parameter name
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
        QuestTracker.SilverPulsatingCaveMoss += PlayerGatherSilverPulsatingCaveMoss;
        QuestTracker.SilverDesertGrassRoot += PlayerGatherSilverDesertGrassRoot;
        QuestTracker.GoldPulsatingCaveMoss += PlayerGatherGoldPulsatingCaveMoss;
        QuestTracker.GoldDesertGrassRoot += PlayerGatherGoldDesertGrassRoot;


    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.SilverPulsatingCaveMoss -= PlayerGatherSilverPulsatingCaveMoss;
        QuestTracker.SilverDesertGrassRoot -= PlayerGatherSilverDesertGrassRoot;
        QuestTracker.GoldPulsatingCaveMoss -= PlayerGatherGoldPulsatingCaveMoss;
        QuestTracker.GoldDesertGrassRoot -= PlayerGatherGoldDesertGrassRoot;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
