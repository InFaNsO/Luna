using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Enemy mEnemy;
    [SerializeField] List<State> mStates = new List<State>();
    private int mCurrentState = 0;

    // Start is called before the first frame update
    void Start()
    {
        mEnemy = GetComponentInParent<Enemy>();

        mStates.Add(new State());

        var states = GetComponentsInChildren<State>();
        for (int i = 0; i < states.Length; ++i)
            mStates.Add(states[i]);

        Debug.AssertFormat(mStates.Count > 1, "No states in state machine");
    }

    // Update is called once per frame
    void Update()
    {
        if (!mEnemy.IsRunning)
            return;

        if (mCurrentState == 0)
        {
            mCurrentState = 1;
            mStates[mCurrentState].Enter();
        }
        mStates[mCurrentState].MyUpdate();
    }
    void OnDrawGizmos()
    {
        mStates[mCurrentState].DebugDraw();
    }

    public void ChangeState(string name)
    {
        if(mCurrentState != 0)
        {
            mStates[mCurrentState].Exit();
        }

        for(int i = 1; i < mStates.Count; ++i)
        {
            if(mStates[i].GetName() == name)
            {
                mCurrentState = i;
                mStates[mCurrentState].Enter();
            }
        }
    }
}
