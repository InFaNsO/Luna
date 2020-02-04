using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class WanderGroundState : State
    {
        [SerializeField] public List<Platform> PlatformsToWander = new List<Platform>();
        Grid_PathFinding finder = new Grid_PathFinding();

        List<Vector3> Path = new List<Vector3>();
        int target = 0;

        public override States Name()
        {
            return States.Wander;
        }

        public override void DrawGizmo(Enemy agent)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + new Vector3(agent.GetVelocity().x * 10, agent.GetVelocity().y * 10, 0.0f));

            for (int i = 0; i < finder.mNodes.Count; ++i)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(finder.mNodes[i].pos, 0.3f);

                Gizmos.color = Color.cyan;

                for (int j = 0; j < finder.mNodes[i].childrenID.Count; ++j)
                {
                    Gizmos.DrawLine(finder.mNodes[i].pos, finder.mNodes[finder.mNodes[i].childrenID[j]].pos);
                }
            }
        }

        public override void Enter(Enemy agent)
        {
            agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, false);
            agent.GetComponent<SpriteRenderer>().color = Color.green;
            if (finder.mNodes.Count == 0)
            {
                if (finder.GameWorld == null)
                    finder.GameWorld = agent.GetWorld();
                finder.Generate(PlatformsToWander);
            }
            agent.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

            target = (int)Random.Range(0.0f, (float)finder.mNodes.Count - 1);


            finder.Calculate(agent.transform.position, finder.mNodes[target].pos);
            Path = finder.GetPath();
            agent.SetDestination(Path[0]);
            agent.SetFinalDestination(Path[Path.Count - 1]);
            Path.RemoveAt(0);
        }

        public override void Update(Enemy agent)
        {
            if(agent.IsNearPlayer(agent.GetSafeDistanceExtended()) && agent.GetWorld().HasLineOfSight(new World.Wall(agent.transform.position, agent.GetWorld().mPlayer.transform.position)))
            {
                agent.mStateMachine.ChangeState(States.GoToPlayer);
                return;
            }



            bool calculateAgain = false;
            if (Vector3.Distance(agent.transform.position, finder.mNodes[target].pos) < agent.GetSafeDistanceReduced())
            {
                calculateAgain = true;
            }

            if (calculateAgain)  //or
            {
                target = (int)Random.Range(0.0f, (float)finder.mNodes.Count - 1);
                finder.Calculate(agent.transform.position, finder.mNodes[target].pos);
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
                target = (int)Random.Range(0.0f, (float)finder.mNodes.Count - 1);
                finder.Calculate(agent.transform.position, finder.mNodes[target].pos);
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
                if(agent.GetDestination().y < Path[0].y)
                {
                    agent.myRB.AddForce(new Vector2(0.0f, agent.mJumpStrength));
                }
                agent.SetDestination(Path[0]);
                Path.RemoveAt(0);

            }
        }

        public override void Exit(Enemy agent)
        {

        }
    }
}
