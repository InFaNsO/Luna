using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public enum States
    {
        None,
        Wander,
        GoToPlayer,
        Attack,
        Suprise
    }

    public abstract class State
    {
        public abstract void Enter(Enemy agent);
        public abstract void Update(Enemy agent);
        public abstract void Exit(Enemy agent);
        public abstract States Name();
    }
}
