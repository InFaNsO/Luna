using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    UI_InputController controls;

    private GameObject startScreen;
    private UI_MainMenu _UIMainMenu;

    private void Awake()
    {
        _UIMainMenu = GetComponent<UI_MainMenu>();
        controls = new UI_InputController();
        controls.UIMainmenuCtrl.ClickTitleCover.performed += _start => ClickTitleCover();
 
    }
    public void ClickTitleCover()
    {
        Debug.Log("ClickTitleCover");
        _UIMainMenu.Touch_StartScreen();
    }
    public void GameStart()
    {
        Debug.Log("game start");
        ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Level1);
    }
    private void FixedUpdate()
    {
        
    }
    private void Update()
    {
      }
    private void OnEnable()
    {
        controls.UIMainmenuCtrl.Enable();

    }
    private void OnDisable()
    {
        controls.UIMainmenuCtrl.Disable();

    }
}