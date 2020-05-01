
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject player;

    // Enter Platform
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = transform;
        }
    }

    // Exit Platform
    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = null;
        }
    }
}
