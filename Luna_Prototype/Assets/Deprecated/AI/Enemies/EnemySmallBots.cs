using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmallBots : Enemy
{
    [SerializeField] LAI.WanderRoofState wanderState = new LAI.WanderRoofState();
    [SerializeField] LAI.GoToPlayerFlyingState gotoState = new LAI.GoToPlayerFlyingState();

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
        mStateMachine.SetAgent(this);
        mStateMachine.AddState<LAI.AttackState>();
        mStateMachine.AddState<LAI.SupriseState>();
        mStateMachine.AddState<LAI.WanderRoofState>(wanderState);
        mStateMachine.AddState<LAI.GoToPlayerFlyingState>(gotoState);
        mStateMachine.ChangeState(LAI.States.Wander);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //state machine update will handle switching state
    }
}
