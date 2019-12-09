using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealingBattry : MonoBehaviour,  IInventoryItem
{
    public float _HealingPower = 10.0f;
    public bool _IsReaUseable = false;
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
        return "HealiingBattry";
    }

    public bool Use(ref Player thePlayer)
    {
        thePlayer.UpdateHealth(_HealingPower);
        return _IsReaUseable;
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
