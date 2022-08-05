using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

[CreateAssetMenu(fileName = "new inventory", menuName = "Inventory system/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savepath;
    public ItemDatabase database;
    public Inventory Container;

    public void AddItem(Item _item, int _amount)
    {

        for (int i = 0; i < Container.items.Count; i++)
        {
            if (Container.items[i].item.id == _item.id)
            {
                Container.items[i].AddAmount(_amount);
                return;
            }
        }
        Container.items.Add(new InventorySlot(_item.id, _item, _amount));
    }

}
[System.Serializable]
public class Inventory
{
    public List<InventorySlot> items = new List<InventorySlot>();
}
[System.Serializable]
public class InventorySlot
{
    public int id;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)
    {
        id = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}