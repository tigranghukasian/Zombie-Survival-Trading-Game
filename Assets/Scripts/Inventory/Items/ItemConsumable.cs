using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory System/Items/ConsumableItem")]
public class ItemConsumable : Item
{
    public int restoreHealthAmount;
    public AudioClip consumeSound;
}
