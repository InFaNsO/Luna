using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


//---------------This may need to put in some where like <combat system> -----------|
public enum Element                                                               //|
{                                                                                 //|
    Luna = 0,                                                                     //|
    Fire,                                                                         //|
    Liquir                                                                        //|
}                                                                                 //|
//---------------This may need to put in some where like <combat system> -----------|

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

public abstract class Weapon : MonoBehaviour
{
    protected Element mElement;
    protected WeaponGrade mGrade;
    protected WeaponType mType;

    [SerializeField] protected string mName;
    [SerializeField] protected int mDamage;
    [SerializeField] protected float mAttackSpeed; // wait how many second to next attack
    protected float mAttackSpeedCounter;

    [SerializeField] protected Bullet mBullet;
    protected Vector3 mFirePositionOffSet;

    public void Awake()
    {
        Assert.IsNotNull(mBullet, "[Weapon] Dont have bullet");     //|--- [SAFTY]: Check to see is there a Collider
    }


    public abstract void Attack();

}