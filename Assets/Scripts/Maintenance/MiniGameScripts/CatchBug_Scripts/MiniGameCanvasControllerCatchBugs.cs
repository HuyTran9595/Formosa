using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCanvasControllerCatchBugs : MiniGameCanvasController
{
    //override opengame method
    public override void Execute()
    {
        base.message = "The bugs are eating the plants! Kill them or lose coins overtime!";
        base.CallNotification(base.message);
        if (panel != null)
        {
            panel.gameObject.SetActive(true);
            panel.gameObject.GetComponent<MGCatchBugsController>().InitiateGame();
        }
        //Debug.Log("Execute in catch bug is called.");
        QuitGame();
    }


}
