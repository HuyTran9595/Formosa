using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//all scripts for each quest must have these functions
public interface SingleQuestInterface 
{
    
    void Subscribe();//subscribe to the appropriate delegates for each quest
    void Unsubscribe(); //when the quest is finished, unsubscribe from appropriate delegate

    bool IsActivated();
}
