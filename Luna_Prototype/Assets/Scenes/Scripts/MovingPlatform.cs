
using UnityEngine;

public class MovingPlatform : MonoBehaviour
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
