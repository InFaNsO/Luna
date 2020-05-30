using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.attachedRigidbody != null)
        {
            transform.parent.GetComponent<CustomWater>().Splash(transform.position.x, Hit.GetComponent<Rigidbody2D>().velocity.y * Hit.GetComponent<Rigidbody2D>().mass / 40f);
        }
    }
}
