//===========================================================================================================================================================
// Filename:	CombatSystem.cs
// Created by:	Mingzhuo Zhang
// Description:	Store some basic data structures for Combat system for our game
//===========================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEditor;


//================================================================================================================
// Enums
//================================================================================================================

//- Enum for Combat Element ----------------------------------------------------------------------------------
public enum Element                                                              
{                                                                                
    Luna = 0,                                                                    
    Fire,                                                                        
    Water                                                                        
}

//- Enum for Weapon ------------------------------------------------------------------------------------------
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

public enum AttackState                                                          
{                                                                                
    AttackState_Parryable = 0,                                                   
    AttackState_NonParryable,                                                    
    AttackState_Max,                                                             
}


//================================================================================================================
// Classes
//================================================================================================================

[Serializable]                                                                   
public class ParryContext                                                        
{                                                                                
    private AttackState mCurrentState;                                           
    public float[] mTimeSlicePropotion = new float[2];                           
    private float mTotalTime;                                                    
    private float mCounter;                                                      
    private bool isActive;                                                       
                                                                                 
    // Getter & Setter                                                           
    public float TotalTime { set { mTotalTime = value; } }                       
    public AttackState CurrentState { get { return mCurrentState; } }            
    public bool Active { get { return isActive; } set { isActive = value; } }    
                                                                                 
                                                                                 
    public void Update(float deltaTime)                                          
    {                                                                            
        if (isActive)                                                            
        {                                                                        
            if (mCounter <= 0.0f)                                                
            {                                                                    
                mCurrentState++;                                                 
                if (mCurrentState == AttackState.AttackState_Max)                
                {                                                                
                    Reset();                                                     
                    return;                                                      
                }                                                                
                mCounter = mTimeSlicePropotion[(int)mCurrentState] * mTotalTime; 
            }                                                                    
            mCounter -= deltaTime;                                               
                                                                                 
        }                                                                        
    }                                                                            
                                                                                 
    public void Reset()                                                          
    {                                                                            
        mCurrentState = AttackState.AttackState_Parryable;                       
        isActive = false;                                                        
        mCounter = mTimeSlicePropotion[(int)mCurrentState] * mTotalTime;         
    }                                                                            
}                                                                                
                                                                                 


