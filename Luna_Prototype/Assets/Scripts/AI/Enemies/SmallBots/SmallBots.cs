using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBots : Enemy
{
    public enum States
    {
        none = -1,
        wander = 0,
        goTo = 1,
        attack = 2
    }
    
    [SerializeField] private LAI.StateMachine<SmallBots> mStateMachine = new LAI.StateMachine<SmallBots>();
    private bool mIsOnRoof = true;
    private States mCurrentState = States.none;

    // Start is called before the first frame update
    public new void Awake()
    {
        base.Awake();
        transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));

        LAI.PathFollowingState<SmallBots> pf = new LAI.PathFollowingState<SmallBots>();
        pf.AddPath(new Vector3(-6.0f, 2.84f, 0.0f));
        pf.AddPath(new Vector3(4.0f, 2.84f, 0.0f));

        mStateMachine.AddState(pf);                                         //0
        mStateMachine.AddState<LAI.SBStateGoToPlayer<SmallBots>>();         //1
        mStateMachine.AddState<LAI.SBStateAttack<SmallBots>>();             //2

        mCurrentState = 0;
        mStateMachine.ChangeState((int)mCurrentState);

        mSteeringModule.AddState<LAI.BehaviourSeek>();
        mSteeringModule.AddState<LAI.BehaviourArrive>();
        mSteeringModule.AddState<LAI.BehaviourObstacleAvoidance>();
    }

    public new void Start()
    {
        base.Start();
       
    }

    private bool IsNear(float distance)
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

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

        mStateMachine.Update();

        if(mIsOnRoof)
        {
            mVelocity.y += 9.8f;        //so that it doesnt fall
        }

        //See if player is close
            //Test line of sight
                //change state
                    //see if close enough to player to attack
                    //if so
                        //change state to attack
                    //else
                    //go to player

        if (mCurrentState == States.wander)          //is wandering on roof top
        {
            if (IsNear(mSafeDistanceExtended))
            {
                if(world.HasLineOfSight(new World.Wall(transform.position, world.mPlayer.transform.position)))
                {
                    if(IsNear(mSafeDistanceReduced))        //ready to attack
                    {
                        mCurrentState = States.attack;
                        mStateMachine.ChangeState((int)mCurrentState);
                    }
                    else                                    //Go to player
                    {
                        mCurrentState = States.goTo;
                        mStateMachine.ChangeState((int)mCurrentState);
                    }
                }
            }
        }

        if (mCurrentState == States.goTo)
        {
            if (world.HasLineOfSight(new World.Wall(transform.position, world.mPlayer.transform.position)))
            {
                if (IsNear(mSafeDistanceReduced))        //ready to attack
                {
                    mCurrentState = States.attack;
                    mStateMachine.ChangeState((int)mCurrentState);
                }
            }
        }

        if(mCurrentState == States.attack)
        {
            //attack stuff
        }
    }
}
