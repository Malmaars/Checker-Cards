using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVisual : GridVisual
{
    public override void CreateBody()
    {
        if (body != null)
        {
            return;
        }
        body = Object.Instantiate(Resources.Load("Prefabs/Tile_Highlight") as GameObject, GridSystem.FetchVector2FromGridpos(myPos), new Quaternion(0,0,0,0));
    }
}
