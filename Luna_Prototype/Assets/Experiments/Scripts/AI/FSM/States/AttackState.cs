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
        mAgent.mSteering.SetActive(SteeringType.Seek, true);

        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.red;

        myWeapon = mAgent.GetComponentInChildren<Weapon>();
    }

    public override void MyUpdate()
    {
        if(!mAgent.myHealth.IsAlive())
        {
            mAgent.mStateMachine.ChangeState("Die");
            return;
        }
        if (mPlayer.transform.forward.x > 0.0f && mAgent.transform.forward.x > 0.0f
            || mPlayer.transform.forward.x < 0.0f && mAgent.transform.forward.x < 0.0f)
        {
            mAgent.transform.Rotate(0.0f,180.0f, 0.0f);
        }

        if (mAgent.mAttackRange.IsTouching(mPlayerCollider))
        {
            myWeapon.Attack(true, mPlayer.transform.position);
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
