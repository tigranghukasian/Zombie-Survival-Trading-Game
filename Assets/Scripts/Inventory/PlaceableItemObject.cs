using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Item", menuName = "Inventory System/ItemObjects/PlaceableItemObject")]
public class PlaceableItemObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Placeable;
    }
}
