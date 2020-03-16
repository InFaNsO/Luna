using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * New Enemy class based on ECS
 */

public class Enemy : Character
{
    //[SerializeField] AI_Zone mZone;
    //Physics Triggers
    public CircleCollider2D mNodeRange;
    public CircleCollider2D mAttackRange;
    public CircleCollider2D mPlayerVisibilityRange;


    [HideInInspector] public AI_Zone mZone;

    //AI
    [HideInInspector] public Agent2D mAgent;
    [HideInInspector] public StateMachine mStateMachine;
    [HideInInspector] public PathFinding mPathFinding;
    [HideInInspector] public SteeringModule mSteering;

    [HideInInspector] public bool IsRunning = false;

    [HideInInspector] public Weapon mWeapon;
    protected bool mIsDropping = false;
    [SerializeField] protected GameObject mDropPrefbs = null;

    //private Animator mAnimator;
    [HideInInspector] public bool mIsStuned = false;
    [HideInInspector] float mStunCounter;

    public EnemyTypes MyType = EnemyTypes.none;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        var wep = GetComponentInChildren<Weapon>();
        if (wep != null)
        {
            mWeapon = wep;
            mWeapon.Picked(gameObject, gameObject.transform.position); // second argument should be the [weapon position] as a individual variable in future
        }
    }
    protected override void Awake()
    {
        base.Awake();
        mAgent = GetComponent<Agent2D>();

        mRigidBody = GetComponent<Rigidbody2D>();
        mStateMachine = GetComponentInChildren<StateMachine>();
        mSteering = GetComponentInChildren<SteeringModule>();

        if (mDropPrefbs != null)
            mIsDropping = true;

        if (MyType == EnemyTypes.BatBot)
            mRigidBody.gravityScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (mIsStuned)
        {
            if (mStunCounter <= 0.0f)
            {
                mIsStuned = false;
            }
            mStunCounter -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != gameObject.tag)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                //1. Bullet.ElementAttribute = Player.ElementAttribute + Weapon.ElementAttribute                \\ TODO
                //2. Bullet.ApplyDamage()                                                                       \\ TODO
                GetHit(bullet.mElement);
                mLastGotHitPosition = other.gameObject.transform.position;              //|
                GetHit(bullet.Damage, other.tag, mLastGotHitPosition);
                var rb = GetComponent<Rigidbody2D>();
                if (rb)
                {
                    var direction = other.gameObject.transform.position.x - transform.position.x;
                    if (direction > 0.0f)
                    {
                        rb.AddForce(new Vector2(-knockBackX, knockBackY));
                    }
                    else if(direction < 0.0f)
                    {
                        rb.AddForce(new Vector2(knockBackX, knockBackY));
                    }
                }
            }
        }
    }


    public void GetHit(float dmg, string tag, Vector3 hitPosition)
    {
        GetHit(dmg, hitPosition);

        if ( mWeapon && (mWeapon.GetAttackState() == AttacState.State_Parriable) && (tag == "Parry"))
        {
            GetStun(1.5f);
        }
    }

    public void GetStun(float stunHowLong)
    {
        mIsStuned = true;
        mStunCounter = stunHowLong;

        mWeapon.WeaponReset();

        //SetAnimator(EnemyAnimation.Stun);
    }

    override public void Die()
    {
        //effect
        Debug.Log("enemy destory");

        if (mIsDropping)
        {
            //spwn inventory
            Instantiate(mDropPrefbs, transform.position, transform.rotation);

        }

        // [Maybe] Update game Score
        // [Maybe] Change anmation state

        //--------------------------//|
        Destroy(gameObject);        //|--- Set this by routin function in future: For giving time to died animation 
        //--------------------------//|
    }

    public void Attack()
    {
        mWeapon.Attack(isGrounded);
        //SetAnimator(EnemyAnimation.Attack);
    }
    //for future
    /*
     * enum EnemyAnimation
        {
            None = 0,
            ToIdel,
            Stun,
            Attack,
        }
    void SetAnimator(EnemyAnimation animationType)
    {
        switch (animationType)
        {
            case EnemyAnimation.None:
                break;
            case EnemyAnimation.ToIdel:
                // mAnimator.SetInteger("Condition", 0);
                break;
            case EnemyAnimation.Stun:
                // mAnimator.SetInteger("Condition", 10);
                break;
            case EnemyAnimation.Attack:
                // mAnimator.SetInteger("Condition", 8);
                break;
            default:
                break;
        }
    }

    public void LateUpdate()
    {
        SetAnimator(EnemyAnimation.ToIdel);
    }
    */


    public bool IsNearTarget(float range)
    {
        if (Vector3.Distance(transform.position, mAgent.mTarget) < range)
        {
            return true;
        }
        return false;
    }
}
