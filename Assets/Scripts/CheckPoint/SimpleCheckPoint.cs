using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCheckPoint : MonoBehaviour
{
    public bool activated = false;
    public bool currentPoint = false;
    public GameObject Player;
    public GameObject particleEffect;

    // Checkpoint Trigger Function
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            Player = collider.gameObject;
            Player.GetComponent<CheckPointTracker>().ResetCheckpoints();
            if (activated == false && currentPoint == false)
            {
                activated = true;
                currentPoint = true;

                CheckPointTracker mCPTracker = Player.GetComponent<CheckPointTracker>();
                mCPTracker.respawnPoint = gameObject;
                mCPTracker.SetRecordedHealth(Player.GetComponent<Player>().myHealth.GetHealth());
                Instantiate(particleEffect, transform.position, new Quaternion());
            }
        }
    }
}
