using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// [Rick H] a gameobject in scene that directs manager to setup ui
/// </summary>
public class SetUpUI : MonoBehaviour
{
    [SerializeField]
    private UIManager.ECanvasType uiToUse;

    private void Awake()
    {
        ServiceLocator.Get<UIManager>().SwitchUI(uiToUse);
    }


}
