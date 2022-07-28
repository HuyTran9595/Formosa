using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


//Currently, only MaintenanceManager.cs implements notification
public class UI_Notification_Manager : MonoBehaviour
{
    [SerializeField] Button noti_prompt_button; //button on the screen, prompt noti UI when clicked
    [SerializeField] Button quit_Button;
    [SerializeField] GameObject noti_list; //place holder for noti_list game object to maneuver easily
    [SerializeField] GameObject noti_list_content; //when we create new noti button, we put it as content's child
    [SerializeField] GameObject default_Noti; //when we create new notification, we create a copy of this in the UI list
    [SerializeField] Text noti_count_text;

    List<NEW_Notification> notification_list = new List<NEW_Notification>(); //control lists of notifications
    List<GameObject> noti_button_list = new List<GameObject>();
    int noti_count = 0;
    private void Start()
    {
        CheckNulls();
        SetupNotiManager();

    }

    private void Update()
    {

        //Test creating new button
        //future: this should be a listener event, prompted when some scripts call a new noti.
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    CreateNewNotiButton();
        //}
    }
    private void SetupNotiManager()
    {
        noti_list.SetActive(false);
        UpdateNotiCount();
    }


    /// <summary>
    /// check for null variables. These should be auto-assigned in the prefab.
    /// </summary>
    private void CheckNulls()
    {
        if( noti_prompt_button == null)
        {
            Debug.LogError("Missing Noti_UI button reference in " + gameObject.name);
        }

        if (quit_Button == null)
        {
            Debug.LogError("Missing Quit button reference in " + gameObject.name);
        }

        if (noti_list == null)
        {
            Debug.LogError("Missing Noti_List gameobject reference in " + gameObject.name);
        }

        if (noti_list_content == null)
        {
            Debug.LogError("Missing noti_list_content gameobject reference in " + gameObject.name);
        }

        if (default_Noti == null)
        {
            Debug.LogError("Missing default_Noti button reference in " + gameObject.name);
        }

        if (noti_count_text == null)
        {
            Debug.LogError("Missing noti_count_text reference in " + gameObject.name);
        }
    }

    /// <summary>
    /// open the noti UI
    /// </summary>
    public void OnNotiPrompButtonClick()
    {
        noti_list.SetActive(true);
    }


    /// <summary>
    /// close the noti UI
    /// </summary>
    public void OnQuitButtonClick()
    {
        noti_list.SetActive(false);
    }

    /// <summary>
    /// add new noti button in the content list
    /// assign call event for the new button
    /// "call" is the function that should be called when the notification is clicked
    /// </summary>
    public void CreateNewNotiButton(UnityAction call, String message, int UniqueID)
    {
        //create new button for the noti
        GameObject newNotiButton = Instantiate(default_Noti) as GameObject;
        newNotiButton.transform.SetParent(noti_list_content.transform, false);

        //set up onClick event for the button
        newNotiButton.GetComponent<Button>().onClick.AddListener(call);
        //set up the button's text message
        newNotiButton.transform.Find("Text").GetComponent<Text>().text = message;

        //manage noti lists
        //Debug.Log("The unique id is: " + UniqueID);

        newNotiButton.GetComponent<NEW_Notification>().AssignUniqueID(UniqueID);
        noti_button_list.Add(newNotiButton);

        UpdateNotiCount();

    }

    //remove a button with matched uniqueID
    //return true if successfully remove, false otherwise
    public bool RemoveNotiButton(int uniqueID)
    {
        //remove the button with matched uniqueID
        for(int i = 0; i < noti_button_list.Count; i++)
        {
            GameObject currentButton = noti_button_list[i];
            if(currentButton != null)
            {
                NEW_Notification currentNoti = currentButton.GetComponent<NEW_Notification>();
                if (currentNoti.CompareUniqueID(uniqueID))
                {
                    noti_button_list.RemoveAt(i);
                    Destroy(currentButton);
                    UpdateNotiCount();
                    return true;
                }
            }

        }
        return false;
    }

    //update the noti_count game object
    private void UpdateNotiCount()
    {
        noti_count = noti_button_list.Count;
        noti_count_text.text = noti_count.ToString();
        if (noti_count == 0)
        {
           
            CreateEmptyNotificationMessage();//add default noti saying that we don't have any notification
            noti_count_text.gameObject.SetActive(false);
        }
        else if(noti_count != 1) // two or more notification, we attempt to remove the default 
        {
            noti_count_text.gameObject.SetActive(true);
            bool removed = RemoveNotiButton(gameObject.GetInstanceID()); //try to remove default noti
            //Debug.Log("Trying to remove default noti... " + removed);

        }
        else //noti count = 1
        {
            noti_count_text.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// When there is no Notification, we create a default one saying "No Unread Notification"
    /// </summary>
    private void CreateEmptyNotificationMessage()
    {
        CreateNewNotiButton(EmptyNotiCall, "No Unread Notifications", gameObject.GetInstanceID());
    }

    private void EmptyNotiCall()
    {
        Debug.Log("Clicking Default Noti Button. This button doesn't do anything.");
    }
}
