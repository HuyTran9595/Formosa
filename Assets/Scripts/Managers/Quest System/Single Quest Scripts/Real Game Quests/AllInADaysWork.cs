using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllInADaysWork : MonoBehaviour, SingleQuestInterface
{
    const int GLOWING_OCEANIC_FUNGI = 0;
    const int CARNIVOROUS_CAVE_VINE = 1;
    const int PULSATING_CAVE_MOSS = 2;
    Quest quest;

    public bool activated = false; 

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherGlowingOceanicFungi(int amount)
    {
        quest.UpdateQuestProgress(GLOWING_OCEANIC_FUNGI, amount);//update parameter name
        return amount;
    }
    int PlayerGatherCarnivorousCaveVine(int amount)
    {
        quest.UpdateQuestProgress(CARNIVOROUS_CAVE_VINE, amount);//update parameter name
        return amount;
    }
    int PlayerGatherPulsatingCaveMoss(int amount)
    {
        quest.UpdateQuestProgress(PULSATING_CAVE_MOSS, amount);//update parameter name
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
        QuestTracker.GlowingOceanicFungi += PlayerGatherGlowingOceanicFungi;
        QuestTracker.CarnivorousCavernVine += PlayerGatherCarnivorousCaveVine;
        QuestTracker.PulsatingCaveMoss += PlayerGatherPulsatingCaveMoss;
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.GlowingOceanicFungi -= PlayerGatherGlowingOceanicFungi;
        QuestTracker.CarnivorousCavernVine -= PlayerGatherCarnivorousCaveVine;
        QuestTracker.PulsatingCaveMoss -= PlayerGatherPulsatingCaveMoss;
    }

    public bool IsActivated()
    {
        return activated;
    }

}
