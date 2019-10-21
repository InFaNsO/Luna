using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EnemyAnimation
{
    None = 0,
    ToIdel,
    Stun,
    Attack,
}

public class Enemy : Character
{
    // Members ---------------------------------------------------------------------------------------------------------------
    [SerializeField] protected Weapon mWeapon;
    [SerializeField] protected bool mIsDropping;
    [SerializeField] protected Key mDropPrefbs;

    //[SerializeField] protected Agent mAgent;
    //[SerializeField] protected World world;

    //private Animator mAnimator;
    bool mIsStuned = false;
    float mStunCounter;

    public void Start()
    {
        //mAgent = new Agent();
        //mAgent.SetWorld(world);
        //world.AddAgent(mAgent);
    }

    public float GetMoveSpeed()
    {
        return mMovementSpeed;
    }

    public float GetJumpStrength()
    {
        return mJumpStrength;
    }

    // MonoBehaviour Functions -----------------------------------------------------------------------------------------------
    new public void Awake()
    {
        base.Awake();

        //Starter();

        mMovementSpeed = 5.0f;
        mJumpStrength = 20.0f;

        //Assert.IsNotNull(GetComponent<BoxCollider2D>(), "[Enemy] Dont have CapsuleCollider");                                      //|--- [SAFTY]: Check to see is there a Collider
        //mAnimator = gameObject.GetComponent<Animator>();                                                                         //|--- [INIT]: Initialize animator

        if (mWeapon != null)
        {
            mWeapon.Picked(gameObject, gameObject.transform.position); // second argument should be the [weapon position] as a individual variable in future
        }
    }

    new public void Update()
    {
        base.Update();

        if (mIsStuned != true)
        {
            //Do AI here
            HardCodeBehavior();
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
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        SetPosition(pos);
    }

    public void LateUpdate()
    {
        SetAnimator(EnemyAnimation.ToIdel);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != gameObject.tag)
        {
            Debug.Log("enemy collide with bullet!_1");
            if (other.GetComponent<Bullet>() != null)
            {
                Debug.Log("enemy collide with bullet!_2");

                GetHit(other.GetComponent<Bullet>().Damage);
            }
            else if (other.GetComponent<Laser>() != null)
            {
                Debug.Log("enemy collide with bullet!_2");

                GetHit(other.GetComponent<Laser>().Damage);
            }
        }
    }


    // Self-define Functions -----------------------------------------------------------------------------------------------

    public void Attack()
    {
        mWeapon.Attack(isGrounded);
        SetAnimator(EnemyAnimation.Attack);
    }

    override public void Die()
    {
        //effect
        Debug.Log("enemy destory");

        if(mIsDropping)
        {
            //spwn inventory
            Instantiate(mDropPrefbs);

        }

        // [Maybe] Update game Score
        // [Maybe] Change anmation state

        //--------------------------//|
        Destroy(gameObject);        //|--- Set this by routin function in future: For giveing time to died animation 
        //--------------------------//|
    }

    override public void GetHit(float dmg)
    {
        mCurrentHealth -= dmg;

        if (mWeapon.GetAttackState() == AttacState.State_Parriable)
        {
            GetStun(1.5f);
        }

        if (mCurrentHealth <= 0.0f)
        {
            Die();
        }
    }

    public void GetStun(float stunHowLong)
    {
        mIsStuned = true;
        mStunCounter = stunHowLong;

        SetAnimator(EnemyAnimation.Stun);
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

    //-------------------------------------------------------------------------------//|
    private int behaviorCounter = 0; // For hard code behavior                       //|
    void HardCodeBehavior()                                                          //|
    {                                                                                //|
        // Hard code behavior                                                        //|
        behaviorCounter++;                                                           //|
        if (behaviorCounter % 100 == 0)                                              //|--- Hard code behavior, Delete this in future
        {                                                                            //|
            Attack();                                                                //|
        }                                                                            //|
    }                                                                                //|
    //-------------------------------------------------------------------------------//|
}

