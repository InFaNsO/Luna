using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    [System.Serializable]
    public class StateMachine
    {
        [SerializeField] private Enemy mAgent;
        [SerializeField] private List<State> mStates = new List<State>();
        private int mCurrentState = -1;

        public void SetAgent(Enemy at)
        {
            mAgent = at;
        }

        public void Initialize(Enemy agent)
        {
            mAgent = agent;
        }

        public void AddState<StateType>(StateType state = null) where StateType : State, new()
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
