using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Inventory : MonoBehaviour
{
    public Sprite _EmptySprite;
    public Player _player;
    public class InventoryItemSlot
    {
        public int itemCount = 0;
        public IInventoryItem theItem;
        public Sprite sprite;
    }

    //---------------------------------------------------//|
    public class EventItemSlot                           //|
    {                                                    //|
        public IInventoryItem theItem;                   //|--- [Mingzhuo Zhang] new added 2020-03-12
        public Sprite sprite;                            //|
    }                                                    //|
    //---------------------------------------------------//|

    public List<InventoryItemSlot> _slots = new List<InventoryItemSlot>();
    private int _totalSlot = 9;

    public List<EventItemSlot> _eventItemSlots = new List<EventItemSlot>();
    private int _totalEventItemSlot = 3;


    //[Rick H], 2019-12-09, uimanager cache
    private UIManager _uiMngr = null;

    void Awake()
    {
        Assert.IsNotNull(_player, "[Inventroy] _Player is null");
    }
    private void Start()
    {
        _uiMngr = ServiceLocator.Get<UIManager>();

    }
    public void UsingItem(int whichSlot)
    {
        if (whichSlot > _slots.Count - 1)
            return;
        if (_slots[whichSlot].theItem == null)
            return;

        if(!_slots[whichSlot].theItem.Use(ref _player))
        {
            _slots[whichSlot].itemCount -= 1;
            if (_slots[whichSlot].itemCount == 0)
            {
                _slots[whichSlot].sprite = _EmptySprite;
                _slots[whichSlot].theItem = null;
                _slots.RemoveAt(whichSlot);//[Rick H], 2019-12-09, swaped this line with the one below
                UpdateUI();
            }
        }
    }


    public void AddItem(IInventoryItem item)
    {
        int addSlot = -1;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].theItem.GetTypeName() == item.GetTypeName())
            {
                _slots[i].itemCount++;
                //_slots[i].theItem = item;
                item.DisableFromLevel();
                //_slots[i].sprite = item.GetSprite();
                addSlot = i;
                break;
            }
        }

        if (addSlot == -1)
        {
            if (_slots.Count == _totalSlot)
                return;                                                 // early out
            InventoryItemSlot newItemSlot = new InventoryItemSlot();
            newItemSlot.itemCount = 1;
            newItemSlot.theItem = item;
            newItemSlot.theItem.DisableFromLevel();
            newItemSlot.sprite = item.GetSprite();

            _slots.Add(newItemSlot);
        }
        UpdateUI();
    }

    //--------------------------------------------------------------------------------------------------------------------//|
    public void AddEventItem(IInventoryItem item)                                                                         //|
    {                                                                                                                     //|
                                                                                                                          //|
            if (_eventItemSlots.Count == _totalEventItemSlot)                                                             //|
                return;                                                 // early out                                      //|
            EventItemSlot newItemSlot = new EventItemSlot();                                                              //|
            newItemSlot.theItem = item;                                                                                   //|
            newItemSlot.theItem.DisableFromLevel();                                                                       //|
            newItemSlot.sprite = item.GetSprite();                                                                        //|
                                                                                                                          //|
            _eventItemSlots.Add(newItemSlot);                                                                             //|
            UpdateUI();                                                                                                   //|
                                                                                                                          //|
    }                                                                                                                     //|
                                                                                                                          //|--- [Mingzhuo Zhang] added 2020-03-12
    public int SearchEventItem(string name) // return the index if found, otherwise return -1                             //|
    {                                                                                                                     //|
        for (int i = 0; i < _eventItemSlots.Count; i++)                                                                   //|
        {                                                                                                                 //|
            if (_eventItemSlots[i].theItem.GetTypeName() == name)                                                         //|
                return i;                                                                                                 //|
        }                                                                                                                 //|
        return -1;                                                                                                        //|
    }                                                                                                                     //|

    public int GetSpecificEventItem(string name) // return the index if found, otherwise return -1                             //|
    {                                                                                                                     //|
        int count = 0;
        for (int i = 0; i < _eventItemSlots.Count; i++)                                                                   //|
        {                                                                                                                 //|
            if (_eventItemSlots[i].theItem.GetTypeName() == name)                                                         //|
                ++count;
        }                                                                                                                 //|
        return count;                                                                                                        //|
    }
    //|
    public void UsingEventItem(int whichSlot)                                                                             //|
    {                                                                                                                     //|
        if (whichSlot > _eventItemSlots.Count - 1)                                                                        //|
            return;                                                                                                       //|
        if (_eventItemSlots[whichSlot].theItem == null)                                                                   //|
            return;                                                                                                       //|
                                                                                                                          //|
        if (!_eventItemSlots[whichSlot].theItem.Use(ref _player))                                                         //|
        {                                                                                                                 //|
            _eventItemSlots[whichSlot].sprite = _EmptySprite;                                                             //|
            _eventItemSlots[whichSlot].theItem = null;                                                                    //|
            _eventItemSlots.RemoveAt(whichSlot);//[Rick H], 2019-12-09, swaped this line with the one below               //|
            UpdateUI();                                                                                                   //|
        }                                                                                                                 //|
    }                                                                                                                     //|
    //--------------------------------------------------------------------------------------------------------------------//|

    public void UpdateUI()
    {
        //var image = _player._LocalLevelManager._InGameUI._InventoryItemButtons[whichSlot].GetComponent<Image>();

        //[Rick H], 2019-12-09, spirte for items should be kept by [UI_Ingame] instead of [Inventory]
        //[Rick H], 2019-12-09, following code might need to be put into [UI_Ingame] and [UIManager]
        //UIManager UIMngr = ServiceLocator.Get<UIManager>();
        var quickSlots = _uiMngr.GetQuickSlot();
        var image_prev = quickSlots[0];//[Rick H] replaced with UIManager service
        var image_centre = quickSlots[1];//[Rick H] replaced with UIManager service
        var image_next = quickSlots[2];//[Rick H] replaced with UIManager service

        //--------------------------------------------------------------------------------//|
        var eventItemSlots = _uiMngr.GetEventItemSlot();                                  //|
        //if (_eventItemSlots.Count <= 0)
        
            foreach (var item in eventItemSlots)
            {
                item.sprite = _EmptySprite;
            }

        
        //else
        //{
            for (int i = 0; i < _eventItemSlots.Count; i++)                               //|
            {                                                                             //|
                if (i >= eventItemSlots.Count)                                            //|--- [Mingzhuo Zhang] added 2020-03-12
                    break;                                                                //|
                                                                                          //|
                eventItemSlots[i].sprite = _eventItemSlots[i].sprite;                     //|
            }                                                                             //|
                                                                                          
        //}
        //--------------------------------------------------------------------------------//|


        if (_slots.Count <= 0)
        {
            image_prev.sprite = _EmptySprite;
            image_centre.sprite = _EmptySprite;
            image_next.sprite = _EmptySprite;
            ServiceLocator.Get<UIManager>().InGame_QuickSlot_Itemcount_UpdateItemCount(0, 0);
            return;
        }
        else
        {
            int selected = _uiMngr.GetSelectedItemInInventory();
            if (selected > _slots.Count - 1)
            {
                _uiMngr.MoveSelectedItemIndex(_slots.Count - 1);
                selected = _uiMngr.GetSelectedItemInInventory();
            }

            //prev code,
            //int prevIdx = selected - 1 < 0 ? _slots.Count - 1 : selected - 1;
            //int nextIdx = selected + 1 > _slots.Count - 1 ? 0 : selected + 1;

            //new 
            int prevIdx = selected - 1;

            int nextIdx = selected + 1;

            //Debug.Log("[-=prev,sele,next,slotcount-] " + prevIdx.ToString() +","+ selected.ToString() + "," + nextIdx.ToString() + ","+ _slots.Count.ToString());


            image_prev.sprite = prevIdx < 0 ? _EmptySprite : _slots[prevIdx].sprite;

            image_centre.sprite = _slots[selected].sprite;

            image_next.sprite = nextIdx > _slots.Count - 1 ? _EmptySprite : _slots[nextIdx].sprite;

            ServiceLocator.Get<UIManager>().InGame_QuickSlot_Itemcount_UpdateItemCount(selected, _slots[selected].itemCount);

        }





    }

    public int GetCount() { return _slots.Count; }

    public void UseItem()
    {
        UsingItem(_uiMngr.GetSelectedItemInInventory());
        UpdateUI();
    }

    public void SelectPrevItem()
    {
        _uiMngr.SelectPrevItem(_slots.Count);
        UpdateUI();
    }

    public void SelectNextItem()
    {
        _uiMngr.SelectNextItem(_slots.Count);
        UpdateUI();
    }
}
