﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class AttackState : State
    {
        [SerializeField] bool canHeal = false;
        int closestTeam = -1;

        public override States Name()
        {
            return States.Attack;
        }

        public override void Enter(Enemy agent)
        {
            agent.GetComponent<SpriteRenderer>().color = Color.red;


            if (canHeal)
            {
                float smalldistance = 100.0f;
                for (int i = 0; i < agent.GetWorld().mAgents.Count; ++i)
                {
                    float distance = Vector3.Distance(agent.transform.position, agent.GetWorld().mAgents[i].transform.position);
                    if(smalldistance < distance)
                    {
                        closestTeam = i;
                    }

                }
            }
        }

        public override void Update(Enemy agent)
        {
            if(canHeal)
            {

                if(agent.GetWorld().mAgents[closestTeam].mCurrentHealth < 0.01f)
                {
                    closestTeam = -1;
                    float smalldistance = 100.0f;
                    for (int i = 0; i < agent.GetWorld().mAgents.Count; ++i)
                    {
                        float distance = Vector3.Distance(agent.transform.position, agent.GetWorld().mAgents[i].transform.position);
                        if (smalldistance < distance && distance < 5.0f)
                        {
                            closestTeam = i;
                        }

                    }
                    if (closestTeam == -1)
                    { canHeal = false; return; }
                }
                //agent.GetWorld().mAgents[closestTeam]; //heal it

                return;
            }

            float rangeDis = agent.GetSafeDistance(); //choose it based on the weapon
            if (agent.IsNearPlayer(rangeDis))
            {
                //use weapons
                agent.UseWerapon();
            }
            else
            {
                //out of range to attack so switch back to go to
                agent.SetDestination(agent.GetWorld().mPlayer.transform.position);
            }

            if(agent.IsNearPlayer(agent.GetSafeDistanceExtended()))
            {
                agent.mStateMachine.ChangeState(States.GoToPlayer);
            }
        }

        public override void Exit(Enemy agent)
        {

        }
    }

}
