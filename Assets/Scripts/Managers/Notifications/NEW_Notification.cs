using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_Notification: MonoBehaviour
{

    //each notification will have a unique ID that is assigned when they create the notification
    //the class that instantiate new_notification should assign its GetInstandID() to uniqueID
    private int uniqueId;
    private bool IsIDassigned = false;

    public NEW_Notification(int uniqueID)
    {
        AssignUniqueID(uniqueID);
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


    //check if the notification's Unique ID matches the input
    public bool CompareUniqueID(int anotherID)
    {
        if (uniqueId == anotherID)
        {
            return true;
        }
        return false;
    }

    public int GetUniqueID()
    {
        return uniqueId;
    }
    

}
