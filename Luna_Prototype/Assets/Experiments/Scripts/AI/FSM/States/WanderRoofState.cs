using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderRoofState : State
{
    [SerializeField] Platform mRoof;

    Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;


    Vector3 left = new Vector3();
    Vector3 right = new Vector3();

    bool shouldFall = false;


    float yPrv = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<Enemy>();
        Debug.AssertFormat(mRoof != null, "Wander Roof State roof needs to be set!!!!");
    }

    public override void Enter()
    {
        mPlayer = mAgent.mZone.mPlayerTransform.GetComponent<Player>();
        mPlayerCollider = mPlayer.GetComponent<Collider2D>();

        mAgent.mSteering.TurnAllOff();
        mAgent.mSteering.SetActive(SteeringType.Arrive, true);
        mAgent.mSteering.SetActive(SteeringType.Seek, true);

        mAgent.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        mAgent.mRigidBody.gravityScale = -1.0f;
        shouldFall = false;


        mAgent.transform.Rotate(Vector3.forward, 180.0f);

        left =  mRoof.transform.position;
        right = mRoof.transform.position;

        left.x -=  mRoof.Width * 0.5f;
        right.x += mRoof.Width * 0.5f;

        left.y -=  mRoof.Height * 0.5f;
        right.y -= mRoof.Height * 0.5f;

        float distanceLeft = Vector3.Distance(mAgent.transform.position, left);
        float distanceRight = Vector3.Distance(mAgent.transform.position, right);

        if (distanceLeft > distanceRight)
            mAgent.mAgent.mTarget = right;
        else
            mAgent.mAgent.mTarget = left;

        CheckTurn();
    }

    public override void MyUpdate()
    {
        if (!mAgent.myHealth.IsAlive())
        {
            mAgent.mStateMachine.ChangeState(EnemyStates.Die.ToString());
            return;
        }
        if (shouldFall)
        {
            //check if its grounded
            if (mAgent.transform.position.y > yPrv - 0.01f && mAgent.transform.position.y < yPrv + 0.01f)
            {
                mAgent.mStateMachine.ChangeState(EnemyStates.Chase.ToString());
                return;
            }
            else
            {
                yPrv = mAgent.transform.position.y;
                return;
            }
        }

        if (mAgent.mPlayerVisibilityRange.IsTouching(mPlayerCollider))
        {
            mAgent.mRigidBody.gravityScale = 1.0f;
            yPrv = mAgent.transform.position.y;
            shouldFall = true;
            mAgent.transform.Rotate(Vector3.forward, 180.0f);
            return;
        }

        bool isClose = mAgent.IsNearTarget(mAgent.mNodeRange.radius);

        if (isClose)
        {
            if (mAgent.IsFacingLeft)
                mAgent.mAgent.mTarget = right;
            else
                mAgent.mAgent.mTarget = left;

            CheckTurn();
        }

    }

    void CheckTurn()
    {
        if (mAgent.mAgent.mTarget.x < mAgent.transform.position.x && !mAgent.IsFacingLeft)
            mAgent.Turn();
        else if (mAgent.mAgent.mTarget.x > mAgent.transform.position.x && mAgent.IsFacingLeft)
            mAgent.Turn();
    }
    public override void DebugDraw()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(left, 0.5f);
        Gizmos.DrawWireSphere(right, 0.5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(mAgent.mAgent.mTarget, 0.8f);
    }

    public override void Exit()
    {
        mAgent.mRigidBody.gravityScale = 1.0f;
    }

    public override string GetName()
    {
        return EnemyStates.Wander.ToString();
    }
}
