using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{ 
    public class NothingState : State
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
            if(agent.IsNearPlayer(agent.GetSafeDistanceExtended()))
            {
                agent.mStateMachine.ChangeState(States.Attack);
            }
        }
    
        public override void Exit(Enemy agent)
        {
    
        }
    }
}
