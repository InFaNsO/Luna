using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class PathFollowingState <AgentType> : State<AgentType> where AgentType : Enemy
    {
        [SerializeField] private List<Vector3> mPath;
        [SerializeField] private bool mLooped = false;
        [SerializeField] private bool mStay = false;
        [SerializeField] private float mStayTime = 2.0f;
        private int mNext = 0;
        private bool setActive = false;
        private float mTimmer = 0.0f;

        public void AddPath(Vector3 pos)
        {
            mPath.Add(pos);
        }
        public void AddPath(List<Vector3> pos)
        {
            mPath.AddRange(pos);
        }
        public void SetLooping(bool b) { mLooped = b; }
        public void SetStay(bool b, float stayTime = 2.0f)
        {
            mStay = b;
            mStayTime = stayTime;
        }

        public override void Enter(AgentType agent)
        {
            agent.GetSteeringModule().TurnAllOff();

            if (!agent.GetSteeringModule().Exists(SteeringType.Seek))
            {
                agent.GetSteeringModule().AddState<BehaviourSeek>();
            }
            if(!agent.GetSteeringModule().IsActive(SteeringType.Seek))
            {
                agent.GetSteeringModule().SetActive(SteeringType.Seek, true);
                setActive = true;
            }
        }

        private void SetDestination(AgentType agent)
        {
            mNext++;
            if (mNext == mPath.Count)
            {
                if (mLooped)
                {
                    mNext = 0;
                }
                else
                {
                    mNext--;
                }
            }
            agent.SetDestination(new Vector2(mPath[mNext].x, mPath[mNext].y));
        }

        private bool IsNear(AgentType agent)
        {
            if (agent.transform.position.x < (mPath[mNext].x + agent.GetSafeDistance()) &&
                agent.transform.position.x > (mPath[mNext].x - agent.GetSafeDistance()))
            {
                if (agent.transform.position.y < (mPath[mNext].y + agent.GetSafeDistance()) &&
                    agent.transform.position.y > (mPath[mNext].y - agent.GetSafeDistance()))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Update(AgentType agent)
        {
            if(IsNear(agent))
            {
                if (mStay)
                {
                    mTimmer += Time.deltaTime;
                    if (mTimmer > mStayTime)
                    {
                        SetDestination(agent);
                        mTimmer = 0.0f;
                    }
                }
                else
                {
                    SetDestination(agent);
                }
            }
            if (mNext < 0)
                mNext = 0;
            if(mNext == mPath.Count)
                mNext -= 1;
        }

        public override void Exit(AgentType agent)
        {
            if(setActive)
            {
                agent.GetSteeringModule().SetActive(SteeringType.Seek, false);
            }
        }
    }
}
