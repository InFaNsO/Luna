using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_WanderRoofState : E_State
{
    [SerializeField] Platform mRoof;

    E_Enemy mAgent;
    Player mPlayer;

    Collider2D mPlayerCollider;


    Vector3 left = new Vector3();
    Vector3 right = new Vector3();

    bool isGoingLeft = false;
    bool shouldFall = false;


    float yPrv = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<E_Enemy>();
        Debug.AssertFormat(mRoof != null, "Wander Roof State roof needs to be set!!!!");
    }

    public override void Enter()
    {
        mPlayer = mAgent.mZone.mPlayerTransform.GetComponent<Player>();
        mPlayerCollider = mPlayer.GetComponent<Collider2D>();

        mAgent.mSteering.TurnAllOff();
        mAgent.mSteering.SetActive(SteeringType.Arrive, true);

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
        {
            isGoingLeft = false;
            mAgent.mAgent.mTarget = right;
        }
        else
        {
            isGoingLeft = true;
            mAgent.mAgent.mTarget = left;
        }
    }

    public override void MyUpdate()
    {
        if (shouldFall)
        {
            //check if its grounded
            if (mAgent.transform.position.y > yPrv - 0.01f && mAgent.transform.position.y < yPrv + 0.01f)
            {
                mAgent.mStateMachine.ChangeState("Chase");
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
            if (isGoingLeft)
            {
                mAgent.mAgent.mTarget = right;
                isGoingLeft = false;
            }
            else
            {
                mAgent.mAgent.mTarget = left;
                isGoingLeft = true;
            }
        }
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
        return "Wander";
    }
}
