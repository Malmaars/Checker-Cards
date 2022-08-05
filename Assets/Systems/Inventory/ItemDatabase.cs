using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory system/Items/Database")]
public class ItemDatabase : ScriptableObject
{
    public CardItem[] items;
    public Dictionary<int, CardItem> itemdict = new Dictionary<int, CardItem>();
    
    public void FillItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].id = i;
            itemdict.Add(i, items[i]);
        }
    }

    public void CreateDatabase()
    {
        itemdict = new Dictionary<int, CardItem>();
    }
}