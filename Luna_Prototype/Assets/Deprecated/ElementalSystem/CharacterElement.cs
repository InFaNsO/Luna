using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalStats : MonoBehaviour
{
    private ElementalAttributes stats;

    public void Add(ElementalAttributes a)
    {
        stats = stats + a;
    }

    public void Sub(ElementalAttributes a)
    {
        stats = stats - a;
    }

    private void Start()
    {
        stats = GetComponent<ElementalAttributes>();
    }
}
