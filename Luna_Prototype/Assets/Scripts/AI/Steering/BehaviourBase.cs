using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public enum SteeringType
    {
        Base,
        Seek,
        Arrive,
        Wander,
        WanderGround,
        ObstacleAvoidance,
    }

    public abstract class SteeringBehaviourBase
    {
        protected bool mActive = false;

        public abstract Vector2 Calculate(Agent agent);
        public abstract SteeringType GetName();

        public Vector2 Truncate(Vector2 v, float amount)
        {
            Vector2 vec = v;
            if (v.x > amount)
                vec.x = amount;
            if (v.y > amount)
                vec.y = amount;
            return vec;
        }

        public void SwichActive() { mActive = !mActive; }
        public void SetActive(bool act) { mActive = act; }
        public bool IsActive() { return mActive; }
    }
}
