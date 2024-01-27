using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Item", menuName = "Inventory System/Items/GunItem")]
public class ItemGun : ItemEquipable
{

    private void Awake()
    {
        stackSize = 1;
        type = ItemType.Gun;
    }
}
