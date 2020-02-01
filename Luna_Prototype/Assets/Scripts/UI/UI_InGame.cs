using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2019-11-14 [Rick H]
/// Control in-game UI,  ui object naming : all [lower case]
/// </summary>
public class UI_InGame : MonoBehaviour, UI_Interface
{
    //gamepad control
    InputController _inputController ;

    //Gauges
    [Header("Gauges")]
    public Image hp_Image;
    public Image stamina_Image;

    //quick slots
    [Header("Quick Slots")]
    public List<Image> quickSlots = new List<Image>();
    [SerializeField] private Sprite _EmptySlot;
    public int selectedSlotInInventory { get; set; }

    //pop up pause menu
    [Header("Pause")]
    [SerializeField]
    private GameObject popUp_pauseGame;
    [SerializeField]
    private GameObject popUp_sureToQuit;


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


    private List<GameObject> _uiPopUpComponents = new List<GameObject>();//list of ref to popup comps
    private float msgBox_timer = 0f;// used by movebox function
    private bool msgBox_isActive = false;//is msg box currently functioning 

    private UIManager _uIManager;

    //
    private void Awake()
    {
        _inputController = new InputController();
        _inputController.UIControl.PopUpMenu.performed += _menu => Button_PauseGame();
        //if (popUp_pauseGame == null)
        {
            popUp_pauseGame = transform.Find("popup_pausegame").gameObject;
            _uiPopUpComponents.Add(popUp_pauseGame);
            popUp_pauseGame.SetActive(false);

            popUp_sureToQuit = transform.Find("popup_suretoquit").gameObject;
            _uiPopUpComponents.Add(popUp_sureToQuit);
            popUp_sureToQuit.SetActive(false);
        }

        //if (popUp_msgbox == null)
        {
            Transform msgboxtrans = transform.Find("MsgBox");
            popUp_msgbox = msgboxtrans.Find("popup_msgbox").gameObject;
            msgBox_From = msgboxtrans.Find("msgbox_from").gameObject.transform;
            msgBox_To = msgboxtrans.Find("msgbox_to").gameObject.transform;
            popUp_msgbox.transform.position = msgBox_From.position;
            _uiPopUpComponents.Add(popUp_msgbox);
            popUp_msgbox.SetActive(false);
        }


        selectedSlotInInventory = 0;
    }
    private void OnEnable()
    {
        _inputController.UIControl.Enable();
        if (_uIManager == null)
            _uIManager = ServiceLocator.Get<UIManager>();
        //_uIManager.SetSelected(transform.Find("pause").gameObject);

    }
    private void OnDisable()
    {

    }



    public void ResetUI()
    {
        foreach (var comp in _uiPopUpComponents)
        {
            comp.SetActive(false);
        }
        foreach (var s in quickSlots)
        {
            s.sprite = null;
        }
        selectedSlotInInventory = 0;

    }

    #region Gauge
    public void UpdateHPGauge(float value)
    {
        hp_Image.fillAmount = value;
    }
    public void UpdateStaminaGauge(float value)
    {
        stamina_Image.fillAmount = value;
    }

    #endregion


    #region PopUp_MsgBox
    public void SetText_MsgBox(string text)
    {
        popUp_msgbox.GetComponentInChildren<Text>().text = text;
    }
    public void PopUp_MsgBox()
    {
        if (!msgBox_isActive)
        {
            popUp_msgbox.SetActive(true);
            StartCoroutine("movebox");
        }
    }
    private IEnumerator movebox()
    {
        msgBox_isActive = true;

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
        msgBox_isActive = false;
        yield return null;
    }
    #endregion

    #region PopUp_PauseGame

    public void Button_PauseGame()
    {
        popUp_pauseGame.SetActive(true);
        _uIManager.SetSelected(popUp_pauseGame.transform.Find("Quit").gameObject);

        //Time.timeScale = 0.0f;
    }

    public void Button_Resume()
    {
        Debug.Log("Button_Resume pressed");
        popUp_pauseGame.SetActive(false);
 
        Time.timeScale = 1.0f;

    }

    public void Button_Quit()
    {
        Debug.Log("Button_Quit pressed");
        popUp_sureToQuit.SetActive(true);
        popUp_pauseGame.SetActive(false);
        _uIManager.SetSelected(transform.Find("popup_suretoquit").transform.Find("no").gameObject);

    }

    public void Button_SureToQuit(int val)//0 no 1 yes
    {

        if (val == 0)
        {
            popUp_sureToQuit.SetActive(false);
        }
        else
        {
            ServiceLocator.Get<GameManager>().SwitchScene(GameManager.ESceneIndex.Mainmenu);
        }
        Time.timeScale = 1.0f;
    }
    #endregion

    #region SelectItem/roll up down
    public void SelectPrevItem(int inventoryCount)
    {
        if (inventoryCount <= 0)
            return;
        selectedSlotInInventory = selectedSlotInInventory - 1 < 0 ? inventoryCount - 1 : selectedSlotInInventory - 1;
        Debug.Log("prev: curr = " + selectedSlotInInventory.ToString());
    }

    public void SelectNextItem(int inventoryCount)
    {
        if (inventoryCount <= 0)
             return;
         selectedSlotInInventory = selectedSlotInInventory + 1 > inventoryCount - 1 ? 0 : selectedSlotInInventory + 1;
        Debug.Log("next: curr = " + selectedSlotInInventory.ToString());

    }

    #endregion

    #region UpdateUI_Img

    //void UpdateQuickSlot_Img(List<Inventory.InventoryItemSlot> invSlots)
    //{
    //    UIManager UIMngr = ServiceLocator.Get<UIManager>();
    //    var quickSlots = UIMngr.GetQuickSlot();
    //    var image_prev = quickSlots[0];//[Rick H] replaced with UIManager service
    //    var image_centre = quickSlots[1];//[Rick H] replaced with UIManager service
    //    var image_next = quickSlots[2];//[Rick H] replaced with UIManager service

    //    if (invSlots.Count <= 0)
    //    {
    //        image_prev.sprite = _EmptySprite;
    //        image_centre.sprite = _EmptySprite;
    //        image_next.sprite = _EmptySprite;

    //        return;
    //    }

    //    int selected = UIMngr.GetSelectedItemInInventory();

    //    int prevIdx = selected - 1 < 0 ? _slots.Count - 1 : selected - 1;

    //    int nextIdx = selected + 1 > _slots.Count - 1 ? 0 : selected + 1;

    //    //Debug.Log("[-=prev,sele,next,slotcount-] " + prevIdx.ToString() +","+ selected.ToString() + "," + nextIdx.ToString() + ","+ _slots.Count.ToString());


    //    image_prev.sprite = _slots[prevIdx].sprite;

    //    image_centre.sprite = _slots[selected].sprite;

    //    image_next.sprite = _slots[nextIdx].sprite;

    //}
    #endregion

}
