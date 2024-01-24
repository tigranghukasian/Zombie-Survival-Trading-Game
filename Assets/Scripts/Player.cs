using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryObject inventoryObject;

    public void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent(out DroppedItem item))
        {
            inventoryObject.AddItem(new Item(item.ItemObject), 1);
            Destroy(item.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventoryObject.Clear();
    }
}
