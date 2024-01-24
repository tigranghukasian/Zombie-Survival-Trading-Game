using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private InventoryObject inventoryObject;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private InventoryItemDisplay mouseObjectDisplayPrefab;
    private Dictionary<InventorySlotDisplay, InventorySlot> inventorySlots = new Dictionary<InventorySlotDisplay, InventorySlot>();
    private bool isSelected;
    private InventoryItemDisplay mouseObject;

    private InventorySlot selectedSlot;

    private InventorySlot selectedSlotData;
    //private 
    private void Start()
    {
        inventoryObject.Load();
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
                mouseObject.SetSprite(inventoryObject.Database.GetItem[inventorySlots[invSlot].Id].sprite);
                mouseObject.GetComponent<Image>().raycastTarget = false;
                selectedSlot = inventorySlots[invSlot];
                isSelected = true;
                selectedSlotData = new InventorySlot( selectedSlot.Id,selectedSlot.Item,selectedSlot.Amount);
                selectedSlot.UpdateSlot(-1, null, 0);
            }
        }
        else
        {
            InventorySlot currentSlot = inventorySlots[invSlot];
            MoveItem(currentSlot, selectedSlot, selectedSlotData);
            selectedSlot = null;
            isSelected = false;
            Destroy(mouseObject.gameObject);
        }
    }

    private void MoveItem(InventorySlot slot1, InventorySlot slot2, InventorySlot slot2Data)
    {
        slot2.UpdateSlot(slot1.Id, slot1.Item, slot1.Amount);
        slot1.UpdateSlot(slot2Data.Id, slot2Data.Item, slot2Data.Amount);

    }

    private void UpdateSlots()
    {
        foreach (KeyValuePair<InventorySlotDisplay, InventorySlot> _slot in inventorySlots)
        {
            if (_slot.Value.Id != -1)
            {
                _slot.Key.GetComponentInChildren<InventoryItemDisplay>().SetSprite(inventoryObject.Database.GetItem[_slot.Value.Item.ItemObject.Id].sprite);
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

    private void OnApplicationQuit()
    {
        inventoryObject.Save();
    }
}
