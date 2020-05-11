using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public Sprite originalObjectSprite;
    public Sprite brokenObjectSprite;
    public AudioSource breakingThings;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = originalObjectSprite;
        breakingThings = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.GetComponent<Bullet>();
        if (bullet)
            GetComponent<SpriteRenderer>().sprite = brokenObjectSprite;
        breakingThings.Play();
    }
}