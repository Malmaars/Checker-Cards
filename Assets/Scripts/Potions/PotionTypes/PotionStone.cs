using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionStone : PlaceablePotion
{
    public PotionStone() : base()
    {
        visual = Object.Instantiate(Resources.Load("Prefabs/Potions/Stone_Potion") as GameObject, position, new Quaternion(0, 0, 0, 0));
    }

    public override void Effect(GridPos _effectTile)
    {
        base.Effect(_effectTile);

        GridSystem.ChangePlaceableType(_effectTile, PlaceableType.Stone);
    }
}
