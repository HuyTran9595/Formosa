using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class of all maintenance event
/// Maintenance manager will call this virtual functions
/// Feedback will be sent from these as well
/// </summary>
public abstract class MaintenanceEvent : MonoBehaviour
{
    public MaintenanceManager MaintenanceManager;
    [Tooltip("Probability to call this event. If 1, then must call this event")]
    [Range(0.01f, 1.0f)]
    public float probablity = 0.1f;
    //public virtual void Start()
    //{
    //    MaintenanceManager = FindObjectOfType<MaintenanceManager>();
    //    MaintenanceManager.AddNewEvent(this);
    //}

    private void Start()
    {
        CheckNulls();
    }
    /// <summary>
    /// Checking for null variables
    /// </summary>
    private void CheckNulls()
    {
        if(MaintenanceManager == null)
        {
            Debug.LogError("Mainenance manager needed in " + gameObject.name);
        }
    }

    public void SetMaintenanaceManager(MaintenanceManager _mam) => MaintenanceManager = _mam;
    
    /// <summary>
    /// Manager will call this function when this event is executed.
    /// Treat as the start point of event
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Manager will call this function when this event is end with good end.
    /// Treat as the wrap up point of event
    /// </summary>
    public abstract void Reward();

    /// <summary>
    /// Manager will call this function when this event is end with bad end.
    /// Treat as the wrap up point of event
    /// </summary>
    public abstract void Punish();

    /// <summary>
    /// Event host call this whenever the event is finished and notify the manager
    /// Param: True if the player pass the game, false otherwise
    /// </summary>
    public void EventEnd(bool goodEnd, string message = "Maintenance completed.")
    {
        MaintenanceManager.EventEnd(goodEnd, message);
    }

    /// <summary>
    /// Event host call this function to get the current game panel
    /// </summary>
    public abstract GameObject GetGamePanel();

    /// <summary>
    /// Child events call this function to signify notification
    /// </summary>
    public void CallNotification(String message)
    {
        //InGameNotiManager.Instance.NewNotification(new InGameNotiManager.IngameNoti(message, gameObject));
        MaintenanceManager.CallNotification(message);

    }
}
