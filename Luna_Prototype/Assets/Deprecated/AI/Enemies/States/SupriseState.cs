using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class SupriseState : State
    {
        public override States Name()
        {
            return States.Suprise;
        }

        public override void Enter(Enemy agent)
        {
            agent.GetComponent<SpriteRenderer>().color = Color.cyan;

        }

        public override void Update(Enemy agent)
        {
            bool done = false;
            //do suprisae stuff

            done = true;
            //then switch to attack
            if(done)
            {
                agent.mStateMachine.ChangeState(States.Attack);
            }
        }

        public override void Exit(Enemy agent)
        {

        }
    }
}