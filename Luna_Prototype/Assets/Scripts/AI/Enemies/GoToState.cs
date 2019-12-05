using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class GoToState<AgentType> : State<AgentType> where AgentType : Enemy
    {
        private bool setOA;
        private bool setAr;

        public override void Enter(AgentType agent)
        {
            agent.GetSteeringModule().TurnAllOff();
            if (!agent.GetSteeringModule().Exists(SteeringType.Arrive))
            {
                agent.GetSteeringModule().AddState<BehaviourSeek>();
            }
            if (!agent.GetSteeringModule().Exists(SteeringType.ObstacleAvoidance))
            {
                agent.GetSteeringModule().AddState<BehaviourObstacleAvoidance>();
            }

            if (!agent.GetSteeringModule().IsActive(SteeringType.Arrive))
            {
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, true);
                setAr = true;
            }
            if (!agent.GetSteeringModule().IsActive(SteeringType.ObstacleAvoidance))
            {
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, true);
                setOA = true;
            }

            agent.SetFinalDestination(agent.GetWorld().mPlayer.transform.position);
        }

        public override void Update(AgentType agent)
        {
            agent.SetFinalDestination(agent.GetWorld().mPlayer.transform.position);

            var diff = agent.transform.position - agent.GetWorld().mPlayer.transform.position;
            if(diff.sqrMagnitude >= agent.GetSafeDistanceReduced() * agent.GetSafeDistanceReduced())
            {
                //player is close enough
            }
            
            if(agent.transform.position.x < agent.GetWorld().mPlayer.transform.position.x)
            {
                //go right
                var dest = agent.transform.position;
                dest.x += 3.0f;
                agent.SetDestination(dest);
            }
            else if (agent.transform.position.x > agent.GetWorld().mPlayer.transform.position.x)
            {
                //go left
                var dest = agent.transform.position;
                dest.x += 3.0f;
                agent.SetDestination(dest);
            }

           // //check if there is a jump object then jump to most optimal one
           // if(!agent.IsJumpCapable() || diff.y < agent.GetSafeDistanceReduced())       //see if enemy can jump or is there a need to jump
           // {
           //     //end the update
           //     return;
           // }
            for(int i = 0; i < agent.GetWorld().mJumpNodes.Count; ++i)
            {
                diff = agent.transform.position - agent.GetWorld().mJumpNodes[i].transform.position;
                if (diff.sqrMagnitude >= agent.GetSafeDistanceReduced() * agent.GetSafeDistanceReduced())
                {
                    float yDiffToPlayer = agent.GetWorld().mPlayer.transform.position.y - agent.transform.position.y;
                    Vector3 jumpPos = new Vector3();
                    //enemy is close to the jump point to jump
                    for(int j = 0; j < agent.GetWorld().mJumpNodes[i].children.Count; ++j)
                    {
                        if(yDiffToPlayer > 0)
                        {
                            if (agent.GetWorld().mJumpNodes[i].children[j].transform.position.y > agent.transform.position.y)
                            {
                                //use this and exit
                                agent.SetDestination(agent.GetWorld().mJumpNodes[i].children[j].transform.position);
                            }
                        }
                        else
                        {
                            if (agent.GetWorld().mJumpNodes[i].children[j].transform.position.y < agent.transform.position.y)
                            {
                                //use this and exit
                                agent.SetDestination(agent.GetWorld().mJumpNodes[i].children[j].transform.position);
                            }
                        }

                    }
                }
            }
        }

        public override void Exit(AgentType agent)
        {
            if (setAr)
                agent.GetSteeringModule().SetActive(SteeringType.Arrive, false);
            if (setOA)
                agent.GetSteeringModule().SetActive(SteeringType.ObstacleAvoidance, false);

        }
    }

}

