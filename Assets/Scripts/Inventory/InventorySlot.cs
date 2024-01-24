using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    private int id;
    private int amount;
    

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
}
