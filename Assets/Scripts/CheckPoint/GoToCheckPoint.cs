using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCheckPoint : MonoBehaviour
{
    public GameObject debugPlayer;
    public float delay; //do -1 if u dont want a delay
    private float i;
    private bool triggerFlag = false;


    private void Start()
    {
        i = delay;
    }
    void Update()
    {
        if(triggerFlag)
        {
            i -= Time.deltaTime;
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        debugPlayer = collision.gameObject;
        if (collision == null)
        { }
        else if (collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            triggerFlag = true;
            if (i < 0.0f)
            {
                collision.gameObject.GetComponent<CheckPointTracker>().Respawn(false);
                collision = null;
                return;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        debugPlayer = collision.gameObject;
        if (collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            triggerFlag = false;
            i = delay;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        debugPlayer = collision.gameObject;
        if(collision == null)
        { }
        else if (collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            triggerFlag = true;
            if (i < 0.0f)
            {
                collision.gameObject.GetComponent<CheckPointTracker>().Respawn(false);
                collision = null;
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        debugPlayer = collision.gameObject;
        if (collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            triggerFlag = false;
            i = delay;
        }
    }
}
