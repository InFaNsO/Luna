using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SteeringBase : MonoBehaviour
{
    public bool mIsActive = false;

    public virtual Vector3 Calculate()
    {
        return new Vector3();
    }

    public virtual SteeringType GetName()
    {
        return SteeringType.Base;
    }

    public void SwichActive() { mIsActive = !mIsActive; }
    public void SetActive(bool act) { mIsActive = act; }
    public bool IsActive() { return mIsActive; }
}
