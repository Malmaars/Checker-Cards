using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract base class for items colleactiable
//addapted by peer lomans based on Unity3D - Scriptable Object Inventory System | Part 1 - 4  https://www.youtube.com/watch?v=_IqTeruf3-s
public enum ItemType //
{
    effect,
    action,
    terrain,
}

public enum Attribute
{
    Cardeffect,
    ExtraDraw, 
    Cardeffect2, 
}
public abstract class ItemObject : ScriptableObject
{
    public int id; 
    public Sprite ui;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Buff[] mybuffs;
    public delegate void myeffect();
    public event myeffect effect;
    public Item CreateItem()
    {
        Item newitem = new Item(this);
        return newitem;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    public Buff[] buffs;
   
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.id;
        buffs = new Buff[item.mybuffs.Length];

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new Buff(item.mybuffs[i].min, item.mybuffs[i].max);
        }
    }
 
}
[System.Serializable]
public class Buff //buff with atrribute based on enums
{
    public Attribute myattribute;
    public int value;
    public int min;
    public int max;
    public Buff(int _min, int _max)
    {
        min = _min;
        max = _max;
        Generatevalue();
    }
    public void Generatevalue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}