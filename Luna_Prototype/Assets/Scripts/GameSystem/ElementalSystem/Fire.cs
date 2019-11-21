using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Elemental
{
    public override void EffectTick() //Status Ailment tick/update, effect logic here.
    {
        if (mCount > 0)
        {
            if(GetPlayer() != null)
            {
                GetPlayer().GetHit((GetActiveIntensity() * Time.deltaTime) * (resistance/100));
            }
            if(GetEnemy() != null)
            {
                GetEnemy().GetHit((GetActiveIntensity() * Time.deltaTime) * (resistance / 100));
            }
            mCount -= 1 * Time.deltaTime;
        }
    }
}
//GetPlayer().mCurrentHealth = Mathf.Ceil(GetPlayer().mCurrentHealth);
