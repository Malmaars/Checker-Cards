using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAble : IPlaceable
{
    //if we wanna keep the checkers on one color,  we just have to check the sum of the gridPos (X + Y position)
    //One color is even, the other is odd. 1+1 = 2, 2+2 = 4, 5+7 = 12
    public GridPos myPos { get; private set; }

    //0 for black, 1 for white,
    public int placeableType { get; private set; }
    int direction;
    
    //in game respresentation of the checker
    protected GameObject body;

    public bool active { get; set; }
    public void OnEnableObject()
    {
        body?.SetActive(true);
    }
    public void OnDisableObject()
    {
        body?.SetActive(false);
    }

    public void InitializePlaceable(GridPos _initPos , int _placeableType)
    {
        placeableType = _placeableType;

        switch (placeableType)
        {
            case 0:
                direction = -1;
                break;

            case 1:
                direction = 1;
                break;
        }
        myPos = _initPos;
        CreateBody();
    }

    //Function to move the checker
    public void UpdatePos(GridPos _pos)
    {
        myPos = _pos;
    }

    //Update the visual representation of the checker
    public void UpdateVisual(GridPos _pos)
    {
        body.transform.position = GridSystem.FetchVector2FromGridpos(_pos);
    }

    //Function to fetch the body of the checker
    public GameObject GetBody()
    {
        return body;
    }

    private void CreateBody()
    {
        GameObject prefab = null;

        switch (placeableType)
        {
            case 0:
                prefab = Resources.Load("Placeables/BlackCircle") as GameObject;
                break;
            case 1:
                prefab = Resources.Load("Placeables/WhiteCircle") as GameObject;
                break;
        }

        if(prefab == null)
        {
            Debug.LogError("No Prefabs available for this placeable type");
            return;
        }

        body = Object.Instantiate(prefab);
    }
}
