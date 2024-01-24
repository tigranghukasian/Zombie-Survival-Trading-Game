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
    private ItemDatabaseObject databaseObject;

    [SerializeField] private Container container;
    public Container Container => container;
    public ItemDatabaseObject DatabaseObject => databaseObject;

    private void OnEnable()
    {
        databaseObject = Resources.Load<ItemDatabaseObject>("ItemDatabase");
    }

    public void AddItem(Item _item, int _amount)
    {
        for (int i = 0; i < container.Slots.Length; i++)
        {
            if (container.Slots[i].Id == _item.Id)
            {
                container.Slots[i].AddAmount(_amount);
                return;
            }
        }

        SetFirstEmptySlot(_item, _amount);

    }

    private InventorySlot SetFirstEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < container.Slots.Length; i++)
        {
            if (Container.Slots[i].Id == -1)
            {
                Container.Slots[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Slots[i];
            }
        }
        //TODO: INVENTORY FULL
        return null;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        IFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(file, saveData);
        file.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open,
                FileAccess.Read);
            container = (Container)formatter.Deserialize(stream);
            stream.Close();
            
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
