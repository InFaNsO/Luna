using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiyingState : State
{
    Enemy mAgent;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<Enemy>();
    }

    public override void Enter()
    {
        mAgent.gameObject.SetActive(false);
    }

    public override void MyUpdate()
    {

    }

    public override void Exit()
    {
    }

    public override string GetName()
    {
        return "Die";
    }

}
