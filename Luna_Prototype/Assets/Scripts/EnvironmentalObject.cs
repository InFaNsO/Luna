using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentObjType
{
    none,
    door,
    lever,
    fallingplatform
}
public interface EnvironmentalObject
{
    EnvironmentObjType GetTypeEnum();
    string GetObjectName();
    Sprite GetSprite();
    void interact(ref Player thePlayer);
}
