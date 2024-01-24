using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory System/ItemObjects/EquipmentItemObject")]
public class EquipmentItemObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Equipment;
    }
}
