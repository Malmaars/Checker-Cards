using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private readonly ObjectPool<CardItem> pool;
    private ItemDatabase database;
    public Deck()
    {
        pool = new ObjectPool<CardItem>();
        database = Resources.Load("itemDatabase") as ItemDatabase;
        //  database.CreateDatabase();
    }

    public void Generatedeck()
    {
        //   Debug.Log("cards known " + database.items.Length);
#if UNITY_STANDALONE_WIN
        if (database?.items == null)
        {
            database = Resources.Load("Scriptableobjects/itemDatabase") as ItemDatabase;
            Debug.Log(database);
            database.CreateDatabase();
            database.FillItems();
        }
#endif
        else
        {
            for (int i = 0; i < database.items.Length; i++)
            {
                CardItem carditem;
                carditem = database.items[i] as CardItem;
                carditem.mypos = new Vector2(i, 0);
                pool.ReturnObjectToPool(carditem); //add card to pool
            }
        }
    }
public void CheckCardPosition(Vector2 _checkpos)
    {
        if (database?.items == null)
        {
            database = Resources.Load("Scriptableobjects/itemDatabase") as ItemDatabase;
            Debug.Log(database);
            database.CreateDatabase();
            database.FillItems();
        }
        Debug.Log("Checking deck for correct card" + _checkpos);
        foreach (CardItem card in database.items)
        {
            if (card.InRangeofCard(_checkpos))
            {
                for (int i = 0; i < DisplayInventory.inventoryObject.transform.childCount; i++) //for every child object visualised on screen
                {
                    card.instancedreference = DisplayInventory.inventoryObject.transform.GetChild(i).gameObject; //assign gameobject to refrence of scriptable obejct
                }
                Debug.Log("Selected ="+card.name + "loading effect");
                card.effect += Effect(card);
            }
        }
    }
   public CardItem Drawcard()
    {
        return pool.RequestItem();
    }

    public ItemObject.myeffect Effect(CardItem _item)
    {
       if( _item.mybuffs[0].myattribute == Attribute.Cardeffect)
        {
            int value;
            value = Random.Range(_item.mybuffs[0].min, _item.mybuffs[0].max);
            //valid action
            //Todo: an action with a chance element
            //example 50% chance to spawn a extra checker
            
        }
        if (_item.mybuffs[0].myattribute == Attribute.ExtraDraw)
        {
            //add card to screen
            DisplayInventory.AddCardToInventory();
        }

        if (_item.mybuffs[0].myattribute == Attribute.Cardeffect2)
        {
            int value = _item.mybuffs[0].value;
            if (value >= 0)
            {
                //valid action
            }
        }
        //get rid of card after using
        GameObject.Destroy( _item.instancedreference);
        Debug.Log("Item Destroyed");
        return null;
    }
}
