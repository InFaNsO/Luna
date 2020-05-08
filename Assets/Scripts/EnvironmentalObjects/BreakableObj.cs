using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObj : Character
{
    SpriteRenderer spriteRenderer;
    public Sprite notBroken;
    public Sprite broken;
    public Vector3 particlePosOffset;
    public ParticleSystem particleOnBroken;
    //public AudioClip mAudioSource;
    //public AudioClip audioOnHit;
    //public AudioClip audioOnBroken;
    bool isbroken = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != gameObject.tag)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                mLastGotHitPosition = other.gameObject.transform.position;
                GetHit(bullet.Damage, mLastGotHitPosition);
            }

            if (myHealth.GetHealth() > 0)
            {
                spriteRenderer.sprite = notBroken;
            }
            else
            {
                spriteRenderer.sprite = broken;
            }
        }

        if(!isbroken && myHealth.GetHealth() <= 0)
        {
            Instantiate(particleOnBroken, transform.position + particlePosOffset, Quaternion.identity);
            isbroken = true;
        }
    }
}
