using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventorySelectionManager
{
    public static InventorySlot SelectedSlotReference { get; set; }
    public static InventorySlot SelectedSlotData { get; set; }
    public static GameObject GhostItem { get; set; }
}
