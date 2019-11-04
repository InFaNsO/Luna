using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class BehaviourSeek : SteeringBehaviourBase
    {
        public override Vector2 Calculate(Agent agent)
        {
            Vector2 v;
            v = agent.GetDestination() - agent.GetPosition();
            v.Normalize();
            v *= agent.GetMaxSpeed();
            // v -= agent.GetVelocity();
            v /= agent.GetMass();
            return v;
        }

        public override SteeringType GetName()
        {
            return SteeringType.Seek;
        }
    }
}