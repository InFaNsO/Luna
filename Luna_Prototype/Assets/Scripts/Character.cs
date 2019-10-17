//===========================================================================================================================================================
// Filename:	Weapon.cs
// Created by:	Mingzhuo Zhang
// Description:	Store basic abstract class for Enemy & Player to inherient from
//===========================================================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// merge test
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float mMaxHealth;
    [SerializeField]
    protected float mCurrentHealth;
    [SerializeField]
    protected float mMovementSpeed;
    [SerializeField]
    protected float mJumpStrength;

    public abstract void GetHit(float dmg);
    public abstract void Die();

    public void Awake()
    {
        mCurrentHealth = mMaxHealth;
    }

    public void Update()
    {
        if (mCurrentHealth <= 0.0f)
        {
            Die();
        }
    }
}