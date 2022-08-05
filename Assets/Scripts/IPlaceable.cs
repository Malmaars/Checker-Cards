using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable : IPoolable
{
    //location of the IPlaceable
    GridPos myPos { get; }

    //int to determine to which player the IPlaceable belongs. It's an int so more player could be added and there can be options for IPlaceables that belong to no-one
    //0 is black, 1 is white
    //this could be an enum maybe
    int placeableType { get;}

    //The first function you should call to, where you set the initial position of the placeable and what type of placeable it is
    void InitializePlaceable(GridPos _initPos, int _placeableType);

    //function to Update the position of an IPlaceable using GridPos
    void UpdatePos(GridPos _pos);

    //function to specifically update the visual feedback for the IPlaceable
    void UpdateVisual(GridPos _pos);

    //function to fetch the body of an IPlaceable
    GameObject GetBody();
}
