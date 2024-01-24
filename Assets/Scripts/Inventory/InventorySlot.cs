using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    private int id;
    private int amount;

    public ItemType[] AllowedItems = new ItemType[0];
    

    public int Id => id;
    public int Amount => amount;
    private Action<InventorySlot> inventorySlotUpdatedCallback;

    public InventorySlot(Action<InventorySlot> _inventorySlotUpdatedCallback)
    {
        id = -1;
        amount = 0;
        inventorySlotUpdatedCallback = _inventorySlotUpdatedCallback;
    }

    public InventorySlot(int _id, int _amount)
    {
        id = _id;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
        inventorySlotUpdatedCallback?.Invoke(this);
    }

    public void UpdateSlot(int _id, int _amount)
    {
        id = _id;
        amount = _amount;
        inventorySlotUpdatedCallback?.Invoke(this);
    }

    public bool CanPlaceItem(Item item)
    {
        if (item == null)
        {
            return true;
        }
        if (AllowedItems.Length <= 0)
        {
            return true;
        }
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (item.type == AllowedItems[i])
            {
                return true;
            }
        }

        return false;
    }
}
