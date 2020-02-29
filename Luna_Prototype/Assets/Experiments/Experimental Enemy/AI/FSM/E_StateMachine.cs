using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_StateMachine : MonoBehaviour
{
    private E_Enemy mEnemy;
    [SerializeField] List<E_State> mStates = new List<E_State>();
    private int mCurrentState = -1;

    // Start is called before the first frame update
    void Start()
    {
        mEnemy = GetComponentInParent<E_Enemy>();
        Debug.AssertFormat(mStates.Count > 0, "No states in state machine");

        var states = GetComponentsInChildren<E_State>();
        for (int i = 0; i < states.Length; ++i)
            mStates.Add(states[i]);

    }

    // Update is called once per frame
    void Update()
    {
        if (!mEnemy.IsRunning)
            return;

        if (mCurrentState == -1)
        {
            mCurrentState = 0;
            mStates[mCurrentState].Enter();
        }
        mStates[mCurrentState].MyUpdate();
    }
    public void OnDrawGizmos()
    {
        mStates[mCurrentState].DebugDraw();
    }

    public void ChangeState(string name)
    {
        if(mCurrentState != -1)
        {
            mStates[mCurrentState].Exit();
        }

        for(int i = 0; i < mStates.Count; ++i)
        {
            if(mStates[i].GetName() == name)
            {
                mCurrentState = i;
                mStates[mCurrentState].Enter();
            }
        }
    }
}
