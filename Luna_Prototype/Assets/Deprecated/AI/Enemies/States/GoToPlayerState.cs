    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{ 
    [System.Serializable]
    public class GoToPlayerState : State
    {
        Grid_PathFinding finder = new Grid_PathFinding();
        List<Vector3> Path = new List<Vector3>();

        int playerNodeID = -1;

        public override States Name()
        {
            return States.GoToPlayer;
        }

        public override void Enter(Enemy agent)
        {
            if (finder.mNodes.Count == 0)
            {
                if (finder.gameWorld == null)
                    finder.gameWorld = agent.GetWorld();
                if (agent.platformsAccesible.Count == 0)
                    finder.Initialize();
                else
                    finder.Initialize(ref agent.platformsAccesible);
            }

            finder.Calculate(agent.transform.position);

            playerNodeID = finder.GetNearestNodeID(agent.GetWorld().mPlayer.transform.position);

            Path = finder.GetPath();
           
            agent.SetDestination(Path[0]);
            agent.SetFinalDestination(Path[Path.Count - 1]);
            Path.RemoveAt(0);
        }

        public override void Update(Enemy agent)
        {
            //this will be changed by checking the weapon the enemy has
            //if weapon is ranged use extended range
            //if weapon is meele use normal range
            float attackRange = agent.GetSafeDistanceExtended();

            //if player is close enought to attck switch to attack
            if (agent.IsNearPlayer(agent.GetSafeDistanceExtended()) && 
                agent.GetWorld().HasLineOfSight(new World.Wall(agent.transform.position, agent.GetWorld().mPlayer.transform.position)))
            {
                agent.mStateMachine.ChangeState(States.Attack);
                return;
            }

            bool calculateAgain = false;
            int pNID = finder.GetNearestNodeID(agent.GetWorld().mPlayer.transform.position);
            if(playerNodeID!= pNID)
            {
                playerNodeID = pNID;
                calculateAgain = true;
            }

            if(calculateAgain)
            {
                finder.Calculate(agent.transform.position);
                Path.Clear();
                Path = finder.GetPath();
                agent.SetDestination(Path[0]);
                Path.RemoveAt(0);
                agent.SetFinalDestination(Path[Path.Count - 1]);
            }

            bool isClose = agent.IsNearDestination(agent.GetSafeDistanceReduced());

            //if player is far away than range then switch to wander 
            if (Path.Count == 0)
            {
                if (!agent.IsNearPlayer(agent.GetSafeDistanceExtended()))
                {
                    agent.mStateMachine.ChangeState(States.Wander);
                }
            }

            if (isClose)
            {
                agent.SetDestination(Path[0]);
                Path.RemoveAt(0);
            }
        }

        public override void Exit(Enemy agent)
    {

    }
    }
}
