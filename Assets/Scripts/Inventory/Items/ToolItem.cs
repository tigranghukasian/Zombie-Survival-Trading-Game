using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Item", menuName = "Inventory System/Items/ToolItem")]
public class ToolItem : Item
{
    private void Awake()
    {
        stackSize = 1;
        type = ItemType.Tool;
    }
}
