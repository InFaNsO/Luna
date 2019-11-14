using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class StateMachine <AgentType> where AgentType : Enemy
    {
        [SerializeField] private AgentType mAgent;
        [SerializeField] private List<State<AgentType>> mStates = new List<State<AgentType>>();
        private int mCurrentState = -1;

        public void SetAgent(AgentType at)
        {
            mAgent = at;
        }

        public void Initialize(AgentType agent)
        {
            mAgent = agent;
        }

        public void AddState<StateType>(StateType state = null) where StateType : State<AgentType>, new()
        {
            if (state == null)
                mStates.Add(new StateType());
            else
                mStates.Add(state);
        }

        public void Update()
        {
            mStates[mCurrentState].Update(mAgent);
        }

        public void ChangeState(int stateID)
        {
            Debug.Assert(stateID < mStates.Count);

            if(mCurrentState != -1)
            {
                mStates[mCurrentState].Exit(mAgent);
            }
            mCurrentState = stateID;
            mStates[mCurrentState].Enter(mAgent);
        }
    }
}
