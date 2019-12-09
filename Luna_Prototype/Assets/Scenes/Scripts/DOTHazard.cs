using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTHazard : Hazard
{
    [SerializeField]
    private float hazardDuration;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Character cha = collision.gameObject.GetComponent<Character>();
        if (cha != null)
            cha.ReceiveDebuff(hazardDamage, hazardDuration);
            Debug.Log("PoisonDebuff");
    }

    // Start is called before the first frame update
    void Start()
    {
        hazardDamage = 2;
        hazardDuration = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
