using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public abstract class State <AgentType> where AgentType : Enemy
    {
        public abstract void Enter(AgentType agent);
        public abstract void Update(AgentType agent);
        public abstract void Exit(AgentType agent);
    }
}
