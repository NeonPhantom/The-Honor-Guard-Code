using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brady_AcidState : Brady_State {

    Transform destination;
    public float navigationUpdate;
    private float navigationTime = 0;

    public Brady_AcidState(Brady_StateController stateController) : base(stateController) { }

    public override void CheckTransitions()
    {
        if (stateController.sprayTimer <= 0)
        {
            stateController.spray = false;
            stateController.bradyAnimator.SetBool("AcidSpray", false);
            stateController.SetState(new Brady_RunState(stateController));
        }
    }
    public override void Act()
    {
        if (stateController.ai.DestinationReached())
        {
            stateController.ai.transform.LookAt(stateController.knifeTarget.transform);
            if (stateController.sprayTimer % 20 == 0)
            {
                stateController.sprayAcid();
            }
            stateController.sprayTimer--;
        }
    }
    
    public override void OnStateEnter()
    {
        stateController.ai.agent.speed = 50f;
        stateController.ai.agent.angularSpeed = 100;
        stateController.ai.agent.acceleration = 200;

        stateController.ai.SetTarget(stateController.centerPosition);
        stateController.ai.transform.LookAt(stateController.ai.target);
        stateController.ai.agent.SetDestination(stateController.ai.target.position);
        
        stateController.bradyAnimator.SetBool("AcidSpray", true);
    }
}
