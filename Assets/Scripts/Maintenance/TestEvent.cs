using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : MaintenanceEvent
{
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Execute()
    {
        //Execute the event
        Debug.Log(name.ToString() + " execute");
        //After awhile
        //...
        //...
        //...
        //Event ended
        EventEnd(true);
    }
    public override void Reward()
    {
        Debug.Log("Event ended with success");
    }
    public override void Punish()
    {
        Debug.Log("Event ended with failure");
    }

    //this function does nothing.
    public override GameObject GetGamePanel()
    {
        return null;
    }
}
