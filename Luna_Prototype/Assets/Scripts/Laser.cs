using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Laser : MonoBehaviour
{
    [SerializeField] private float mDamage;
    private int lostEffectFramCount = 1;
    private BoxCollider2D boxCollider;

    public float Damage { get { return mDamage; } set { mDamage = value; } }

    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Assert.IsNotNull(boxCollider, "[Laser] Dont have collider");       //|--- [SAFTY]: Check to see is there a Collider
    }

    private void FixedUpdate()
    {
        if (lostEffectFramCount == 0)
        {
            boxCollider.enabled = false;
            lostEffectFramCount = 1;
        }
        else
        {
            lostEffectFramCount--;
        }

    }

    public void Fire()
    {
        boxCollider.enabled = true;
        lostEffectFramCount = 1;
    }
}
