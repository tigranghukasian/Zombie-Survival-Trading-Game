using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryHolder inventoryHolder;

    public void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent(out DroppedItem droppedItem))
        {
            inventoryHolder.Inventory.AddItem(droppedItem.Item, 1);
            Destroy(droppedItem.gameObject);
        }
    }
}
