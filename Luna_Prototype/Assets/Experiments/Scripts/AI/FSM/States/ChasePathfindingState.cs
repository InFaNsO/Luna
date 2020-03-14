using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePathfindingState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;

    PathFinding finder;

    int mPlayerNodeID = 0;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<Enemy>();
    }

    public override void Enter()
    {
        mPlayer = mAgent.mZone.mPlayerTransform.GetComponent<Player>();
        mPlayerCollider = mPlayer.GetComponent<Collider2D>();

        mAgent.mSteering.TurnAllOff();
        mAgent.mSteering.SetActive(SteeringType.Arrive, true);
        mAgent.mSteering.SetActive(SteeringType.Seek, true);

        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        finder = mAgent.mZone.mPathFinding;

        UpdatePath();
    }

    public override void MyUpdate()
    {
        CheckTurn();

        //check for states
        if (!mAgent.myHealth.IsAlive())
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Die.ToString());
            return;
        }
        if (mAgent.mAttackRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Attack.ToString());
            return;
        }
        if (!mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Wander.ToString());
            return;
        }
        int pnid = finder.GetNearestNodeID(mPlayer.transform.position);

        if (mPlayerNodeID != pnid)
            UpdatePath(pnid);
        else if (mAgent.mAgent.mPath.Count == 0)
            UpdatePath();

        UpdateNode();
    }

    void CheckTurn()
    {
        if (mAgent.mAgent.mTarget.x < mAgent.transform.position.x && !mAgent.IsFacingLeft)
            mAgent.Turn();
        else if (mAgent.mAgent.mTarget.x > mAgent.transform.position.x && mAgent.IsFacingLeft)
            mAgent.Turn();
    }

    void UpdatePath(int id = -1)
    {
        if (id == -1)
            mPlayerNodeID = finder.GetNearestNodeID(mPlayer.transform.position);
        else
            mPlayerNodeID = id;

        mAgent.mAgent.mPath.Clear();
        mAgent.mAgent.mPath = finder.GetPath();
        mAgent.mAgent.mPath.Add(mPlayer.transform.position);

        mAgent.mAgent.mTarget = mAgent.mAgent.mPath[0];
        mAgent.mAgent.mPath.RemoveAt(0);
    }

    void UpdateNode()
    {
        bool isX = (int)mAgent.transform.position.x == (int)mAgent.mAgent.mTarget.x;
        bool isClose = mAgent.IsNearTarget(mAgent.mNodeRange.radius);
        if (isClose || isX)
        {
            if (mAgent.mAgent.mPath.Count > 0)
            {
                mAgent.mAgent.mTarget = mAgent.mAgent.mPath[0];
                mAgent.mAgent.mPath.RemoveAt(0);
            }
        }
    }

    public override void DebugDraw()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(mAgent.transform.position, mAgent.transform.position + new Vector3(mAgent.mRigidBody.velocity.x * 10, mAgent.mRigidBody.velocity.y * 10, 0.0f));

        Gizmos.DrawWireSphere(mAgent.mAgent.mTarget, 0.5f);

        Gizmos.color = Color.green;

        if (mAgent.mAgent.mPath.Count == 0)
            return;

        Gizmos.DrawWireSphere(mAgent.mAgent.mPath[mAgent.mAgent.mPath.Count - 1], 0.5f);

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
    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return EnemyStates.Chase.ToString();
    }

}
