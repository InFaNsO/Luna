using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInventoryItem
{
    string GetTypeName();
    Sprite GetSprite();
    void DisableFromLevel();
    bool Use(ref Player thePlayer);
}
