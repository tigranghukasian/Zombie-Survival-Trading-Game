using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventoryHolder equipmentHolder;

    private int selectedEquipmentSlot = 0;
    [SerializeField] private Transform itemParent;
    private GameObject equippedItem;
    private IEquipable equippedEquipable;

    private void Awake()
    {
        Application.targetFrameRate = -1;
    }

    private void Start()
    {
        for (int i = 0; i < equipmentHolder.Inventory.GetSlotsLength(); i++)
        {
            equipmentHolder.Inventory.GetSlot(i).inventorySlotUpdatedCallback += SlotUpdated;
        }
       
        SelectEquipmentSlot(0);
    }

    private void SlotUpdated(InventorySlot slot)
    {
        if(equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot) == slot)
        {
            UpdateEquippedItem(slot);
        }
    }

    private void Update()
    {
        CheckKeysForSelectingEquipment();
        if (Input.GetMouseButtonDown(0) && equippedItem != null)
        {
            equippedEquipable.Fire();
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
        selectedEquipmentSlot = slotIndex;
        var slot = equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot);
        equipmentHolder.InventoryUI.SelectSlot(slot);
        UpdateEquippedItem(slot);
    }

    private void UpdateEquippedItem(InventorySlot slot)
    {
        if (equippedItem != null)
        {
            Destroy(equippedItem);
            equippedEquipable = null;
        }

        if (slot.Id == -1)
        {
            return;
        }
        var playerDisplay = equipmentHolder.Inventory.ItemDatabase.GetItem(slot.Id).playerDisplay;
        if (playerDisplay != null)
        {
            var playerItem = Instantiate(playerDisplay, itemParent);
            equippedItem = playerItem;
            equippedEquipable = playerItem.GetComponent<IEquipable>();
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
