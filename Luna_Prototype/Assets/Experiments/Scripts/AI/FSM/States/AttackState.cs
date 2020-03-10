using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;
    Weapon myWeapon;

    bool steeringOff = false;

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
        SetSteering();
        steeringOff = true;


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
        if (!mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Wander.ToString());
            return;
        }


        if (mAgent.mAttackRange.IsTouching(mPlayerCollider))
        {
            mAgent.mSteering.TurnAllOff();
            steeringOff = true;

            myWeapon.Attack(true, mPlayer.transform.position);
        }
        else if (!steeringOff)
            SetSteering();

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
    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return EnemyStates.Attack.ToString();
    }

}
