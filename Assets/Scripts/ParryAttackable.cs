using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttackable : MonoBehaviour
{
    public enum ParryLevel
    {
        poor = 0,
        great = 1,
        perfect = 2
    }

    Player mOwner;

    bool mParrySignal = false;
    bool mStartParry = false;
    bool mIsParryHit = false;

    [System.Serializable]
    public struct ParryContext
    {
        public float poor_check_distance;
        public float great_check_distance;
        public float perfect_check_distance;
    }

    public ParryContext mContext;

    void Awake()
    {
        mOwner = GetComponent<Player>();
    }

    void Update()
    {

        // Take this out when set up with controller

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    Parry();
        //}

        //-------------------------------------------
    }

    void LateUpdate()
    {
        mStartParry = false;
        mIsParryHit = false;
        if (mOwner.CurrentWeapon == null || !mOwner.stamina.IsStaminaSufficient(mOwner.CurrentWeapon.mBaseStaminaCostForParry) || !mOwner.isGrounded)
        {
            StopParry();
        }
    }
    public void Parry()
    {
        if (mOwner.CurrentWeapon != null && mOwner.stamina.IsStaminaSufficient(mOwner.CurrentWeapon.mBaseStaminaCostForParry) && mOwner.isGrounded)
        {
            mOwner.CurrentWeapon.WeaponReset();
            mOwner.CurrentWeapon.parryCollider.enabled = true;
            mParrySignal = true;
            mStartParry = true;
        }
    }

    public void StopParry()
    {
        mOwner.CurrentWeapon.parryCollider.enabled = false;
        mParrySignal = false;
    }

    public bool IsParrying()
    {
        return mParrySignal;
    }

    public bool IsParryHit()
    {
        return mIsParryHit;
    }

    public bool IsStartParrying()
    {
        return mStartParry;
    }

    public void ReduceStamine(float receiveDmg)
    {
        mOwner.stamina.UseStamina_Overflow(mOwner.CurrentWeapon.GetParryCost(receiveDmg));
        mIsParryHit = true;
    }

    public ParryLevel GetParryLevel(Vector3 bulletPosition)
    {
        float currentDistance = Vector3.Distance(gameObject.transform.position, bulletPosition);

        if (currentDistance <= mContext.perfect_check_distance)
            return ParryLevel.perfect;
        if (currentDistance <= mContext.great_check_distance)
            return ParryLevel.great;
        if (currentDistance <= mContext.poor_check_distance)
            return ParryLevel.poor;
        return ParryLevel.poor;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f,0.3f);
        Gizmos.DrawSphere(transform.position, mContext.poor_check_distance);
        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.3f);
        Gizmos.DrawSphere(transform.position, mContext.great_check_distance);
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
        Gizmos.DrawSphere(transform.position, mContext.perfect_check_distance);
    }
}
