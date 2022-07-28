using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Pet_AI ai;
    public State(Pet_AI _ai)
    {
        ai = _ai;
    }
    public virtual IEnumerator start()
    {
        yield break;
    }
    public virtual void onClick()
    {
    
    }

    public virtual void onEnd()
    {

    }

    public virtual void onArrive()
    {

    }
}

//Idle state of the pet
//Standing still and ready to go next position
public class FSM_Idle : State
{
    public FSM_Idle(Pet_AI ai) : base(ai) { }

    public override IEnumerator start()
    {
        Debug.Log("Idle");
        float time = 3f;//= Random.Range(0.5f, 5f);
        yield return new WaitForSeconds(time);
        Debug.Log("Done waiting");
        ai.SetState(new FSM_RandomWalk(ai));
    }    

    public override void onClick()
    {
        ai.SetState(new FSM_Wonder(ai));
    }
}

public class FSM_RandomWalk : State
{
    public FSM_RandomWalk(Pet_AI ai) : base(ai) { }

    public override IEnumerator start()
    {
        ai.RandMove();
        if(ai.anim)
            ai.anim.SetTrigger("Walk");
        yield break;
    }

    public override void onArrive()
    {
        if (ai.anim)
            ai.anim.SetTrigger("Stop");
        if (ai.IsReadyToHarvest || ai.isHungry)
            ai.SetState(new FSM_BacktoHome(ai));
        else
            ai.SetState(new FSM_Idle(ai));
    }

}

public class FSM_StayForHarvest : State
{
    public FSM_StayForHarvest(Pet_AI ai) : base(ai) { }

    public override IEnumerator start()
    {
        ai.ShowHarvestIcon();
        yield break;
    }
    public override void onClick()
    {
        ai.Harvest();
        ai.SetState(new FSM_RandomWalk(ai));
    }
}

public class FSM_Wonder : State
{
    public FSM_Wonder(Pet_AI ai) : base(ai) { }
}

public class FSM_BacktoHome : State
{
    public FSM_BacktoHome(Pet_AI ai) : base(ai) { }

    public override IEnumerator start()
    {
        ai.BackToHome();
        if (ai.anim)
            ai.anim.SetTrigger("Walk");
        yield break;
    }

    public override void onArrive()
    {
        if (ai.anim)
            ai.anim.SetTrigger("Stop");
        if (ai.isHungry)
            ai.SetState(new FSM_StayForFeed(ai));
        else if (ai.IsReadyToHarvest)
            ai.SetState(new FSM_StayForHarvest(ai));
        else if (ai.IsReadyToPet)
            ai.SetState(new FSM_StayForPet(ai));
        else
        {
            Debug.Log("Error");
            ai.SetState(new FSM_Idle(ai));
        }
    }
}

public class FSM_StayForFeed : State
{
    public FSM_StayForFeed(Pet_AI ai) : base(ai) { }
    public override IEnumerator start()
    {
        ai.ShowHungryIcon();
        yield break;
    }
    public override void onClick()
    {
        //TODO

        ai.ShowFeedPanel();
        //ai.Feed();
        //ai.Harvest();
        //ai.SetState(new Idle(ai));
    }
}

public class FSM_StayForPet : State
{
    public FSM_StayForPet(Pet_AI ai) : base(ai) { }
    public override IEnumerator start()
    {
        ai.ShowPettingIcon();
        yield break;
    }
    public override void onClick()
    {
        //TODO

        ai.Petting();
        ai.SetState(new FSM_RandomWalk(ai));
    }
}
