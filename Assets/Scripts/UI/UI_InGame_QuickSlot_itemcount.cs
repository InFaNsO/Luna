using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_QuickSlot_itemcount : MonoBehaviour
{
    [SerializeField]
    private List<Text> _ItemCount;

    private void Awake()
    {

        foreach (var t in _ItemCount)
        {
            t.text = "0";
        }
    }

    public void UpdateItemCount(int slot , int count)
    {
        _ItemCount[slot].text = count.ToString();
    }
}
