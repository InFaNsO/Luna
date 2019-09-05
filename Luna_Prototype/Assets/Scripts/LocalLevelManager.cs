using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Assertions;

public class LocalLevelManager : MonoBehaviour
{
    public InGameUI _InGameUI;

    void Awake()
    {
        Assert.IsNotNull(_InGameUI, "[LocalLevelManager] _InGameUI is null");
    }
}
