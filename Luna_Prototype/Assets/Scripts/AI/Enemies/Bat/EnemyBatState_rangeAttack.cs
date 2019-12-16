using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class EnemyBatState_rangeAttack: State
    {
        public float toIdelDelay = 3.0f;
        private float toIdelCounter = 0.0f;
        public float attackSpeed = 1.0f;
        private float attackSpeedCounter = 0.0f;

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

            toIdelCounter = 0.0f;
            attackSpeedCounter = 0.0f;
        }

        public override void Update(Enemy agent)
        {
            if (toIdelCounter >= toIdelDelay)
            {
                agent.mStateMachine.ChangeState((int)Enemy_Bat.States.Idel);
                return;
            }

            if(attackSpeedCounter >= attackSpeed)
            {
                Enemy_Bat batAgent = (Enemy_Bat)agent;

                batAgent.mBullet.tag = agent.gameObject.tag;

                Bullet newBullet = Object.Instantiate(batAgent.mBullet, new Vector3(0, 0, 0), Quaternion.identity);
                Vector3 dir = Vector3.Normalize(agent.transform.position - agent.GetWorld().mPlayer.transform.position);
                newBullet.Fire(agent.tag, batAgent.mDamage, agent.transform.position, -dir, WeaponType.Range);
                //recoil force to make it looks good
                agent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                agent.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 100.0f * Time.deltaTime, ForceMode2D.Impulse);

                attackSpeedCounter = 0.0f;
            }

            toIdelCounter += Time.deltaTime;
            attackSpeedCounter += Time.deltaTime;

        }

        public override void Exit(Enemy agent)
        {
            agent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}