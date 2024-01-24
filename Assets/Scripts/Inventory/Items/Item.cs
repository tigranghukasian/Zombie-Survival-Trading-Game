using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Pistol,
    Rifle,
    Tool,
    Consumable,
    Structure,
    Default
}
public abstract class Item : ScriptableObject
{
    public int id;
    public string name;
    public GameObject playerDisplay;
    public Sprite sprite;
    public string description;
    public int stackSize = 1;
    public ItemType type;
}
