using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBotHeal : Enemy
{
    [SerializeField] LAI.WanderRoofState wanderState = new LAI.WanderRoofState();
    [SerializeField] LAI.GoToBehindEnemy gotoState = new LAI.GoToBehindEnemy();
    [SerializeField] LAI.AttackState attackState = new LAI.AttackState();

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
        mStateMachine.AddState<LAI.SupriseState>();
        mStateMachine.AddState<LAI.AttackState>(attackState);
        mStateMachine.AddState<LAI.WanderRoofState>(wanderState);
        mStateMachine.AddState<LAI.GoToBehindEnemy>(gotoState);
        mStateMachine.ChangeState(LAI.States.Wander);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //state machine update will handle switching state
    }
}
