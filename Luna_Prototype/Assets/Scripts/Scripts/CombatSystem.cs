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

public enum MoveTimeSliceType                                                          
{
    Type_None = 0,
    Type_Parryable,                                                   
    Type_DealtDmg,
    Type_Transition,
    Type_Reset,
}

public enum AttacState
{
    State_Parriable,
    State_NonParriable,
}

//================================================================================================================
// Classes
//================================================================================================================

[Serializable]
public class MoveTimeSlice
{
    public MoveTimeSliceType mType;
    public float mTimeSlicePropotion;
    [System.NonSerialized] public float mSliceTotalTime;

    public void Load(float mTotalTime, float lastTimeSlicePropotion)
    {
        Assert.AreNotEqual(mTimeSlicePropotion, 0.0f, "MoveTimeSlize can not be zero");
        Assert.AreEqual(mTimeSlicePropotion > lastTimeSlicePropotion, true, "MoveTimeSlice can not have negative duration");
        Assert.AreEqual(mTimeSlicePropotion <= 1.0f, true, "MoveTimeSlice can not greater than total time slice");

        mSliceTotalTime = (mTimeSlicePropotion - lastTimeSlicePropotion) * mTotalTime;
    }
}

[Serializable]                                                                   
public class MoveContext                                                        
{
    [System.NonSerialized] public float mTotalTime;
    public MoveTimeSlice[] mMoveTimeSlices;
    [System.NonSerialized] public int mCurrentSlice = 0;
    [System.NonSerialized] public int mTransitionSliceCount = 0;
    [System.NonSerialized] public MoveTimeSliceType mCurrentTimeSliceType;
    [System.NonSerialized] public float mCounter;
    [System.NonSerialized] public bool mHaveDealtDmg = false;
    private int mOwnerMoveIndex;

    private int totalSliceCount;
    private bool isActive;                                                       
                                                                                 
    // Getter & Setter        
    public bool Active { get { return isActive; } set { isActive = value; } }    
                                                                                 
    public void Load(float totalTime, int totalTransitionMove, int moveId)
    {
            

        mTotalTime = totalTime;

        //----------------------------------------------------------------------
        int totalTransitionSlice = 0;
        float lastTimeSlicePropotion = 0.0f;
        foreach (var timeSlice in mMoveTimeSlices)
        {
            timeSlice.Load(mTotalTime, lastTimeSlicePropotion);
            if (timeSlice.mType == MoveTimeSliceType.Type_Transition)
                ++totalTransitionSlice;
            lastTimeSlicePropotion = timeSlice.mTimeSlicePropotion;
        }

        mOwnerMoveIndex = moveId;
        Assert.AreEqual(totalTransitionSlice, totalTransitionMove, $"{mOwnerMoveIndex }MoveContext [transition time slice] count not match [transition move count]");

        totalSliceCount = mMoveTimeSlices.Length;
        Reset();
    }
    
    public void Update(float deltaTime)                                          
    {                                                                            
        if (isActive)                                                            
        {                                                                        
            if ( mCounter <= 0.0f)                                                
            {
                //-------------------------------------------------------------------------------//|
                if (mMoveTimeSlices[mCurrentSlice].mType == MoveTimeSliceType.Type_Transition)   //|
                {                                                                                //|--- A transition slice passed, so increase the transitionsliceCount
                    mTransitionSliceCount++;                                                     //|
                }                                                                                //|
                //-------------------------------------------------------------------------------//|

                Core.Debug.Log($"{mMoveTimeSlices[mCurrentSlice].mType } {mCurrentSlice} {mOwnerMoveIndex}");
                mCurrentSlice++;
                mHaveDealtDmg = false;
                if (mCurrentSlice == totalSliceCount)                
                {                                                                
                    Reset();                                                     
                    return;                                                      
                }
                mCounter = mMoveTimeSlices[mCurrentSlice].mSliceTotalTime;
            }                                                                    
            mCounter -= deltaTime;                                               
                                                                                 
        }                                                                        
    }                                                                            
                                                                                 
    public void Reset()                                                          
    {
        mCurrentTimeSliceType = MoveTimeSliceType.Type_None;                       
        isActive = false;
        mHaveDealtDmg = false;
        mCurrentSlice = 0;
        mTransitionSliceCount = 0;
        mCounter = mMoveTimeSlices[0].mSliceTotalTime;         
    }
    
    public MoveTimeSliceType GetCurrentTimeSliceType()
    {
        return mMoveTimeSlices[mCurrentSlice].mType;
    }
    public int GetTransitionSliceCount()
    {
        return mTransitionSliceCount;
    }
}                                                                                
                                                                                 


