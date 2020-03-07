using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderFlyingState : State
{
    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;

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
        mAgent.mSteering.SetActive(SteeringType.Wander, true);
        mAgent.mSteering.SetActive(SteeringType.Seek, true);

        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    public override void MyUpdate()
    {
        if (!mAgent.myHealth.IsAlive())
        {
            mAgent.mStateMachine.ChangeState("Die");
            return;
        }
        if (mAgent.mAttackRange.IsTouching(mPlayerCollider))// && hitInfo.collider == mPlayerCollider)
        {
            mAgent.mStateMachine.ChangeState("Attack");
            return;
        }
        else if (mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))// && hitInfo.collider == mPlayerCollider)
        {
            mAgent.mStateMachine.ChangeState("Chase");
            return;
        }
    }

    public override void DebugDraw()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mAgent.mAgent.mTarget, 0.5f);
    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return "Wander";
    }

}
