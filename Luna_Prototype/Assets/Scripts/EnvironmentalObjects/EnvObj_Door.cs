using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObj_Door : MonoBehaviour, EnvironmentalObject
{
    public Sprite _sprite;
    public string _name = "door";
    public bool locked = true;
    public bool collidePlayer = false;
    public int mKeyCount = 1;
    private int mKeyUsed = 0;

    public string GetObjectName()
    {
        return _name;
    }

    public Sprite GetSprite()
    {
        return _sprite;
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

    public void Update()
    {
       if(!locked)
        {
            // Door opening logics
            gameObject.SetActive(false);
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
