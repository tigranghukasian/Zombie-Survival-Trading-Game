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
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;

    public Item(ItemObject _item)
    {
        Name = _item.name;
        Id = _item.Id;
    }
}
