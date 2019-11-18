using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCheckPoint : MonoBehaviour
{
    public GameObject debugPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        debugPlayer = collision.gameObject;
        if(collision.gameObject.GetComponent<CheckPointTracker>() != null)
        {
            collision.gameObject.GetComponent<CheckPointTracker>().Respawn(false);
        }

    }
}
