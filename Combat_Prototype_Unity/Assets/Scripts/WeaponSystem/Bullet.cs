using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string mName;

    [SerializeField] private float mDamage;
    public float Damage { get { return mDamage; } set { mDamage = value; } }

    [SerializeField] private Element mElement = Element.Luna;
    public Element Element { get { return mElement; } }

    [SerializeField] private float mSpeed = 1.0f;

    [SerializeField] private float mLifeTime = 10.0f;
    private float mLifeTimeCounter;

    private bool mIsMelee;

    private Vector3 mFireDirection;

    public void Awake()
    {
        Assert.IsNotNull(GetComponent<CapsuleCollider>(), "[Bullet] Dont have collider");       //|--- [SAFTY]: Check to see is there a Collider
        Assert.AreNotEqual(mLifeTime > 0.0f, false, "[Bullet] Dont have lifeTime");             //|--- [SAFTY]: Check to see is there a 0 life time
    }

    public void Fire(string tag, float dmg, Vector3 initPos, Vector3 direction, WeaponType isMelee)
    {
        gameObject.tag = tag;
        mDamage = dmg;
        mFireDirection = direction.normalized;
        gameObject.transform.position = initPos;
        mIsMelee = (isMelee == WeaponType.Melee);
        mLifeTimeCounter = mLifeTime;

        gameObject.SetActive(true);
    }

    public void Update()
    {
        if (mLifeTimeCounter > 0.0f)
        {
            gameObject.transform.position += mFireDirection * mSpeed;
            mLifeTimeCounter -= Time.deltaTime;
        }
        else
        {
            Die();
        }

    }

    public void Die()
    {
        gameObject.SetActive(false);
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

    private void EarlyUpdate()
    {
        
    }

    private void LateUpdate()
    {
        if (mIsMelee)
            Die();
    }
}
