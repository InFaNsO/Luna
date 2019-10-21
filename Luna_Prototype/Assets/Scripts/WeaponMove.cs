using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class WeaponMove
{
    [SerializeField] protected int mDamage;
    [System.NonSerialized] public float mAnimationPlayTime;
    [System.NonSerialized] public float mAttackSpendTime;
    [SerializeField] protected float mAttackSpeedMutiplier = 1.0f; // wait how many second to next attack
    protected float mAttackSpeedCounter;
    [SerializeField] MoveContext mMoveContext;
    [SerializeField] int[] mToMoveId; // Store the move id that this move can go to
    [SerializeField] protected Bullet mBullet;
    [SerializeField] protected Transform mFirePosition;
    [SerializeField] protected AnimationClip mAnimationClip;
    private Weapon mWeapon;
    
    [System.NonSerialized] public Animator mWeaponAnimator;

    private Element mElement;

    // Getter & Setter -------------------------------------------------------------------------------------------------------
    public float AttackSpeed { get { return mAttackSpendTime; } set { mAttackSpendTime = value; } }
    public Element Element { set { mElement = value; } }

    public void Load(Weapon weapon, Animator animator, Element element = Element.Luna)
    {
        mWeapon = weapon;

        Assert.IsNotNull(mBullet, "[Weapon] Dont have bullet");                                                                //|--- [SAFTY]: Check to see is there a bullet prefeb
        Assert.AreNotEqual(mAttackSpeedMutiplier, 0.0f, "[Weapon] mAttackSpeedMutiplier not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        Assert.IsNotNull(mAnimationClip, "[Weapon] mAnimationClip not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized

        mWeaponAnimator = animator;

        mAnimationPlayTime = mAnimationClip.length;
        mAttackSpendTime = mAnimationPlayTime * mAttackSpeedMutiplier;

        mMoveContext.Load(mAttackSpendTime, mToMoveId.Length);

        mElement = element;
    }

    public void Enter()
    {
        mWeaponAnimator.SetInteger("State", 1);
        mMoveContext.Active = true;
    }

    public void Exit()
    {
        mWeaponAnimator.SetInteger("ToNextCombo", mMoveContext.GetTransitionSliceCount());
        mMoveContext.Reset();
    }

    public void Update(float deltaTime)
    {
        mMoveContext.Update(Time.deltaTime);

        if (mMoveContext.Active)
        {
            
            if (mMoveContext.GetCurrentTimeSliceType() == MoveTimeSliceType.Type_Parryable)
            {
                //TODO :: make weapon become parrable
            }
            else if ((mMoveContext.GetCurrentTimeSliceType() == MoveTimeSliceType.Type_DealtDmg) && (mMoveContext.mHaveDealtDmg == false))
            {
                ShootBullet();
                mMoveContext.mHaveDealtDmg = true;
                //gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }
        }
    }

    public bool IsFinish()
    {
        return (!mMoveContext.Active);
    }

    public void ShootBullet()
    {
        Bullet newBullet = Object.Instantiate(mBullet, new Vector3(0, 0, 0), Quaternion.identity);
        newBullet.Fire(mWeapon.tag, mDamage, mWeapon.transform.TransformPoint(mWeapon.transform.localPosition + mFirePosition.localPosition), mWeapon.transform.right, mWeapon.mType);
        Debug.Log("attacked");
    }

    public MoveTimeSliceType GetMoveCurrentTimeSliceType()
    {
        return mMoveContext.GetCurrentTimeSliceType();
    }

    public int GetNextTransitionMoveIndex()
    {
        return mToMoveId[mMoveContext.GetTransitionSliceCount()];
    }

    public bool IsActive()
    {
        return mMoveContext.Active;
    }

    public void Reset()
    {
        mMoveContext.Reset();
    }
}
