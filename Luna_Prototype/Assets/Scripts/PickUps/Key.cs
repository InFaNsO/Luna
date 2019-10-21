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
        GameObject door = null; //to-be closest door
        float nd = 0;
        float d = 0;
        for (int i = 0; i < doors.Length; i++)
        {
            if(doors.Length == 1) // there is only one door
            {
                door = doors[i];
                break;
            }
            else
            {
                Vector2 player_pos = thePlayer.transform.position;
                Vector2 door_pos = doors[i].transform.position;
                if (i == 0)
                {
                    d = Vector2.Distance(player_pos, door_pos);
                    nd = Vector2.Distance(player_pos, door_pos);
                    door = doors[i];
                }
                else
                {
                    nd = Vector2.Distance(player_pos, door_pos);
                }

                if(nd < d)
                {
                    door = doors[i];
                }
                ++i;
            }
        } //find closest door

        if (door != null) //Unlock the door
        {
            EnvObj_Door mDoor = door.GetComponent<EnvObj_Door>();
            if (mDoor.collidePlayer)
            {
                mDoor.Unlock();
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
                other.GetComponent<Inventory>().AddItem(this);
            }
        }
    }
}
