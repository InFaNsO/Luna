using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoomTriggerVol : MonoBehaviour
{
    public Camera cam;
    public float destSize = 0f;
    public float time = 0f;

    private void Awake()
    {
         cam = FindObjectOfType<Camera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            cam.GetComponent<CameraController>().CameraZoom(destSize, time);
        }
    }
}
