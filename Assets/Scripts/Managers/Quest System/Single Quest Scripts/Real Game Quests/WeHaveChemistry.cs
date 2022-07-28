using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeHaveChemistry : MonoBehaviour, SingleQuestInterface
{
    const int PULSATING_CAVE_MOSS = 0;
    const int GRASS_ROOT_ENHANCER = 1;

    Quest quest;
    public bool activated = false;

    private void Start()
    {
        quest = gameObject.GetComponent<Quest>();
    }

    //TODO: change function name and update function calls
    int PlayerGatherPulsatingCaveMoss(int amount)
    {
        quest.UpdateQuestProgress(PULSATING_CAVE_MOSS, amount);//update parameter name
        return amount;
    }
    int PlayerGatherGrassRootEnhancer(int amount)
    {
        quest.UpdateQuestProgress(GRASS_ROOT_ENHANCER, amount);//update parameter name
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
        QuestTracker.PulsatingCaveMoss += PlayerGatherPulsatingCaveMoss;
        QuestTracker.GatherGrassRootEnhancer += PlayerGatherGrassRootEnhancer;
    }

    public void Unsubscribe()
    {
        activated = false;
        Debug.Log(gameObject.name + " unsubscribe");
        QuestTracker.PulsatingCaveMoss -= PlayerGatherPulsatingCaveMoss;
        QuestTracker.GatherGrassRootEnhancer -= PlayerGatherGrassRootEnhancer;
    }

    public bool IsActivated()
    {
        return activated;
    }

}
