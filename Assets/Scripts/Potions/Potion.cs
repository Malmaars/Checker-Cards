using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : IPotion
{
    protected GameObject visual;
    public Vector2 position { get; set; }

    public int radius { get; set; }
    public int price { get; set; }

    public Potion()
    {
        //currently all potions share the same radius
        radius = 1;
    }

    public void SetPosition(Vector2 _pos)
    {
        position = _pos;
        visual.transform.position = position;
    }

    //since all potions do something when you click on a specific tile, we can check the tile on the gridsystem, and an effect can take place
    public virtual void Effect(GridPos _effectTile) { }
    public virtual bool AffectableTile(GridPos _tile) { return false; }

    //rather inefficient, but I've put in a lot of time, and am way over the time I should put into this, so this'll have to do
    public virtual void RemovePotion()
    {
        visual.SetActive(false);
    }
}
