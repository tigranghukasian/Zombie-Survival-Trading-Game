using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private InventoryType inventoryType;
    [SerializeField] protected Inventory inventory;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private InventoryUI inventoryUI;

    public Inventory Inventory => inventory;

    private void Awake()
    {
        inventory = InventoryFactory.CreateInventory(inventoryType, itemDatabase, inventoryUI.UpdateSlotUI);
        inventoryUI.Init(inventory);
    }
}
