using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Inventory System/ItemObjects/FoodItemObject")]
public class FoodItemObject : ItemObject
{
    public int restoreHungerValue;
    private void Awake()
    {
        type = ItemType.Consumable;
    }
}
