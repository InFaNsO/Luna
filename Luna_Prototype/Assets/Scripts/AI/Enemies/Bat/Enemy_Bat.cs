using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    public enum States
    {
        none = -1,
        Idel = 0,
        goTo,
        attack,
    }

    [SerializeField] private LAI.StateMachine<Enemy_Bat> mStateMachine = new LAI.StateMachine<Enemy_Bat>();
    private States mCurrentState = States.none;

    new void Awake()
    {
        base.Awake();
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        //Add State

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

    public bool IsNear(float distance)
    {
        if (transform.position.x + distance > world.mPlayer.transform.position.x &&
            transform.position.x - distance < world.mPlayer.transform.position.x)
        {
            if (transform.position.y + distance > world.mPlayer.transform.position.y &&
                transform.position.y - distance < world.mPlayer.transform.position.y)
                return true;
        }
        return false;
    }
}
