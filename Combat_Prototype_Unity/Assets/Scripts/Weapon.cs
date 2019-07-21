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

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] private string mName;
    [SerializeField] private float mDamage;
    public float Damage{ get { return mDamage; } }
    [SerializeField] private Element mElement;

    [SerializeField] private float mSpeed;
    [SerializeField] private float mLifeTime;
    private float mLifeTimeCounter;

    [SerializeField] private bool mIsMelee;

    private GameObject parentWeapon;
    private Vector3 mFireDirection;
    
    public void Awake()
    {
        Assert.IsNotNull(GetComponent<BoxCollider>(), "[Bullet] Dont have collider");     //|--- [SAFTY]: Check to see is there a Collider
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != gameObject.tag)
        {
            if (collision.GetComponent<Charactor>() != null)
            {
                Die();
            }
        }
    }

    public abstract void Fire(float dmg, float speed, Vector3 initPos, Vector3 direction);
    public abstract void Update();
    public abstract void Die();

    private void LateUpdate()
    {
        if (mIsMelee)
            Die();
    }
}

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
    [SerializeField] private Element mElement;
    [SerializeField] private WeaponGrade mGrade;
    [SerializeField] private WeaponType mType;

    [SerializeField] private string mName;
    [SerializeField] private int mDamage;
    [SerializeField] private float mAttackSpeed; // wait how many second to next attack
    private float mAttackSpeedCounter;

    [SerializeField] private Bullet mBullet;
    [SerializeField] private Vector3 mFirePosition;

    public void Awake()
    {
        Assert.IsNotNull(mBullet, "[Weapon] Dont have bullet");     //|--- [SAFTY]: Check to see is there a Collider
    }


    public abstract void Attack();

}