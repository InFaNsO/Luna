using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollisionHandler : MonoBehaviour
{
    AI_Zone myZone;




    // Start is called before the first frame update
    void Start()
    {
        myZone = GetComponentInParent<AI_Zone>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
             myZone.AwakeEnemies();
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            myZone.SleepEnemies();
        }
    }
}
