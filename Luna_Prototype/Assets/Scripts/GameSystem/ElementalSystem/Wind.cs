using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Elemental
{
    bool Flow = false; //Attuned to the wind, increasing attack speed.
    float originalAtkSpeed;

    public override void EffectTick()
    {
        if(mCount > 0 && !Flow)
        {
            if(GetPlayer() != null)
            {
                originalAtkSpeed = GetPlayer().GetAttackSpeed();
            }
        }
        if(mCount > 0 && Flow)
        {
            if(GetPlayer() != null) //Need to know how attack speed works
            {
                
            }
            mCount -= 1 * Time.deltaTime;
        }
        else if(mCount <= 0 && Flow)
        {
            Flow = false;
            
        }
    }
}
