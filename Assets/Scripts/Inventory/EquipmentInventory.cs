using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : Inventory
{
    
    public EquipmentInventory(int _inventorySize, ItemDatabase _database, Action<InventorySlot> inventorySlotUpdatedCallback) : base(_inventorySize, _database, inventorySlotUpdatedCallback)
    {
        slots[0] = new InventorySlot(inventorySlotUpdatedCallback);
        slots[0].AllowedItems = new ItemType[]
        {
            ItemType.Pistol
        };
        slots[1] = new InventorySlot(inventorySlotUpdatedCallback);
        slots[1].AllowedItems = new ItemType[]
        {
            ItemType.Rifle
        };
        slots[2] = new InventorySlot(inventorySlotUpdatedCallback);
        slots[2].AllowedItems = new ItemType[]
        {
            ItemType.Tool
        };
    }
}
