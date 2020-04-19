using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State {

    Transform destination;
    public float navigationUpdate;
    private float navigationTime = 0;

    public ChaseState(StateController stateController) : base(stateController) { }

    public override void CheckTransitions()
    {
        if (!stateController.CheckIfInRange("Player") || (stateController.hero.invisible && stateController.hero.Emma.activeInHierarchy))
        {
            stateController.SetState(new PatrolState(stateController));
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
        }

        if (stateController.enemyHealth <= 0)
        {
            stateController.enemyAnimator.SetTrigger("Dead");
        }
    }

    
    public override void OnStateEnter()
    {
        stateController.ai.agent.speed = 50f;
        stateController.ai.agent.angularSpeed = 100;
        stateController.ai.agent.acceleration = 200;

        stateController.enemyAnimator.SetBool("IsRunning", true);
    }
}
