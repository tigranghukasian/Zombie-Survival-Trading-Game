using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryType
{
    Player,
    Equipment,
    Trader,
    Chest
}
public static class InventoryFactory
{
    public static Inventory CreateInventory(InventoryType type, ItemDatabase database, Action<InventorySlot> inventorySlotUpdatedCallback)
    {
        switch (type)
        {
            case InventoryType.Player:
                return new PlayerInventory(12, database, inventorySlotUpdatedCallback);
            case InventoryType.Equipment:
                return new EquipmentInventory(3, database, inventorySlotUpdatedCallback);
            case InventoryType.Trader:
                return new TraderInventory(1, database, inventorySlotUpdatedCallback);
            default:
                throw new ArgumentException("Invalid inventory type");
        }
    }
}
