using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventoryHolder equipmentHolder;

    private int selectedEquipmentSlot = 0;
    [SerializeField] private Transform toolParentTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAnimatorEventChecker playerAnimatorEventChecker;
    [SerializeField] private ToolDetection toolDetection;

    private bool playingAnimation;
    private Equipable equippedItem;

    private void Awake()
    {
        Application.targetFrameRate = -1;
        playerAnimatorEventChecker.OnHit += OnAnimationHit;
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
            DestroyCurrentEquippedItem();
            EquipSlot(slot);
        }
    }

    private void OnAnimationHit()
    {
        playingAnimation = false;
        if (equippedItem is ToolEquipable)
        {
            ToolEquipable toolEquipable = (ToolEquipable)equippedItem;
            toolEquipable.Fire();
        }
    }

    private void Update()
    {
        CheckKeysForSelectingEquipment();
        if (Input.GetMouseButton(0) && equippedItem != null)
        {
            equippedItem.Use();
            if (equippedItem is ToolEquipable && !playingAnimation)
            {
                playingAnimation = true;
                animator.SetTrigger("Swing");
            }
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
        equipmentHolder.InventoryUI.AddSelectedCrownToSlot(slot);
        DestroyCurrentEquippedItem();
        EquipSlot(slot);
    }

    private Item SelectedItem()
    {
        var slot = equipmentHolder.Inventory.GetSlot(selectedEquipmentSlot);
        return equipmentHolder.Inventory.ItemDatabase.GetItem(slot.Id);
    }

    private void DestroyCurrentEquippedItem()
    {
        if (equippedItem != null)
        {
            Destroy(equippedItem.gameObject);
        }
    }

    private void EquipSlot(InventorySlot slot)
    {
        if (slot.Id == -1)
        {
            return;
        }

        Item slotItem = equipmentHolder.Inventory.ItemDatabase.GetItem(slot.Id);
       
        var playerDisplay = slotItem.playerDisplay;
        if (playerDisplay != null)
        {
            var playerItem = Instantiate(playerDisplay, transform);
            equippedItem = playerItem.GetComponent<Equipable>();
            Debug.Log(" equiped item is " + equippedItem);
            if (equippedItem is ToolEquipable)
            {
                ToolEquipable toolEquipable = (ToolEquipable)equippedItem;
                toolEquipable.ToolDetection = toolDetection;
                toolEquipable.transform.SetParent(toolParentTransform, false);
            }
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
