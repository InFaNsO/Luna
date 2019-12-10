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

    public List<InventoryItemSlot> _slots = new List<InventoryItemSlot>();
    private int _totalSlot = 9;

    void Awake()
    {
        Assert.IsNotNull(_player, "[Inventroy] _Player is null");
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
                UpdateUI(whichSlot);
                _slots.RemoveAt(whichSlot);
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
            UpdateUI(_slots.Count - 1);
        }
    }

    private void UpdateUI(int whichSlot)
    {
        //var image = _player._LocalLevelManager._InGameUI._InventoryItemButtons[whichSlot].GetComponent<Image>();

        int prevIdx = whichSlot - 1 < 0 ? _slots.Count - 1 : whichSlot - 1;

        int nextIdx = whichSlot + 1 > _slots.Count - 1 ? 0 : whichSlot + 1;


        var image_centre = ServiceLocator.Get<UIManager>().GetQuickSlot()[0];//[Rick H] replaced with UIManager service
        image_centre.sprite = _slots[whichSlot].sprite;

        var image_prev = ServiceLocator.Get<UIManager>().GetQuickSlot()[1];//[Rick H] replaced with UIManager service
        image_prev.sprite = _slots[prevIdx].sprite;

        var image_next = ServiceLocator.Get<UIManager>().GetQuickSlot()[2];//[Rick H] replaced with UIManager service
        image_next.sprite = _slots[nextIdx].sprite;
    }

}
