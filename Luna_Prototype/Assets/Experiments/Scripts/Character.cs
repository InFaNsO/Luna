using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [HideInInspector] public Health myHealth;
    [HideInInspector] public Rigidbody2D mRigidBody;

    [SerializeField]
    public float mJumpStrength = 2.0f;
    [SerializeField]
    public float mMovementSpeed = 2.0f;

    //Weapon Minghuo
    [HideInInspector] public bool isGrounded;
    public Transform mAttackMomentumPos;

    //Elemental Parts
    //public ElementalAttributes elementalAttributes;
    [HideInInspector] public ElementalAttributes element;

    public float knockBackX = 1.0f;
    public float knockBackY = 1.0f;

    public ParticleSystem mGetHitParticle;
    protected Vector3 mLastGotHitPosition;
    //Keep Movement Track
    [HideInInspector] public bool IsFacingLeft = false;

    private HealthBar mHealthBar;

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
        mHealthBar = GetComponentInChildren<HealthBar>();
    }

    protected virtual void Start()
    {
        myHealth = GetComponent<Health>();
        isGrounded = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!myHealth.IsAlive())
            Die();

        mHealthBar.UpdateHealthBar(myHealth.GetHealth() / myHealth.GetMaxHealth());
    }


    public virtual void GetHit(float dmg)
    {
        myHealth.TakeDamage(dmg);
    }
    public virtual void GetHit(ElementalAttributes dmg)
    {
        element.ApplyDamage(this, false);
    }

    protected void GetHit(float dmg, Vector3 hitPosition)
    {
        GetHit(dmg);
        if (!mGetHitParticle)
            return;

        ParticleSystem newParticle = Instantiate(mGetHitParticle, hitPosition, Quaternion.identity);
        newParticle.Play();
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 180.0f);
        IsFacingLeft = !IsFacingLeft;
    }

    public virtual void Die()
    {

    }
}
