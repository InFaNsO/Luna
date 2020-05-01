using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Elemental
{
    public override void EffectTick()
    {
        if (mCount >= 0)
        {
            if (GetPlayer() != null)
            {
                GetPlayer().GetHit((potency * GetActiveIntensity()/100) * (resistance / 100));
            }
            if (GetEnemy() != null)
            {
                GetEnemy().GetHit((potency * GetActiveIntensity()/100) * (resistance / 100));
            }
            mCount = 0 * Time.deltaTime;
        }
    }
}
