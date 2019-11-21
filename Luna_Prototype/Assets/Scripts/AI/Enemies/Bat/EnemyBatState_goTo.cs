using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LAI
{
    public class EnemyBatState_goTo<AgentType> : State<AgentType> where AgentType : Enemy_Bat
    {
        private bool setOA;
        private bool setSeek;
        private float attackRange = 1.6f;

        private float attackDelay = 0.5f;
        private float attackPosOffSet = 1.0f;

        private float delayCounter = 0.0f;
        private bool startDelay = false;


        public override void Enter(AgentType agent)
        {
            delayCounter = 0.0f;
            startDelay = false;

            agent.GetSteeringModule().TurnAllOff();
            if (!agent.GetSteeringModule().Exists(SteeringType.Arrive))
            {
                agent.GetSteeringModule().AddState<BehaviourArrive>();
            }
            if (!agent.GetSteeringModule().Exists(SteeringType.ObstacleAvoidance))
            {
                agent.GetSteeringModule().AddState<BehaviourObstacleAvoidance>();
            }

            if (!agent.GetSteeringModule().IsActive(SteeringType.Arrive))
            {
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, true);
                setSeek = true;
            }
            if (!agent.GetSteeringModule().IsActive(SteeringType.ObstacleAvoidance))
            {
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, true);
                setOA = true;
            }

            Vector3 dir = Vector3.Normalize(agent.transform.position - agent.GetWorld().mPlayer.transform.position);
            Vector3 attackPos = dir * attackRange;
            attackPos.y = agent.GetWorld().mPlayer.transform.position.y + attackRange;
            agent.SetDestination(attackPos);
        }

        public override void Update(AgentType agent)
        {
            if (Vector3.Distance(agent.GetPosition(), agent.GetDestination()) < attackPosOffSet)
            {
                startDelay = true;
            }

            if (startDelay)
            {
                delayCounter += Time.deltaTime;
            }

            if (delayCounter >= attackDelay)
                agent.mStateMachine.ChangeState((int)Enemy_Bat.States.rangeAttack);
        }

        public override void Exit(AgentType agent)
        {
            if (setSeek)
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, false);
            if (setOA)
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, false);

        }
    }
}