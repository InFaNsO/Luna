using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LAI
{
    public class EnemyBatState_goTo : State
    {
        private bool setOA;
        private bool setSeek;
        private float attackRange = 1.6f;

        private float attackDelay = 0.5f;
        private float attackPosOffSet = 1.0f;

        private float delayCounter = 0.0f;
        private bool startDelay = false;

        public override States Name()
        {
            return States.GoToPlayer;
        }

        public override void Enter(Enemy agent)
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

        public override void Update(Enemy agent)
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
            {
                int rand = Random.Range(1, 2);
                if (rand == 0)
                    agent.mStateMachine.ChangeState((int)Enemy_Bat.States.rangeAttack);
                else
                    agent.mStateMachine.ChangeState((int)Enemy_Bat.States.meleeAttack);
            }
        }

        public override void Exit(Enemy agent)
        {
            if (setSeek)
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, false);
            if (setOA)
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, false);

        }
    }
}