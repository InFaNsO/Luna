using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class EnemyBatState_meleeAttack : State 
    {
        public float toIdelDelay = 3.0f;
        private float toIdelCounter = 0.0f;
        bool isOnPlayerleft = false;
        float yDifferent = 0.0f;
        Vector3 destPos;
        bool isAttacked = false;

        public override States Name()
        {
            return States.Attack;
        }

        public override void Enter(Enemy agent)
        {
            agent.SetVelocity(Vector2.zero);
            agent.GetSteeringModule().TurnAllOff();

            if (!agent.GetSteeringModule().Exists(SteeringType.Arrive))
            {
                agent.GetSteeringModule().AddState<BehaviourArrive>();
            }
            if (!agent.GetSteeringModule().IsActive(SteeringType.Arrive))
            {
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, true);
            }

            isAttacked = false;
            isOnPlayerleft = agent.transform.position.x < agent.GetWorld().mPlayer.transform.position.x;
            yDifferent = agent.transform.position.y - agent.GetWorld().mPlayer.transform.position.y;

            Vector3 dir = Vector3.Normalize(agent.GetWorld().mPlayer.transform.position - agent.transform.position);
            float distance = Vector3.Magnitude(agent.GetWorld().mPlayer.transform.position - agent.transform.position);
            destPos = dir * distance * 2.0f;
            agent.SetDestination(destPos);

            toIdelCounter = 0.0f;
        }

        public override void Update(Enemy agent)
        {
            if (toIdelCounter >= toIdelDelay)
            {
                agent.mStateMachine.ChangeState((int)Enemy_Bat.States.Idel);
                return;
            }

            Vector3 agentPos = agent.transform.position;
            Vector3 playerPos = agent.GetWorld().mPlayer.transform.position;

            if (isOnPlayerleft)
            {
                if (agentPos.x > playerPos.x || agentPos.y < playerPos.y)
                    agent.SetDestination(destPos + new Vector3(0.0f, yDifferent * 2.0f, 0.0f));
            }
            else
            {
                if (agentPos.x < playerPos.x || agentPos.y < playerPos.y)
                    agent.SetDestination(destPos + new Vector3(0.0f, yDifferent * 2.0f, 0.0f));
            }

            if (Vector3.SqrMagnitude(agent.GetWorld().mPlayer.transform.position - agent.transform.position) < 0.25f && !isAttacked)
            {
                Enemy_Bat batAgent = (Enemy_Bat)agent;
                Bullet newBullet = Object.Instantiate(batAgent.mMeleeBullet, new Vector3(0, 0, 0), Quaternion.identity);
                newBullet.Awake();
                newBullet.Fire(agent.tag, batAgent.mDamage, agent.transform.position, Vector3.down, WeaponType.Melee);
                isAttacked = true;
            }

            toIdelCounter += Time.deltaTime;
        }

        public override void Exit(Enemy agent)
        {
            agent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}