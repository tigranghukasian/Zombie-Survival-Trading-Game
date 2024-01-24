using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private InventorySlot[] slots;
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
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot(inventorySlotUpdatedCallback);
        }

        itemDatabase = _database;
    }

    public void AddItem(Item item, int amount)
    {
        if (item.stackSize <= 0)
        {
            return;
        }
        bool added = false;
        while (amount > 0)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Id == item.id && slots[i].Amount < item.stackSize)
                {
                    int spaceInSlot = item.stackSize - slots[i].Amount;
                    int amountToAdd = Mathf.Min(spaceInSlot, amount);

                    slots[i].AddAmount(amountToAdd);
                    amount -= amountToAdd;
                    added = true;
                    if (amount <= 0)
                    {
                        break;
                    }
                }
            }
            if (!added)
            {
                if (SetFirstEmptySlot(item, Mathf.Min(item.stackSize, amount)) == null)
                {
                    // TODO : INVENTORY FULL
                    break;
                }
                amount -= Mathf.Min(item.stackSize, amount);
            }
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
    public static void SwapItemsFromTempData(InventorySlot slot1, InventorySlot emptySlot, InventorySlot tempData)
    {
        InventorySlot tempSlot = new InventorySlot(slot1.Id, slot1.Amount);
        emptySlot.UpdateSlot(tempSlot.Id, tempSlot.Amount);
        slot1.UpdateSlot(tempData.Id, tempData.Amount);
    }
}
