using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy_Bat : Enemy
{
    public Bullet mBullet;
    public int mDamage = 10;

    public enum States
    {
        none = -1,
        Idel = 0,
        goTo,
        rangeAttack,
        meleeAttack
    }

    private States mCurrentState = States.none;

    new void Awake()
    {
        Assert.IsNotNull(mBullet, "[Enemy_Bat] mBullet is Null");

        base.Awake();
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        //Add State
        mStateMachine.AddState<LAI.EnemyBatState_idel>();         //0
        mStateMachine.AddState<LAI.EnemyBatState_goTo>();         //1
        mStateMachine.AddState<LAI.EnemyBatState_rangeAttack>();         //2
        mStateMachine.AddState<LAI.EnemyBatState_meleeAttack>();        //3

        mCurrentState = 0;
        mStateMachine.SetAgent(this);
        mStateMachine.ChangeState((int)mCurrentState);

        mSteeringModule.AddState<LAI.BehaviourSeek>();
        mSteeringModule.AddState<LAI.BehaviourArrive>();
        mSteeringModule.AddState<LAI.BehaviourObstacleAvoidance>();
    }

    public new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        mStateMachine.Update();
    }

    // - Behavior functions ------------------------------------------------------
    public void FlipFace()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
    }


}
