using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gun,
    Tool,
    Rifle,
    Consumable,
    Structure,
    Default
}
public abstract class Item : ScriptableObject
{
    public int id;
    public string name;
    public int price;
    public Sprite sprite;
    public string description;
    public int stackSize = 1;
    public ItemType type;
}
