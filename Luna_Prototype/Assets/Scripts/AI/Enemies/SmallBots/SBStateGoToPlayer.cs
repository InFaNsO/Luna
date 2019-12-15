using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class SBStateGoToPlayer: State
    {
        private bool setOA;
        private bool setAr;

        public override void Enter(Enemy agent)
        {
            agent.GetSteeringModule().TurnAllOff();
            if (!agent.GetSteeringModule().Exists(SteeringType.Arrive))
            {
                agent.GetSteeringModule().AddState<BehaviourSeek>();
            }
            if (!agent.GetSteeringModule().Exists(SteeringType.ObstacleAvoidance))
            {
                agent.GetSteeringModule().AddState<BehaviourObstacleAvoidance>();
            }

            if (!agent.GetSteeringModule().IsActive(SteeringType.Arrive))
            {
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, true);
                setAr = true;
            }
            if (!agent.GetSteeringModule().IsActive(SteeringType.ObstacleAvoidance))
            {
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, true);
                setOA = true;
            }

            agent.SetDestination(agent.GetWorld().mPlayer.transform.position);
        }

        public override void Update(Enemy agent)
        {

        }

        public override void Exit(Enemy agent)
        {
            if(setAr)
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, false);
            if (setOA)
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, false);

        }
    }
}
