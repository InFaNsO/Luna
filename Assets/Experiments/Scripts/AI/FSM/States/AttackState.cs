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
    float attackTime = 2.0f;
    float attackTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<Enemy>();

    }

    public override void Enter()
    {
        attackTimer = 0.0f;
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
        var DIS = Vector2.Distance(mAgent.transform.position, mPlayer.transform.position);
        Debug.Log("player distsance: " + DIS.ToString());
        Debug.Log("visablity: " + mAgent.mPlayerVisibilityRange.radius);
        if (DIS > mAgent.mPlayerVisibilityRange.radius && attackTimer > attackTime)
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Wander.ToString());
            return;
        }

        if (DIS <= mAgent.mAttackRange.radius)
        {
            if (!steeringOff)
            {
                mAgent.mSteering.TurnAllOff();
                steeringOff = true;
            }

            //[Mingzhuo Zhang Edit]VVVVVVVVVVVVV
            mAgent.Attack();
            //[Mingzhuo Zhang Edit]^^^^^^^^^^^^^

            Debug.Log("Attacjing");
        }
        else if (steeringOff)
            SetSteering();

        attackTimer += Time.deltaTime;
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
