using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCheckPoint : MonoBehaviour
{
    public bool activated = false;
    public bool currentPoint = false;
    public GameObject Player;
    public GameObject particleEffect;
    public GameObject particlePosition;
    private SpriteRenderer mSpriteRenderer;
    public Sprite offSprite;
    public Sprite onSprite;

    void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mSpriteRenderer.sprite = offSprite;
    }

    // Checkpoint Trigger Function
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            Player = collider.gameObject;
            Player.GetComponent<CheckPointTracker>().ResetCheckpoints();
            if (activated == false && currentPoint == false)
            {
                activated = true;
                currentPoint = true;

                CheckPointTracker mCPTracker = Player.GetComponent<CheckPointTracker>();
                mCPTracker.respawnPoint = gameObject;
                Player.GetComponent<CheckPointTracker>().ResetSprites();
                mCPTracker.SetRecordedHealth(Player.GetComponent<Player>().myHealth.GetHealth());
                mCPTracker.InitializeEnemyStates();
                mSpriteRenderer.sprite = onSprite;

                Instantiate(particleEffect, particlePosition.transform.position, new Quaternion());
            }
        }
    }
}
