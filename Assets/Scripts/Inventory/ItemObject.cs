using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Placeable,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite sprite;
    public int stackSize = 64;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}

[System.Serializable]
public class Item
{
    private ItemObject itemObject;
    public ItemObject ItemObject => itemObject;

    public Item(ItemObject _item)
    {
        itemObject = _item;
    }
}
