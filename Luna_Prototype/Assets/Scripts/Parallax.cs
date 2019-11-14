using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float imageLength, startPosition;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosition = transform.position.x;
        imageLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

        if (temp > startPosition + imageLength)
            startPosition += imageLength;
        else if (temp < startPosition - imageLength)
            startPosition -= imageLength;
    }
}
