using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brady_AttackState : Brady_State {

    Transform destination;
    public float navigationUpdate;
    private float navigationTime = 0;

    public Brady_AttackState(Brady_StateController stateController) : base(stateController) { }

    public override void CheckTransitions()
    {
        if (!stateController.CheckIfInRange("Player"))
        {
            stateController.SetState(new Brady_RunState(stateController));
        }
        if (stateController.bradyHealth < stateController.halfHealth && stateController.spray == true)
        {
            stateController.SetState(new Brady_AcidState(stateController));
        }
    }
    public override void Act()
    {
        if (stateController.enemyToChase != null)
        {

            destination = stateController.enemyToChase.transform;
            stateController.ai.SetTarget(destination);
            stateController.ai.transform.LookAt(stateController.ai.target);

            stateController.ai.agent.SetDestination(stateController.ai.target.position);

            if (stateController.InAttackRange("Player"))
            {
                stateController.bradyAnimator.SetTrigger("Punch");
                if (stateController.bradyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
                {
                    stateController.punch.SetActive(true);
                }
                else
                {
                    stateController.punch.SetActive(false);
                }
            }

            if (stateController.bradyHealth < stateController.halfHealth && stateController.spray == true)
            {
                stateController.ai.SetTarget(stateController.centerPosition);
                stateController.ai.transform.LookAt(stateController.ai.target);
                stateController.ai.agent.SetDestination(stateController.ai.target.position);
            }

        }

    }

    
    public override void OnStateEnter()
    {
        stateController.ai.agent.speed = 10f;
        stateController.ai.agent.angularSpeed = 10;
        stateController.ai.agent.acceleration = 30;
    }
}
