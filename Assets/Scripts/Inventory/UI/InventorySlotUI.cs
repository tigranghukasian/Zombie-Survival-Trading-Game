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
    private InventoryUI inventoryUI;
    private InventorySlot slot;
    private Inventory inventory;
    private ItemDatabase database;
    private Player player;

    public void Init(InventoryUI _inventoryUI, InventorySlot _slot, Inventory _inventory, Player _player)
    {
        inventoryUI = _inventoryUI;
        slot = _slot;
        inventory = _inventory;
        database = inventory.ItemDatabase;
        player = _player;

        SetupSlotAllowedItemsBg();
    }

    private void SetupSlotAllowedItemsBg()
    {
        slotTypeBg.color = Color.clear;
        if (slot.AllowedItems.Length == 0)
        {
            return;
        }
        if (slot.AllowedItems[0] == ItemType.Gun)
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
        amountText.text = amount is 0 or 1? "": amount.ToString();
        inventoryItemUI.SetSprite(sprite, amount != 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (slot.Id != -1 && database.GetItem(slot.Id) is ItemConsumable)
            {
                var consumable = (ItemConsumable)database.GetItem(slot.Id);
                if (slot.Amount - 1 > 0)
                {
                    slot.UpdateSlot(slot.Id, slot.Amount - 1);
                }
                else
                {
                    slot.UpdateSlot(-1, 0);
                }
                if (consumable.consumeSound != null)
                {
                    inventoryUI.PlaySound(consumable.consumeSound);
                }
             
                player.AddHealth(consumable.restoreHealthAmount);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(!InventorySelectionManager.IsSlotSelected) 
            {
                if (slot.Id != -1)
                {
                    var slotItem = database.GetItem(slot.Id);
                    if (slotItem is ItemStructure)
                    {
                        ItemStructure structureItem = (ItemStructure)slotItem;
                        StructurePlacementManager.Instance.SetCurrentActiveStructurePrefab(structureItem.structurePrefab);
                    }
                    SelectSlot();
                    CreateGhostItem();
                    slot.UpdateSlot(-1, 0);
                }
            
            }
            else {
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

                StartCoroutine(DelayedDeselect());
            }
        }
    }

    private void SelectSlot()
    {
        InventorySelectionManager.SelectedSlotReference = slot;
        InventorySelectionManager.SelectedSlotData = new InventorySelectionManager.SlotData(slot.Id, slot.Amount);
        InventorySelectionManager.IsSlotSelected = true;
        InventorySelectionManager.OnSelectedSlotDataChanged += OnSelectedSlotDataChanged;
    }

    private void DeselectSlot()
    {
        InventorySelectionManager.SelectedSlotData = new InventorySelectionManager.SlotData { Id = -1, Amount = 0 };
        InventorySelectionManager.SelectedSlotReference = null;
        InventorySelectionManager.IsSlotSelected = false;
        InventorySelectionManager.OnSelectedSlotDataChanged -= OnSelectedSlotDataChanged;
        StructurePlacementManager.Instance.SetCurrentActiveStructurePrefab(null);
        DestroyGhostItem();
    }

    private void OnSelectedSlotDataChanged(InventorySelectionManager.SlotData slotData)
    {
        if (slotData.Amount <= 0)
        {
            StartCoroutine(DelayedDeselect());
        }
    }
    
    private IEnumerator DelayedDeselect()
    {
        yield return new WaitForSeconds(0.01f);
        DeselectSlot();
    }

    private void CreateGhostItem()
    {
        InventorySelectionManager.GhostItem = Instantiate(ghostObject, Input.mousePosition,Quaternion.identity, transform.parent.parent);
        InventorySelectionManager.GhostItem.GetComponent<GhostInventoryItemUI>().SetSprite(inventory.ItemDatabase.GetItem(slot.Id).sprite, slot.Amount);
    }

    private void DestroyGhostItem()
    {
        Destroy(InventorySelectionManager.GhostItem);
    }
}
