using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        mElement = Element.Luna;
        mGrade = WeaponGrade.Common;
        mType = WeaponType.Melee;

        mFirePositionOffSet = new Vector3(0.0f,0.0f,0.0f);
    }

    public override void Attack()
    {
        Bullet newBullet = Instantiate(mBullet, new Vector3(0, 0, 0), Quaternion.identity);
        newBullet.Fire(gameObject.tag,mDamage, gameObject.transform.position + mFirePositionOffSet, gameObject.transform.forward, mType);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
