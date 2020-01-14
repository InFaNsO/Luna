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
        if(!mSteeringModule.Exists(LAI.SteeringType.Seek))
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
        mStateMachine.AddState(wanderState);
        mStateMachine.AddState(gotoState);
        mStateMachine.ChangeState(LAI.States.Wander);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //state machine update will handle switching state
    }
    public void OnDrawGizmos()
    {
        for (int i = 0; i < pathFinder.mNodes.Count; ++i)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(pathFinder.mNodes[i].coord, 0.3f);

            Gizmos.color = Color.cyan;

            for (int j = 0; j < pathFinder.mNodes[i].children.Count; ++j)
            {
                Gizmos.DrawLine(pathFinder.mNodes[i].coord, pathFinder.mNodes[pathFinder.mNodes[i].children[j]].coord);
            }
        }

    }
}
