using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pistol Item", menuName = "Inventory System/Items/PistolItem")]
public class PistolItem : Item
{

    private void Awake()
    {
        stackSize = 1;
        type = ItemType.Pistol;
    }
}
