using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class BehaviourWander : SteeringBehaviourBase
    {
        private struct Circle
        {
            public Vector2 center;
            public float radius;
        }


        public override Vector2 Calculate(Agent agent)
        {
            Circle wander;
            wander.center.x = (agent.GetPosition().x + ((agent.GetSafeDistance() * 0.5f) * agent.GetHeading().x));
            wander.center.y = (agent.GetPosition().y + ((agent.GetSafeDistance() * 0.5f) * agent.GetHeading().y));
            wander.radius = 50.0f;

            Vector2 random;
            random.x = Random.Range(-100, 100);
            random.y = Random.Range(-100, 100);

            random.Normalize();

            Vector2 target = random * wander.radius;
            target += wander.center;


            Vector2 desired, steer;
            desired = (target - agent.GetPosition()) * agent.GetMaxSpeed();
            steer = desired - agent.GetVelocity();
            steer /= agent.GetMass();

            agent.SetDestination(target);

            return steer;
        }

        public override SteeringType GetName()
        {
            return SteeringType.Wander;
        }
    }
}
