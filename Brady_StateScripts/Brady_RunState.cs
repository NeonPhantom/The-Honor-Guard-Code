using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brady_RunState : Brady_State {

    Transform destination;

    public Brady_RunState(Brady_StateController stateController) : base(stateController) { }

    public override void CheckTransitions()
    {
        if (stateController.CheckIfInRange("Player"))
        {
            stateController.SetState(new Brady_AttackState(stateController));
        }
        if (stateController.bradyHealth < stateController.halfHealth && stateController.spray == true)
        {
            stateController.SetState(new Brady_AcidState(stateController));
        }
    }
    public override void Act()
    {
        stateController.ai.transform.LookAt(stateController.ai.target);

        if (destination == null || stateController.ai.DestinationReached())
        {
            stateController.StartCoroutine(stateController.throwKnives());
            destination = stateController.GetNextNavPoint();
            stateController.ai.SetTarget(destination);
            stateController.ai.agent.SetDestination(destination.position);
        }
        

    }
    public override void OnStateEnter()
    {
        stateController.punch.SetActive(false);
        destination = stateController.GetNextNavPoint();
        if (stateController.ai.agent != null)
        {
            stateController.ai.agent.speed = 50f;
            stateController.ai.agent.angularSpeed = 100;
            stateController.ai.agent.acceleration = 200;
        }

        stateController.bradyAnimator.SetBool("IsRunning", true);
        stateController.ai.SetTarget(destination);
    }
}
