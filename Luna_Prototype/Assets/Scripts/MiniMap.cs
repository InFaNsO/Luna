using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if(player)
        {
            Vector3 newPosition = player.position;
            newPosition.z = -10;
            transform.position = newPosition;
        }
    }
}
