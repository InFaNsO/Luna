//===========================================================================================================================================================
// Filename:	Bullet.cs
// Created by:	Mingzhuo Zhang
// Description:	Store basic data structure for bullet game object
//===========================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : MonoBehaviour
{
    // Members --------------------------------------------------------------------------------------------------------------
    [SerializeField] private string mName;
    [SerializeField] private float mDamage;
    [SerializeField] private float mSpeed = 1.0f;
    [SerializeField] private float mLifeTime = 10.0f;

    private float mLifeTimeCounter;
    private bool mIsMelee;
    private Vector3 mFireDirection;

    // Getter & Setter ------------------------------------------------------------------------------------------------------
    public float Damage { get { return mDamage; } set { mDamage = value; } }
    //public Element Element { get { return mElement; } set { mElement = value; } }
    public ElementalAttributes mElement;

    int exitFrameCount = 1;
    // MonoBehaviour Functions -----------------------------------------------------------------------------------------------
    public void Awake()
    {
        Assert.IsNotNull(GetComponent<Collider2D>(), "[Bullet] Dont have collider");            //|--- [SAFTY]: Check to see is there a Collider
        Assert.AreNotEqual(mLifeTime > 0.0f, false, "[Bullet] Dont have lifeTime");             //|--- [SAFTY]: Check to see is there a 0 life time

        mElement = GetComponent<ElementalAttributes>();
        if (mElement == null)
            mElement = new ElementalAttributes();
        Assert.IsNotNull(mElement);
    }

    public void Update()
    {
        if (mLifeTimeCounter > 0.0f)
        {
            gameObject.transform.position += mFireDirection * mSpeed * Time.deltaTime;
            mLifeTimeCounter -= Time.deltaTime;
        }
        else
        {
            Die();
        }

    }

    private void FixedUpdate()
    {
        if (mIsMelee && exitFrameCount == 0)
        {
            Die();
            exitFrameCount = 1;
        }
        else if(mIsMelee)
        {
            exitFrameCount--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag != gameObject.tag) && (gameObject.tag != "Parry"))
        {
            if ((collision.GetComponent<Character>() != null) || ( !mIsMelee && collision.CompareTag("Ground")))
            {
                Die();
            }
        }
    }

    // Self-define functions --------------------------------------------------------------------------------------------------
    public void Fire(string tag, float dmg, Vector3 initPos, Vector3 direction, WeaponType weaponType)
    {
        //mElement.RefreshStats();


        gameObject.tag = tag;
        mDamage = dmg;
        mFireDirection = direction.normalized;
        gameObject.transform.position = initPos;
        mIsMelee = (weaponType == WeaponType.Melee);
        mLifeTimeCounter = mLifeTime;

        if (weaponType == WeaponType.Melee)
        {
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            mSpeed = 0.0f;
        }
        gameObject.SetActive(true);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
