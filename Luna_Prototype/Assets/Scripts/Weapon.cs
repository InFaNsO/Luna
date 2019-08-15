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
    [SerializeField] protected Transform mFirePosition;

    [SerializeField] protected GameObject InLevelBody;
    [SerializeField] protected CircleCollider2D pickUpCollider;
    [SerializeField] protected SpriteRenderer inlevelBody_spriteRender;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    protected Animator mAnimator;

    bool mHaveAttack = false;

    private Transform _localLevelManagerTransform;

    // Getter & Setter -------------------------------------------------------------------------------------------------------
    public float AttackSpeed { get { return mAttackSpeed; } set { mAttackSpeed = value; } }

    // MonoBehaviour Functions -----------------------------------------------------------------------------------------------
    public void Awake()
    {
        Assert.IsNotNull(mBullet, "[Weapon] Dont have bullet");                                                                //|--- [SAFTY]: Check to see is there a bullet prefeb
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[0], 0.0f, "[Weapon] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[1], 0.0f, "[Weapon] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.AreEqual(mParryContext.mTimeSlicePropotion.Length, 2, "[Weapon] Parry context TimeSlice count should be 3");    //|--- [SAFTY]: Check to see if parry context TimeSlice count is 3

        mParryContext.TotalTime = mAttackSpeed;                                                                                //|--- [INIT]: Initliaze the total time cycle for parry system in ParryContext
        mParryContext.Reset();
        mBullet.Element = mElement;                                                                                            //|--- [INIT]: Initliaze the element of the bullet

        mAnimator = gameObject.GetComponentInChildren<Animator>();                                                                                  //|--- [INIT]: Initliaze the animator
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
    }

    void Update()
    {
        mParryContext.Update(Time.deltaTime);

        if (mParryContext.Active)
        {
            if (mParryContext.CurrentState == AttackState.AttackState_Parryable)
            {
                //gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            }
            else if ((mParryContext.CurrentState == AttackState.AttackState_NonParryable) && (mHaveAttack == false))
            {
                ShootBullet();
                mHaveAttack = true;
                //gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }
        }
    }

    void LateUpdate()
    {
        if (mAnimator.gameObject.activeSelf)
        {
            mAnimator.SetInteger("State", 0);
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
        Debug.Log("[Weapon] start attack 0");
        if (mParryContext.Active == false)
        {
            Debug.Log("[Weapon] start attack 1");
            mParryContext.Active = true;
            mHaveAttack = false;

            if (mAnimator.gameObject.activeSelf)
            {
                mAnimator.SetInteger("State", 1);
            }

            Debug.Log(mParryContext.Active);
        }

    }

    public void ShootBullet()
    {
        Bullet newBullet = Instantiate(mBullet, new Vector3(0, 0, 0), Quaternion.identity);
        newBullet.Fire(gameObject.tag, mDamage, gameObject.transform.TransformPoint(gameObject.transform.localPosition + mFirePosition.localPosition), gameObject.transform.right, mType);
        Debug.Log("attacked");
    }

    public void Picked(GameObject owner, Vector2 position)
    {
        gameObject.tag = owner.tag;

        InLevelBody.gameObject.SetActive(false);
        boxCollider.enabled = false;
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
                other.GetComponent<Player>().PickWeaopn(gameObject.GetComponent<Weapon>());
            }
        }
    }
    //----------------------------------------------------------------------------------//|
    //- End Edit -----------------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
}