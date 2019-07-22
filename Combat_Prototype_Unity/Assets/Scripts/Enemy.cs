using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Charactor : MonoBehaviour     
{
    [SerializeField] protected float mMaxHealth;
    protected float mCurrentHealth;

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

public class Enemy : Charactor
{
    [SerializeField] Weapon mWeapon;
    [SerializeField] GameObject mParryStateIndicator;
    [SerializeField] ParryContext mParryContext = new ParryContext();
    private Animator mAnimator;
    bool mIsStuned = false;
    float mStunCounter;
    bool mHaveAttack = false;

    private int behaviorCounter = 0; // For hard code behavior

    new public void Awake()
    {
        base.Awake();
        Assert.IsNotNull(GetComponent<CapsuleCollider>(), "[Enemy] Dont have collider");                                      //|--- [SAFTY]: Check to see is there a Collider
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[0], 0.0f, "[Enemy] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[1], 0.0f, "[Enemy] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[2], 0.0f, "[Enemy] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreEqual(mParryContext.mTimeSlicePropotion.Length, 3,   "[Enemy] Parry context TimeSlice count should be 3");  //|--- [SAFTY]: Check to see if parry context TimeSlice count is 3

        mAnimator = gameObject.GetComponent<Animator>();
        mParryContext.TotalTime = mWeapon.AttackSpeed;
        mParryContext.Reset();
        mWeapon.gameObject.tag = "Enemy";
    }

    public void Attack()
    {
        if (mWeapon != null)
        {
            if (mParryContext.Active == false)
            {
                Debug.Log("start attack");
                mParryContext.Active = true;
                mHaveAttack = false;
                // Do animation
                mAnimator.SetInteger("Condition", 8);
            }
        }
    }

    override public void Die()
    {
        //effect
        Debug.Log("enemy destory");

        // [Maybe] Update game Score
        // [Maybe] Change anmation state

        gameObject.SetActive(false);
    }

    override public void GetHit(float dmg)
    {
        mCurrentHealth -= dmg;

        if (mParryContext.CurrentState == AttackState.Preparing)
        {
            // do animation staff
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

        //Do animation staff
        mAnimator.SetInteger("Condition", 10);
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

    new public void Update()
    {
        base.Update();
        mParryContext.Update(Time.deltaTime);
        if (mParryContext.Active)
        {
            if (mParryContext.CurrentState == AttackState.Preparing)
            {
                mParryStateIndicator.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            }
            else if ((mParryContext.CurrentState == AttackState.Attacking) && (mHaveAttack == false))
            {
                mWeapon.Attack();
                mHaveAttack = true;
                mParryStateIndicator.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }
            else if (mParryContext.CurrentState == AttackState.Reseting)
            {
                mParryStateIndicator.GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
            }
        }

        if (mIsStuned != true)
        {
            //Do control
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

        // Hard code behavior
        behaviorCounter++;
        if (behaviorCounter % 100 == 0)
        {
            Attack();
        }
    }

    public void LateUpdate()
    {
        mAnimator.SetInteger("Condition", 0);
    }
}
