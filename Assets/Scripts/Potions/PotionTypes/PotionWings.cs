using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionWings : PlaceablePotion
{
    public PotionWings() : base()
    {
        visual = Object.Instantiate(Resources.Load("Prefabs/Potions/Wing_Potion") as GameObject, position, new Quaternion(0, 0, 0, 0));
    }

    public override void Effect(GridPos _effectTile)
    {
        base.Effect(_effectTile);

        GridSystem.ChangePlaceableType(_effectTile, PlaceableType.Winged);
    }
}
