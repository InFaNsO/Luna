using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Key : MonoBehaviour, IInventoryItem
{
    public Sprite _sprite;
    public void DisableFromLevel()
    {
        gameObject.SetActive(false);
    }

    public Sprite GetSprite()
    {
        return _sprite;
    }

    public string GetTypeName()
    {
        return "KeyDoor";
    }

    public bool Use(ref Player thePlayer)
    {
        //unlock door
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        doors = GameObject.FindGameObjectsWithTag("Door");
        EnvObj_Door door = null; //to-be closest door
        float nd = 0;
        float d = 0;
        for (int i = 0; i < doors.Length; i++)
        {
            door = doors[i].GetComponent<EnvObj_Door>();
            if (door != null)
            {
                if (door.collidePlayer && door.locked && door.isActiveAndEnabled)
                {
                    break;
                }
            }
        } //find closest door

        if (door != null && door.locked) //Unlock the door
        {
            if (door.collidePlayer)
            {
                door.Unlock();
                return false; //Place holder
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>() != null)
            {
                other.GetComponent<Inventory>().AddEventItem(this);
            }
        }
    }
}
