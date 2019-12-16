using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HardCodeAttackBehavior : MonoBehaviour
{
    public float attackSpeed = 2.0f;
    private float behaviorCounter = 0;

    LAI.States mCurrentState = LAI.States.None;

    private Enemy mEnemy;
    void Awake()
    {
        mEnemy = GetComponent<Enemy>();
        Assert.IsNotNull(mEnemy, "GameObject dont have Enemy script componet");

        mEnemy.mStateMachine.SetAgent(mEnemy);
        mEnemy.mStateMachine.AddState<LAI.DoNothingState>();
        mEnemy.mStateMachine.ChangeState(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (mEnemy.mIsStuned != true)
        {
            HardCodeBehavior();
        }
    }
    //-------------------------------------------------------------------------------//|
                                                                                     //|
    void HardCodeBehavior()                                                          //|
    {                                                                                //|
        // Hard code behavior                                                        //|
        behaviorCounter += Time.deltaTime;                                           //|
        if (behaviorCounter >= attackSpeed)                                              //|--- Hard code behavior, Delete this in future
        {                                                                            //|
            mEnemy.Attack();                                                         //|
            behaviorCounter = 0.0f;
        }                                                                            //|
    }                                                                                //|
    //-------------------------------------------------------------------------------//|
}
