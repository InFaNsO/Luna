using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttackable : MonoBehaviour
{
    public Bullet mParryCollider;
    Player mOwner;

    public float mParryCooldown = 1.0f;
    float mParryCounter = 0.0f;

    // Getter & Setter
    public bool IsParrying() { return mParryCounter < mParryCooldown; }
    public int ParryCooldown { set { mParryCooldown = value; } }

    public 
    void Awake()
    {
        mOwner = GetComponent<Player>();
        mParryCounter = mParryCooldown;
    }

    void Update()
    {
        if(mParryCounter < mParryCooldown)
            mParryCounter += Time.deltaTime;

        // Take this out when set up with controller

        if(Input.GetKeyDown(KeyCode.P))
        {
            Parry();
        }

        //-------------------------------------------
    }

    public void Parry()
    {
        if (mParryCounter >= mParryCooldown)
        {
            mOwner.CurrentWeapon.WeaponReset();
            Bullet newBullet = Object.Instantiate(mParryCollider, new Vector3(0, 0, 0), Quaternion.identity);
            newBullet.Fire("Parry", 0, mOwner.transform.position, mOwner.transform.right, WeaponType.Melee);
            mParryCounter = 0.0f;
        }
    }
}
