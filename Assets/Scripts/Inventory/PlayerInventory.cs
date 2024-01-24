using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public PlayerInventory(int _inventorySize, ItemDatabase _database, Action<InventorySlot> inventorySlotUpdatedCallback) : base(_inventorySize, _database, inventorySlotUpdatedCallback)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot(inventorySlotUpdatedCallback);
        }
    }
}
