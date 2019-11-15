using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [Rick H] 2019-11-15
/// service for UI requests
/// </summary>
public class UIManager : MonoBehaviour
{
    public enum ECanvasType
    {
        Mainmenu = 0,
        InGame = 1
    }
    [Header("UI Prefabs")]
    public Canvas mainmenuPrefab;
    public Canvas inGamePrefab;

    public Canvas currentCanvas { get; set; }

    private List<Canvas> _uiInstances = new List<Canvas>();
    private UI_InGame _uiInGame;//a reference to 'UI_Ingame' in 'inGamePrefab'

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (mainmenuPrefab != null)//index 0
        {
            CreateUI(mainmenuPrefab);
        }
        else
        {
            Debug.Log("[UIManager] mainmenuPrefab is null");
        }

        if (inGamePrefab != null)//index 1
        {
            CreateUI(inGamePrefab);
            _uiInGame = _uiInstances[(int)ECanvasType.InGame].GetComponent<UI_InGame>();
         }
        else
        {
            Debug.Log("[UIManager] inGamePrefab is null");
        }

        foreach (var ui in _uiInstances)
        {
            ui.gameObject.SetActive(false);
        }
        currentCanvas = _uiInstances[0];
    }
    private void CreateUI(Canvas canvanPrefab)
    {
        Canvas instance = Instantiate(canvanPrefab);
        DontDestroyOnLoad(instance);
        instance.transform.SetParent(gameObject.transform);
        _uiInstances.Add(instance);
    }

    public void SwitchUI(ECanvasType type)
    {
        currentCanvas.gameObject.SetActive(false);
        currentCanvas = _uiInstances[(int)type];
        currentCanvas.gameObject.GetComponent<UI_Interface>().ResetUI();
        currentCanvas.gameObject.SetActive(true);
    }


    #region public In-game UI Services

    public List<Image> GetQuickSlot()
    {
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])
        {
            return _uiInGame.quickSlots;
        }
        Debug.Log("[UIManager] quick slot not avaliable, current scene does not contain such ui");
        return null;
    }
    public void PopUpMessageBox()
    {
        if (currentCanvas == inGamePrefab)
        {
            _uiInGame.PopUp_MsgBox();
        }
    }

    #endregion

}
