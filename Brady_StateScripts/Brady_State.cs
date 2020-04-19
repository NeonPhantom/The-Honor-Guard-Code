using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brady_State {

    protected Brady_StateController stateController;
    //constructor
    public Brady_State(Brady_StateController stateController)
    {
        this.stateController = stateController;
    }
    public abstract void CheckTransitions();

    public abstract void Act();

    public virtual void OnStateEnter() { }

    public virtual void OnStateExit() { }



	
}
