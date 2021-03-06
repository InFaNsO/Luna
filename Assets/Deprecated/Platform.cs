﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float Width = 0.0f;
    public float Height = 0.0f;

    //Add element system

    // Start is called before the first frame update
    void Start()
    {
        if (Width < 0.0f)
            Width = -Width;
        if (Height < 0.0f)
            Height = -Height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Vector3 tl = transform.position;
        tl.x -= Width * 0.5f;
        tl.y += Height * 0.5f;

        Vector3 br = transform.position;
        br.x += Width * 0.5f;
        br.y -= Height * 0.5f;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(tl, 0.1f);
        Gizmos.DrawWireSphere(br, 0.1f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var zone = collision.GetComponent<AI_Zone>();
        if(zone != null)
        {
            zone.mPlatforms.Add(this);
        }
    }
}
