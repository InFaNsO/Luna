using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringArrive : SteeringBase
{
    Enemy agent;

    public void Start()
    {
        agent = GetComponentInParent<Enemy>();
    }

    public override Vector3 Calculate()
    {
        Vector2 desiered, steer;
        desiered = (agent.mAgent.mTarget - agent.transform.position);
        steer = desiered - agent.mRigidBody.velocity;
        steer /= agent.mRigidBody.mass;
        return new Vector3(steer.x, steer.y, 0.0f);
    }

    public override SteeringType GetName()
    {
        return SteeringType.Arrive;
    }
}
