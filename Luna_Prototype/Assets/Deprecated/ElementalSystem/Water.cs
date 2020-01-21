using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Elemental
{
    private bool init = false;
    public bool debuffDrenched = false;
    private float originalSpeed;
    public override void EffectTick()
    {
        if(!init)
        {
            if(GetPlayer() != null)
            {
                originalSpeed = GetPlayer().mMovementSpeed;
            }
            if(GetEnemy() != null)
            {
                originalSpeed = GetEnemy().mMovementSpeed;
            }
            if(intensityStatus > 100)
            {
                intensityStatus = 100;
                Debug.Log("Water intensity status is over 100, no movement debuff is applied to enemy.");
            }
            init = true;
        }
        if (mCount >= 0) //Effect is active
        {
            debuffDrenched = true; //Drenched in water, susceptible to lightning attacks
            if (GetPlayer() != null)
            {
                //uncomment this after playerSpeedModifier is implemented
                //GetPlayer().GetComponent<PlayerController>().playerSpeedModifier = ((GetActiveIntensity() / 100) * (resistance / 100));
            }
            if (GetEnemy() != null)
            {
                GetEnemy().mMovementSpeed = originalSpeed * ((GetActiveIntensity() / 100) * (resistance / 100));
            }
            mCount -= 1 * Time.deltaTime;
            if(mCount <= 0) //Effect has ended
            {
                debuffDrenched = false;
                if (GetPlayer() != null)
                {
                    //uncomment this after playerSpeedModifier is implemented
                    //GetPlayer().GetComponent<PlayerController>().playerSpeedModifier = 1.0f;
                    GetPlayer().mMovementSpeed = originalSpeed;
                }
                if (GetEnemy() != null)
                {
                    GetEnemy().mMovementSpeed = originalSpeed;
                }
            }
        }
    }
}
