using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class StateMachine
    {
        private Enemy mAgent;
        private List<State> mStates = new List<State>();
        private int mCurrentState = 0;

        public void SetAgent(Enemy at)
        {
            mAgent = at;
        }

        public void Initialize(Enemy agent)
        {
            mAgent = agent;
        }

        public States GetCurrentState() { return mStates[mCurrentState].Name(); }

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

        public void DrawGizmo()
        {
            mStates[mCurrentState].DrawGizmo(mAgent);
        }

        public void ChangeState(int stateID)
        {
            Debug.Assert(stateID < mStates.Count);

            if(mCurrentState != 1000)
            {
                mStates[mCurrentState].Exit(mAgent);
            }
            mCurrentState = stateID;
            mStates[mCurrentState].Enter(mAgent);
        }

        public void ChangeState(States state)
        {
            for(int i = 0; i < mStates.Count; ++i)
            {
                if(mStates[i].Name() == state)
                {
                    ChangeState(i);
                    return;
                }
            }
        }
    }
}
