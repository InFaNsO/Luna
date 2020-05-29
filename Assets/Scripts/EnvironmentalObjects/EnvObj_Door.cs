using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObj_Door : MonoBehaviour, EnvironmentalObject
{
    private SpriteRenderer mRenderer;
    public Sprite _spriteClosed;
    public Sprite _spriteOpened;
    public string _name = "door";
    public bool locked = true;
    public bool collidePlayer = false;
    public float openDelay = 0.5f;
    public float moveMaxSpeed = 5.0f;
    public float moveStartSpeed = 0.5f;
    public float acceleration = 1.0f;
    public float moveDistance = 3.0f;
    private float distanceMoved = 0.0f;
    private float currentSpeed;
    private float timer = 0.0f;
    private Vector3 mTransform;

    public int mKeyCount = 1;
    private int mKeyUsed = 0;

    public string GetObjectName()
    {
        return _name;
    }

    public Sprite GetSprite()
    {
        return mRenderer.sprite;
    }

    public void interact(ref Player thePlayer)
    {
        if(locked)
        {
            locked = false;
        }
        else
        {
            locked = true;
        }
    }

    EnvironmentObjType EnvironmentalObject.GetTypeEnum()
    {
        return EnvironmentObjType.door;
    }

    public void Unlock()
    {
        if(locked)
        {
            ++mKeyUsed;
            if (mKeyUsed >= mKeyCount)
            {
                locked = false;
            }
        }
    }

    public void Start()
    {
        mRenderer = gameObject.GetComponent<SpriteRenderer>();
        mRenderer.sprite = _spriteClosed;
        currentSpeed = moveStartSpeed;
    }

    public void Update()
    {
       if(locked)
       {
            mRenderer.sprite = _spriteClosed;
       }
       else
       {
            mRenderer.sprite = _spriteOpened;
            // Door opening logics
            if (timer > openDelay)
            {
                if (distanceMoved < moveDistance)
                {
                    mTransform.y = currentSpeed * Time.deltaTime;
                    gameObject.transform.position += mTransform;
                    distanceMoved += mTransform.y;
                }
                if(currentSpeed < moveMaxSpeed)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
       }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collidePlayer = true;

            var inventory = collision.gameObject.GetComponent<Inventory>();
            if (inventory)
            {
                int keyCount = inventory.GetSpecificEventItem("KeyDoor");

                if (keyCount >= mKeyCount)
                {
                    for (int i = 0; i < mKeyCount; i++)
                    {
                        var itemIndex = inventory.SearchEventItem("KeyDoor");
                        if (itemIndex != -1)
                            inventory.UsingEventItem(itemIndex);
                    }
                }
                else
                {
                    return;
                }
                
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidePlayer = false;
        }
    }
}
