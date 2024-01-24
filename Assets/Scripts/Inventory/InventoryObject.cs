using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryObject", menuName = "Inventory System/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    private ItemDatabase database;

    [SerializeField] private Container container;
    public Container Container => container;
    public ItemDatabase Database => database;

    private void OnEnable()
    {
        database = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    public void AddItem(Item _item, int _amount)
    {
        if (_item.ItemObject.stackSize <= 0)
        {
            return;
        }
        while (_amount > 0)
        {
            bool added = false;
            
            for (int i = 0; i < container.Slots.Length; i++)
            {
                if (container.Slots[i].Id == _item.ItemObject.Id && container.Slots[i].Amount < _item.ItemObject.stackSize)
                {
                    int spaceInSlot = _item.ItemObject.stackSize - container.Slots[i].Amount;
                    int amountToAdd = Mathf.Min(spaceInSlot, _amount);

                    container.Slots[i].AddAmount(amountToAdd);
                    _amount -= amountToAdd;
                    added = true;

                    if (_amount <= 0) break;
                }
            }

            if (!added)
            {
                if (SetFirstEmptySlot(_item, Mathf.Min(_item.ItemObject.stackSize, _amount)) == null)
                {
                    // TODO : INVENTORY FULL
                    break;
                }
                _amount -= Mathf.Min(_item.ItemObject.stackSize, _amount);
            }
        }

    }

    private InventorySlot SetFirstEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < container.Slots.Length; i++)
        {
            if (Container.Slots[i].Id == -1)
            {
                Container.Slots[i].UpdateSlot(_item.ItemObject.Id, _item, _amount);
                return Container.Slots[i];
            }
        }
        //TODO: INVENTORY FULL
        return null;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string json = JsonUtility.ToJson(container, true);
        File.WriteAllText(string.Concat(Application.dataPath, savePath), json);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.dataPath, savePath)))
        {
            string saveString = File.ReadAllText(string.Concat(Application.dataPath, savePath));
            container = JsonUtility.FromJson<Container>(saveString);

        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Container();
    }
    
}

[System.Serializable]
public class Container
{
    [SerializeField] InventorySlot[] slots = new InventorySlot[15];
    
    public InventorySlot[] Slots => slots;
}

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private int id;
    [SerializeField] Item item;
    [SerializeField] int amount;

    public Item Item
    {
        get => item;
        set => item = value;
    }

    public int Id => id;

    public int Amount => amount;

    public InventorySlot()
    {
        id = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int _id, Item _item, int _amount)
    {
        id = _id;
        item = _item;
        amount = _amount;
    }
    
    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        id = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int _value)
    {
        amount += _value;
    }
}
