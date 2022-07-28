using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//any class that wants to call notification must have these functions
//so when the player clicks the notification, the appropriate action will be called
//as well as when the noti is finished
public interface NotiInterface
{
    //example: when the player clicks maintenance notification, the appropriate mini game panel should pop up
    void OnNotificationClick();


    //example: when the player finishes the mini game, notification should be cleared
    void OnNotificationQuit();
}

