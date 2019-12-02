using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Elemental
{
    bool debuffRooted = false; //Entangled by vegetations, movement had been impeded.
    bool canRefresh = false; //Rooted debuff does not refresh when applied multiple times before it wears off
    public override void EffectTick()
    {
        if (!debuffRooted && mCount > 0 && !canRefresh)
        {
            mActiveDuration += mCount * (resistance / 100);
            mCount = 0;
            debuffRooted = true;
        }
        else if(canRefresh && mCount > 0)
        {
            mActiveDuration = mCount * (resistance / 100);
            mCount = 0;
            debuffRooted = true;
        }

        if (debuffRooted)
        {
            if (mActiveDuration > 0)
            {
                if (GetPlayer() != null) //Stun Player
                {

                }
                if (GetEnemy() != null) //Stun Enemy
                {
                    GetEnemy().mIsStuned = true;
                }
                mActiveDuration -= 1 * Time.deltaTime;
            }
            else
            {
                if (GetPlayer() != null) //Un-stun Player
                {

                }
                if (GetEnemy() != null) //Un-stun Enemy
                {
                    GetEnemy().mIsStuned = false;
                }
                debuffRooted = false;
                if (!canRefresh)
                {
                    mCount = 0;
                }
            }
        }
    }
}
