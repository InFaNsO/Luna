using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Health myHealth;
    public Rigidbody2D mRigidBody;

    [SerializeField]
    public float mJumpStrength = 2.0f;
    [SerializeField]
    public float mMovementSpeed = 2.0f;

    //Weapon Minghuo
    public bool isGrounded;
    public Transform mAttackMomentumPos;

    //Elemental Parts
    //public ElementalAttributes elementalAttributes;
    public ElementalAttributes element;

    public float knockBackX = 1.0f;
    public float knockBackY = 1.0f;

    protected virtual void Awake()
    {
        if (transform.Find("AttackMomentumPos") != null)
        {
            mAttackMomentumPos = transform.Find("AttackMomentumPos").transform;                              //|--- [Mingzhuo Zhang] Edit: finding the mAttackMomentumPos
        }
        else
        {
            mAttackMomentumPos = new GameObject().transform;
            mAttackMomentumPos.SetParent(gameObject.transform);
            mAttackMomentumPos.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }

        element = GetComponent<ElementalAttributes>();
        if (element == null)
            element = new ElementalAttributes();

        //element.Randomize();        //Empty for now
    }

    protected virtual void Start()
    {
        myHealth = GetComponent<Health>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myHealth.IsAlive())
            Die();
    }


    public virtual void GetHit(float dmg)
    {
        myHealth.TakeDamage(dmg);
    }
    public virtual void GetHit(ElementalAttributes dmg)
    {
        element.ApplyDamage(this, false);
    }
    

    public virtual void Die()
    {

    }
}
