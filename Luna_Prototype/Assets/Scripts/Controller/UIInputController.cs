using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    InputController controls;

    private void Awake()
    {
        controls = new InputController();
        controls.UIControl.GameStart.performed += _start => GameStart();
    }

    public void GameStart()
    {
        Debug.Log("game start");
        ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Level1);
    }
}