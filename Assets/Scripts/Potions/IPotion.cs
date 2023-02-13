using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPotion
{
    //the position the potion will be at
    Vector2 position { get; set; }

    //the size of the potion. This is needed when checking if the player actually clicks on the potion
    int radius { get; set; }

    //how expensive the potion is to get
    int price { get; set; }

    //its effect when bought. All effects happen on a specific gridpos given
    void Effect(GridPos _effectTile);

    //function to check if the potion can be used on a specific tile
    bool AffectableTile(GridPos _tile);
}
