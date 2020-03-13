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

    public Bullet mParryCollider;
    Player mOwner;

    public float mParryCooldown = 1.0f;
    float mParryCounter = 0.0f;
    bool mParrySignal = false;

    // Getter & Setter
    public bool IsParrying() { return mParryCounter < mParryCooldown; }
    public int ParryCooldown { set { mParryCooldown = value; } }

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
        mParryCounter = mParryCooldown;
    }

    void Update()
    {
        if (mParryCounter < mParryCooldown)
            mParryCounter += Time.deltaTime;
        else
            mParrySignal = true;

        // Take this out when set up with controller

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    Parry();
        //}

        //-------------------------------------------
    }

    public void Parry()
    {
        if (mParryCounter >= mParryCooldown && mOwner.CurrentWeapon != null)
        {
            mOwner.CurrentWeapon.WeaponReset();
            Bullet newBullet = Object.Instantiate(mParryCollider, new Vector3(0, 0, 0), Quaternion.identity);
            newBullet.Fire("Parry", 0, mOwner.transform.position, mOwner.transform.right, WeaponType.Melee);
            mParryCounter = 0.0f;
        }
    }

    public bool GetParrySignalForAnimator()
    {
        bool ret = false;
        if (IsParrying())
        {
            ret = mParrySignal;
            mParrySignal = false;
        }
        return ret;
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
