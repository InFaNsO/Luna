using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Pathfinding;

public class MZEnemyBat : Character
{
    public float mMeleeAttackRange;
    public float mRangeAttackRange;
    public float mVisibilityRange;

    [HideInInspector] public AIPath aiPath;
    private GameObject mPlayer;
    public MZEnemy_AnimationController mAnimationController;

    protected bool mIsDropping = false;
    [SerializeField] protected GameObject mDropPrefbs = null;

    public enum States
    {
        none = -1,
        Idel = 0,
        goTo,
        rangeAttack,
        meleeAttack
    }

    [System.Serializable]
    public class IdleContext
    {
        public float idelTime = 2.0f;
        [HideInInspector] public float idelCounter = 0.0f;
        [HideInInspector] public float time;
        [HideInInspector] public Vector3 newPos;
    }
    [System.Serializable]
    public class GotoContext
    {
        public float attackDelay = 0.5f;
        [HideInInspector] public float delayCounter = 0.0f;
        [HideInInspector] public bool startDelay = false;
        public float gotoSpeed = 2.0f;
    }
    [System.Serializable]
    public class MeleeAttackContext
    {
        public Bullet mMeleeBullet;
        public int mMeleeDamage = 10;

        public float toIdelDelay = 3.0f;
        [HideInInspector] public float toIdelCounter = 0.0f;
        [HideInInspector] public bool isOnPlayerleft = false;
        [HideInInspector] public float yDifferent = 0.0f;
        [HideInInspector] public Vector3 destPos;
        [HideInInspector] public bool isAttacked = false;
        public float chargeSpeed = 5.0f;
    }
    [System.Serializable]
    public class RangeAttackContext
    {
        public Bullet mRangeBullet;
        public int mRangeDamage = 10;
        public float toIdelDelay = 3.0f;
        [HideInInspector] public float toIdelCounter = 0.0f;
        public float attackSpeed = 1.0f;
        [HideInInspector] public float attackSpeedCounter = 0.0f;
    }

    public IdleContext mIdleContext;
    public GotoContext mGotoContext;
    public RangeAttackContext mRangeAttackContext;
    public MeleeAttackContext mMeleeAttackContext;

    States mCurrentState;
    // Start is called before the first frame update
    
