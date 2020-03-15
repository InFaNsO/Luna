using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPathFindingState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;
    Weapon myWeapon;

    PathFinding finder;
    bool steeringOff = false;
    int mPlayerNodeID = -1;
    // Start is called before the first frame update    
    [SerializeField] float LowJumpMultiplier = 0.8f;
    [SerializeField] float HighJumpMultiplier = 1.0f;
    void Start()
    {
        mAgent = GetComponentInParent<Enemy>();

    }

    public override void Enter()
    {
        finder = mAgent.mZone.mPathFinding;
        mPlayer = mAgent.mZone.mPlayerTransform.GetComponent<Player>();
        mPlayerCollider = mPlayer.GetComponent<Collider2D>();

        mAgent.mSteering.TurnAllOff();
        SetSteering();


        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.red;

        myWeapon = mAgent.GetComponentInChildren<Weapon>();

        mAgent.mAgent.mTarget = mPlayer.transform.position;
        CheckTurn();
    }

    public override void MyUpdate()
    {
        mAgent.mAgent.mTarget = mPlayer.transform.position;
        CheckTurn();

        if (!mAgent.myHealth.IsAlive())
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Die.ToString());
            return;
        }
        var DIS = Vector3.Distance(mAgent.transform.position, mPlayer.transform.position);
        Debug.Log("player distsance: " + DIS.ToString());
        if (DIS > mAgent.mPlayerVisibilityRange.radius && mAgent.mAgent.mPath.Count <= 1)
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Wander.ToString());
            return;
        }


        if (Vector3.Distance(mAgent.transform.position, mPlayer.transform.position) <= mAgent.mAttackRange.radius)
        {
            if (!steeringOff)
            {
                mAgent.mSteering.TurnAllOff();
                steeringOff = true;
            }
            myWeapon.Attack(true, mPlayer.transform.position);
        }
        else if (steeringOff)
            SetSteering();

        if (!steeringOff)
        {
            int pnid = finder.GetNearestNodeID(mPlayer.transform.position);

            if (mPlayerNodeID != pnid)
                UpdatePath(pnid);
            else if (mAgent.mAgent.mPath.Count == 0)
                UpdatePath();

            UpdateNode();
        }
    }

    void CheckTurn()
    {
        if (mAgent.mAgent.mTarget.x < mAgent.transform.position.x && !mAgent.IsFacingLeft)
            mAgent.Turn();
        else if (mAgent.mAgent.mTarget.x > mAgent.transform.position.x && mAgent.IsFacingLeft)
            mAgent.Turn();
    }

    void SetSteering()
    {
        mAgent.mSteering.SetActive(SteeringType.Arrive, true);
        mAgent.mSteering.SetActive(SteeringType.Seek, true);
        steeringOff = false;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mAgent.mAgent.mTarget, 0.5f);
    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return EnemyStates.Attack.ToString();
    }

}
