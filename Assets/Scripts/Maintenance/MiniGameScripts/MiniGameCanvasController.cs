using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Canvas))]
public class MiniGameCanvasController : MaintenanceEvent
{
    [SerializeField] protected GameObject panel;
    private Button button;
    protected String message = "Match the farm animals! Fix them or lose coins over time!";
    [SerializeField] protected GameObject reward_panel;
    [SerializeField] protected GameObject punish_panel;

    public  void Start()
    {

        QuitGame();
        if (panel == null)
        {
            Debug.Log("Panel is null");
        }
    }

    public override GameObject GetGamePanel()
    {
        return panel;
    }

    public void QuitGame()
    {
        if (panel)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// Manager will call this function when this event is executed.
    /// Treat as the start point of event
    /// </summary>
    public override void Execute(){
        base.CallNotification(message);
       // Debug.Log("Execute function has been called.");
        if (panel != null)
        {
            //Debug.Log("Activate panel");
            panel.gameObject.SetActive(true);
            panel.GetComponent<MGFixWireController>().InitiateGame();//override this line
            QuitGame();
        }
    }

    /// <summary>
    /// Manager will call this function when this event is end with good end.
    /// Treat as the wrap up point of event
    /// </summary>
    public override void Reward(){
        Debug.Log("Reward function has been called.");
        if (reward_panel == null)
        {
            Debug.Log("Panel is null");
        }
        if (reward_panel != null)
        {
            //Debug.Log("Activate panel");
            reward_panel.gameObject.SetActive(true);            
        }
    }

    /// <summary>
    /// Manager will call this function when this event is end with bad end.
    /// Treat as the wrap up point of event
    /// </summary>
    public override void Punish(){
        Debug.Log("Punish function has been called.");
        if (punish_panel == null)
        {
            Debug.Log("Panel is null");
        }
        if (punish_panel != null)
        {
            //Debug.Log("Activate panel");
            punish_panel.gameObject.SetActive(true);
        }
    }




}
