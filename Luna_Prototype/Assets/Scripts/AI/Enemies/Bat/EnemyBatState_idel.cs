using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LAI
{
    public class EnemyBatState_idel : State
    {
        private float idelTime = 2.0f;
        private float idelCounter = 0.0f;
        private float time;
        private Vector3 newPos;
        public override void Enter(Enemy agent)
        {
            agent.GetSteeringModule().TurnAllOff();
            agent.SetVelocity(Vector2.zero);
            time = 0.0f;
            idelCounter = 0.0f;
        }

        public override void Update(Enemy agent)
        {
            time += Time.deltaTime * 5.0f;
            newPos.x = agent.transform.position.x + Mathf.Sin(time * 2.0f) * 0.01f;
            newPos.y = agent.transform.position.y + Mathf.Cos(time * 2.0f) * 0.005f;
            agent.transform.position = newPos;

            idelCounter += Time.deltaTime;
            if (idelCounter < idelTime)
                return;

            if (agent.IsNearPlayer(agent.GetSafeDistanceExtended()))
            {
                if (agent.GetWorld().HasLineOfSight(new World.Wall(agent.transform.position, agent.GetWorld().mPlayer.transform.position)))
                {
                    if (agent.IsNearPlayer(agent.GetSafeDistanceReduced()))        //ready to attack
                    {
                        agent.mStateMachine.ChangeState((int)Enemy_Bat.States.rangeAttack);
                    }
                    else                                    //Go to player
                    {
                        agent.mStateMachine.ChangeState((int)Enemy_Bat.States.goTo);
                    }
                }
            }

        }

        public override void Exit(Enemy agent)
        {

        }
    }
}

