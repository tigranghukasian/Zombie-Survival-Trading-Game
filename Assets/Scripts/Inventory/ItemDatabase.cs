using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/ItemDatabase")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Item[] items;
    public Dictionary<int, Item> ItemDictionary = new Dictionary<int, Item>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].id = i;
            ItemDictionary.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        ItemDictionary = new Dictionary<int, Item>();
    }

    public Item GetItem(int id)
    {
        if (ItemDictionary.TryGetValue(id, out Item item))
        {
            return item;
        }

        return null;
    }
}
