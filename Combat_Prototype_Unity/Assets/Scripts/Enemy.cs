using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charactor : MonoBehaviour     
{
    protected float mCurrentHealth;
    protected float mMaxHealth;
    public abstract void GetHit(float dmg);
}

public class EnemyCharactor : Charactor
{
    [SerializeField] Weapon mWeapon;

    public void Attack()
    {
        mWeapon.Attack();
    }

    public void Die()
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

    void OnTriggerEnter2D(Collider other)
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
