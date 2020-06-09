using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderFlyingState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;

    [SerializeField] List<Transform> Positions;
    [SerializeField] bool IsLooped = false;

    List<Vector3> path = new List<Vector3>();

    bool isCurrentReverse = false;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<Enemy>();
        Debug.AssertFormat(Positions != null, "Add Positions for bats Wander");

        for(int i = 0; i < Positions.Count; ++i)
        {
            path.Add(Positions[i].position);
        }
    }

    public override void Enter()
    {
        mPlayer = mAgent.mZone.mPlayerTransform.GetComponent<Player>();
        mPlayerCollider = mPlayer.GetComponent<Collider2D>();

        mAgent.mSteering.TurnAllOff();
        //mAgent.mSteering.SetActive(SteeringType.Wander, true);
        mAgent.mSteering.SetActive(SteeringType.Seek, true);

       // mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.blue;

        mAgent.mRigidBody.gravityScale = 0.0f;

        mAgent.mAgent.mPath = path;
        isCurrentReverse = false;
        UpdatePath();
        CheckTurn();
    }

    public override void MyUpdate()
    {
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
        if (Vector3.Distance(mAgent.transform.position, mPlayer.transform.position) <= mAgent.mPlayerVisibilityRange.radius)
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Chase.ToString());
            return;
        }
        bool isClose = mAgent.IsNearTarget(mAgent.mNodeRange.radius);

        if (isClose)
            UpdatePath();

        CheckTurn();
    }

    public override void DebugDraw()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mAgent.mAgent.mTarget, 0.5f);
    }

    void UpdatePath()
    {
        if (IsLooped)
        {
            ++index;
            if (index >= path.Count)
                index = 0;
        }
        else
        {
            if (isCurrentReverse)
            {
                --index;
                if (index < 0)
                {
                    isCurrentReverse = false;
                    index = 0;
                }
            }
            else
            {
                ++index;
                if(index >= path.Count)
                {
                    index = path.Count - 1;
                    isCurrentReverse = true;
                }
            }
        }

        mAgent.mAgent.mTarget = mAgent.mAgent.mPath[index];
    }

    void CheckTurn()
    {
        if (mAgent.mAgent.mTarget.x < mAgent.transform.position.x && !mAgent.IsFacingLeft)
            mAgent.Turn();
        else if (mAgent.mAgent.mTarget.x > mAgent.transform.position.x && mAgent.IsFacingLeft)
            mAgent.Turn();
    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return EnemyStates.Wander.ToString();
    }

}
