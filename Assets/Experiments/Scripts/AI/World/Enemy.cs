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

    // Combat Info
    public Bullet mMeleeBullet;
    public float mMeleeDmg = 10.0f;
    public float mAttackSpeed = 1.0f;

    // Animation control
    public AttackBot_AnimationController mAnimationController;

    [HideInInspector] public AI_Zone mZone;

    //AI
    [HideInInspector] public Agent2D mAgent;
    [HideInInspector] public StateMachine mStateMachine;
    [HideInInspector] public PathFinding mPathFinding;
    [HideInInspector] public SteeringModule mSteering;

    [HideInInspector] public bool IsRunning = false;

    protected bool mIsDropping = false;
    [SerializeField] protected GameObject mDropPrefbs = null;

    //private Animator mAnimator;

    public EnemyTypes MyType = EnemyTypes.none;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
    new void Update()
    {
        base.Update();
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
                        rb.AddForce(new Vector2(-knockBackX, knockBackY),ForceMode2D.Impulse);
                    }
                    else if(direction < 0.0f)
                    {
                        rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Impulse);
                    }
                }
            }
        }
    }


    public void GetHit(float dmg, string tag, Vector3 hitPosition)
    {
        GetHit(dmg, hitPosition);
        GetStunInternal();
    }

    public new void GetStun(float stunHowLong)
    {
        GetStun(stunHowLong);
        //SetAnimator(EnemyAnimation.Stun);
    }

    override public void Die()
    {
        //effect
        Debug.Log("enemy destory");
        mAnimationController.GoDeathAnimation();
        base.mHealthBar.gameObject.SetActive(false);
        //_SFXGroup.PlaySFX("Death");
    }

    public void RealDie()
    {
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
        mAnimationController.GoAttackAnimation(1.0f / mAttackSpeed);
    }

    public void RealAttack()
    {
        Bullet newBullet = Object.Instantiate(mMeleeBullet, new Vector3(0, 0, 0), Quaternion.identity);
        newBullet.Awake();
        newBullet.Fire(gameObject.tag, mMeleeDmg, transform.position, Vector3.down, WeaponType.Melee);
    }

    private void GetStunInternal()
    {
        mIsStuned = true;
        mAnimationController.GoStunAnimation();

    }

    public void RestFromStun()
    {
        mIsStuned = false;
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;
        }
    }

    public bool IsNearTarget(float range)
    {
        if (Vector3.Distance(transform.position, mAgent.mTarget) < range)
        {
            return true;
        }
        return false;
    }
}
