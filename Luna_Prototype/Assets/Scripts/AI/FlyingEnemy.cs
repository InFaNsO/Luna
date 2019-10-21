using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy               //uses seek behaviour
{
    [SerializeField] private SteeringModule mSteeringModule;

    public new void Start()
    {
        mAgent = new Agent();
        mSteeringModule = new SteeringModule();

        mSteeringModule.Initialize();
        mAgent.SetWorld(world);
        mSteeringModule.SetAgent(mAgent);
        world.AddAgent(mAgent);

        mSteeringModule.TurnAllOff();
        mSteeringModule.SetActive(SteeringType.Seek, true);
        mSteeringModule.SetActive(SteeringType.ObstacleAvoidance, true);
    }

    public new void Update()
    {
        Vector2 v = mSteeringModule.Calculate();
        v *= Time.deltaTime * mAgent.GetMaxSpeed();
        v *= 0.1f;
        mAgent.SetVelocity(v);


        v.x += transform.position.x;
        v.y += transform.position.y;

        Vector3 p = new Vector3(v.x, v.y, 0.0f);

        transform.position = p;


        if ((mAgent.GetPosition() - world.mPlayer.GetPosition()).SqrMagnitude() > ((mAgent.GetRadius() + world.mPlayer.GetRadius()) * (mAgent.GetRadius() + world.mPlayer.GetRadius())))
        {
            //do damage
        }

        mAgent.SetDestination(world.mPlayer.GetPosition());
    }
}

