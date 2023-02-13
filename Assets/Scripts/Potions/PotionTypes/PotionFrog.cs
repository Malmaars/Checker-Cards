using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFrog : PlaceablePotion
{
    public PotionFrog() : base() 
    {
        visual = Object.Instantiate(Resources.Load("Prefabs/Potions/Frog_Potion") as GameObject, position, new Quaternion(0,0,0,0));
    }

    public override void Effect(GridPos _effectTile)
    {
        base.Effect(_effectTile);

        GridSystem.ChangePlaceableType(_effectTile, PlaceableType.Frog);
        //make a pawn so it can jump horizontally and vertically
    }
}
