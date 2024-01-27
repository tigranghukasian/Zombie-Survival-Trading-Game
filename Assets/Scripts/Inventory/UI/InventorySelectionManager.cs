using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySelectionManager
{
    public static InventorySlot SelectedSlotReference { get; set; }
    public static SlotData SelectedSlotData { get; set; }
    public static GameObject GhostItem { get; set; }

    public static bool IsSlotSelected { get; set; }
    public static Action<SlotData> OnSelectedSlotDataChanged;

    public static void RemoveItemFromSelectedSlotData(int amount)
    {
        var newSelectedSlotData = SelectedSlotData;
        newSelectedSlotData.Amount -= amount;
        SelectedSlotData = newSelectedSlotData;
        OnSelectedSlotDataChanged?.Invoke(SelectedSlotData);
    }

    public struct SlotData
    {
        public int Id;
        public int Amount;
        
        public SlotData(int id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
