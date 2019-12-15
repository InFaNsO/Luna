using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public abstract class State
    {
        public abstract void Enter(Enemy agent);
        public abstract void Update(Enemy agent);
        public abstract void Exit(Enemy agent);
    }
}
