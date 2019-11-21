using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Elemental
{
    public override void EffectTick()
    {
        if (mCount != 0)
        {
            if (mElementalAttribute.mPlayer != null)
            {
                Rigidbody2D rb = GetPlayer().GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    //rb.AddForce(-((GetPlayer().transform.position - GetPlayer.LastGotHit()).normalized) * (GetActiveIntensity() * (resistance/100))); //Uncomment this line when Player.LastGotHit() is implemented
                }
            }
            if (mElementalAttribute.mEnemy != null)
            {
                Rigidbody2D rb = GetEnemy().GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(-((GetEnemy().transform.position - GameObject.FindObjectOfType<Player>().transform.position).normalized) * (GetActiveIntensity() * (resistance / 100)));
                }
            }
            mCount = 0;
        }
    }
}
