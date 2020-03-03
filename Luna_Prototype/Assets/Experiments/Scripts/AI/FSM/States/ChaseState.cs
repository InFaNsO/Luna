using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
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

        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;

        mPlayerNodeID = finder.GetNearestNodeID(mPlayer.transform.position);
        finder = mAgent.mZone.mPathFinding;
        finder.Calculate(mAgent.transform.position, mPlayer.transform.position);
        mAgent.mAgent.mPath = finder.GetPath();

    }

    public override void MyUpdate()
    {
        if (mAgent.mAttackRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState("Attack");
            return;
        }
        if (!mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            mAgent.mStateMachine.ChangeState("Wander");
            return;
        }
        int pnid = finder.GetNearestNodeID(mPlayer.transform.position);
        if(mPlayerNodeID != pnid)
        {
            mPlayerNodeID = pnid;
            finder.FindPath(mAgent.transform.position, mPlayerNodeID);
            mAgent.mAgent.mPath.Clear();
            mAgent.mAgent.mPath = finder.GetPath();
            mAgent.mAgent.mPath.Add(mPlayer.transform.position);
        }
        else if (mAgent.mAgent.mPath.Count == 0)
        {
            mPlayerNodeID = finder.GetNearestNodeID(mPlayer.transform.position);
            finder.FindPath(mAgent.transform.position, mPlayerNodeID);
            mAgent.mAgent.mPath.Clear();
            mAgent.mAgent.mPath = finder.GetPath();
            mAgent.mAgent.mPath.Add(mPlayer.transform.position);
            
            mAgent.mAgent.mTarget = mAgent.mAgent.mPath[0];
            mAgent.mAgent.mPath.RemoveAt(0);
        }
        

    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return "Chase";
    }

}
