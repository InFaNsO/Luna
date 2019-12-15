using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBot : Enemy
{
    public enum States
    {
        wander = 0,
        goTo = 1,
        attack = 2,
        suprise = 3
    }

    States mCurrentState = States.wander;
    [SerializeField] List<int> platformIndexToWander = new List<int>();

    private new void Awake()
    {
        base.Awake();
        mSteeringModule.AddState<LAI.BehaviourSeek>();
        mSteeringModule.SetActive(LAI.SteeringType.Seek, true);
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        LAI.WanderState ws = new LAI.WanderState();
        for (int i = 0; i < platformIndexToWander.Count; ++i)
        {
            ws.PlatformsToWander.Add(world.mPlatforms[platformIndexToWander[i]]);
        }
        mStateMachine.AddState<LAI.WanderState>(ws);
        mCurrentState = States.wander;
        mStateMachine.ChangeState((int)mCurrentState);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        mStateMachine.Update();
    }
}
