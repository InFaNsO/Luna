using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class DoNothingState : State
    {
        public override States Name()
        {
            return States.None;
        }

        public override void Enter(Enemy agent)
        {

        }

        public override void Update(Enemy agent)
        {
            
        }

        public override void Exit(Enemy agent)
        {

        }
    }
}