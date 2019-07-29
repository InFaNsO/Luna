using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Weapon mWeapon;
    [SerializeField] private ParryContext mParryContext = new ParryContext();
    [SerializeField] private PlayerController mController;

    private Animator mAnimator;

    override public void GetHit(float dmg)
    {
        Debug.Log("[Player] Player receives damage");
        mCurrentHealth -= dmg;

    }
    override public void Die()
    {
        Debug.Log("[Player] Player Dead, curr hp : " + mCurrentHealth + "max hp " + mMaxHealth);

        gameObject.SetActive(false);
    }

    public void Parry()
    {
        Debug.Log("[Player] Parry called");
    }
    public void Attack()
    {
        Debug.Log("[Player] Attack called");
        if (mWeapon != null)
        {
            if (mParryContext.Active == false)
            {
                Debug.Log("[Player] start attack");
                mParryContext.Active = true;
                
                // Do animation
                mAnimator.SetInteger("Condition", 8);
            }
        }

    }
    public void InflictDamage()
    {
        Debug.Log("[Player] InflictDamage called");

    }


    new public void Awake()
    {
        base.Awake();
        //Assert.IsNotNull(GetComponent<CapsuleCollider>(), "[Player] Dont have collider");                                      //|--- [SAFTY]: Check to see is there a Collider
        //Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[0], 0.0f, "[Player] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        //Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[1], 0.0f, "[Player] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        //Assert.AreNotEqual(mParryContext.mTimeSlicePropotion[2], 0.0f, "[Player] Parry context not initialized");              //|--- [SAFTY]: Check to see if parry context got initialized
        //Assert.AreEqual(mParryContext.mTimeSlicePropotion.Length, 3, "[Player] Parry context TimeSlice count should be 3");  //|--- [SAFTY]: Check to see if parry context TimeSlice count is 3

        mAnimator = gameObject.GetComponent<Animator>();
        mParryContext.TotalTime = mWeapon.AttackSpeed;
        mParryContext.Reset();
        mWeapon.gameObject.tag = "Player";
    }

    new public void Update()
    {
        //[TOFIX] in player move controller.cs ,  GetKeyUp vec3 should be (0,0,0) rather than (-1, 0, 0 )
        base.Update();
        mParryContext.Update(Time.deltaTime);

        if (Input.GetKey(KeyCode.K))
        {
            Attack();
        }
    }
    public void LateUpdate()
    {
        mAnimator.SetInteger("Condition", 0);
    }

}
