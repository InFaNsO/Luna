using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Attach : MonoBehaviour
{
    public GameObject player;

    // Enter Platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = transform;
        }
    }

    // Exit Platform
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = null;
        }
    }
}
