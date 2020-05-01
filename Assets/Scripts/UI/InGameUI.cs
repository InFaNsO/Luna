using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class InGameUI : MonoBehaviour
{
    public Image _HealthImage;
    public List<Button> _InventoryItemButtons = new List<Button>();

    void Awake()
    {
        Assert.IsNotNull(_HealthImage, "[InGameUI] _healthImage is null");
    }

    public void UpdateHealthBar(float value)
    {
        _HealthImage.fillAmount = value;
    }
}
