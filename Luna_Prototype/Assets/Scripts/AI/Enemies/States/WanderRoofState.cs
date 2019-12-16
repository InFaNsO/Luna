using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class WanderRoofState : State
    {
        [SerializeField] Platform roof = new Platform();

        Vector3 left = new Vector3();
        Vector3 right = new Vector3();

        bool isGoingLeft = false;

        public override States Name()
        {
            return States.Wander;
        }

        public override void Enter(Enemy agent)
        {
            left = roof.transform.position;
            right = roof.transform.position;

            left.x -= roof.Width * 0.5f;
            right.x += roof.Width * 0.5f;

            left.y -= roof.Height * 0.5f;
            right.y -= roof.Height * 0.5f;

            float distanceLeft = Vector3.Distance(agent.transform.position, left);
            float distanceRight = Vector3.Distance(agent.transform.position, right);

            if(distanceLeft > distanceRight)
            {
                isGoingLeft = false;
                agent.SetDestination(right);
            }
            else
            {
                isGoingLeft = true;
                agent.SetDestination(left);
            }
        }

        public override void Update(Enemy agent)
        {
            float findRange = agent.GetSafeDistanceExtended(); 
            if (agent.IsNearPlayer(findRange) && agent.GetWorld().HasLineOfSight(new World.Wall(agent.transform.position, agent.GetWorld().mPlayer.transform.position)))
            {
                agent.mStateMachine.ChangeState(States.GoToPlayer);
                return;
            }

            bool isClose = agent.IsNearDestination(agent.GetSafeDistanceReduced());

            if (isClose)
            {
                if (isGoingLeft)
                {
                    agent.SetDestination(right);
                    isGoingLeft = false;
                }
                else
                {
                    agent.SetDestination(left);
                    isGoingLeft = true;
                }
            }
        }

        public override void Exit(Enemy agent)
        {

        }
    }
}