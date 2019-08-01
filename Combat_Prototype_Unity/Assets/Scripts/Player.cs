using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Weapon mWeapon1;
    [SerializeField]
    private Weapon mWeapon2;
    [SerializeField]
    private Weapon mCurrentWeapon;

    [SerializeField]
    private bool mIsIFrameOn;
    [SerializeField]
    private float mIFrameCD;
    [SerializeField]
    private float mIFrameDuration;
    [SerializeField]
    private float mIFrameDistance;


    public float GetMoveSpeed()
    {
        return mMovementSpeed;
    }

    public float GetJumpStrength()
    {
        return mJumpStrength;
    }

    public float GetIFrameDistance()
    {
        return mIFrameDistance;
    }

    public void ObtainNewWeapon(Weapon newWeapon)
    {
        if(mWeapon1 == null)
        {
            mWeapon1 = newWeapon;
            mCurrentWeapon = mWeapon1;
        }
        else if(mWeapon2 == null)
        {
            mWeapon2 = newWeapon;
            mCurrentWeapon = mWeapon2;
        }
    }

    public void DropWeapon()
    {
        if(mCurrentWeapon == mWeapon1)
        {
            Debug.Log("Weapon 1 dropped");
            mWeapon1 = null;
            mCurrentWeapon = null;
        }
        else if(mCurrentWeapon == mWeapon2)
        {
            Debug.Log("Weapon 2 dropped");
            mWeapon2 = null;
            mCurrentWeapon = null;
        }
    }

    public void SwitchWeapon()
    {
        if(mCurrentWeapon == mWeapon1)
        {
            if(mWeapon2 != null)
            {
                mCurrentWeapon = mWeapon2;
                Debug.Log("Switch to Weapon 2");
            }
        }else if(mCurrentWeapon == mWeapon2)
        {
            if (mWeapon1 != null)
            {
                mCurrentWeapon = mWeapon1;
                Debug.Log("Switch to Weapon 1");
            }
        }
    }

    public float GetAttackSpeed()
    {
        return 1.0f;
            //mCurrentWeapon.AttackSpeed;
    }

    public bool Dodge()
    {
        if(mIFrameCD <= 0)
        {
            mIsIFrameOn = true;
            mIFrameDuration = 1;
            mIFrameCD = 4;
            return true;
        }
        return false;
    }

    override public void GetHit(float dmg)
    {
        Debug.Log("[Player] Player receives damage");
        if (mIsIFrameOn == false)
        {
            mCurrentHealth -= dmg;
        }

        if(mCurrentHealth <= 0)
        {
            Die();
        }
    }
    override public void Die()
    {
        Debug.Log("[Player] Player Dead, curr hp : " + mCurrentHealth + "max hp " + mMaxHealth);

        gameObject.SetActive(false);
    }

    public void Attack()
    {
        Debug.Log("[Player] Attack called");
        if ((mCurrentWeapon == null) && ((mWeapon1 != null) || (mWeapon2 != null)))
        {
            if(mWeapon1 != null)
            {
                mCurrentWeapon = mWeapon1;
                Debug.Log("Weapon 1 equipped");
                mCurrentWeapon.Attack();
            }
            else
            {
                mCurrentWeapon = mWeapon2;
                Debug.Log("Weapon 2 equipped");
                mCurrentWeapon.Attack();
            }
        }
        else if(mCurrentWeapon != null)
        {
            mCurrentWeapon.Attack();
        }
        else
        {
            Debug.Log("No Available weapon to attack.");
        }
    }

    new public void Awake()
    {
        base.Awake();
        mIsIFrameOn = true;
        mIFrameCD = 4.0f;
        mIFrameDuration = 1.0f;
        mMovementSpeed = 5.0f;
        mJumpStrength = 20.0f;
        mIFrameDistance = 150.0f;
}

    new public void Update()
    {
        //[TOFIX] in player move controller.cs ,  GetKeyUp vec3 should be (0,0,0) rather than (-1, 0, 0 )
        base.Update();
        mIFrameCD -= Time.deltaTime;
        mIFrameDuration -= Time.deltaTime;
        if(mIFrameDuration <= 0)
        {
            mIsIFrameOn = false;
        }
    }

    public void LateUpdate()
    {
    }

}
