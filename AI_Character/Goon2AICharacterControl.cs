using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    public class Goon2AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public Transform target;                                    // target to aim for

        public float navigationUpdate;
        private float navigationTime = 0;
        Rigidbody rigidbody;
        public bool dead = false;

        private void Start()
        {
            //get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
	        agent.updateRotation = true;
	        agent.updatePosition = false;
            rigidbody = GetComponent<Rigidbody>();
            
        }


        private void Update()
        {
            if (!dead)
            {
                if (target != null)
                {
                    rigidbody.constraints = RigidbodyConstraints.None;
                    agent.SetDestination(target.position);
                }
                else
                {
                    rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
                }
            } else
            {
                rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
            }

        }

        protected void LateUpdate()
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }

        public bool DestinationReached()
        {
            return agent.remainingDistance < agent.stoppingDistance;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
