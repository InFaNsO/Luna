using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    public virtual void Enter()
    {

    }
    public virtual void MyUpdate()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void DebugDraw()
    {

    }
    public virtual string GetName()
    {
        return "Base";
    }
}
