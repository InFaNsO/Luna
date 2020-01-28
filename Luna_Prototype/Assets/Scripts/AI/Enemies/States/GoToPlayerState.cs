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
            agent.GetComponent<SpriteRenderer>().color = Color.yellow;

            agent.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            finder = agent.pathFinder;
           // if (finder.mNodes.Count == 0)
           // {
           //     if (finder.gameWorld == null)
           //         finder.gameWorld = agent.GetWorld();
           //     if (agent.platformsAccesible.Count == 0)
           //         finder.Initialize();
           //     else
           //         finder.Initialize(ref agent.platformsAccesible);
           // }

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
            float attackRange = agent.GetSafeDistance()- 1.0f;

            //if player is close enought to attck switch to attack
            if (agent.IsNearPlayer(Mathf.Abs(attackRange)) && 
                agent.GetWorld().HasLineOfSight(new World.Wall(agent.transform.position, agent.GetWorld().mPlayer.transform.position)))
            {
                agent.mStateMachine.ChangeState(States.Attack);
                return;
            }
            //if player is far away than range then switch to wander 
            if (!agent.IsNearPlayer(agent.GetSafeDistanceExtended() + 5.0f))
            {
                agent.mStateMachine.ChangeState(States.Wander);
                return;
            }

            bool calculateAgain = false;
            int pNID = finder.GetNearestNodeID(agent.GetWorld().mPlayer.transform.position);
            if(playerNodeID!= pNID)
            {
                playerNodeID = pNID;
                calculateAgain = true;
            }

            if (calculateAgain)  //or
            {
                finder.Calculate(agent.transform.position);
                Path.Clear();
                Path = finder.GetPath();
                Path.Add(agent.transform.position);
                agent.SetDestination(Path[0]);
                Path.RemoveAt(0);
                agent.SetFinalDestination(Path[Path.Count - 1]);
                agent.SetFinalDestination(agent.GetWorld().mPlayer.transform.position);
            }
            else if (Path.Count == 0)
            {
                finder.Calculate(agent.transform.position);
                Path.Clear();
                Path = finder.GetPath();
                Path.Add(agent.GetWorld().mPlayer.transform.position);

                if (Path.Count > 0)
                {
                    agent.SetDestination(Path[0]);
                    Path.RemoveAt(0);
                    agent.SetFinalDestination(Path[Path.Count - 1]);
                }
                else
                {
                    agent.SetDestination(agent.GetWorld().mPlayer.transform.position);
                }
            }

            bool isClose = agent.IsNearDestination(agent.GetSafeDistanceReduced());

            if (isClose && Path.Count > 0)
            {
                agent.SetDestination(Path[0]);
                Path.RemoveAt(0);
            }
        }

        public override void DrawGizmo(Enemy agent)
        {
            Gizmos.color = Color.red;
            if (Path.Count > 0)
            {
                Gizmos.DrawLine(agent.transform.position, agent.GetDestination());
                for (int i = 1; i < Path.Count; ++i)
                {
                    Gizmos.DrawWireSphere(Path[i - 1], 0.3f);
                    Gizmos.DrawLine(Path[i - 1], Path[i]);
                }
                Gizmos.DrawLine(Path[Path.Count - 1], agent.GetWorld().mPlayer.transform.position);
            }
            else
                Gizmos.DrawLine(agent.transform.position, agent.GetWorld().mPlayer.transform.position);
        }

        public override void Exit(Enemy agent)
        {

        }
    }
}
