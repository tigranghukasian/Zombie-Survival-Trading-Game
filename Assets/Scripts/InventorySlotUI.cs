using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private InventoryItemUI inventoryItemUI;
    [SerializeField] private GameObject ghostObject;
    private InventorySlot slot;
    private Inventory inventory;

    public void Init(InventorySlot _slot, Inventory _inventory)
    {
        slot = _slot;
        inventory = _inventory;
    }
    public void UpdateItemUI(Sprite sprite, int amount)
    {
        amountText.text = amount == 0? "": amount.ToString();
        inventoryItemUI.SetSprite(sprite, amount != 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventorySelectionManager.SelectedSlotReference == null && slot.Id != -1)
        {
            InventorySelectionManager.SelectedSlotReference = slot;
            InventorySelectionManager.SelectedSlotData = new InventorySlot(slot.Id, slot.Amount);
            InventorySelectionManager.GhostItem = Instantiate(ghostObject, transform.parent.parent);
            InventorySelectionManager.GhostItem.GetComponent<GhostInventoryItemUI>().SetSprite(inventory.ItemDatabase.GetItem[slot.Id].sprite, slot.Amount);
            slot.UpdateSlot(-1, 0);

            //CREATE GAMEOBJECT FOR GHOST
        }
        else
        {
            Inventory.SwapItemsFromTempData( slot,InventorySelectionManager.SelectedSlotReference, InventorySelectionManager.SelectedSlotData);
            InventorySelectionManager.SelectedSlotData = null;
            InventorySelectionManager.SelectedSlotReference = null;
            Destroy(InventorySelectionManager.GhostItem);
        }
    }
}
