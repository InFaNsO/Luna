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

    [SerializeField] private ParticleSystem mParticle;

    private float mLifeTimeCounter;
    private bool mIsMelee;
    private Vector3 mFireDirection;

    // Getter & Setter ------------------------------------------------------------------------------------------------------
    public float Damage { get { return mDamage; } set { mDamage = value; } }
    //public Element Element { get { return mElement; } set { mElement = value; } }
    public ElementalAttributes mElement;

    int exitFrameCount = 1;

    public Character mOwner;
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
    public void Start()
    {
        if (mElement == null)
           mElement = GetComponent<ElementalAttributes>();
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
        if (collision.gameObject.tag == "Util")
            return;

        if ((collision.gameObject.tag != gameObject.tag) && (gameObject.tag != "Parry") && (collision.gameObject.tag != "Parry"))
        {
            if ((collision.GetComponent<Character>() != null) || ( !mIsMelee && collision.CompareTag("Ground")))
            {
                Die();
            }
            ParticleSystem newParticle = Instantiate(mParticle, transform.position, Quaternion.identity);
            newParticle.Play();
        }
        else if(collision.gameObject.tag == "Parry" /*&& !mIsMelee*/)
        {
            ParryAttackable parryObj = collision.gameObject.GetComponentInParent<ParryAttackable>();
            ParryAttackable.ParryLevel parryLevel = parryObj.GetParryLevel(gameObject.transform.position);

            parryObj.AdjustFacing(transform.position.x);

            if (!mIsMelee)
            {
                gameObject.tag = collision.gameObject.transform.parent.tag;
            }

            parryObj.ReduceStamine(mDamage);

            switch (parryLevel)
            {
                case ParryAttackable.ParryLevel.poor:
                    {
                        Vector3 flipDir = Vector3.Normalize(mFireDirection + new Vector3(0.0f,1.0f,0.0f));
                        mFireDirection = flipDir;
                    }
                    break;
                case ParryAttackable.ParryLevel.great:
                    {
                        Vector3 flipDir = Vector3.Normalize(gameObject.transform.position - collision.gameObject.transform.position);
                        mFireDirection = flipDir;
                    }
                    break;
                case ParryAttackable.ParryLevel.perfect:
                    if (mOwner)
                    {
                        Vector3 flipDir = Vector3.Normalize(mOwner.transform.position - gameObject.transform.position);
                        mFireDirection = flipDir;
                    }
                    else
                    {
                        Vector3 flipDir = Vector3.Normalize(gameObject.transform.position - collision.gameObject.transform.position);
                        mFireDirection = flipDir;
                    }
                    break;
                default:
                    break;
            }

            
        }
    }

    // Self-define functions --------------------------------------------------------------------------------------------------
    public void Fire(string tag, float dmg, Vector3 initPos, Vector3 direction, WeaponType weaponType, Character owner = null)
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
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            mSpeed = 0.0f;
        }
        gameObject.SetActive(true);

        if (owner != null)
            mOwner = owner;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
