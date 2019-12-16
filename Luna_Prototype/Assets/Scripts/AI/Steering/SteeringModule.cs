using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAI
{
    public class SteeringModule
    {
        private Agent mAgent;
        private List<SteeringBehaviourBase> mBehaviours = new List<SteeringBehaviourBase>();

        public void SetAgent(Agent a) { mAgent = a; }
        public void Initialize()
        {
        }

        public void AddState<SteeringState>() where SteeringState : SteeringBehaviourBase, new()
        {
            mBehaviours.Add(new SteeringState());
        }

        public void TurnAllOn()
        {
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                mBehaviours[i].SetActive(true);
            }
        }
        public void TurnAllOff()
        {
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                mBehaviours[i].SetActive(false);
            }
        }

        public void SwitchActive(SteeringType name)
        {
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                if (mBehaviours[i].GetName() == name)
                {
                    mBehaviours[i].SwichActive();
                    return;
                }
            }
        }
        public void SwitchActive(int index)
        {
            if (index < mBehaviours.Count)
            {
                mBehaviours[index].SwichActive();
            }
        }

        public bool IsActive(SteeringType name)
        {
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                if (mBehaviours[i].GetName() == name)
                {
                    return mBehaviours[i].IsActive();
                }
            }
            return false;
        }
        public bool IsActive(int index)
        {
            if (index < mBehaviours.Count && index >= 0)
            {
                return mBehaviours[index].IsActive();
            }
            return false;
        }

        public void SetActive(SteeringType name, bool active)
        {
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                if (mBehaviours[i].GetName() == name)
                {
                    mBehaviours[i].SetActive(active);
                    return;
                }
            }
        }
        public void SetActive(int index, bool active)
        {
            if (index < mBehaviours.Count && index >= 0)
            {
                mBehaviours[index].SetActive(active);
            }
        }

        public bool Exists(SteeringType name)
        {
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                if (mBehaviours[i].GetName() == name)
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 Calculate()
        {
            Vector2 total = new Vector2();
            //mAgent.SetDestination(mBehaviours[2].Calculate(mAgent));
            //total += mBehaviours[0].Calculate(mAgent);
            //return total;
            //
            for (int i = 0; i < mBehaviours.Count; ++i)
            {
                if (mBehaviours[i].IsActive())
                {
                    total += (mBehaviours[i].Calculate(mAgent)).normalized;
                }
            }

            return total;
        }

    }
}

