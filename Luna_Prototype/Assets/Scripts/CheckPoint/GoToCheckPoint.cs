using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCheckPoint : MonoBehaviour
{
    public GameObject debugPlayer;

    private void OnCollisionStay2D(Collision2D collision)
    {
        debugPlayer = collision.gameObject;
        if(collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            collision.gameObject.GetComponent<CheckPointTracker>().Respawn(false);
        }

    }
}
