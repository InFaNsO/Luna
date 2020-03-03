using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringSeek : SteeringBase
{
    Enemy agent;

    public void Start()
    {
        agent = GetComponentInParent<Enemy>();
    }

    public override Vector3 Calculate()
    {
        Vector3 v;
        v = agent.mAgent.mTarget - agent.transform.position;
        v.Normalize();
        v *= agent.mAgent.mMaxSpeed;
        v /= agent.mRigidBody.mass;
        return v;
    }

    public override SteeringType GetName()
    {
        return SteeringType.Seek;
    }
}
