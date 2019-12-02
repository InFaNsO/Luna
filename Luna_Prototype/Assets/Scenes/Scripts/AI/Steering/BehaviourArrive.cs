using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class BehaviourArrive : SteeringBehaviourBase
    {
        public override Vector2 Calculate(Agent agent)
        {
            Vector2 desiered, steer;
            desiered = (agent.GetDestination() - agent.GetPosition());
            steer = desiered - agent.GetVelocity();
            steer /= agent.GetMass();
            return steer;
        }

        public override SteeringType GetName()
        {
            return SteeringType.Arrive;
        }
    }
}