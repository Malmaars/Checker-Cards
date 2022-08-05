using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.UIBehaviour.html
public class DisplayInventory //hand visualizer
{
    public static GameObject inventoryObject;
    public InventoryObject inventory; //displayed inventory
    public GameObject inventoryprefab; //item that gets card sprites assigned
    public int xStart; 
    public int yStart; 
    public int xSpacer;
    public int ySpacer;    
    public int column;

    /// <summary>
    /// inventory system for managing cards on screen
    /// </summary>
    /// <param name="x">x start location</param>
    /// <param name="y">y start location</param>
    /// <param name="xpace">spacing between items</param>
    /// <param name="column">Preffered items per column</param>
    /// <param name="yspace">spacing between items</param>
    /// <param name="_inventory">InventoryObject</param>
    public DisplayInventory(int _x,int _y, int _xpace, int _column, int _yspace, InventoryObject _inventory)
    {
        xStart = _x;
        yStart = _y;
        xSpacer = _xpace;
        column = _column;
        ySpacer = _yspace;
        inventory = _inventory;
      //  inventory.database.FillItems();
      //  inventory.database.CreateDatabase();
        inventoryprefab = GenerateInventory(inventory);
        
    }
    public static void AddCardToInventory()
    {
        var obj = Object.Instantiate(Resources.Load("Prefabs/Card") as GameObject);
        obj.name = "card";
        obj.transform.SetParent(inventoryObject.transform);
    }
    private GameObject GenerateInventory(InventoryObject _inventory)
    {
        GameObject inventoryholder = Object.Instantiate(new GameObject());
        inventoryholder.name = "Inventory";
        Debug.Log("Items available to player: " + _inventory.Container.items.Count);
        foreach (InventorySlot slot in _inventory.Container.items)
        {
            for (int i = 0; i < slot.amount; i++)
            {
                var obj = Object.Instantiate(Resources.Load("Prefabs/Card") as GameObject);
                obj.name = "card " + i;
                obj.transform.SetParent(inventoryholder.transform);
            }
        }
        inventoryObject = inventoryholder;
        return inventoryObject;
    }

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    public Vector3 GetPosition(int index)//assign inventory location
    {
        return new Vector3(xStart + (xSpacer * (index % column)), yStart + (-ySpacer * (index / column)), 0f); //use start locations with offset --0--0--0--;
    }
    public void CreateDisplay()
    {
        GameObject obj = inventoryprefab;
        for (int i = 0; i < inventory.Container.items.Count; i++)
        {
            InventorySlot slot = inventory.Container.items[i];
            Debug.Log("getting items: " + slot.item.id); //+ "sprite: " +obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite);
            SpriteRenderer a = Resources.Load("imgs/Card" + i) as SpriteRenderer;
            obj.transform.GetChild(i).GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("imgs/Card" + i); //a.sprite; //inventory.database.itemdict[i].ui;
            obj.transform.GetChild(i).GetComponent<Transform>().localPosition = GetPosition(i);
            obj.transform.GetChild(i).GetComponent<Transform>().localPosition = GetPosition(i);
            itemsDisplayed.Add(slot, obj);
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.items.Count; i++) //item already exist update count
        {
            InventorySlot slot = inventory.Container.items[i];
            if (itemsDisplayed.ContainsKey(slot))
            {
                return;
            }
            else //new item 
            {
                    var obj = GameObject.Instantiate(inventoryprefab, Vector3.zero, Quaternion.identity); //mising transform
                    obj.transform.GetChild(i).GetComponentInChildren<SpriteRenderer>().sprite = inventory.database.itemdict[slot.item.id].ui;
                    obj.GetComponent<Transform>().localPosition = GetPosition(i);
                    itemsDisplayed.Add(inventory.Container.items[i], obj);
            }
        }
    }
}
