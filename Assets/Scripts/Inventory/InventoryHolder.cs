using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected Inventory inventory;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private InventoryUI inventoryUI;

    public Inventory Inventory => inventory;

    private void Awake()
    {
        inventory = new Inventory(inventorySize, itemDatabase, inventoryUI.UpdateSlotUI);
        inventoryUI.Init(inventory);
    }
}
