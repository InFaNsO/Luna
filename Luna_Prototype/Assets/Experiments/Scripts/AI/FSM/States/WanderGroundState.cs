
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderGroundState : State
{
    Enemy agent;
    PathFinding finder;
    SteeringModule mSteeringModule;

    Collider2D mPlayerCollider;

    int target;

    private void Start()
    {
        agent = GetComponentInParent<Enemy>();
        mSteeringModule = agent.mSteering;
    }

    public override void Enter()
    {
        finder = agent.mPathFinding;

        mSteeringModule.TurnAllOff();
        mSteeringModule.SetActive(SteeringType.Arrive, true);

        agent.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        agent.mRigidBody.gravityScale = 1.0f;

        float f = Random.value;

        target = (int)(f * finder.mNodes.Count);

        finder.FindPath(agent.transform.position, target);
        agent.mAgent.mPath = finder.GetPath();
        agent.mAgent.mTarget = agent.mAgent.mPath[0];
        agent.mAgent.mPath.RemoveAt(0);

        mPlayerCollider = agent.mZone.mPlayerTransform.GetComponent<Collider2D>();
    }
    public override void MyUpdate()
    {
        if(mPlayerCollider == null)
        {
            mPlayerCollider = agent.mZone.mPlayerTransform.GetComponent<Collider2D>();
            if (mPlayerCollider == null)
                return;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(agent.transform.position, agent.transform.forward, agent.mPlayerVisibilityRange.radius, LayerMask.NameToLayer("Charachter"));

        if (agent.mAttackRange.IsTouching(mPlayerCollider))// && hitInfo.collider == mPlayerCollider)
        {
            agent.mStateMachine.ChangeState("Attack");
            return;
        }
        else if(agent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))// && hitInfo.collider == mPlayerCollider)
        {
            agent.mStateMachine.ChangeState("Chase");
            return;
        }

        //if (calculateAgain)  //or
        //{
        //    target = (int)(Random.value * finder.mNodes.Count);
        //    finder.FindPath(agent.transform.position, target);
        //    agent.mAgent.mPath.Clear();
        //    agent.mAgent.mPath = finder.GetPath();
        //    if (agent.mAgent.mPath.Count != 0)
        //    {
        //        agent.mAgent.mTarget = agent.mAgent.mPath[0];
        //        agent.mAgent.mPath.RemoveAt(0);
        //    }
        //}
        if (agent.mAgent.mPath.Count == 0)
        {
            target = (int)(Random.value * finder.mNodes.Count);
            finder.FindPath(agent.transform.position, target);
            agent.mAgent.mPath.Clear();
            agent.mAgent.mPath = finder.GetPath();

            if (agent.mAgent.mPath.Count > 0)
            {
                agent.mAgent.mTarget = agent.mAgent.mPath[0];
                agent.mAgent.mPath.RemoveAt(0);
            }
        }

        bool isClose = agent.IsNearTarget(agent.mNodeRange.radius);

        if (isClose && agent.mAgent.mPath.Count != 0)
        {
            if (agent.mAgent.mTarget.y < agent.mAgent.mPath[0].y)
            {
               // agent.mRigidBody.velocity = new Vector2(agent.mRigidBody.velocity.x ,  0.0f);
                var force = new Vector2((agent.transform.position.x < agent.mAgent.mPath[0].x ? agent.mAgent.mMaxSpeed : agent.mAgent.mMaxSpeed * -1), agent.mAgent.mJumpStrength * 1);
                agent.mRigidBody.AddForce(force , ForceMode2D.Impulse);
            }
            else if (agent.mAgent.mTarget.y != agent.mAgent.mPath[0].y)
            {
                var force = new Vector2((agent.transform.position.x < agent.mAgent.mPath[0].x ? agent.mAgent.mMaxSpeed : agent.mAgent.mMaxSpeed * -1), agent.mAgent.mJumpStrength * 0.5f);
                agent.mRigidBody.AddForce(force, ForceMode2D.Impulse);
            }
            agent.mAgent.mTarget = agent.mAgent.mPath[0];
            agent.mAgent.mPath.RemoveAt(0);
        }

    }

    public override void Exit()
    {

    }

    public override void DebugDraw()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(agent.transform.position, agent.transform.position + new Vector3(agent.mRigidBody.velocity.x * 10, agent.mRigidBody.velocity.y * 10, 0.0f));

        Gizmos.DrawWireSphere(agent.mAgent.mTarget, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(agent.mAgent.mPath[agent.mAgent.mPath.Count - 1], 0.5f);

        for (int i = 0; i < finder.mNodes.Count; ++i)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(finder.mNodes[i].pos, 0.3f);

            Gizmos.color = Color.cyan;

            for (int j = 0; j < finder.mNodes[i].childrenID.Count; ++j)
            {
                Gizmos.DrawLine(finder.mNodes[i].pos, finder.mNodes[finder.mNodes[i].childrenID[j]].pos);
            }
        }

        var Path = agent.mAgent.mPath;
        Gizmos.color = Color.red;
        if (Path.Count > 0)
        {
            for (int i = 1; i < Path.Count; ++i)
            {
                Gizmos.DrawWireSphere(Path[i - 1], 0.3f);
                Gizmos.DrawLine(Path[i - 1], Path[i]);
            }
            Gizmos.DrawLine(Path[Path.Count - 1], agent.mZone.mPlayerTransform.position);
        }
        else
            Gizmos.DrawLine(agent.transform.position, agent.mZone.mPlayerTransform.position);
    }

    public override string GetName()
    {
        return "Wander";
    }
}
