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

    //--------------------------------------------------------------------------//|
   
    [SerializeField] protected string mName;                                    //|
    [SerializeField] protected WeaponGrade mGrade;                              //|--- TODO:: make them as WeaponPorperty struct
    [SerializeField] public WeaponType mType;                                //|
    public float mParryCD = 1.0f;
                                                                                //--------------------------------------------------------------------------//|

    //[SerializeField] protected int mDamage;
    //[SerializeField] protected float mAttackSpeed; // wait how many second to next attack
    //protected float mAttackSpeedCounter;
    //[SerializeField] ParryContext mParryContext;
    //[SerializeField] protected Bullet mBullet;
    //[SerializeField] protected Transform mFirePosition;

    public WeaponMove[] mMoves;
    [System.NonSerialized] public int mCurrentMoveIndex = 0;
    public int mAirMoveIndex = 0;

    //------------------------------------------------------------------------------------//|
    [SerializeField] protected GameObject InLevelBody;                                    //|
    [SerializeField] protected CircleCollider2D pickUpCollider;                           //|--- Render purpose
    [SerializeField] protected SpriteRenderer inlevelBody_spriteRender;                   //|
    //------------------------------------------------------------------------------------//|

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    //public bool isOnGround = false;

    public Animation mAnimation;
    [System.NonSerialized] public Animator mAnimator;

    bool mHaveAttack = false;

    private Transform _localLevelManagerTransform;


    public ElementalAttributes mElement;
    public ElementalAttributes mOwnerElement;

    public Vector3 mTargetPos;

    // Getter & Setter -------------------------------------------------------------------------------------------------------
    //public float AttackSpeed { get { return mAttackSpeed; } set { mAttackSpeed = value; } }

    // MonoBehaviour Functions -----------------------------------------------------------------------------------------------
    public void Awake()
    {

        mAnimator = gameObject.GetComponentInChildren<Animator>();
        Assert.IsNotNull(mAnimator, "[Weapon] Dont have an animtor");                                                          //|--- [SAFTY]: Check to see if Animator is null

        rb = gameObject.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "[Weapon] Dont have rigidbody2D");                                                                //|--- [SAFTY]: Check to see is there a rigidbody2D
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        Assert.IsNotNull(boxCollider, "[Weapon] Dont have boxCollider");                                                       //|--- [SAFTY]: Check to see is there a collider

        Assert.IsNotNull(pickUpCollider, "[Weapon] Dont have pickUpCollider");                                                 //|--- [SAFTY]: Check to see is there a collider
        Assert.IsNotNull(inlevelBody_spriteRender, "[Weapon] Dont have inlevelBody_spriteRender");                             //|--- [SAFTY]: Check to see is there a inlevelBody_spriteRender

        _localLevelManagerTransform = GameObject.Find("LocalLevelManager").gameObject.transform;                               //|--- [INIT]: Get the global transform from LocalLevelManager gameObject
        Assert.IsNotNull(_localLevelManagerTransform, "[Weapon] Doesnt found localLevelManager");                              //|--- [SAFTY]: Check to see is there a collider

        switch (mGrade)
        {
            case WeaponGrade.Common:
                inlevelBody_spriteRender.color = Color.white;
                break;
            case WeaponGrade.Rare:
                inlevelBody_spriteRender.color = Color.blue;
                break;
            case WeaponGrade.Legendary:
                inlevelBody_spriteRender.color = Color.yellow;
                break;
            default:
                break;
        }

        Assert.AreNotEqual(mMoves.Length, 0, "[Weapon] moves not initialized");

        mElement = GetComponent<ElementalAttributes>();
        if (mElement == null)
            mElement = new ElementalAttributes();

        Assert.IsNotNull(mElement);
        for (int i = 0; i < mMoves.Length; ++i)
        {
            mMoves[i].Load(gameObject.GetComponent<Weapon>(), mAnimator, i, mElement);
        }
    }


    void FixedUpdate()
    {
        if(mCurrentMoveIndex != -1)
        {
            mMoves[mCurrentMoveIndex].Update(Time.deltaTime);
            if (mMoves[mCurrentMoveIndex].IsFinish())
            {
                mCurrentMoveIndex = 0;
            }
        }
    }

    void LateUpdate()
    {
        if ((mAnimator.gameObject.activeSelf)/*&&(mMoves[mCurrentMoveIndex].GetMoveCurrentTimeSliceType() == MoveTimeSliceType.Type_Reset)*/)
        {
            mAnimator.SetInteger("State", 0);
            mAnimator.SetInteger("ToNextCombo", -1);
            mAnimator.SetBool("IsReseting", false);
        }
    }

    // Self-define functions --------------------------------------------------------------------------------------------------
    public AttacState GetAttackState()
    {
        AttacState ret = AttacState.State_NonParriable;
        switch (mType)
        {
            case WeaponType.Melee:
                ret = mMoves[mCurrentMoveIndex].GetMoveCurrentTimeSliceType() == MoveTimeSliceType.Type_Parryable ? AttacState.State_Parriable : AttacState.State_NonParriable;
                break;
            case WeaponType.Range:
                ret = AttacState.State_NonParriable;
                break;
            default:
                break;
        }
        return ret;
    }

    public void Attack(bool isOnGournd, float targetPosX = float.NegativeInfinity, float targetPosY = float.NegativeInfinity, float targetPosZ = float.NegativeInfinity)    // This function is for all the previous work which dont have the targetPos paramater
    {
        Vector3 targetPos = new Vector3(targetPosX, targetPosY, targetPosZ);
        
        Attack(isOnGournd, targetPos);
    }

    public void Attack(bool isOnGournd, Vector3 targetPos)
    {
        mTargetPos = targetPos;
        var currentMove = mMoves[mCurrentMoveIndex];
        if (!currentMove.IsActive())
        {
            if (isOnGournd)
            {
                mAnimator.SetBool("IsOnGround", true);
                mCurrentMoveIndex = mAirMoveIndex + 1;
                mMoves[mCurrentMoveIndex].Enter();
            }
            else
            {
                mAnimator.SetBool("IsOnGround", false);
                mCurrentMoveIndex = mAirMoveIndex;
                mMoves[mCurrentMoveIndex].Enter();
            }

        }
        else
        {
            if (currentMove.GetMoveCurrentTimeSliceType() == MoveTimeSliceType.Type_Transition)
            {
                mCurrentMoveIndex = currentMove.GetNextTransitionMoveIndex();
                currentMove.Exit();
                mMoves[mCurrentMoveIndex].Enter();
            }
        }

    }

    public void WeaponReset()
    {
        mAnimator.SetBool("IsReseting", true);
        foreach (var move in mMoves)
        {
            move.Reset();
        }
    }

    public float GetCurrentAttackTime()
    {
        return mMoves[mCurrentMoveIndex].mAttackSpendTime;
    }


    public void Picked(GameObject owner, Vector2 position)
    {
        gameObject.tag = owner.tag;

        Character chara = owner.GetComponent<Character>();
        mOwnerElement = chara.mElement;
        Assert.IsNotNull(mOwnerElement);

        var parry = owner.GetComponent<ParryAttackable>();
        if (parry)
            parry.mParryCooldown = mParryCD;

        InLevelBody.gameObject.SetActive(false);
        boxCollider.enabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        gameObject.transform.SetParent(owner.transform);
        gameObject.transform.position = position;
        gameObject.transform.rotation = new Quaternion();
    }

    public void ThrowAway(Vector2 directionForce)
    {
        gameObject.transform.SetParent(_localLevelManagerTransform);

        rb.isKinematic = false;
        boxCollider.enabled = true;
        InLevelBody.gameObject.SetActive(true);
        rb.AddForce(directionForce, ForceMode2D.Impulse);

        gameObject.tag = "PickUp";
    }

    private void FixUpdate()
    {
        
    }

    //----------------------------------------------------------------------------------//|
    //- Mingzhuo Zhang Edit ------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>() != null)
            {
                if (gameObject.CompareTag("PickUp"))
                other.GetComponent<Player>().PickWeaopn(gameObject.GetComponent<Weapon>());
            }
        }
    }
    //----------------------------------------------------------------------------------//|
    //- End Edit -----------------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
}