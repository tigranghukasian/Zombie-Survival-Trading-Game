using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventoryHolder equipmentHolder;

    private int selectedEquipmentSlot = 0;
    [SerializeField] private IEquipable equippedItem;
    [SerializeField] private Gun gun;

    private void Awake()
    {
        Application.targetFrameRate = -1;
    }

    private void Start()
    {
        //quipmentHolder.Inventory.GetSlot(0).inventorySlotUpdatedCallback += SlotUpdated;
        SelectEquipmentSlot(0);
    }

    private void SlotUpdated(InventorySlot slot)
    {
        
    }

    private void Update()
    {
        CheckKeysForSelectingEquipment();
        if (Input.GetMouseButtonDown(0) && equippedItem != null)
        {
            equippedItem.Fire();
        }
    }

    private void CheckKeysForSelectingEquipment()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectEquipmentSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectEquipmentSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectEquipmentSlot(2);
        }
    }

    private void SelectEquipmentSlot(int slotIndex)
    {
        if (slotIndex < equipmentHolder.Inventory.GetSlotsLength()) // Assuming SlotsCount is the number of slots
        {
            selectedEquipmentSlot = slotIndex;
            var slot = equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot);
            equipmentHolder.InventoryUI.SelectSlot(slot);
            equippedItem = gun;
        }
    }

    public void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent(out DroppedItem droppedItem))
        {
            inventoryHolder.Inventory.AddItem(droppedItem.Item, 1);
            Destroy(droppedItem.gameObject);
        }
    }
}
