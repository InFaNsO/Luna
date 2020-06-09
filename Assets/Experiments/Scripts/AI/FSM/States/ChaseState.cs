using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;


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

       // mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
    }

    public override void MyUpdate()
    {
        mAgent.mAgent.mTarget = mPlayer.transform.position;
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
        if (Vector3.Distance(mAgent.transform.position, mPlayer.transform.position) > mAgent.mPlayerVisibilityRange.radius) //mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Wander.ToString());
            return;
        }
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
        return EnemyStates.Chase.ToString();
    }

}
