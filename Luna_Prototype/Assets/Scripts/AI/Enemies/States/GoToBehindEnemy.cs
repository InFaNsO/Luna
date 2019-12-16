using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{ 
    public class GoToBehindEnemy : State
    {
        Grid_PathFinding finder = new Grid_PathFinding();
        List<Vector3> Path = new List<Vector3>();

        int buddyNodeID = -1;

        int enemyID2HealGlobal = -1;
        int enemyID2HealLocal = -1;
        List<Agent> nearbyTeam = new List<Agent>();

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

            float distance = 100.0f;
            float smallDistance = 100.0f;
            //find enemy to heal
            if (nearbyTeam.Count == 0)
            {
                var enemies = agent.GetWorld().mAgents;
                for(int i = 0; i < enemies.Count; ++i)
                {
                    distance = Vector3.Distance(agent.transform.position, enemies[i].transform.position);
                    if (distance < agent.GetSafeDistanceExtended())
                    {
                        if(smallDistance > distance)
                        {
                            smallDistance = distance;
                            enemyID2HealGlobal = i;
                            enemyID2HealLocal = nearbyTeam.Count;
                        }
                        nearbyTeam.Add(enemies[i]);
                    }
                }
            }

            Vector3 dest = nearbyTeam[enemyID2HealLocal].transform.position;
            if(agent.GetWorld().mPlayer.transform.position.x > dest.x)
            {
                //player is in right
                dest.x -= agent.GetSafeDistance();
            }
            else
            {
                dest.x += agent.GetSafeDistance();
            }

            finder.Calculate(agent.transform.position, dest);

            buddyNodeID = finder.GetNearestNodeID(nearbyTeam[enemyID2HealLocal].transform.position);

            Path = finder.GetPath();

            agent.SetDestination(Path[0]);
            agent.SetFinalDestination(Path[Path.Count - 1]);
            Path.RemoveAt(0);

            //if player is on its right 
            //go to its left 
            //else
            //go to its right
        }
    
        public override void Update(Enemy agent)
        {
            float attackRange = agent.GetSafeDistanceExtended();

            //if player is close enought to attck switch to attack
            if (agent.IsNearPlayer(agent.GetSafeDistanceExtended()) &&
                agent.GetWorld().HasLineOfSight(new World.Wall(agent.transform.position, agent.GetWorld().mPlayer.transform.position)))
            {
                agent.mStateMachine.ChangeState(States.Attack);
                return;
            }

            bool calculateAgain = false;
            int pNID = finder.GetNearestNodeID(nearbyTeam[enemyID2HealLocal].transform.position);
            if (buddyNodeID != pNID)
            {
                buddyNodeID = pNID;
                calculateAgain = true;
            }

            if (calculateAgain)
            {
                finder.Calculate(agent.transform.position, nearbyTeam[enemyID2HealLocal].transform.position);
                Path.Clear();
                Path = finder.GetPath();
                agent.SetDestination(Path[0]);
                Path.RemoveAt(0);
                agent.SetFinalDestination(Path[Path.Count - 1]);
            }

            bool isClose = agent.IsNearDestination(agent.GetSafeDistanceReduced());
            bool isAlive = false;
            if (enemyID2HealLocal > -1)
                isAlive = nearbyTeam[enemyID2HealLocal].mCurrentHealth < 0.01f ? false : true;

            if(!isAlive)
            {
                nearbyTeam.RemoveAt(enemyID2HealLocal);
                enemyID2HealLocal = -1;
                float distance = 100.0f;
                float smallDistance = distance;

                for (int i = 0; i < nearbyTeam.Count; ++i)
                {
                    distance = Vector3.Distance(agent.transform.position, nearbyTeam[i].transform.position);
                    if (distance < smallDistance)
                    {
                        smallDistance = distance;
                        enemyID2HealLocal = i;
                    }
                }
            }

            //if player is far away than range then switch to wander 
            if (Path.Count == 0)
            {
                agent.mStateMachine.ChangeState(States.Attack);
                return;
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
