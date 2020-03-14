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

    public AIPath aiPath;
    private GameObject mPlayer;

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
        public float idelCounter = 0.0f;
        public float time;
        public Vector3 newPos;
    }
    [System.Serializable]
    public class GotoContext
    {
        public float attackDelay = 0.5f;
        public float delayCounter = 0.0f;
        public bool startDelay = false;
        public float gotoSpeed = 2.0f;
    }
    [System.Serializable]
    public class MeleeAttackContext
    {
        public Bullet mMeleeBullet;
        public int mMeleeDamage = 10;

        public float toIdelDelay = 3.0f;
        public float toIdelCounter = 0.0f;
        public bool isOnPlayerleft = false;
        public float yDifferent = 0.0f;
        public Vector3 destPos;
        public bool isAttacked = false;
        public float chargeSpeed = 5.0f;
    }
    [System.Serializable]
    public class RangeAttackContext
    {
        public Bullet mRangeBullet;
        public int mRangeDamage = 10;
        public float toIdelDelay = 3.0f;
        public float toIdelCounter = 0.0f;
        public float attackSpeed = 1.0f;
        public float attackSpeedCounter = 0.0f;
    }

    IdleContext mIdleContext;
    GotoContext mGotoContext;
    RangeAttackContext mRangeAttackContext;
    MeleeAttackContext mMeleeAttackContext;

    States mCurrentState;
    // Start is called before the first frame update
    new void Awake()
    {
        Assert.IsNotNull(mRangeAttackContext.mRangeBullet, "[Enemy_Bat] mRangeBullet is Null");
        Assert.IsNotNull(mMeleeAttackContext.mMeleeBullet, "[Enemy_Bat] mMeleeBullet is Null");
        base.Awake();

        aiPath = GetComponent<AIPath>();
        mPlayer = GameObject.Find("Player");

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
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
                        if (Vector2.Distance(transform.position, mPlayer.transform.position) <= mRangeAttackRange)        //ready to attack
                            GoToRangeAttack();
                        else                                    //Go to player
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
                    int rand = Random.Range(1, 2);
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

                    // TODO let animator does it job
                    //Bullet newBullet = Object.Instantiate(batAgent.mBullet, new Vector3(0, 0, 0), Quaternion.identity);
                    //Vector3 dir = Vector3.Normalize(agent.transform.position - agent.GetWorld().mPlayer.transform.position);
                    //newBullet.Fire(agent.tag, batAgent.mDamage, agent.transform.position, -dir, WeaponType.Range);

                    Vector3 dir = Vector3.Normalize(transform.position - mPlayer.transform.position);
                    //recoil force to make it looks good
                    mRigidBody.velocity = Vector2.zero;
                    mRigidBody.AddForce(dir * 100.0f * Time.deltaTime, ForceMode2D.Impulse);

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

                if (Vector3.SqrMagnitude(mPlayer.transform.position - transform.position) < 0.25f && !mMeleeAttackContext.isAttacked)
                {
                    // TODO let animimator handle this
                    //Bullet newBullet = Object.Instantiate(batAgent.mMeleeBullet, new Vector3(0, 0, 0), Quaternion.identity);
                    //newBullet.Awake();
                    //newBullet.Fire(agent.tag, batAgent.mDamage, agent.transform.position, Vector3.down, WeaponType.Melee);
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
        attackPos.y = mPlayer.transform.position.y + mRangeAttackRange;
        aiPath.destination = attackPos;
        mCurrentState = States.goTo;
    }
    void GoToRangeAttack()
    {
        aiPath.destination = gameObject.transform.position;

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

        Vector3 dir = Vector3.Normalize(mPlayer.transform.position - transform.position);
        float distance = Vector3.Magnitude(mPlayer.transform.position - transform.position);
        mMeleeAttackContext.destPos = dir * distance;
        aiPath.destination = mMeleeAttackContext.destPos;

        mMeleeAttackContext.toIdelCounter = 0.0f;
        mCurrentState = States.meleeAttack;
    }
}
