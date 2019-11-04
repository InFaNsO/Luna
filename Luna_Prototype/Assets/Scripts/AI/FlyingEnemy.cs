using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy               //uses seek behaviour
{
    //[SerializeField] private LAI.SteeringModule mSteeringModule;


    private float timer = 0.0f;
    public new void Start()
    {
        //mAgent = new Agent();
        mSteeringModule = new LAI.SteeringModule();

        mSteeringModule.Initialize();
        //mAgent.SetWorld(world);
        mSteeringModule.SetAgent(this);
        world.AddAgent(this);

        mSteeringModule.AddState<LAI.BehaviourSeek>();
        mSteeringModule.AddState<LAI.BehaviourObstacleAvoidance>();

        mSteeringModule.TurnAllOff();
        mSteeringModule.SetActive(LAI.SteeringType.Seek, true);
        mSteeringModule.SetActive(LAI.SteeringType.ObstacleAvoidance, true);
    }

    public new void Update()
    {
        Vector2 v = mSteeringModule.Calculate();
        v *= Time.deltaTime * GetMaxSpeed();
        v *= 0.1f;
        SetVelocity(v);


        v.x += transform.position.x;
        v.y += transform.position.y;

        SetHeading(v.normalized);

        Vector3 p = new Vector3(v.x, v.y, 0.0f);

        transform.position = p;

        if (timer > 3.0f)
            SetDestination(world.mPlayer.transform.position);

        if ((GetPosition() - world.mPlayer.GetPosition()).SqrMagnitude() < ((GetRadius() + world.mPlayer.GetRadius()) * (GetRadius() + world.mPlayer.GetRadius())))
        {
            mWeapon.Attack(false);
        }

        SetDestination(world.mPlayer.GetPosition());
        timer += Time.deltaTime;
    }
}

