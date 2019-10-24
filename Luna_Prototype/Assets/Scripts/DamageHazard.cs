using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHazard : Hazard
{
    public void OnCollisionStay2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null && !cha.GetHazardBool())
        {
            cha.GetHit(hazardDamage);
            cha.SetHazardBool();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hazardDamage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
