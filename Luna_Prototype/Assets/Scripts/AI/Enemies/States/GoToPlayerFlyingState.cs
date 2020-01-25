using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class GoToPlayerFlyingState : State
    {
        [SerializeField] Platform ground = new Platform();
        [SerializeField] bool isBat = false;
        Vector3 groundTarget = new Vector3();

        bool hasReachedGround = false;

        public override States Name()
        {
            return States.GoToPlayer;
        }

        public override void Enter(Enemy agent)
        {
            agent.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (isBat)
            {
                hasReachedGround = true;
                agent.SetDestination(agent.GetWorld().mPlayer.transform.position);
            }
            else
            {
                groundTarget = agent.transform.position;
                groundTarget.y = ground.transform.position.y + (ground.Height * 0.5f);
                groundTarget.x -= 1.0f;

                agent.SetDestination(groundTarget);
            }
        }

        public override void Update(Enemy agent)
        {
            float attackRange = agent.GetSafeDistance();
            if(agent.IsNearPlayer(attackRange))
            {
                agent.mStateMachine.ChangeState(States.Attack);
                return;
            }
            if(!hasReachedGround && Mathf.Abs(agent.transform.position.y - groundTarget.y) < agent.GetSafeDistanceReduced() )
            {
                agent.SetDestination(agent.GetWorld().mPlayer.transform.position);
                hasReachedGround = true;
            }

            if(!agent.IsNearPlayer(agent.GetSafeDistanceExtended()) && hasReachedGround)
            {
                agent.mStateMachine.ChangeState(States.Wander);
                return;
            }

            if(hasReachedGround)
            {
                agent.SetDestination(agent.GetWorld().mPlayer.transform.position);
            }
        }

        public override void Exit(Enemy agent)
        {
            agent.GetComponent<Rigidbody2D>().gravityScale = 1;

        }
    }
}