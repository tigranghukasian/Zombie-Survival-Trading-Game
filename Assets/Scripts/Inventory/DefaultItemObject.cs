using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Inventory System/ItemObjects/DefaultItemObject")]
public class DefaultItemObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}
