using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SteeringModule : MonoBehaviour
{
    E_Enemy mAgent;
    [SerializeField] List<E_SteeringBase> mBehaviours = new List<E_SteeringBase>();
    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponentInParent<E_Enemy>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Calculate()
    {
        Vector3 total = new Vector3();

        for (int i = 0; i < mBehaviours.Count; ++i)
        {
            if (mBehaviours[i].IsActive())
            {
                total += mBehaviours[i].Calculate();
            }
        }

        return total;
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

}
