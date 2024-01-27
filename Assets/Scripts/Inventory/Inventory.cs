using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory
{
    [SerializeField] protected InventorySlot[] slots;
    [SerializeField] private ItemDatabase itemDatabase;

    public ItemDatabase ItemDatabase => itemDatabase;

    public int GetSlotsLength()
    {
        return slots.Length;
    }

    public InventorySlot GetSlot(int i)
    {
        if (Utilities.IsIndexValid(slots, i))
        {
            return slots[i];
        }

        return null;
    }


    public Inventory(int _inventorySize, ItemDatabase _database, Action<InventorySlot> inventorySlotUpdatedCallback)
    {
        slots = new InventorySlot[_inventorySize];
        itemDatabase = _database;
    }

    public void AddItem(Item item, int amount)
    {
        Debug.Log("ADD ITEM " + item.name + " AMOUNT " + amount);
        if (item.stackSize <= 0)
        {
            return;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Id == item.id && slots[i].Amount < item.stackSize)
            {
                
                int spaceLeft = item.stackSize - slots[i].Amount;
                if (spaceLeft >= amount)
                {
                    slots[i].UpdateSlot(item.id, slots[i].Amount + amount);
                    return;
                }
                slots[i].UpdateSlot(item.id, slots[i].Amount + spaceLeft);
                amount -= spaceLeft;
            }
        }
        while (amount > 0)
        {
            InventorySlot emptySlot = SetFirstEmptySlot(item, Mathf.Min(amount, item.stackSize));
            if (emptySlot == null)
            {
                // TODO: Handle the case where the inventory is full.
                break;
            }
            amount -= emptySlot.Amount;
        }
      
    }

    private InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Id == -1)
            {
                slots[i].UpdateSlot(item.id, amount);
                return slots[i];
            }
        }
        //TODO: INVENTORY FULL
        return null;
    }
    
    public static void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        InventorySlot tempSlot = new InventorySlot(slot1.Id, slot1.Amount);
        slot1.UpdateSlot(slot2.Id, slot2.Amount);
        slot2.UpdateSlot(tempSlot.Id, tempSlot.Amount);
    }
    public static void SwapItemsFromTempData(InventorySlot newSlot, InventorySlot oldSlot, InventorySelectionManager.SlotData tempData)
    {
        InventorySlot tempSlot = new InventorySlot(newSlot.Id, newSlot.Amount);
        oldSlot.UpdateSlot(tempSlot.Id, tempSlot.Amount);
        newSlot.UpdateSlot(tempData.Id, tempData.Amount);
    }
}
