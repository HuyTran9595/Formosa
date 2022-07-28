using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiForAFunGuy : MonoBehaviour, SingleQuestInterface
{
    const int GRASS_ROOT_ENHANCER = 0;
    const int GOLD_GLOWING_OCEANIC_FUNGI = 1;
    const int SILVER_GLOWING_OCEANIC_FUNGI = 2;
    Quest quest;
    public bool activated = false;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGrassRootEnhancer(int amount)
    {
        quest.UpdateQuestProgress(GRASS_ROOT_ENHANCER, amount);//update parameter name
        return amount;
    }

    int PlayerGatherGoldGlowingOceanicFungi(int amount)
    {
        quest.UpdateQuestProgress(GOLD_GLOWING_OCEANIC_FUNGI, amount);//update parameter name
        return amount;
    }

    int PlayerGatherSilverGlowingOceanicFungi(int amount)
    {
        quest.UpdateQuestProgress(SILVER_GLOWING_OCEANIC_FUNGI, amount);//update parameter name
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
        QuestTracker.GatherGrassRootEnhancer += PlayerGatherGrassRootEnhancer;
        QuestTracker.GoldGlowingOceanicFungi += PlayerGatherGoldGlowingOceanicFungi;
        QuestTracker.SilverGlowingOceanicFungi += PlayerGatherSilverGlowingOceanicFungi;

    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GatherGrassRootEnhancer -= PlayerGatherGrassRootEnhancer;
        QuestTracker.GoldGlowingOceanicFungi -= PlayerGatherGoldGlowingOceanicFungi;
        QuestTracker.SilverGlowingOceanicFungi -= PlayerGatherSilverGlowingOceanicFungi;
    }
    public bool IsActivated()
    {
        return activated;
    }
}
