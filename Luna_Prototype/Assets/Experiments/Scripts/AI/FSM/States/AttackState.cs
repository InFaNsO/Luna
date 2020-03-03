using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;
    Weapon myWeapon;

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

        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.red;

        myWeapon = mAgent.GetComponentInChildren<Weapon>();
    }

    public override void MyUpdate()
    {
        if (mAgent.mAttackRange.IsTouching(mPlayerCollider))
        {
            //myWeapon.Attack(true, mPlayer.transform.position);
        }
        else
            mAgent.mAgent.mTarget = mPlayer.transform.position;

        if (!mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState("Wander");
            return;
        }
    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return "Attack";
    }

}
