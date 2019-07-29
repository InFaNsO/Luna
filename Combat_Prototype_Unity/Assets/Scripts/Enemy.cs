//===========================================================================================================================================================
// Filename:	Enemy.cs
// Created by:	Mingzhuo Zhang
// Description:	Store basic data structure for enemy game object
//===========================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

enum EnemyAnimation
{
    None = 0,
    ToIdel,
    Stun,
    Attack,
}

public class Enemy : Character
{
    // Members ---------------------------------------------------------------------------------------------------------------
    [SerializeField] Weapon mWeapon;

    private Animator mAnimator;
    bool mIsStuned = false;
    float mStunCounter;

    // MonoBehaviour Functions -----------------------------------------------------------------------------------------------
    new public void Awake()
    {
        base.Awake();
        Assert.IsNotNull(GetComponent<CapsuleCollider>(), "[Enemy] Dont have CapsuleCollider");                                      //|--- [SAFTY]: Check to see is there a Collider

        mAnimator = gameObject.GetComponent<Animator>();                                                                             //|--- [INIT]: Initialize animator
    }

    new public void Update()
    {
        base.Update();

        if (mIsStuned != true)
        {
            //Do AI here
            HardCodeBehavior();
        }
        else
        {
            if (mStunCounter <= 0.0f)
            {
                mIsStuned = false;
                // Do animation
            }
            mStunCounter -= Time.deltaTime;
        }
    }

    public void LateUpdate()
    {
        SetAnimator(EnemyAnimation.ToIdel);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != gameObject.tag)
        {
            if (other.GetComponent<Bullet>() != null)
            {
                Debug.Log("enemy collide with bullet!");

                GetHit(other.GetComponent<Bullet>().Damage);
            }
        }
    }
    // Self-define Functions -----------------------------------------------------------------------------------------------

    public void Attack()
    {
        mWeapon.Attack();
        SetAnimator(EnemyAnimation.Attack);
    }

    override public void Die()
    {
        //effect
        Debug.Log("enemy destory");

        // [Maybe] Update game Score
        // [Maybe] Change anmation state

        //--------------------------//|
        Destroy(gameObject);        //|--- Set this by routin function in future: For giveing time to died animation 
        //--------------------------//|
    }

    override public void GetHit(float dmg)
    {
        mCurrentHealth -= dmg;

        if (mWeapon.GetAttackState() == AttackState.AttackState_Parryable)
        {
            GetStun(1.5f);
        }

        if (mCurrentHealth <= 0.0f)
        {
            Die();
        }
    }

    public void GetStun(float stunHowLong)
    {
        mIsStuned = true;
        mStunCounter = stunHowLong;

        SetAnimator(EnemyAnimation.Stun);
    }

    void SetAnimator(EnemyAnimation animationType)
    {
        switch (animationType)
        {
            case EnemyAnimation.None:
                break;
            case EnemyAnimation.ToIdel:
                mAnimator.SetInteger("Condition", 0);
                break;
            case EnemyAnimation.Stun:
                mAnimator.SetInteger("Condition", 10);
                break;
            case EnemyAnimation.Attack:
                mAnimator.SetInteger("Condition", 8);
                break;
            default:
                break;
        }
    }

    //-------------------------------------------------------------------------------//|
    private int behaviorCounter = 0; // For hard code behavior                       //|
    void HardCodeBehavior()                                                          //|
    {                                                                                //|
        // Hard code behavior                                                        //|
        behaviorCounter++;                                                           //|
        if (behaviorCounter % 100 == 0)                                              //|--- Hard code behavior, Delete this in future
        {                                                                            //|
            Attack();                                                                //|
        }                                                                            //|
    }                                                                                //|
    //-------------------------------------------------------------------------------//|
}
