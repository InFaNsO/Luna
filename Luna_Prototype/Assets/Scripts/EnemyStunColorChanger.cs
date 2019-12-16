using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyStunColorChanger : MonoBehaviour
{
    Enemy mEnemy;
    SpriteRenderer mSpriteRender;
    // Start is called before the first frame update
    void Awake()
    {
        mEnemy = GetComponent<Enemy>();
        Assert.IsNotNull(mEnemy, "GameObject dont have Enemy script componet");

        mSpriteRender = GetComponentInChildren<SpriteRenderer>();
        Assert.IsNotNull(mSpriteRender, "GameObject dont have sprite render componet");
    }

    // Update is called once per frame
    void Update()
    {
        if (mEnemy.mIsStuned)
        {
            mSpriteRender.color = Color.red;
        }
        else
        {
            mSpriteRender.color = Color.white;
        }
    }
}
