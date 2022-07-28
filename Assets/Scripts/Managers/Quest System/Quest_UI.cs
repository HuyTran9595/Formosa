using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//This class manage UI for 1 quest.
[System.Serializable]
public class Quest_UI : MonoBehaviour
{
    //each quest UI will have a unique ID that is assigned when QUest_UI_manager create the UI
    //this unique ID is used to track the quest in quest list and update/delete accordingly
    private int uniqueId;
    private bool IsIDassigned = false;

    //these are text components that should be matched in each quest object
    //they should be auto assigned in prefab
    public Text quest_name_text;
    public Text quest_description_text;
    public Text quest_reward_text;
    public Text quest_progress_text;

    public Button questFinishedButton;
    //also called after the quest is created
    private void Start()
    {
        CheckNulls();

        //TO QA TESTER: Comment out this line to test the Quest UI. 
        //Comment out this line allow you to finish the quest instantly
        questFinishedButton.gameObject.SetActive(false);
    }

    //check for null variables. These variables should be assigned in the prefab.
    private void CheckNulls()
    {
        if (quest_name_text == null)
        {
            Debug.LogError("Missing Quest_Name_Text in " + gameObject.name);
        }
        if (quest_description_text == null)
        {
            Debug.LogError("Missing Quest_Description_Text in " + gameObject.name);
        }
        if (quest_reward_text == null)
        {
            Debug.LogError("Missing Quest_Reward_Text in " + gameObject.name);
        }
        if (quest_progress_text == null)
        {
            Debug.LogError("Missing Quest_Progress_Text in " + gameObject.name);
        }
        if (questFinishedButton == null)
        {
            Debug.LogError("Missing Quest Finished Button in " + gameObject.name);
        }
    }


    //set the UI information
    public void SetQuestName(string quest_name)
    {
        quest_name_text.text = quest_name;
    }
    public void SetQuestDescription(string quest_description)
    {
        quest_description_text.text = quest_description;
    }
    public void SetQuestReward(int quest_exp_reward, int quest_coin_reward)
    {
        string quest_text = "Reward: ";
        if(quest_exp_reward != 0)
        {
            quest_text +=  quest_exp_reward + " EXP";
            if (quest_coin_reward != 0)
            {
                quest_text += ", ";
            }
        }

        if(quest_coin_reward != 0)
        {
            quest_text += quest_coin_reward + " Star Gold";
        }


        quest_reward_text.text = quest_text;
    }
   
    /// <summary>
    /// Update quest progress on the UI
    /// </summary>
    public void SetQuestProgress(ref List<String> Tracker_Names, ref List<int> Tracker_Progresses,
                                ref List<int> Tracker_Achieved)
    {
        string progress = "";
        for(int i = 0; i < Tracker_Names.Count; i++)
        {
            progress += Tracker_Names[i] + ": " + Tracker_Progresses[i] + "/" + Tracker_Achieved[i] + "\n";
        }
        quest_progress_text.text = progress;
    }




    //call this to assign unique id
    //only viable 1 time
    //return true if assign successfully, otherwise false
    public bool AssignUniqueID(int uniqueID)
    {
        //if there is a unique ID, can't assign new one
        if (IsIDassigned)
        {
            return false;
        }
        this.uniqueId = uniqueID;
        IsIDassigned = true;
        return true;

    }

    public int GetUniqueID()
    {
        return uniqueId;
    }

    //check if the notification's Unique ID matches the input
    public bool CompareUniqueID(int anotherID)
    {
        if (uniqueId == anotherID)
        {
            return true;
        }
        return false;
    }


    //quest.cs will call this function when the quest is finished and ready to be removed.
    public void ActivateQuestFinishButton()
    {
        questFinishedButton.gameObject.SetActive(true);
        //Debug.Log("quest finish button is activated.");
    }
}
