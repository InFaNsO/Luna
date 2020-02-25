using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * New Enemy class based on ECS
 */

public class E_Enemy : MonoBehaviour
{
    //[SerializeField] E_AI_Zone mZone;
    //Physics Triggers
    public CircleCollider2D mNodeRange;
    public CircleCollider2D mAttackRange;
    public CircleCollider2D mPlayerVisibilityRange;

    public Rigidbody2D mRigidBody;

    public E_AI_Zone mZone;

    //AI
    public E_Agent2D mAgent;
    public E_StateMachine mStateMachine;
    public E_PathFinding mPathFinding;
    public E_SteeringModule mSteering;

    // Start is called before the first frame update
    void Start()
    {
        //mPathFinding = mZone.mPathFinding;
    }
    private void Awake()
    {
        mAgent = GetComponent<E_Agent2D>();

        mRigidBody = GetComponentInChildren<Rigidbody2D>();
        mStateMachine = GetComponentInChildren<E_StateMachine>();
        mSteering = GetComponentInChildren<E_SteeringModule>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool IsNearTarget(float range)
    {
        if (Vector3.Distance(transform.position, mAgent.mTarget) < range)
        {
            return true;
        }
        return false;
    }
}
