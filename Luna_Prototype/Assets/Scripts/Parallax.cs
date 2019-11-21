using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float imageWidth, imageLength, startPositionX, startPositionY;
    public GameObject cam;
    public float parallaxEffectX, parallaxEffectY;

    void Start()
    {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        imageWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        imageLength = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void FixedUpdate()
    {
        //float tempX = (cam.transform.position.x * (1 - parallaxEffectX));
        float distX = (cam.transform.position.x * parallaxEffectX);

        //float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        float distY = (cam.transform.position.y * parallaxEffectY);

        transform.position = new Vector3(startPositionX + distX, startPositionY + distY, transform.position.z);

        //if (tempX > startPositionX + imageWidth)
        //    startPositionX += imageWidth;
        //else if (tempX < startPositionX - imageWidth)
        //    startPositionX -= imageWidth;

        //if (tempY > startPositionY + imageLength)
        //    startPositionY += imageLength;
        //else if (tempY < startPositionY - imageLength)
        //    startPositionY -= imageLength;
    }
}