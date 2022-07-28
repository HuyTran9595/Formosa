using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//This class represents 1 general quest in the quest system
//It handles the quest logic and quest UI
[System.Serializable]
public class Quest : MonoBehaviour
{

    public string quest_name = "DEFAULT_QUEST_NAME"; //there should be a script with the same quest name attached to the same quest object
                                                     //ex: quest name = Level Up -> LevelUp.cs should be attached to the same object
    public string quest_description = "DEFAULT_QUEST_DESCRIPTION";

    public int quest_exp_reward = 50;
    public int quest_coin_reward = 5;
    public int quest_unlocked_level = 0; //quest unlocked at level 0, currently not used 

    //reference to the quest UI held by the Quest_UI_Manager.
    //this variable should be assigned when the quest system is read by Quest_UI_Manager
    public Quest_UI quest_UI;
    private bool quest_UI_initialized = false;


    //These 3 list are to keep track of progress and update them on the UI
    //The size of the 3 lists MUST BE equal
    //Example: tracker_names[0] = "Gather Grassroot" -> the progress is 0/5
    // -> tracker_progresses[0] = 0 and tracker_achieved[0] = 5
    //We have a list because a quest can track many things.
    //Each quest has individual script to edit these 3 lists as they progress.
    public List<String> Tracker_Names = new List<String>();
    public List<int> Tracker_Progresses = new List<int>();
    public List<int> Tracker_Achieved = new List<int>();
    public bool isFinished = false;

    Quest_UI_Manger quest_UI_manager;
    QuestSystem quest_system;
    public SingleQuestInterface quest_specific_script;

    //these are to reward player
    Level playerLeveler;
    Inventory playerInventory; 
    private void Start()
    {
        GetVariables();
        StartCoroutine(CheckNullsAfterTime(5));

    }

    private void GetVariables()
    {
        quest_UI_manager = GameObject.FindWithTag("Quest_UI_Manager").GetComponent<Quest_UI_Manger>();
        quest_system = GameObject.FindWithTag("Quest System").GetComponent<QuestSystem>();
        quest_specific_script = gameObject.GetComponent<SingleQuestInterface>();
        playerLeveler = GameObject.FindObjectOfType<Level>();
        playerInventory = GameObject.FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        InitializeQuestUI();
    }

    //init quest UI once.
    private void InitializeQuestUI()
    {
        if (!quest_UI_initialized)
        {
            UpdateQuestUI();
            quest_UI_initialized = true;
        }
    }

    private void UpdateQuestUI()
    {
        //Debug.Log("Name: " + gameObject.name);
        quest_UI.SetQuestName(quest_name);
        quest_UI.SetQuestDescription(quest_description);
        quest_UI.SetQuestReward(quest_exp_reward, quest_coin_reward);
        quest_UI.SetQuestProgress(ref Tracker_Names, ref Tracker_Progresses, ref Tracker_Achieved);

    }

    private void CheckNulls()
    {
        if(quest_UI == null)
        {
            Debug.LogError("Missing Quest_UI game object reference in " + gameObject.name);
        }
        if (quest_UI_manager == null)
        {
            Debug.LogError("Missing quest_UI_manager reference in " + gameObject.name);
        }
        if (quest_system == null)
        {
            Debug.LogError("Missing quest_system reference in " + gameObject.name);
        }
        if (quest_specific_script == null)
        {
            Debug.LogError("Missing quest_specific_script reference in " + gameObject.name);
        }
        if (playerLeveler == null)
        {
            Debug.LogError("Missing player leveler reference in " + gameObject.name);
        }
        if (playerInventory == null)
        {
            Debug.LogError("Missing player inventory reference in " + gameObject.name);
        }

    }

    //check for nulls after 5 seconds
    //we do this because the system needs time to read, create and assign quest_UI
    IEnumerator CheckNullsAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        CheckNulls();
    }

    //tracker index is the index of the tracker in the Tracker_Names list
    //each individual quest script will call this function to update UI
    //isAdded = true means we add the progress parameter to the current progress
    //isAdded = false means progress is the actual progress -> don't need to add them together
    //isMatched is for true/false quest. If isMatched = true -> we finish the current track
    public void UpdateQuestProgress(int tracker_index, int progress, bool isAdded = true, bool isMatched = false)
    {
        if(tracker_index <0 || tracker_index >= Tracker_Names.Count)
        {
            Debug.LogWarning("Index out of bound at " + gameObject.name);
            return;
        }

        //this is for true/false quest
        if (isMatched)
        {
            Tracker_Progresses[tracker_index] = Tracker_Achieved[tracker_index];
        }


        //if current track is not finishes, we update the track
        if(Tracker_Progresses[tracker_index]  < Tracker_Achieved[tracker_index])
        {
            if (isAdded)
            {
                Tracker_Progresses[tracker_index] += progress;
            }
            else
            {
                Tracker_Progresses[tracker_index] = progress;
            }
            

            //make sure it doesnt overflow the required number
            if(Tracker_Progresses[tracker_index] > Tracker_Achieved[tracker_index])
            {
                Tracker_Progresses[tracker_index] = Tracker_Achieved[tracker_index];
            }
        }
        quest_UI.SetQuestProgress(ref Tracker_Names, ref Tracker_Progresses, ref Tracker_Achieved);
        CheckQuestProgress();
    }

    //check if the quest is fnished. If so, reward the player and activate finish quest button
    public bool CheckQuestProgress()
    {
        for(int i = 0; i < Tracker_Names.Count; i++)
        {
            if(Tracker_Progresses[i] != Tracker_Achieved[i])
            {
                isFinished = false;
                return isFinished;
            }
        }
        //Debug.Log("Quest is finished.");
        quest_UI.ActivateQuestFinishButton();
        return isFinished;
    }


    /// <summary>
    /// when player hit finish quest button, delete the quest from UI and related objects and reward player
    /// </summary>
    public void OnFinishQuestButtonClicked()
    {
        Debug.Log("TODO: reward player");
        RewardPlayer();
        int quest_UI_unique_ID = quest_UI.GetUniqueID();
        quest_UI_manager.RemoveQuestUI(quest_UI_unique_ID);//remove UI for this quest
        quest_system.RemoveQuest(quest_name);//remove quest object in the quest system
        quest_specific_script.Unsubscribe();//unsubscribe from quest tracker
    }

    //TODO: finish this method
    private void RewardPlayer()
    {
        playerLeveler.Exp(quest_exp_reward);
        Debug.Log("Exp: " + quest_exp_reward);
        playerInventory.CoinUpdate(quest_coin_reward);
        Debug.Log("Coin: " + quest_coin_reward);
    }


    //added this function to fix some bugs when the quests subscribe to quest tracker and then go inactive due to player level not enough
    public void OnFirstTimeActivate()
    {
        quest_specific_script.Subscribe();
    }
}
