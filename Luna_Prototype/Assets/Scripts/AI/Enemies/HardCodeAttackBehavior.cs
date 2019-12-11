using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HardCodeAttackBehavior : MonoBehaviour
{
    public float attackSpeed = 2.0f;
    private float behaviorCounter = 0;

    private Enemy mEnemy;
    void Awake()
    {
        mEnemy = GetComponent<Enemy>();
        Assert.IsNotNull(mEnemy, "GameObject dont have Enemy script componet");
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
