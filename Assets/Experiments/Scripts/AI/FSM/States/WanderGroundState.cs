
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

    [SerializeField] float LowJumpMultiplier = 0.8f;
    [SerializeField] float HighJumpMultiplier = 1.0f;

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
        mSteeringModule.SetActive(SteeringType.Seek, true);

        agent.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        agent.mRigidBody.gravityScale = 1.0f;

        target = Random.Range(0, finder.mNodes.Count);

        finder.FindPath(agent.transform.position, target);
        agent.mAgent.mPath = finder.GetPath();

        UpdatePath();

        mPlayerCollider = agent.mZone.mPlayerTransform.GetComponent<Collider2D>();



    }
    public override void MyUpdate()
    {
        if (!agent.myHealth.IsAlive())
        {
            agent.mStateMachine.ChangeState(EnemyStates.Die.ToString());
            return;
        }
        if (mPlayerCollider == null)
        {
            mPlayerCollider = agent.mZone.mPlayerTransform.GetComponent<Collider2D>();
            if (mPlayerCollider == null)
                return;
        }

        CheckTurn();

        if (agent.mAttackRange.IsTouching(mPlayerCollider))
        {
            agent.mStateMachine.ChangeState(EnemyStates.Attack.ToString());
            return;
        }
        else if(agent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            agent.mStateMachine.ChangeState(EnemyStates.Chase.ToString());
            return;
        }

        bool isX = (int)agent.transform.position.x == (int)agent.mAgent.mTarget.x;
        bool isClose = agent.IsNearTarget(agent.mNodeRange.radius);
        if (agent.mAgent.mPath.Count == 0 && isClose || agent.mAgent.mPath.Count == 0 && isX)
        {
            target = Random.Range(0, finder.mNodes.Count);
            finder.FindPath(agent.transform.position, target);
            agent.mAgent.mPath.Clear();
            agent.mAgent.mPath = finder.GetPath();

            if (agent.mAgent.mPath.Count > 0)
            {
                UpdatePath();
            }
        }

        bool isEnd = isClose && agent.mAgent.mPath.Count != 0;

        if (agent.mAgent.mPath.Count > 0 && isEnd || isX)
        {
            if (agent.mAgent.mTarget.y < agent.mAgent.mPath[0].y)
            {
               // agent.mRigidBody.velocity = new Vector2(agent.mRigidBody.velocity.x ,  0.0f);
                var force = new Vector2((agent.transform.position.x < agent.mAgent.mPath[0].x ? agent.mMovementSpeed : agent.mMovementSpeed  * -1), agent.mJumpStrength * HighJumpMultiplier);
                agent.mRigidBody.AddForce(force , ForceMode2D.Impulse);
            }
            else if (agent.mAgent.mTarget.y != agent.mAgent.mPath[0].y)
            {
                var force = new Vector2(agent.mMovementSpeed * agent.transform.position.x < agent.mAgent.mPath[0].x ? 1.0f : -1.0f, agent.mJumpStrength * LowJumpMultiplier);
                agent.mRigidBody.AddForce(force, ForceMode2D.Impulse);
            }

            UpdatePath();
        }

    }

    void UpdatePath()
    {
        agent.mAgent.mTarget = agent.mAgent.mPath[0];
        agent.mAgent.mPath.RemoveAt(0);
    }

    void CheckTurn()
    {
        if (agent.mAgent.mTarget.x < agent.transform.position.x && !agent.IsFacingLeft)
            agent.Turn();
        else if (agent.mAgent.mTarget.x > agent.transform.position.x && agent.IsFacingLeft)
            agent.Turn();
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

        if (agent.mAgent.mPath.Count == 0)
            return;

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
        return EnemyStates.Wander.ToString();
    }
}
