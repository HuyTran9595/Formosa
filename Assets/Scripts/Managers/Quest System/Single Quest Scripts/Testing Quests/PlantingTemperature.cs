using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingTemperature : MonoBehaviour, SingleQuestInterface
{
    const int PLANTING_TEMPERATURE = 0; //tracking index of Grass Root Seed is 0 in this quest
    Quest quest;

    private void Start()
    {
        Subscribe();
        quest = gameObject.GetComponent<Quest>();
    }

    //update progress when player gather seeds
    int PlayerPlantingTemperature(int temp)
    {
        int goal = quest.Tracker_Achieved[PLANTING_TEMPERATURE];//the goal temperature
        Debug.Log("Goal = " + goal);
        if(temp == goal)
        {
            quest.UpdateQuestProgress(PLANTING_TEMPERATURE, temp, false, true); //true at the end means we match the quest goal
        }
        return temp;
    }

    public void Subscribe()
    {
        QuestTracker.PlantingTemperature += PlayerPlantingTemperature;
    }

    public void Unsubscribe()
    {
        QuestTracker.PlantingTemperature -= PlayerPlantingTemperature;
    }
    public bool activated = false;
    public bool IsActivated()
    {
        return activated;
    }
}

