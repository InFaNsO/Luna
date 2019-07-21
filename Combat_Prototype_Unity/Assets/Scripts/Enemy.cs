using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charactor : MonoBehaviour     
{
    float mCurrentHealth;
    float mMaxHealth;
    public abstract void GetHit(float dmg);        
}                                                   

public class EnemyCharactor : Charactor
{
    [SerializeField] Weapon mWeapon;

    public void Attack()
    {

    }

    public void Die()
    {
        //effect
        Debug.Log("enemy destory");

        // [Maybe] Update game Score

        gameObject.SetActive(false);
    }

    override public void GetHit(float dmg) 
    {
        
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
