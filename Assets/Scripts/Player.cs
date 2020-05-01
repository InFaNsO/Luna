 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : Character
{
    [SerializeField]
    private Weapon mWeapon1;
    [SerializeField]
    private Weapon mWeapon2;
    [SerializeField]
    public Weapon mCurrentWeapon;

    [SerializeField]
    private Weapon mNearbyWeapon;

    [SerializeField]
    private bool mIsIFrameOn;
    [SerializeField]
    private float mIFrameCD;
    [SerializeField]
    private float mIFrameDuration;
    [SerializeField]
    private float mDashSpeed = 14f;
    private bool isDouleJumpEnabled;

    [SerializeField]
    private int mLevel;
    [SerializeField]
    private int mCurrentExp;
    [SerializeField]
    private int mRequiredExp;
    [SerializeField]
    private int mAttack;
    [SerializeField]
    private int mDefense;

    private CheckPointTracker myCheckpointTracker;

    public LocalLevelManager _LocalLevelManager;                       //|--- [Mingzhuo Zhang] Edit: add localLevelManager to create a way to communicate with UI
    public GameObject mWeaponPosition;

    public Vector3 LastGotHitPosition { get { return mLastGotHitPosition; } }       //|--- [Mingzhuo Zhang] Edit:  For Kevin(Element system)
    

    private ParryAttackable mParryAttackable;

    public RuntimeAnimatorController mDefaultRunTimeAniamtorController;
    // Getter & Setter
    public Weapon CurrentWeapon { get { return mCurrentWeapon; } }

    private CameraController mMainCamera;

    
    protected override void Start()
    {
        base.Start();

        myCheckpointTracker = GetComponent<CheckPointTracker>();
    }


    public void LevelUp()
    {
        mLevel++;
        mCurrentExp = mCurrentExp - mRequiredExp;
        mRequiredExp = 2 * mRequiredExp;
        mAttack++;
        mDefense++;
        mMovementSpeed += 0.5f;
        mJumpStrength += 25.0f;
    }

    public void GainExp(int exp)
    {
        mCurrentExp += exp;
    }

    private void ExpCheck()
    {
        if(mCurrentExp >= mRequiredExp)
        {
            LevelUp();
        }
    }


    public bool GetDoubleJumpEnable()
    {
        return isDouleJumpEnabled;
    }

    public float GetMoveSpeed()
    {
        if (mCurrentWeapon && mCurrentWeapon.mIsAttacking && isGrounded && !mCurrentWeapon.mMoves[mCurrentWeapon.mCurrentMoveIndex].mIsAirMove)
        {
            //return mMovementSpeed * 0.3f;
            return 0.0f;
        }

        if (mParryAttackable.IsParrying() && isGrounded)
        {
            return 0.0f;
        }
        return mMovementSpeed;
    }

    public float GetJumpStrength()
    {
        return mJumpStrength;
    }

    public float GetDashSpeed()
    {
        return mDashSpeed;
    }

    //[RH] press button to pick up weapon
    public void AddNearbyWeapon(Weapon newWeapon)
    {
        mNearbyWeapon = newWeapon;
    }
    public void ClearNearbyWeapon()
    {
        mNearbyWeapon = null;
    }
    public void PickUpNearbyWeapon()
    {
        if (mNearbyWeapon != null)
        {

            PickWeaopn(mNearbyWeapon);
            ClearNearbyWeapon();
        }
    }
    //[RH] press button to pick up weapon


    public void DropWeapon()
    {
        if ((mWeapon1 != null) || (mWeapon2 != null))
        {
            //                                 |  add a condition |                                                             //|--- [Mingzhuo Zhang] Edit: prevent trying drop NULL Weapon
            //                                 v                  v                                                             //|
            if ((mCurrentWeapon == mWeapon1) && (mWeapon1 != null))                                                             //|
            {
                Debug.Log("Weapon 1 dropped");
                mWeapon1.ThrowAway(new Vector3(gameObject.transform.right.x, gameObject.transform.up.y, 0.0f));
                mWeapon1.tag = "PickUp";
                mWeapon1 = null;
                mCurrentWeapon = null;
                if (mWeapon2 != null)                                                                                           //|--- [Mingzhuo Zhang] Edit: achieve automatic switch weapon
                    EquipWeapon(mWeapon2);                                                                                  //|
            }
            //                                 |  add a condition |                                                             //|--- [Mingzhuo Zhang] Edit: prevent trying drop NULL Weapon
            //                                 v                  v                                                             //|
            else if ((mCurrentWeapon == mWeapon2) && (mWeapon2 != null))                                                        //|
            {
                Debug.Log("Weapon 2 dropped");
                mWeapon2.ThrowAway(new Vector3(gameObject.transform.right.x, gameObject.transform.up.y, 0.0f));
                mWeapon2.tag = "PickUp";
                mWeapon2 = null;
                mCurrentWeapon = null;

                if (mWeapon1 != null)                                                                                           //|--- [Mingzhuo Zhang] Edit: achieve automatic switch weapon
                    EquipWeapon(mWeapon1);                                                                                  //|
            }
            UpdateWeaponSlotInUI();

        }
        else
        {
            Debug.Log("There are no weapons to drop.");
        }
    }

    public void SwitchWeapon()
    {
        if (!CurrentWeapon)
        {
            return;
        }
        if (CurrentWeapon.mIsAttacking)
        {
            return;
        }
        if(mCurrentWeapon == mWeapon1)
        {
            if(mWeapon2 != null)
            {
                EquipWeapon(mWeapon2);
                Debug.Log("Switch to Weapon 2");
            }
        }else if(mCurrentWeapon == mWeapon2)
        {
            if (mWeapon1 != null)
            {
                EquipWeapon(mWeapon1);
                Debug.Log("Switch to Weapon 1");
            }
        }
        UpdateWeaponSlotInUI();

    }

    public float GetAttackSpeed()
    {
        if (mCurrentWeapon)
        {
            return mCurrentWeapon.GetCurrentAttackTime();
        }
        else
        {
            return 1.0f;
        }
    }

    public bool Dodge()
    {
        if (mIFrameCD <= 0)
        {
            mIsIFrameOn = true;
            mIFrameDuration = 1;
            mIFrameCD = 2;
            return true;
        }
        return false;
    }

    
    override public void GetHit(float dmg)
    {
        mMainCamera.Shake();

        Debug.Log("[Player] Player receives damage");
        //if (mIsIFrameOn == false)
            //mCurrentHealth -= dmg;
            UpdateHealth(-dmg);                                                                     //|--- [Mingzhuo Zhang] Edit: use update health function instead, so we can update UI
    }
    override public void Die()
    {
        Debug.Log("[Player] Player Dead, curr hp : " + myHealth.GetHealth() + "max hp " + myHealth.GetMaxHealth());

        if (gameObject.GetComponent<CheckPointTracker>() != null)
        {
            if (gameObject.GetComponent<CheckPointTracker>().respawnPoint != null)
            {
                gameObject.GetComponent<CheckPointTracker>().Respawn(true);
                ServiceLocator.Get<UIManager>().UpdateHPGauge(myHealth.GetHealth() / myHealth.GetMaxHealth());
            }
            else
            {
                ServiceLocator.Get<UIManager>().UpdateHPGauge(myHealth.GetHealth()  / myHealth.GetMaxHealth());
                gameObject.SetActive(false);
                ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Mainmenu);             //|--- [Rick H] Edit: Call GameMngr
            }
        }
    }

    public void Attack()
    {
        Core.Debug.Log("Attack");
        Debug.Log("[Player] Attack called");

        if (mParryAttackable.IsParrying())
            return;

        if ((mCurrentWeapon == null) && ((mWeapon1 != null) || (mWeapon2 != null)))
        {
            if(mWeapon1 != null)
            {
                EquipWeapon(mWeapon1);
                Debug.Log("Weapon 1 equipped");
                mCurrentWeapon.Attack(isGrounded);
            }
            else
            {
                EquipWeapon(mWeapon2);
                Debug.Log("Weapon 2 equipped");
                mCurrentWeapon.Attack(isGrounded);
            }
        }
        else if(mCurrentWeapon != null)
        {
            Debug.Log("current weapon attack");
            mCurrentWeapon.Attack(isGrounded);
        }
        else
        {
            Debug.Log("No Available weapon to attack.");
        }
    }

    public void Awake()
    {
        base.Awake();
        _LocalLevelManager = GameObject.Find("LocalLevelManager").GetComponent<LocalLevelManager>();    //|--- [Mingzhuo Zhang] Edit: add localLevelManager to create a way to communicate with UI
        Assert.IsNotNull(_LocalLevelManager, "[Player] _LocalLevelManager is null");                    //|--- [Mingzhuo Zhang] Edit: add localLevelManager to create a way to communicate with UI

        mParryAttackable = GetComponent<ParryAttackable>();
        Assert.IsNotNull(mParryAttackable, "[Player] _ParryAttackable is null");

        mDefaultRunTimeAniamtorController = GetComponent<Animator>().runtimeAnimatorController;

        isDouleJumpEnabled = true;

        mRequiredExp = 10;
        mCurrentExp = 0;

        //----------------------------------------------------------------------------------//|
        //- Mingzhuo Zhang Edit ------------------------------------------------------------//|
        //----------------------------------------------------------------------------------//|
        if (mWeapon1 != null)                                                               //|
        {                                                                                   //|
            mWeapon1.Picked(gameObject, mWeaponPosition.transform.position);                     //|    // second argument should be the [weapon position] as a individual variable in future
        }                                                                                   //|
        if (mWeapon2 != null)                                                               //|
        {                                                                                   //|
            mWeapon2.Picked(gameObject, mWeaponPosition.transform.position);                     //|    // second argument should be the [weapon position] as a individual variable in future
        }                                                                                   //|
                                                                                            //|
                                                                                            //----------------------------------------------------------------------------------//|
                                                                                            //- End Edit -----------------------------------------------------------------------//|
                                                                                            //----------------------------------------------------------------------------------//|

        mMainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        Assert.IsNotNull(mMainCamera, "[Player.cs] Can not find MainCamera controller in the scene.");
    }

    private void FixedUpdate()
    {
        mIFrameCD -= Time.deltaTime;
        mIFrameDuration -= Time.deltaTime;
        if (mIFrameDuration <= 0)
        {
            mIsIFrameOn = false;
        }
    }

    public void Update()
    {
        ExpCheck();
    }

    public void LateUpdate()
    {
        if(!myHealth.IsAlive())
        {
            myCheckpointTracker.Respawn(true);            
        }
    }

    //----------------------------------------------------------------------------------//|
    //- Mingzhuo Zhang Edit ------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
    void OnTriggerEnter2D(Collider2D other)                                             //|
    {                                                                                   //|
        if (other.tag != gameObject.tag)                                                //|
        {                                                                               //|--- [Mingzhuo Zhang] make player can take dmage from bullet

            var bullet = other.GetComponent<Bullet>();
            if ((bullet != null) && (other.tag != "Parry"))       //|
            {     //|
                if(bullet.mElement != null)
                    GetHit(bullet.mElement);
                
                mLastGotHitPosition = other.gameObject.transform.position;              //|
                GetHit(bullet.Damage, mLastGotHitPosition);
            }                                                                       //|
        }                                                                               //|
    }                                                                                   //|
    //----------------------------------------------------------------------------------//|
    //- End Edit -----------------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|


    //----------------------------------------------------------------------------------//|
    //- Mingzhuo Zhang Edit ------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
    public void PickWeaopn(Weapon newWeapon)                                            //|
    {                                                                                   //|
                                                                                        //|
        if (mWeapon1 == null)                                                           //|
        {                                                                               //|
            mWeapon1 = newWeapon;                                                       //|
            mWeapon1.Picked(gameObject, mWeaponPosition.transform.position);                 //|
            if (mCurrentWeapon == null)                                                 //|
                EquipWeapon(mWeapon1);                                              //|
        }                                                                               //|
        else if (mWeapon2 == null)                                                      //|--- [Mingzhuo Zhang] add a public function for pickUp weapon. This function will trigger by the collision of the weapon collider
        {                                                                               //|
            mWeapon2 = newWeapon;                                                       //|
            mWeapon2.Picked(gameObject, mWeaponPosition.transform.position);                 //|
            if (mCurrentWeapon == null)                                                 //|
                EquipWeapon(mWeapon2);                                              //|
        }                                                                               //|
                                                                                        //|
        UpdateWeaponSlotInUI();
                                                                                        //|
    }                                                                                   //|
    //----------------------------------------------------------------------------------//|
    //- End Edit -----------------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|


    //----------------------------------------------------------------------------------//|
    //- Mingzhuo Zhang Edit ------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
    public void UpdateHealth(float changeValue)                                         //|
    {
        if (changeValue > 0)
            myHealth.TakeHealth(changeValue);
        else
            myHealth.TakeDamage(Mathf.Abs(changeValue));

        ServiceLocator.Get<UIManager>().UpdateHPGauge(myHealth.GetHealth() / myHealth.GetMaxHealth());
                                                                    //|
    }                                                                                   //|
    //----------------------------------------------------------------------------------//|
    //- End Edit -----------------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|

    //----------------------------------------------------------------------------------//|
    //- Mingzhuo Zhang Edit ------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|
    private void EquipWeapon(Weapon nextWeapon)                                         //|
    {                                                                                   //|


        mCurrentWeapon = nextWeapon;                                                    //|--- [Mingzhuo Zhang] Add a function for equip weapon;
        mCurrentWeapon.Equip(gameObject);                                               //|

    }                                                                                   //|
    //----------------------------------------------------------------------------------//|
    //- End Edit -----------------------------------------------------------------------//|
    //----------------------------------------------------------------------------------//|


    private void UpdateWeaponSlotInUI()
    {
        //[RH] performance?
        Sprite currWeaponSprite = null, secWeaponSprite = null;
        if (mCurrentWeapon != null)
        {
            if (mCurrentWeapon == mWeapon1)
            {
                currWeaponSprite = mWeapon1.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite;
                secWeaponSprite = mWeapon2 != null ? mWeapon2.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite : null;
            }
            else if (mCurrentWeapon == mWeapon2)
            {
                currWeaponSprite = mWeapon2.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite;
                secWeaponSprite = mWeapon1 != null ? mWeapon1.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite : null;
            }

        }


        ServiceLocator.Get<UIManager>().
            UI_Ingame_UpdateWeaponSprite(currWeaponSprite, secWeaponSprite);

    }
}
