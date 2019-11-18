using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTracker : MonoBehaviour
{
    public GameObject respawnPoint;


    public void Respawn(bool heal)
    {
        if (respawnPoint != null)
        {
            if (heal)
            {
                GetComponent<Player>().mCurrentHealth = GetComponent<Player>().mMaxHealth; // heal the player before respawning them
            }
            transform.position = respawnPoint.transform.position;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Player>().mCurrentHealth <= 0)
        {
            Respawn(true);
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != null)
        {
            respawnPoint = collision.gameObject;
        }
    }
    */
}
