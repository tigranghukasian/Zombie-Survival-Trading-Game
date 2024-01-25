using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Dictionary<InventorySlot, InventorySlotUI> inventorySlotUis = new Dictionary<InventorySlot, InventorySlotUI>();
    [SerializeField] private GameObject inventorySlotUiPrefab;
    [SerializeField] private GameObject selectionUI;
    

    public void Init(Inventory _inventory)
    {
        inventory = _inventory;
        for (int i = 0; i < inventory.GetSlotsLength(); i++)
        {
            var invSlot = Instantiate(inventorySlotUiPrefab, transform);
            var slot = inventory.GetSlot(i);
            invSlot.GetComponent<InventorySlotUI>().Init(slot, inventory);
            inventorySlotUis.Add(slot, invSlot.GetComponent<InventorySlotUI>());
        }
    }

    public void AddSelectedCrownToSlot(InventorySlot slot)
    {
        if (selectionUI == null)
        {
            return;
        }
        var selectedSlotUI = inventorySlotUis[slot];
        selectionUI.gameObject.SetActive(true);
        
        //This had to be called, because otherwise the positioning of the selectedSlotUI was wrong
        LayoutRebuilder.ForceRebuildLayoutImmediate(selectedSlotUI.transform.parent.GetComponent<RectTransform>());
        
        selectionUI.transform.position = selectedSlotUI.transform.position;
    }
    
    public void UpdateSlotUI(InventorySlot updatedSlot)
    {
        var invSlot = inventorySlotUis[updatedSlot];
        if (invSlot != null)
        {
            if (updatedSlot.Id == -1)
            {
                invSlot.UpdateItemUI(null, 0);
                return;
            }
            invSlot.UpdateItemUI(inventory.ItemDatabase.GetItem(updatedSlot.Id)?.sprite, updatedSlot.Amount);
        }
    }
}
