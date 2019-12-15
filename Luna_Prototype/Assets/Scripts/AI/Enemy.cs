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
    [SerializeField] protected LAI.SteeringModule mSteeringModule = new LAI.SteeringModule();
    [SerializeField] protected LAI.Grid_PathFinding pathFinder = new LAI.Grid_PathFinding();
    [SerializeField] public LAI.StateMachine mStateMachine = new LAI.StateMachine();

    //[SerializeField] protected Agent mAgent;
    //[SerializeField] protected World world;

    //private Animator mAnimator;
    public bool mIsStuned = false;
    float mStunCounter;

    public LAI.SteeringModule GetSteeringModule() { return mSteeringModule; }

    public void Start()
    {
        //mAgent = new Agent();
        //mAgent.SetWorld(world);
        world.AddAgent(this);
        mStateMachine.SetAgent(this);
        mSteeringModule.SetAgent(this);

        pathFinder.gameWorld = world;
        pathFinder.Initialize();

        if (mWeapon != null)
        {
            mWeapon.Picked(gameObject, gameObject.transform.position); // second argument should be the [weapon position] as a individual variable in future
        }
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

        
    }

    public new void Update()
    {
        mVelocity += mSteeringModule.Calculate();
        mVelocity *= Time.deltaTime;
        base.Update();

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
        Vector2 pos = new Vector2(transform.position.x + mVelocity.x, transform.position.y + mVelocity.y);
        SetPosition(pos);
    }

    public void LateUpdate()
    {
        SetAnimator(EnemyAnimation.ToIdel);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & 1 << gameObject.layer) == 0)
        {
            
            if (other.GetComponent<Bullet>() != null)
            {
                GetHit(other.GetComponent<Bullet>().Damage, other.tag);
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

    public override void GetHit(float dmg)
    {
        mCurrentHealth -= dmg;

        if ((mWeapon.GetAttackState() == AttacState.State_Parriable))
        {
            GetStun(1.5f);
        }

        if (mCurrentHealth <= 0.0f)
        {
            Die();
        }
    }

    public void GetHit(float dmg, string tag)
    {
        mCurrentHealth -= dmg;

        if ((mWeapon.GetAttackState() == AttacState.State_Parriable) && (tag == "Parry"))
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

        mWeapon.WeaponReset();

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


    public bool IsNearPlayer(float distance)
    {
        if (Vector3.Distance(gameObject.transform.position, GetWorld().mPlayer.transform.position) < distance)
        {
            return true;
        }
        return false;
    }
    public bool IsNearDestination(float distance)
    {
        if (Vector3.Distance(transform.position, mDestination) < distance)
        {
            return true;
        }
        return false;
    }

}

