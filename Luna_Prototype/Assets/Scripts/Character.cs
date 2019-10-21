//===========================================================================================================================================================
// Filename:	Weapon.cs
// Created by:	Mingzhuo Zhang
// Description:	Store basic abstract class for Enemy & Player to inherient from
//===========================================================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// merge test
public abstract class Character : Agent
{
    [SerializeField]
    protected float mMaxHealth;
    [SerializeField]
    protected float mCurrentHealth;
    [SerializeField]
    protected float mMovementSpeed;
    [SerializeField]
    protected float mJumpStrength;

    //the four variables below are used for hazard damage  - william
    [SerializeField]
    private bool mWasTheCharacterInAnHazardInThePastSecond;
    [SerializeField]
    private float mReceiveHazardDebuffCD;
    [SerializeField]
    private int mReceivedHazardDamage;
    [SerializeField]
    private int mReceivedHazardDuration;

    public bool isGrounded = false;                                    //|--- [Mingzhuo Zhang] Edit: I need to know is player is on ground or not for my weapon combo system

    public abstract void GetHit(float dmg);
    public abstract void Die();


    public void ReceiveDebuff(int debuffDamage, int debuffDuration)
    {
        mReceivedHazardDamage = debuffDamage;
        mReceivedHazardDuration = debuffDuration;
    }

    public void Awake()
    {
        mCurrentHealth = mMaxHealth;
        mWasTheCharacterInAnHazardInThePastSecond = false;
        mReceiveHazardDebuffCD = 0.0f;
        mReceivedHazardDamage = 0;
        mReceivedHazardDuration = 0;
    }

    public void Update()
    {
        if (mCurrentHealth <= 0.0f)
        {
            Die();
        }

        if(mWasTheCharacterInAnHazardInThePastSecond)
        {
            mReceiveHazardDebuffCD += Time.deltaTime;
        }

        if(mReceiveHazardDebuffCD > 1.0f)
        {
            mWasTheCharacterInAnHazardInThePastSecond = false;
            mReceiveHazardDebuffCD = 0.0f;
        }

    }
}