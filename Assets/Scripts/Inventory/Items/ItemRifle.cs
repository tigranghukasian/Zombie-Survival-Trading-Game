using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rifle Item", menuName = "Inventory System/Items/RifleItem")]
public class ItemRifle : ItemEquipable
{
    private void Awake()
    {
        stackSize = 3;
        type = ItemType.Rifle;
    }
}
