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

        public override States Name()
        {
            return States.Wander;
        }

        public override void Enter(Enemy agent)
        {
            if (finder.mNodes.Count == 0)
            {
                if (finder.GameWorld == null)
                    finder.GameWorld = agent.GetWorld();
                //finder.Initialize(ref PlatformsToWander);
            }

            Vector3 final = PlatformsToWander[PlatformsToWander.Count - 1].transform.position;
            final.x += PlatformsToWander[PlatformsToWander.Count - 1].Width * 0.5f;
            final.y += PlatformsToWander[PlatformsToWander.Count - 1].Height * 0.5f;
            finder.Calculate(agent.transform.position, final);
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

            bool isClose = agent.IsNearDestination(agent.GetSafeDistanceReduced());

            if(Path.Count == 0 && isClose)
            {
                //generate
                float rand = Random.Range(0, PlatformsToWander.Count - 1.0f);
                int index = (int)rand;
                bool isLeft = Random.value > 0.5f ? false : true;

                Vector3 final = PlatformsToWander[index].transform.position;
                final.x = isLeft ? final.x - (PlatformsToWander[index].Width * 0.5f) : final.x + (PlatformsToWander[index].Width * 0.5f);
                final.y = PlatformsToWander[index].Height * 0.5f;
                finder.Calculate(agent.transform.position, final);
                Path = finder.GetPath();
                agent.SetDestination(Path[0]);
                agent.SetFinalDestination(Path[Path.Count - 1]);
                Path.RemoveAt(0);
            }
            else if(isClose)
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
