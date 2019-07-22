using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEditor;


//---------------This may need to put in some where like <combat system> -----------|
public enum Element                                                               //|
{                                                                                 //|
    Luna = 0,                                                                     //|
    Fire,                                                                         //|
    Liquir                                                                        //|
}                                                                                 //|
                                                                                  //|
public enum AttackState                                                           //|
{                                                                                 //|
    Preparing = 0,                                                                //|
    Attacking,                                                                    //|
    Reseting,                                                                     //|
    AttackState_Max                                                               //|
}                                                                                 //|
                                                                                  //|
[Serializable]                                                                    //|
public class ParryContext                                                         //|
{                                                                                 //|
    private AttackState mCurrentState;                                             //|
    public float[] mTimeSlicePropotion = new float[3];                            //|
    private float mTotalTime;                                                     //|
    private float mCounter;                                                        //|
    private bool isActive;                                                         //|
                                                                                  //|
    // Getter & Setter                                                            //|
    public float TotalTime { set { mTotalTime = value; } }                        //|
    public AttackState CurrentState { get { return mCurrentState; } }
    public bool Active { get { return isActive; } set { isActive = value; } }

                                                                                  //|
    public void Update(float deltaTime)                                           //|
    {                                                                             //|
        if (isActive)                                                             //|
        {                                                                         //|
            if (mCounter <= 0.0f)                                                 //|
            {                                                                     //|
                mCurrentState++;                                                  //|
                if (mCurrentState == AttackState.AttackState_Max)                 //|
                {                                                                 //|
                    Reset();                                                      //|
                    return;                                                       //|
                }                                                                 //|
                mCounter = mTimeSlicePropotion[(int)mCurrentState] * mTotalTime;  //|
            }                                                                     //|
            mCounter -= deltaTime;                                                //|
                                                                                  //|
        }                                                                         //|
    }                                                                             //|
                                                                                  //|
    public void Reset()                                                           //|
    {                                                                             //|
        mCurrentState = AttackState.Preparing;                                    //|
        isActive = false;                                                         //|
        mCounter = mTimeSlicePropotion[(int)mCurrentState] * mTotalTime;          //|
    }                                                                             //|
}                                                                                 //|
                                                                                  //|
//---------------This may need to put in some where like <combat system> -----------|

public enum WeaponGrade
{
    Common = 0,
    Rare,
    Legendary
}

public enum WeaponType
{
    Melee = 0,
    Range
}

public abstract class Weapon : MonoBehaviour
{
    protected Element mElement;
    protected WeaponGrade mGrade;
    protected WeaponType mType;

    [SerializeField] protected string mName;
    [SerializeField] protected int mDamage;
    [SerializeField] protected float mAttackSpeed; // wait how many second to next attack
    protected float mAttackSpeedCounter;

    [SerializeField] protected Bullet mBullet;
    protected Vector3 mFirePositionOffSet;

    // Getter & Setter
    public float AttackSpeed { get { return mAttackSpeed; } set { mAttackSpeed = value; } }

    public void Awake()
    {
        Assert.IsNotNull(mBullet, "[Weapon] Dont have bullet");     //|--- [SAFTY]: Check to see is there a Collider
    }


    public abstract void Attack();

}