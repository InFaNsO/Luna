using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCheckPoint : MonoBehaviour
{
    public bool activated = false;
    public bool currentPoint = false;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && activated == false && currentPoint == false)
        {
            Player = collider.gameObject;
            activated = true;
            currentPoint = true;
            /*
            if (Player.GetComponent<CheckPointTracker>() != null)
            {
                Player.GetComponent<CheckPointTracker>().respawnPoint.GetComponent<SimpleCheckPoint>().currentPoint = false;
            }
            */
            Player.GetComponent<CheckPointTracker>().respawnPoint = gameObject;
        }
    }
}
