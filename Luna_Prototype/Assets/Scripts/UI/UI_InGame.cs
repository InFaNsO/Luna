using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2019-11-14 [Rick H]
/// Control in-game UI,  ui object naming : all [lower case]
/// </summary>
public class UI_InGame : MonoBehaviour
{
    //pop up pause menu
    [Header("Pause")]
    [SerializeField]
    private GameObject popUp_pauseGame;


    //pop up message box
    [Header("PopUp_Message Box")]

    [SerializeField]
    [Tooltip("msgbox init position")]
    private Transform msgBox_From;
    [SerializeField]
    [Tooltip("msgbox destination")]
    private Transform msgBox_To;
    [SerializeField]
    private GameObject popUp_msgbox;

    [SerializeField]
    private float msgBox_move_time = 1f;
    [SerializeField]
    private float msgBox_move_speed = 1f;
    [SerializeField]
    private float msgBox_stay_time = 1f;

    private float msgBox_timer = 0f;// used by movebox function
    private bool msgBox_isActive = false;//is msg box currently functioning 

    //
    private void Awake()
    {
        if (popUp_pauseGame == null)
        {
            popUp_pauseGame = transform.Find("popup_pausegame").gameObject;
            popUp_pauseGame.SetActive(false);
        }

        if (popUp_msgbox == null)
        {
            popUp_msgbox = transform.Find("popup_msgbox").gameObject;
            msgBox_From = transform.Find("msgbox_from").gameObject.transform;
            msgBox_To = transform.Find("msgbox_to").gameObject.transform;
            popUp_msgbox.transform.position = msgBox_From.position;
            popUp_msgbox.SetActive(false);
        }

    }


    #region PopUp_MsgBox
    public void PopUp_MsgBox()
    {
        popUp_msgbox.SetActive(true);
        StartCoroutine("movebox");
    }
    private IEnumerator movebox()
    {
        //move in
        msgBox_timer = 0.0f;
        while (msgBox_timer < msgBox_move_time)
        {
            msgBox_timer += Time.deltaTime;
            popUp_msgbox.transform.Translate(msgBox_move_speed * Time.deltaTime * new Vector3(msgBox_To.position.x - popUp_msgbox.transform.position.x, msgBox_To.position.y - popUp_msgbox.transform.position.y, msgBox_To.position.z - popUp_msgbox.transform.position.z));
            yield return null;
        }

        //wait
        yield return new WaitForSeconds(msgBox_stay_time);

        //move out
        msgBox_timer = 0.0f;
        while (msgBox_timer < msgBox_move_time)
        {
            msgBox_timer += Time.deltaTime;
            popUp_msgbox.transform.Translate(msgBox_move_speed * Time.deltaTime * new Vector3( msgBox_From.transform.position.x - popUp_msgbox.transform.position.x, msgBox_From.transform.position.y - popUp_msgbox.transform.position.y, msgBox_From.transform.position.z - popUp_msgbox.transform.position.z));
            yield return null;
        }

        //double check
        popUp_msgbox.transform.position = msgBox_From.position;
        popUp_pauseGame.SetActive(false);
        yield return null;
    }
    #endregion

    #region PopUp_PauseGame

    public void Button_PauseGame()
    {
        popUp_pauseGame.SetActive(true);
    }

    public void Button_Resume()
    {
        Debug.Log("Button_Resume pressed");
        popUp_pauseGame.SetActive(false);
    }

    public void Button_Quit()
    {
        Debug.Log("Button_Quit pressed");
        ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.MainMenu);
    }
    #endregion

}
