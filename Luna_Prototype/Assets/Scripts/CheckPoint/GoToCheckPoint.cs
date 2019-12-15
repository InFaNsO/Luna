using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCheckPoint : MonoBehaviour
{
    public GameObject debugPlayer;
    public float delay;
    private void OnCollisionStay2D(Collision2D collision)
    {
        debugPlayer = collision.gameObject;
        if(collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            for (float i = delay; i > 0.0f; i -= Time.deltaTime)
            {

            }
            collision.gameObject.GetComponent<CheckPointTracker>().Respawn(false);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        debugPlayer = collision.gameObject;
        if (collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            for (float i = delay; i > 0.0f; i -= Time.deltaTime)
            {

            }
            collision.gameObject.GetComponent<CheckPointTracker>().Respawn(false);
        }

    }
}
