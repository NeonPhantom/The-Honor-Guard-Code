using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State {

    Transform destination;

    public PatrolState(StateController stateController) : base(stateController) { }

    public override void CheckTransitions()
    {
        if (stateController.CheckIfInRange("Player") && (!stateController.hero.invisible || !stateController.hero.Emma.activeInHierarchy))
        {
            stateController.SetState(new ChaseState(stateController));
        }
    }
    public override void Act()
    {
        stateController.ai.transform.LookAt(stateController.ai.target);

        if (destination == null || stateController.ai.DestinationReached())
        {
            destination = stateController.GetNextNavPoint();
            stateController.ai.SetTarget(destination);
            stateController.ai.agent.SetDestination(destination.position);
        }
        
        
    }
    public override void OnStateEnter()
    {
        destination = stateController.GetNextNavPoint();
        if (stateController.ai.agent != null)
        {
            stateController.ai.agent.speed = 10f;
            stateController.ai.agent.angularSpeed = 10;
            stateController.ai.agent.acceleration = 30;
        }

        stateController.enemyAnimator.SetBool("IsRunning", false);
        stateController.ai.SetTarget(destination);
    }

}
