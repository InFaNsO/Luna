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
    [SerializeField] float LowJumpMultiplier = 0.8f;
    [SerializeField] float HighJumpMultiplier = 1.0f;

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
        if (Vector3.Distance(mAgent.transform.position, mPlayer.transform.position) <= mAgent.mAttackRange.radius)
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Attack.ToString());
            return;
        }
        if (!mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider) && mAgent.mAgent.mPath.Count <= 1)
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

        finder.FindPath(mAgent.transform.position, mPlayerNodeID);

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

        bool isEnd = isClose && mAgent.mAgent.mPath.Count != 0;

        if (isEnd || isX)
        {
            if (mAgent.mAgent.mTarget.y < mAgent.mAgent.mPath[0].y)
            {
                // mAgent.mRigidBody.velocity = new Vector2(mAgent.mRigidBody.velocity.x ,  0.0f);
                var force = new Vector2((mAgent.transform.position.x < mAgent.mAgent.mPath[0].x ? mAgent.mMovementSpeed : mAgent.mMovementSpeed * -1), mAgent.mJumpStrength * HighJumpMultiplier);
                mAgent.mRigidBody.AddForce(force, ForceMode2D.Impulse);
            }
            else if (mAgent.mAgent.mTarget.y != mAgent.mAgent.mPath[0].y)
            {
                var force = new Vector2(mAgent.mMovementSpeed * mAgent.transform.position.x < mAgent.mAgent.mPath[0].x ? 1.0f : -1.0f, mAgent.mJumpStrength * LowJumpMultiplier);
                mAgent.mRigidBody.AddForce(force, ForceMode2D.Impulse);
            }

        }

        if(mAgent.mAgent.mPath.Count > 0)
            mAgent.mAgent.mPath[mAgent.mAgent.mPath.Count - 1] = mPlayer.transform.position;
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

        var Path = mAgent.mAgent.mPath;
        Gizmos.color = Color.red;
        if (Path.Count > 0)
        {
            for (int i = 1; i < Path.Count; ++i)
            {
                Gizmos.DrawWireSphere(Path[i - 1], 0.3f);
                Gizmos.DrawLine(Path[i - 1], Path[i]);
            }
            Gizmos.DrawLine(Path[Path.Count - 1], mAgent.mZone.mPlayerTransform.position);
        }
        else
            Gizmos.DrawLine(mAgent.transform.position, mAgent.mZone.mPlayerTransform.position);
    }

    //public override void DebugDraw()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(mAgent.transform.position, mAgent.transform.position + new Vector3(mAgent.mRigidBody.velocity.x * 10, mAgent.mRigidBody.velocity.y * 10, 0.0f));
    //
    //    Gizmos.DrawWireSphere(mAgent.mAgent.mTarget, 0.5f);
    //
    //    Gizmos.color = Color.green;
    //
    //    if (mAgent.mAgent.mPath.Count == 0)
    //        return;
    //
    //    Gizmos.DrawWireSphere(mAgent.mAgent.mPath[mAgent.mAgent.mPath.Count - 1], 0.5f);
    //
    //    for (int i = 0; i < finder.mNodes.Count; ++i)
    //    {
    //        Gizmos.color = Color.magenta;
    //        Gizmos.DrawWireSphere(finder.mNodes[i].pos, 0.3f);
    //
    //        Gizmos.color = Color.cyan;
    //
    //        for (int j = 0; j < finder.mNodes[i].childrenID.Count; ++j)
    //        {
    //            Gizmos.DrawLine(finder.mNodes[i].pos, finder.mNodes[finder.mNodes[i].childrenID[j]].pos);
    //        }
    //    }
    //}

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return EnemyStates.Chase.ToString();
    }

}
