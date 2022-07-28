using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to handle and call all the Maintenance Event
/// </summary>
public class MaintenanceManager : MonoBehaviour, NotiInterface
{
    [SerializeField] List<MaintenanceEvent> maintenanceEvents = new List<MaintenanceEvent>();
    [Tooltip("How frequent to call event")]
    public float frequency = 10.0f;
    GameObject currentGamePanel; //panel of the current game in play, update this when changing game.
    MaintenanceEvent currentEvent;
    float timer;
    int DEBUG_count; //Debug
    bool DEBUG_stop = false; //Debug

    //notification handling
    UI_Notification_Manager ui_notification_manager;

    //to decrease coin if he doesn't fix stuff
    Inventory playerInventory;
    [SerializeField] int PunishAfterXseconds = 10;
    [SerializeField] int PunishInterval = 2;
    [SerializeField] int CoinSubtracted = -1;

    void Awake()
    {
        GetMaintenanceEvents();
        timer = frequency;
        foreach (var events in maintenanceEvents)
        {
            if (events != null)
                events.SetMaintenanaceManager(this);
        }
    }

    private void Start()
    {
        //get the UI_Nofication_manager component
        GameObject[] ui_Notification_Managers = GameObject.FindGameObjectsWithTag("UI_Notification_Manager");
        ui_notification_manager = ui_Notification_Managers[0].GetComponent<UI_Notification_Manager>();
        if(ui_notification_manager == null)
        {
            Debug.LogError("could not find UI_Nofication_manager component");
        }


        playerInventory = GameObject.FindObjectOfType<Inventory>();
        if (playerInventory == null)
        {
            Debug.LogError("Missing player inventory reference in " + gameObject.name);
        }
    }


    /// <summary>
    /// loop through all child objects and put them in maintenanceEvents list
    /// assuming that each child object is a minigame prefab.
    /// </summary>
    private void GetMaintenanceEvents()
    {
        foreach(Transform child in transform)
        {
            MaintenanceEvent childMaintenanceEvent = child.gameObject.GetComponent<MaintenanceEvent>();
            AddNewEvent(childMaintenanceEvent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update timer
        if(currentEvent == null && !DEBUG_stop)
            timer -= Time.deltaTime;
        //if time is up, call random event
        if(timer <= 0 && currentEvent == null)
        {
            MaintenanceEvent newEvent = RandomNewEvent();
            currentEvent = newEvent;
            currentGamePanel = newEvent.GetGamePanel();
            newEvent.Execute();            
            InvokeRepeating(nameof(DecreaseCoinOvertime), PunishAfterXseconds, PunishInterval); 
            DEBUG_count++;
        }
        if (DEBUG_count > 1000)
            DEBUG_stop = true;

        //CheckMiniGameStatus();
    }

    //this method will decrease player coin by 1 every 10 seconds so that he has motivation to finish maintenance events
    private void DecreaseCoinOvertime()
    {
        if (playerInventory.Coins > 0)
        playerInventory.CoinUpdate(-1 * Math.Abs(CoinSubtracted));
    }

    public void AddNewEvent(MaintenanceEvent _event)
    {
        maintenanceEvents.Add(_event);
       // Debug.Log(_event.gameObject.name + " added to list, current size is :" + maintenanceEvents.Count);
    }

    MaintenanceEvent RandomNewEvent()
    {
        float probTotal = 0;
        Queue<MaintenanceEvent> eventQ = new Queue<MaintenanceEvent>();
        foreach (var _event in maintenanceEvents)
        {
            if (_event.probablity == 1.0f)
                return _event;
            probTotal += _event.probablity;
            eventQ.Enqueue(_event);
        }
        float v = UnityEngine.Random.Range(0, probTotal);
        while(eventQ.Count!=0)
        {
            MaintenanceEvent tempEvent = eventQ.Dequeue();
            v -= tempEvent.probablity;
            if (v <= 0)
                return tempEvent;
        }
        Debug.Log("You shouldn't see this message. If yes, well...");
        return null;
    }
    public void EventEnd(bool goodEnd, string message)
    {
        if (goodEnd)
            currentEvent.Reward();
        else
            currentEvent.Punish();
        currentEvent = null;
        timer = frequency;
        CancelInvoke(nameof(DecreaseCoinOvertime)); //cancel subtracting coin when player not fixing
        OnNotificationQuit();//end notification
        CallNotificationOnMaintenanceFinished(message);
    }



    /// <summary>
    /// Resume the game when player hits F
    /// Quit the game when player hits Escape
    /// </summary>
    private void CheckMiniGameStatus()
    {
        //start mini game
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentGamePanel.SetActive(true);
        }

        //quit mini game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGamePanel)
            {
                currentGamePanel.SetActive(false);
            }
        }
    }



    /// <summary>
    /// call this function to signify new notification to the game, when new maintenance event is created
    /// OnNotificationClick actually do something
    /// </summary>
    public void CallNotification(String message)
    {
        //uncomment this to work with Andy's code
        //InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti(message, gameObject));

        ui_notification_manager.CreateNewNotiButton(OnNotificationClick, message, GetInstanceID());

        
    }



    /// <summary>
    /// call this function when a maintenance event is finished
    /// create an empty notification saying the player fixed something
    /// when the player click this noti, removes the noti
    /// </summary>
    public void CallNotificationOnMaintenanceFinished(String message)
    {
        ui_notification_manager.CreateNewNotiButton(OnNotificationQuit, message, GetInstanceID());
    }


    //when the player click the notification, the game should pops up
    public void OnNotificationClick()
    {
        currentGamePanel.SetActive(true);
    }



    //when the game ends, we should remove the notification 
    public void OnNotificationQuit()
    {
        bool removed = ui_notification_manager.RemoveNotiButton(GetInstanceID());
        if (!removed)
        {
            Debug.LogError("cannot remove notification. Something is wrong.");
        }
    }

}
