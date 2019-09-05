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
    int _totalSlot = 9;

    void Awake()
    {
        Assert.IsNotNull(_player, "[Inventroy] _Player is null");
    }

    public void UsingItem(int whichSlot)
    {
        if (whichSlot > _slots.Count)
            return;
        if (_slots[whichSlot].theItem == null)
            return;

        if(!_slots[whichSlot].theItem.Use(ref _player))
        {
            _slots[whichSlot].itemCount -= 1;
            if (_slots[whichSlot].itemCount == 0)
            {
                _slots[whichSlot].theItem = null;
                _slots[whichSlot].sprite = _EmptySprite;
                UpdateUI(whichSlot);
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
        var image = _player._LocalLevelManager._InGameUI._InventoryItemButtons[whichSlot].GetComponent<Image>();
        image.sprite = _slots[whichSlot].sprite;
    }

}
