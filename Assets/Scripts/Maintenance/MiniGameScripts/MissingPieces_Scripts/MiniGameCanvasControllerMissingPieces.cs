using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCanvasControllerMissingPieces : MiniGameCanvasController
{


    public override void Execute()
    {
        base.message = "Some pieces of the painting are missing! Fix them or lose coins overtime!";
        base.CallNotification(base.message);
        if (panel != null)
        {
            panel.gameObject.SetActive(true);
            panel.gameObject.GetComponent<MGMissingPiecesController>().InitiateGame();
        }
        //Debug.Log("Execute in catch bug is called.");
        QuitGame();
    }




}
