using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//since there are multiple potions with multiple types of effects, it would be good to group them
//This is the type that only works when it is thrown on a basic placeable
public class PlaceablePotion : Potion
{
    public PlaceablePotion() : base() { }
    public override void Effect(GridPos _effectTile)
    {
        //a few error messages to ensure this function isn't incorrectly called
        if (!GridSystem.CheckIfGridposIsInsideGrid(_effectTile))
        {
            Debug.LogError("The given Gridpos does not fall within the playable grid. Be sure to use the AffectableTile function to check if you can use Effect");
            return;
        }

        if (GridSystem.CheckGridPosition(_effectTile) == null)
        {
            Debug.LogError("The given Gridpos doesn't have a placeable on it. Be sure to use the AffectableTile function to check if you can use Effect");
            return;
        }

        if (GridSystem.CheckGridPosition(_effectTile).placeableType != PlaceableType.Basic)
        {
            Debug.LogError("There is a placeable, but it's not the correct type. Be sure to use the AffectableTile function to check if you can use Effect");
            return;
        }
    }

    public override bool AffectableTile(GridPos _tile)
    {
        if (!GridSystem.CheckIfGridposIsInsideGrid(_tile))
        {
            //the tile is outside of the grid
            return false;
        }

        if (GridSystem.CheckGridPosition(_tile) != null && (GridSystem.CheckGridPosition(_tile).placeableType == PlaceableType.Basic))
        {
            //the placeable type is a basic white or black checker, and can be affected
            return true;
        }

        //the tile is inside the grid, but not a placeable or tile that this potion can be used on
        return false;

    }
}
