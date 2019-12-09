using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class SBStateGoToPlayer<AgentType> : State<AgentType> where AgentType : Enemy
    {
        private bool setOA;
        private bool setAr;

        public override void Enter(AgentType agent)
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

        public override void Update(AgentType agent)
        {

        }

        public override void Exit(AgentType agent)
        {
            if(setAr)
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, false);
            if (setOA)
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, false);

        }
    }
}
