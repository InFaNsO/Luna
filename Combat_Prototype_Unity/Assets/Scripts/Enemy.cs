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

    new public void Awake()
    {
        base.Awake();
        Assert.IsNotNull(GetComponent<CapsuleCollider>(), "[Enemy] Dont have collider");     //|--- [SAFTY]: Check to see is there a Collider
        mWeapon.gameObject.tag = "Enemy";
    }

    public void Attack()
    {
        if (mWeapon != null)
            mWeapon.Attack();
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
        if (mCurrentHealth <= 0.0f)
        {
            Die();
        }
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
}
