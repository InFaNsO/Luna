using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class BehaviourWanderGround : SteeringBehaviourBase
    {
        private struct Circle
        {
            public Vector2 center;
            public float radius;
        }


        public override Vector2 Calculate(Agent agent)
        {
            return new Vector2(-1.0f, 1.0f);
        }

        public override SteeringType GetName()
        {
            return SteeringType.WanderGround;
        }
    }
}