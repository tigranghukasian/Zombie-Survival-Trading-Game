using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private InventoryItemUI inventoryItemUI;
    [SerializeField] private Image slotTypeBg;
    [SerializeField] private Color slotTypeBgColor;
    [SerializeField] private Sprite pistolSlotSprite;
    [SerializeField] private Sprite rifleSlotSprite;
    [SerializeField] private Sprite toolSlotSprite;
    [SerializeField] private GameObject ghostObject;
    private InventorySlot slot;
    private Inventory inventory;

    public void Init(InventorySlot _slot, Inventory _inventory)
    {
        slot = _slot;
        inventory = _inventory;

        SetupSlotAllowedItemsBg();
    }

    private void SetupSlotAllowedItemsBg()
    {
        slotTypeBg.color = Color.clear;
        if (slot.AllowedItems.Length == 0)
        {
            return;
        }
        if (slot.AllowedItems[0] == ItemType.Pistol)
        {
            slotTypeBg.sprite = pistolSlotSprite;
            slotTypeBg.color = slotTypeBgColor;
        }
        else if (slot.AllowedItems[0] == ItemType.Rifle)
        {
            slotTypeBg.sprite = rifleSlotSprite;
            slotTypeBg.color = slotTypeBgColor;
        }
        else if (slot.AllowedItems[0] == ItemType.Tool)
        {
            slotTypeBg.sprite = toolSlotSprite;
            slotTypeBg.color = slotTypeBgColor;
        }
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
            InventorySelectionManager.GhostItem = Instantiate(ghostObject, Input.mousePosition,Quaternion.identity, transform.parent.parent);
            InventorySelectionManager.GhostItem.GetComponent<GhostInventoryItemUI>().SetSprite(inventory.ItemDatabase.GetItem(slot.Id).sprite, slot.Amount);
            slot.UpdateSlot(-1, 0);

            //CREATE GAMEOBJECT FOR GHOST
        }
        else if(InventorySelectionManager.SelectedSlotReference != null)
        {
            ItemDatabase database = inventory.ItemDatabase;
            var selectedSlotReference = InventorySelectionManager.SelectedSlotReference;
            var selectedSlotData = InventorySelectionManager.SelectedSlotData;
            var newSlot = slot;
            var selectedSlotItem = database.GetItem(selectedSlotData.Id);
            var newSlotItem = database.GetItem(newSlot.Id);

            if (selectedSlotReference.CanPlaceItem(newSlotItem) && newSlot.CanPlaceItem(selectedSlotItem))
            {
                Inventory.SwapItemsFromTempData(newSlot, selectedSlotReference, selectedSlotData);
            }
            else
            {
                InventorySelectionManager.SelectedSlotReference.UpdateSlot(InventorySelectionManager.SelectedSlotData.Id,InventorySelectionManager.SelectedSlotData.Amount);
            }
            InventorySelectionManager.SelectedSlotData = null;
            InventorySelectionManager.SelectedSlotReference = null;
            Destroy(InventorySelectionManager.GhostItem);
        }
    }
}
