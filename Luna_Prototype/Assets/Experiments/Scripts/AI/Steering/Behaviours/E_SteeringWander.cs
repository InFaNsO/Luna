using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SteeringWander : E_SteeringBase
{
    E_Enemy agent;

    struct MySphere
    {
        public Vector3 center;
        public float radius;
    }


    public void Start()
    {
        agent = GetComponentInParent<E_Enemy>();
    }

    public override Vector3 Calculate()
    {
        MySphere wander = new MySphere();
        wander.center.x = (agent.transform.position.x + ((agent.mAttackRange.radius * 0.5f) * agent.transform.forward.x));
        wander.center.y = (agent.transform.position.y + ((agent.mAttackRange.radius * 0.5f) * agent.transform.forward.y));
        wander.radius = agent.mAttackRange.radius;

        Vector3 random = Vector3.zero;
        random.x = Random.Range(-1, 1);
        random.y = Random.Range(-1, 1);

        random.Normalize();

        random *= wander.radius;

        Vector3 target = random + wander.center;

        agent.mAgent.mTarget = target;
        return Vector3.zero;
    }

    public override SteeringType GetName()
    {
        return SteeringType.Wander;
    }
}
