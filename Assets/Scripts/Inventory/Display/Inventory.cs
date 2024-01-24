using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryObject inventoryObject;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private InventoryItemDisplay mouseObjectDisplayPrefab;
    private Dictionary<InventorySlotDisplay, InventorySlot> inventorySlots = new Dictionary<InventorySlotDisplay, InventorySlot>();
    private bool isSelected;
    private InventoryItemDisplay mouseObject;

    private InventorySlot selectedSlot;
    //private 
    private void Start()
    {
        CreateSlots();
    }

    private void UpdateMouseObjectPosition()
    {
        if (isSelected && mouseObject != null)
        {
            mouseObject.transform.position = Input.mousePosition;
        }
    }
    public void CreateSlots()
    {
        for (int i = 0; i < inventoryObject.Container.Slots.Length; i++)
        {
            var invSlot = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<InventorySlotDisplay>();
            inventorySlots.Add(invSlot, inventoryObject.Container.Slots[i]);
            
            AddEvent(invSlot, EventTriggerType.PointerDown, delegate { OnPointerDown(invSlot); });
        }
    }

    private void OnPointerDown(InventorySlotDisplay invSlot)
    {
        if (!isSelected)
        {
            if (inventorySlots[invSlot].Id >= 0)
            {
                mouseObject = Instantiate(mouseObjectDisplayPrefab, transform.parent).GetComponent<InventoryItemDisplay>();
                mouseObject.SetSprite(inventoryObject.DatabaseObject.GetItem[inventorySlots[invSlot].Id].sprite);
                mouseObject.GetComponent<Image>().raycastTarget = false;
                selectedSlot = inventorySlots[invSlot];
                isSelected = true;
            }
        }
        else
        {
            InventorySlot currentSlot = inventorySlots[invSlot];
            InventorySlot tempSlot = new InventorySlot(currentSlot.Id, currentSlot.Item, currentSlot.Amount);
            inventorySlots[invSlot] = selectedSlot;
            selectedSlot = tempSlot;
        }
    }

    private void UpdateSlots()
    {
        foreach (KeyValuePair<InventorySlotDisplay, InventorySlot> _slot in inventorySlots)
        {
            if (_slot.Value.Id != -1)
            {
                _slot.Key.GetComponentInChildren<InventoryItemDisplay>().SetSprite(inventoryObject.DatabaseObject.GetItem[_slot.Value.Item.Id].sprite);
                _slot.Key.SetAmount(_slot.Value.Amount);
            }
            else
            {
                _slot.Key.GetComponentInChildren<InventoryItemDisplay>().HideSprite();
                _slot.Key.SetAmount(0);
            }
        }
    }

    private void AddEvent(InventorySlotDisplay obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    private void Update()
    {
        UpdateSlots();
        UpdateMouseObjectPosition();
    }
}
