using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class represent the Quest System. 
//For now this class holds all the information of all the quest.
//The Quest_UI_Manager will read information from this class to create quest_UI 
//and assign them to each quest accordingly.
public class QuestSystem : MonoBehaviour
{
    //static int TOTAL_QUEST_NUMER = 3; //how many quests are there in the game?
    public List<Quest> quest_list = new List<Quest>(); //assign the quest here
    [SerializeField] GameObject quest_list_gameobject;
    Quest_UI_Manger quest_UI_manager;


    private void Start()
    {
        quest_UI_manager = GameObject.FindWithTag("Quest_UI_Manager").GetComponent<Quest_UI_Manger>();
        if(quest_UI_manager == null)
        {
            Debug.Log("Cannot find quest_UI_manager in " + gameObject.name);
        }
        StartQuestSystem(1); //assumming player level 1
        quest_UI_manager.ReadAndAssignQuest();
        quest_UI_manager.UpdateQuestCount();
    }

    //load the quest at the beginning of the game
    //assume player starts at level 1
    private void StartQuestSystem(int currentLevel)
    {
        Quest[] allQuests = quest_list_gameobject.GetComponentsInChildren<Quest>(true);
        foreach(Quest quest in allQuests)
        {
            //we will add these quests
            if(quest.quest_unlocked_level == currentLevel || quest.quest_unlocked_level <= 1)
            {
                quest.gameObject.SetActive(true);
                quest_list.Add(quest);
                

            }
            else //we inactivate these quests
            {
                //if(quest.gameObject.activeSelf)//if the quest is active, that means it subscribe when started, so we need to unsubscribe
                //{
                //    quest.quest_specific_script.Unsubscribe();
                //}

                if (quest.GetComponent<SingleQuestInterface>().IsActivated())
                {
                    quest.quest_specific_script.Unsubscribe();
                }

                quest.gameObject.SetActive(false);
            }
        }
        
    }




    //go through the quest, check the progress and update UI
    public void UpdateQuestProgresses()
    {
        foreach (Quest quest in quest_list)
        {
            quest.CheckQuestProgress();
        }
    }
    
    //called when the quest is finished
    //remove the quest that match the quest_name parameter
    public bool RemoveQuest(string quest_name)
    {
        for (int i = 0; i < quest_list.Count; i++)
        {
            if(quest_list[i].quest_name == quest_name)
            {
                quest_list.RemoveAt(i);
                return true;
            }
        }
        quest_UI_manager.UpdateQuestCount();
        return false;
    }

    //this function is called when player level up to unlock new quests
    public void UpdateActiveQuests(int currentPlayerLevel)
    {
        //go though all child objects
        Quest[] allQuests = quest_list_gameobject.GetComponentsInChildren<Quest>(true);

        //if any obj has quest-lvl == currentPlayer lvl and it is INACTIVE
            //unlock it
        foreach(Quest quest in allQuests)
        {
            if (!quest.gameObject.activeSelf && quest.quest_unlocked_level == currentPlayerLevel)
            {
                quest.gameObject.SetActive(true);//active the quest unlocked at currentPlayerLevel
                quest_list.Add(quest);//Add it to the quest_list
                quest.quest_specific_script.Subscribe();
                //call Quest_UI_manager to create a new UI, and attach this new UI to the newly unlocked quest
                GameObject quest_UI = quest_UI_manager.CreateNewQuestUI();
                quest.quest_UI = quest_UI.GetComponent<Quest_UI>();
                quest.quest_UI.questFinishedButton.onClick.AddListener(quest.OnFinishQuestButtonClicked);
                quest.quest_UI.questFinishedButton.onClick.AddListener(quest_UI_manager.UpdateQuestCount);
            }
        }
        quest_UI_manager.UpdateQuestCount();
        return;
    }


}
