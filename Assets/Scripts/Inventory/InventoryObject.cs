using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryObject", menuName = "Inventory System/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    [SerializeField] List<InventorySlot> container = new List<InventorySlot>();

    public List<InventorySlot> Container => container;

    public void AddItemObject(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].Item == _item)
            {
                container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            container.Add(new InventorySlot(_item, _amount));
        }
       
    }

    public void Clear()
    {
        container.Clear();
    }
}

[System.Serializable]
public class InventorySlot
{
    [SerializeField] ItemObject item;
    [SerializeField] int amount;

    public ItemObject Item => item;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int _value)
    {
        amount += _value;
    }
}
