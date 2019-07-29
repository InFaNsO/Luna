//===========================================================================================================================================================
// Filename:	Weapon.cs
// Created by:	Mingzhuo Zhang
// Description:	Store basic data structure for weapon game object
//===========================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    // Members --------------------------------------------------------------------------------------------------------------
    [SerializeField] protected Element mElement;
    [SerializeField] protected WeaponGrade mGrade;
    [SerializeField] protected WeaponType mType;

    [SerializeField] protected string mName;
    [SerializeField] protected int mDamage;
    [SerializeField] protected float mAttackSpeed; // wait how many second to next attack
    protected float mAttackSpeedCounter;
    [SerializeField] ParryContext mParryContext;

    [SerializeField] protected Bullet mBullet;
    protected Vector3 mFirePositionOffSet;

    bool mHaveAttack = false;

    // Getter & Setter -------------------------------------------------------------------------------------------------------
    public float AttackSpeed { get { return mAttackSpeed; } set { mAttackSpeed = value; } }

    // MonoBehaviour Functions -----------------------------------------------------------------------------------------------
    public void Awake()
    {
        Assert.IsNotNull(mBullet, "[Weapon] Dont have bullet");     //|--- [SAFTY]: Check to see is there a Collider
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[0], 0.0f, "[Enemy] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[1], 0.0f, "[Enemy] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreEqual(mParryContext.mTimeSlicePropotion.Length, 2, "[Enemy] Parry context TimeSlice count should be 3");    //|--- [SAFTY]: Check to see if parry context TimeSlice count is 3

        mParryContext.TotalTime = AttackSpeed;                                                                                //|--- [INIT]: Initliaze the total time cycle for parry system in ParryContext
        mParryContext.Reset();
        mBullet.Element = mElement;                                                                                           //|--- [INIT]: Initliaze the element of the bullet
    }

    void Update()
    {
        mParryContext.Update(Time.deltaTime);

        if (mParryContext.Active)
        {
            if (mParryContext.CurrentState == AttackState.AttackState_Parryable)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            }
            else if ((mParryContext.CurrentState == AttackState.AttackState_NonParryable) && (mHaveAttack == false))
            {
                ShootBullet();
                mHaveAttack = true;
                gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }
        }
    }

    // Self-define functions --------------------------------------------------------------------------------------------------
    public AttackState GetAttackState()
    {
        AttackState ret = AttackState.AttackState_NonParryable;
        switch (mType)
        {
            case WeaponType.Melee:
                ret = mParryContext.CurrentState;
                break;
            case WeaponType.Range:
                ret = AttackState.AttackState_NonParryable;
                break;
            default:
                break;
        }
        return ret;
    }

    public void Attack()
    {
        if (mParryContext.Active == false)
        {
            Debug.Log("[Weapon] start attack");
            mParryContext.Active = true;
            mHaveAttack = false;
        }
    }

    public void ShootBullet()
    {
        Bullet newBullet = Instantiate(mBullet, new Vector3(0, 0, 0), Quaternion.identity);
        newBullet.Fire(gameObject.tag, mDamage, gameObject.transform.position + mFirePositionOffSet, gameObject.transform.forward, mType);
        Debug.Log("attacked");
    }

}