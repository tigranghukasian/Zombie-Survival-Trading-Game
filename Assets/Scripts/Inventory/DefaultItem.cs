using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Inventory System/Items/DefaultItem")]
public class DefaultItem : Item
{
    private void Awake()
    {
        stackSize = 32;
        type = ItemType.Default;
    }
}
