using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable : IPoolable
{
    //location of the IPlaceable
    GridPos myPos { get; }

    GameObject body { get; set; }

    //function to Update the position of an IPlaceable using GridPos
    void UpdatePos(GridPos _pos);

    //function to specifically update the visual feedback for the IPlaceable
    void UpdateVisual(GridPos _pos);

    //function to fetch the body of an IPlaceable
    GameObject GetBody();

    void CreateBody();
}


