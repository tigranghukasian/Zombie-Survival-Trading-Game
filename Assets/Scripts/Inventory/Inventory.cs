using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryObject inventoryObject;
    [SerializeField] private GameObject inventorySlotPrefab;
    private Dictionary<GameObject, InventorySlot> inventorySlots = new Dictionary<GameObject, InventorySlot>();
    private void Start()
    {
        CreateSlots();
    }

    public void CreateSlots()
    {
        for (int i = 0; i < inventoryObject.Container.Count; i++)
        {
            var invSlot = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform);
            inventorySlots.Add(invSlot, inventoryObject.Container[i]);
        }
    }
}
