using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This class manages the UI of the Quest system
//Some functions are similars to those of UI_Notification_manager.cs
public class Quest_UI_Manger : MonoBehaviour
{
    [SerializeField] Button quest_prompt_button; //Quest button on the screen, prompt UI when clicked
    [SerializeField] Button quit_Button;
    [SerializeField] GameObject quest_list; //place holder for  quest_list game object to maneuver easily
    [SerializeField] GameObject quest_list_content; //when we create new quest, we put it as content's child
    [SerializeField] GameObject default_quest_UI; //every quest has the same default UI
    [SerializeField] QuestSystem quest_system;
    [SerializeField] Text Quest_count_text;
    [SerializeField] Button no_quest_button = null;
    int quest_count = 0;
    //this list is used to delete/update the gameobject
    public List<GameObject> quest_UI_gameobject_list = new List<GameObject>(); 

    private void Start()
    {
        GetVariables();
        CheckNulls();



    }

    private void Update()
    {
        //Test();
    }


    private void GetVariables()
    {
        quest_system = GameObject.FindWithTag("Quest System").GetComponent<QuestSystem>();
        if(quest_system == null)
        {
            Debug.Log("Quest system is null");
        }
        //the other variables should be assigned in prefab.
    }

    //Read the quest information from the Quest System
    //create and assign new quest UI accordingly
    public void ReadAndAssignQuest()
    {
        if(quest_system == null)
        {
            quest_system = GameObject.FindWithTag("Quest System").GetComponent<QuestSystem>();
        }
        foreach(Quest quest in quest_system.quest_list)
        {
            GameObject new_quest_UI = CreateNewQuestUI();//create new quest UI
            quest.quest_UI = new_quest_UI.GetComponent<Quest_UI>();//assign that quest UI to the current quest
            //quest.quest_UI.questFinishedButton.onClick.AddListener(quest.OnFinishQuestButtonClicked); //add lister to finish quest button
            //quest.quest_UI.questFinishedButton.onClick.AddListener(UpdateQuestCount);
        }
        SetupQuestUIManager();
    }



    public GameObject CreateNewQuestUI()
    {
        //create new button for the noti
        GameObject new_quest_UI_gameobject = Instantiate(default_quest_UI) as GameObject;//create new game object
        new_quest_UI_gameobject.transform.SetParent(quest_list_content.transform, false);//set parent 
        Quest_UI quest_UI = new_quest_UI_gameobject.GetComponent<Quest_UI>();
        quest_UI.AssignUniqueID(new_quest_UI_gameobject.GetInstanceID());//assign unique ID
        quest_UI_gameobject_list.Add(new_quest_UI_gameobject); //add the new UI to the UI list 
        return new_quest_UI_gameobject;
    }

    private void SetupQuestUIManager()
    {
        quest_list.SetActive(false);
    }


    /// <summary>
    /// check for null variables. These should be auto-assigned in the prefab.
    /// </summary>
    private void CheckNulls()
    {
        if (quest_prompt_button == null)
        {
            Debug.LogError("Missing Noti_UI button reference in " + gameObject.name);
        }

        if (quit_Button == null)
        {
            Debug.LogError("Missing Quit button reference in " + gameObject.name);
        }

        if (quest_list == null)
        {
            Debug.LogError("Missing Noti_List gameobject reference in " + gameObject.name);
        }

        if (quest_list_content == null)
        {
            Debug.LogError("Missing noti_list_content gameobject reference in " + gameObject.name);
        }

        if (default_quest_UI == null)
        {
            Debug.LogError("Missing default_Noti button reference in " + gameObject.name);
        }
        if (quest_system == null)
        {
            Debug.LogError("Missing quest_system reference in " + gameObject.name);
        }
        if(no_quest_button == null)
        {
            Debug.LogError("Missing No Quest Button reference in " + name);
        }

    }

    /// <summary>
    /// open the quest UI
    /// </summary>
    public void OnQuestButtonClick()
    {
        quest_list.SetActive(true);
        quest_system.UpdateQuestProgresses();
        //UpdateQuestCount();
    }

    public void UpdateQuestCount()
    {
        
        if(quest_system == null)
        {
            Debug.Log("Quest system is null in " + name);
        }
        if(quest_system.quest_list == null)
        {
            Debug.Log("Quest list is null in " + name);
        }
        quest_count = quest_system.quest_list.Count;
        //Debug.Log("quest count = " + quest_count);
        Quest_count_text.text = quest_count.ToString();
        if (quest_count == 0)
        {
            Quest_count_text.gameObject.SetActive(false);
            no_quest_button.gameObject.SetActive(true); //activate no quest message
        }
        else
        {
            Quest_count_text.gameObject.SetActive(true);
            no_quest_button.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// close the quest UI
    /// </summary>
    public void OnQuitButtonClick()
    {
        quest_list.SetActive(false);
    }




    //remove a quest UI with matched uniqueID
    //return true if successfully remove, false otherwise
    public bool RemoveQuestUI(int uniqueID)
    {
        //remove the button with matched uniqueID
        for (int i = 0; i < quest_UI_gameobject_list.Count; i++)
        {
            GameObject currentQuestUIGameObject = quest_UI_gameobject_list[i];
            Quest_UI currentQuestUI = currentQuestUIGameObject.GetComponent<Quest_UI>();
            if (currentQuestUI.CompareUniqueID(uniqueID))
            {
                quest_UI_gameobject_list.RemoveAt(i);
                Destroy(currentQuestUIGameObject);
                return true;
            }
        }
        return false;
    }


    //when we hit "r", the game should remove the first quest in the list
    //when we hit "a", the game should add a default quest
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(quest_UI_gameobject_list.Count > 0)
            {
                int uniqueID = quest_UI_gameobject_list[0].GetInstanceID();
                RemoveQuestUI(uniqueID);
            }
            else
            {
                Debug.Log("the quest list is empty");
            }
            
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateNewQuestUI();
        }
    }
}
