using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardeningAchievement : MonoBehaviour
{
    [SerializeField] List<GameObject> decoratingTrees = new List<GameObject>();
    //notification handling
    UI_Notification_Manager ui_notification_manager;
    private string NewTreeMessage = "Achievement Plant Unlocked at level ";
    private void Start()
    {
        SetUpDecoratingTrees();
        QuestTracker.LevelUp += OnPlayerLevelUp;


        //get the UI_Nofication_manager component
        GameObject[] ui_Notification_Managers = GameObject.FindGameObjectsWithTag("UI_Notification_Manager");
        ui_notification_manager = ui_Notification_Managers[0].GetComponent<UI_Notification_Manager>();
        if (ui_notification_manager == null)
        {
            Debug.LogError("could not find UI_Nofication_manager component");
        }
    }

    private void SetUpDecoratingTrees()
    {
        foreach (GameObject tree in decoratingTrees)
        {
            //Debug.Log("setting up tree " + gameObject.name);
            tree.SetActive(false);
        }
    }

    public int OnPlayerLevelUp(int currentLevel)
    {
        //this means player unlocked all the decorating trees
        if(currentLevel <= 0 || currentLevel > 30)
        {
            return currentLevel;
        }
        if((currentLevel % 3) == 0) //if currentLevel divisible by 3
        {
            int index = currentLevel / 3 - 1; //the index of the tree we gonna unlock
                                               //Improve: better unlock sequence other than just activate the tree
            NewUnlockSequence(index, currentLevel);
        }
        //if not, we don't unlock anything
        return currentLevel;
    }

    private void NewUnlockSequence(int index, int currentLevel)
    {
        string message =  NewTreeMessage + currentLevel + "!";
        if(decoratingTrees[index] == null)
        {
            Debug.Log("tree is null");
        }
        else
        {
            decoratingTrees[index].SetActive(true);
        }
        
        ui_notification_manager.RemoveNotiButton(GetInstanceID());
        ui_notification_manager.CreateNewNotiButton(OnNotificationQuit, message, GetInstanceID());

    }

    public void OnNotificationQuit()
    {
        ui_notification_manager.OnQuitButtonClick();
        bool removed = ui_notification_manager.RemoveNotiButton(GetInstanceID());
        if (!removed)
        {
            Debug.LogError("cannot remove notification. Something is wrong.");
        }
    }
}