    new void Awake()
    {
        Assert.IsNotNull(mRangeAttackContext.mRangeBullet, "[Enemy_Bat] mRangeBullet is Null");
        Assert.IsNotNull(mMeleeAttackContext.mMeleeBullet, "[Enemy_Bat] mMeleeBullet is Null");
        Assert.AreNotEqual(0, mRangeAttackContext.attackSpeed);
        base.Awake();

        aiPath = GetComponent<AIPath>();
        mPlayer = GameObject.Find("Player");
        mRigidBody = GetComponent<Rigidbody2D>();

        mRigidBody.gravityScale = 0.0f;
        

        if (mDropPrefbs != null)
            mIsDropping = true;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (mIsStuned)
        {
            return;
        }

        switch (mCurrentState)
        {
            case States.none:
                break;
            case States.Idel:
                mIdleContext.time += Time.deltaTime * 5.0f;
                mIdleContext.newPos.x = transform.position.x + Mathf.Sin(mIdleContext.time * 2.0f) * 0.6f * Time.deltaTime;
                mIdleContext.newPos.y = transform.position.y + Mathf.Cos(mIdleContext.time * 2.0f) * 0.3f * Time.deltaTime;
                transform.position = mIdleContext.newPos;

                mIdleContext.idelCounter += Time.deltaTime;
                if (mIdleContext.idelCounter < mIdleContext.idelTime)
                    return;

                if (Vector2.Distance(transform.position, mPlayer.transform.position) <= mVisibilityRange)
                {
                        //if (Vector2.Distance(transform.position, mPlayer.transform.position) <= mRangeAttackRange)        //ready to attack
                        //    GoToRangeAttack();
                        //else                                    //Go to player
                            GoToGoto();
                }
                break;
            case States.goTo:
                if (aiPath.velocity.magnitude < 0.05f)
                {
                    mGotoContext.startDelay = true;
                }

                if (mGotoContext.startDelay)
                {
                    mGotoContext.delayCounter += Time.deltaTime;
                }

                if (mGotoContext.delayCounter >= mGotoContext.attackDelay)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                        GoToRangeAttack();
                    else
                        GoToMeleeAttack();
                }
                break;
            case States.rangeAttack:
                if (mRangeAttackContext.toIdelCounter >= mRangeAttackContext.toIdelDelay)
                {
                    GoToIdle();
                    break;
                }

                if (mRangeAttackContext.attackSpeedCounter >= mRangeAttackContext.attackSpeed)
                {
                    
                    mAnimationController.GoRangeAttackAnimation(1.0f / mRangeAttackContext.attackSpeed);

                    Vector3 dir = Vector3.Normalize(transform.position - mPlayer.transform.position);
                    //recoil force to make it looks good
                    //mRigidBody.velocity = Vector2.zero;
                    //mRigidBody.AddForce(dir * 100.0f * Time.deltaTime, ForceMode2D.Impulse);

                    mRangeAttackContext.attackSpeedCounter = 0.0f;
                }

                mRangeAttackContext.toIdelCounter += Time.deltaTime;
                mRangeAttackContext.attackSpeedCounter += Time.deltaTime;
                break;
            case States.meleeAttack:
                if (mMeleeAttackContext.toIdelCounter >= mMeleeAttackContext.toIdelDelay)
                {
                    GoToIdle();
                    break;
                }

                Vector3 agentPos = transform.position;
                Vector3 playerPos =mPlayer.transform.position;

                //if (Vector3.SqrMagnitude(mPlayer.transform.position - transform.position) < mMeleeAttackRange * mMeleeAttackRange && !mMeleeAttackContext.isAttacked)
                if((Vector3.SqrMagnitude(mPlayer.transform.position - transform.position) < mMeleeAttackRange || Vector3.Distance(transform.position, aiPath.destination) <= 0.1f) && !mMeleeAttackContext.isAttacked)
                {
                    mAnimationController.GoMeleeAttackAnimation();
                    mMeleeAttackContext.isAttacked = true;
                }

                mMeleeAttackContext.toIdelCounter += Time.deltaTime;
                break;
            default:
                break;
        }

       
    }

    void GoToIdle()
    {
        aiPath.destination = gameObject.transform.position;
        mIdleContext.time = 0.0f;
        mIdleContext.idelCounter = 0.0f;
        mCurrentState = States.Idel;
    }
    void GoToGoto()
    {
        aiPath.maxSpeed = mGotoContext.gotoSpeed;

        mGotoContext.delayCounter = 0.0f;
        mGotoContext.startDelay = false;

        var playPos = mPlayer.transform.position;
        Vector3 dir = Vector3.Normalize(transform.position - mPlayer.transform.position);
        Vector3 attackPos = playPos + dir * mRangeAttackRange;
        //if (attackPos.y < mPlayer.transform.position.y)
        //{
            attackPos.y = mPlayer.transform.position.y + mRangeAttackRange * 0.5f;
        //}
        aiPath.destination = attackPos;
        mCurrentState = States.goTo;
    }
    void GoToRangeAttack()
    {
        aiPath.destination = gameObject.transform.position + 10.0f * Vector3.Normalize(mPlayer.transform.position - transform.position);

        mRangeAttackContext.toIdelCounter = 0.0f;
        mRangeAttackContext.attackSpeedCounter = 0.0f;
        mCurrentState = States.rangeAttack;
    }
    void GoToMeleeAttack()
    {
        aiPath.maxSpeed = mMeleeAttackContext.chargeSpeed;

        mMeleeAttackContext.isAttacked = false;
        mMeleeAttackContext.isOnPlayerleft = transform.position.x < mPlayer.transform.position.x;
        mMeleeAttackContext.yDifferent = transform.position.y - mPlayer.transform.position.y;

        //Vector3 dir = Vector3.Normalize(mPlayer.transform.position - transform.position);
        //float distance = Vector3.Magnitude(mPlayer.transform.position - transform.position);
        //mMeleeAttackContext.destPos = dir * distance;
        aiPath.destination = mPlayer.transform.position;

        mMeleeAttackContext.toIdelCounter = 0.0f;
        mCurrentState = States.meleeAttack;
    }

    public void RangeAttack()
    {
       
        Bullet newBullet = Object.Instantiate(mRangeAttackContext.mRangeBullet, new Vector3(0, 0, 0), Quaternion.identity);
        Vector3 dir = Vector3.Normalize(transform.position - mPlayer.transform.position);
        newBullet.Fire(gameObject.tag, mRangeAttackContext.mRangeDamage, transform.position, -dir, WeaponType.Range, this);
    }

    public void MeleeAttack()
    {
        Bullet newBullet = Object.Instantiate(mMeleeAttackContext.mMeleeBullet, new Vector3(0, 0, 0), Quaternion.identity);
        newBullet.Awake();
        newBullet.Fire(gameObject.tag, mMeleeAttackContext.mMeleeDamage, transform.position, Vector3.down, WeaponType.Melee);
    }

    public void GetHit(float dmg, string tag, Vector3 hitPosition)
    {
        GetHit(dmg, hitPosition);
        GetStun();
    }

    private void GetStun()
    {
        mIsStuned = true;
        mAnimationController.GoStunAnimation();
    }

    public void RestFromStun()
    {
        mIsStuned = false;
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
                //GetHit(bullet.mElement);
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
                    else if (direction < 0.0f)
                    {
                        rb.AddForce(new Vector2(knockBackX, knockBackY));
                    }
                }
            }
        }
    }

    override public void Die()
    {
        //effect
        Debug.Log("enemy destory");
        mAnimationController.GoDeathAnimation();
        base.mHealthBar.gameObject.SetActive(false);
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
        gameObject.SetActive(false);        //|--- Set this by routin function in future: For giving time to died animation 
        //--------------------------//|
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
        Gizmos.DrawSphere(transform.position, mMeleeAttackRange);
        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.3f);
        Gizmos.DrawSphere(transform.position, mRangeAttackRange);
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
        Gizmos.DrawSphere(transform.position, mVisibilityRange);
    }
}

