using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    private EventSystem _eventSystem;
    public EventSystem EventSystem { get => _eventSystem; }
    private GameObject _lastSelectedGO;
    //gamepad control
    //private InputController _inputController;
    //public InputController InputController { get=> _inputController; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _eventSystem = GetComponentInChildren<EventSystem>();
        if (_eventSystem == null)
        {
            Debug.Log("[UIManager] _eventSystem is null");
        }
 

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
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    //private void Update()
    //{
    //    if (_eventSystem.currentSelectedGameObject != null)
    //    {
    //        _lastSelectedGO = _eventSystem.currentSelectedGameObject;
    //    }
    //}

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

    public void SetSelected(GameObject gameObject)
    {
        _eventSystem.SetSelectedGameObject(gameObject);
     }
    //public void SetSelectedAsLastSelected()
    //{
    //    _eventSystem.SetSelectedGameObject(_lastSelectedGO);
    //}

    public void UpdateTimeCountDown(float time)
    {
        _uiInGame.UpdateTimeCountDown(time);
    }
    public void ClearTimeCountDown()
    {
        _uiInGame.ClearTimeCountDown();
    }

    #region public In-game UI Services

    public void InGame_QuickSlot_Itemcount_UpdateItemCount(int slot, int count)
    {
        _uiInGame.UpdateItemCount(slot, count);
    }


    public void UI_Ingame_UpdateWeaponSprite(Sprite currWeapon, Sprite secWeapon)
    {
        _uiInGame.UI_Ingame_UpdateWeaponSprite(currWeapon, secWeapon);
    }



    public void UpdateHPGauge(float val)
    {
        _uiInGame.UpdateHPGauge(val);
    }
    public void UpdateStaminaGauge(float val)
    {
        _uiInGame.UpdateStaminaGauge(val);
    }

    public List<Image> GetQuickSlot()
    {
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])
        {
            return _uiInGame.quickSlots;
        }
        Debug.Log("[UIManager] quick slot not avaliable, current scene does not contain such ui");
        return null;
    }

    //-------------------------------------------------------------------------------------------------------//|
    public List<Image> GetEventItemSlot()                                                                    //|
    {                                                                                                        //|
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])                                          //|
        {                                                                                                    //|
            return _uiInGame.eventItemSlots;                                                                     //|--- [Mingzhuo Zhang] added 2020-03-12
        }                                                                                                    //|
        Debug.Log("[UIManager] event item slot not avaliable, current scene does not contain such ui");           //|
        return null;                                                                                         //|
    }                                                                                                        //|
    //-------------------------------------------------------------------------------------------------------//|

    public int GetSelectedItemInInventory()
    {
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])
        {
            return _uiInGame.selectedSlotInInventory;
        }
        Debug.Log("[UIManager] quick slot not avaliable, current scene does not contain such ui");
        return 0;
    }

    /// <summary>
    /// update the [selectedItemIndex] when [inventory] size changed
    /// </summary>
    /// <param name="moveToIndex"></param>
    public void MoveSelectedItemIndex(int moveToIndex)
    {
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])
        {
            _uiInGame.selectedSlotInInventory = moveToIndex;
        }else
        Debug.Log("[UIManager] quick slot not avaliable, current scene does not contain such ui");
     }

    public void SelectPrevItem(int inventoryCount)
    {
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])
        {
            _uiInGame.SelectPrevItem(inventoryCount);
        }else
        Debug.Log("[UIManager] quick slot not avaliable, current scene does not contain such ui");
     }
    public void SelectNextItem(int inventoryCount)
    {
        if (currentCanvas == _uiInstances[(int)ECanvasType.InGame])
        {
            _uiInGame.SelectNextItem(inventoryCount);
        }else
        Debug.Log("[UIManager] quick slot not avaliable, current scene does not contain such ui");
    }


    public void PopUpMessageBox(float duration, string text, Sprite background)
    {
        //if (currentCanvas == inGamePrefab)
        {
            _uiInGame.PopUp_MsgBox(duration,text,background);
        }
    }

    #endregion

}
