using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rifle Item", menuName = "Inventory System/Items/RifleItem")]
public class RifleItem : Item
{
    private void Awake()
    {
        stackSize = 1;
        type = ItemType.Rifle;
    }
}
