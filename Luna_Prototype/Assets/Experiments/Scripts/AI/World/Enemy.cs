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


    public AI_Zone mZone;

    //AI
    public Agent2D mAgent;
    public StateMachine mStateMachine;
    public PathFinding mPathFinding;
    public SteeringModule mSteering;

    public bool IsRunning = false;

    public Weapon mWeapon;
    [SerializeField] protected bool mIsDropping;
    [SerializeField] protected Key mDropPrefbs;

    //private Animator mAnimator;
    public bool mIsStuned = false;
    float mStunCounter;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsStuned != true)
        {
            //Do AI here

        }
        else
        {
            if (mStunCounter <= 0.0f)
            {
                mIsStuned = false;
                // Do animation
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
                GetHit(bullet.Damage, other.tag);
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

    public void GetHit(float dmg, string tag)
    {
        myHealth.TakeDamage(dmg);

        if ((mWeapon.GetAttackState() == AttacState.State_Parriable) && (tag == "Parry"))
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
            Instantiate(mDropPrefbs);

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
