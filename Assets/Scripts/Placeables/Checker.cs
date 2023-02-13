using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enum for Placeable Types
public enum PlaceableType
{
    Basic,
    Frog,
    Stone,
    Fire,
    Grapple,
    Shield,
    Winged
}

//Enum for Placeable Colors/sides
public enum PlaceableColor
{
    Neutral,
    White,
    Black
}

public class Checker : IPlaceable
{
    //if we wanna keep the checkers on one color,  we just have to check the sum of the gridPos (X + Y position)
    //One color is even, the other is odd. 1+1 = 2, 2+2 = 4, 5+7 = 12
    public GridPos myPos { get; private set; }

    //enum to note what type of placeable this is
    public PlaceableType placeableType { get; private set; }

    //enum to determine to which player the IPlaceable belongs.
    public PlaceableColor color { get; private set; }
    
    //in game respresentation of the checker
    public GameObject body { get; set; }

    private GridPos[] possibleMoves;

    private GridPos[] possibleAttacks;

    //determines which direction the checker moves to
    private GridPos direction;

    public bool active { get; set; }
    public void OnEnableObject()
    {
        body?.SetActive(true);
    }
    public void OnDisableObject()
    {
        body?.SetActive(false);
    }

    //The first function you should call to, where you set the initial position of the placeable and what type of placeable it is
    public void InitializePlaceable(GridPos _initPos , PlaceableType _placeableType, PlaceableColor _color)
    {
        color = _color;
        myPos = _initPos;
        CreateBody();
        ChangeColor(_color);
        ChangeType(_placeableType);
        //ChangeSprite();
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

    public GridPos[] GetPossibleAttacks()
    {
        return possibleAttacks;
    }

    public GridPos[] GetPossibleMoves()
    {
        return possibleMoves;
    }

    public void SetDirection(GridPos _direction)
    {
        direction = _direction;
    }

    //Create the visual representation of the placeable, based on the placeable type
    public void CreateBody()
    {
        if (body != null)
        {
            //it seems there already is a body for this placeable
            return;
        }

        GameObject prefab = null;

        switch (color)
        {
            case PlaceableColor.Black:
                prefab = Resources.Load("Placeables/BlackCircle") as GameObject;
                break;
            case PlaceableColor.White:
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

    public void ChangeType(PlaceableType _newType)
    {
        //change this placeable's type
        placeableType = _newType;

        //these moves should be relative to direction the checkers are coming from
        switch (placeableType)
        {
            case PlaceableType.Basic:
                possibleMoves = new GridPos[] { new GridPos(1, 1), new GridPos(-1, 1) };
                possibleAttacks = new GridPos[] { new GridPos(1, 1), new GridPos(-1, 1), new GridPos(1, -1), new GridPos(-1, -1) };
                break;
            case PlaceableType.Frog:
                possibleMoves = new GridPos[] { new GridPos(1, 1), new GridPos(-1, 1), new GridPos(-2, 0), new GridPos(2, 0), new GridPos(0, 2) };
                possibleAttacks = new GridPos[] { new GridPos(1, 1), new GridPos(-1, 1), new GridPos(1, -1), new GridPos(-1, -1) };
                break;
            case PlaceableType.Shield:
                possibleMoves = new GridPos[] { new GridPos(0, -1), new GridPos(-1, 0), new GridPos(1, 0), new GridPos(0, 1) };
                possibleAttacks = new GridPos[] {new GridPos(1, -1), new GridPos(-1, -1), new GridPos(1, 1), new GridPos(-1, 1) };
                break;
            case PlaceableType.Winged:
                possibleMoves = new GridPos[] { new GridPos(1, 1), new GridPos(-1, 1), new GridPos(1, -1), new GridPos(-1, -1) };
                possibleAttacks = new GridPos[] { new GridPos(1, 1), new GridPos(-1, 1), new GridPos(1, -1), new GridPos(-1, -1) };
                break;
        }

        for(int i = 0; i < possibleMoves.Length; i++)
        {
            possibleMoves[i] = new GridPos(possibleMoves[i].x * direction.x, possibleMoves[i].y * direction.y);
        }

        ChangeSprite();
    }

    public void ChangeColor(PlaceableColor _newColor)
    {
        color = _newColor;

        if(color == PlaceableColor.White)
        {
            SetDirection(new GridPos(1, 1));
        }
        if (color == PlaceableColor.Black)
        {
            SetDirection(new GridPos(1, -1));
        }

        ChangeSprite();
    }

    //function to call whenever a change of sprite is needed
    private void ChangeSprite()
    {
        Sprite newSprite = null;
        switch (color)
        {
            case PlaceableColor.White:
                switch (placeableType)
                {
                    case PlaceableType.Basic:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Regular_Piece_White");         
                        break;

                    case PlaceableType.Frog:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Frog_Piece_White");
                        break;

                    case PlaceableType.Stone:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Stone_Piece_White");
                        break;
                    
                    case PlaceableType.Shield:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Shield_Piece_White");
                        break;
                    
                    case PlaceableType.Winged:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Wing_Piece_White");
                        break;

                    case PlaceableType.Grapple:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Grapple_Piece_White");
                        break;
                }
                break;

            case PlaceableColor.Black:
                switch (placeableType)
                {
                    case PlaceableType.Basic:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Regular_Piece_Black");
                        break;

                    case PlaceableType.Frog:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Frog_Piece_Black");
                        break;

                    case PlaceableType.Stone:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Stone_Piece_Black");
                        break;

                    case PlaceableType.Shield:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Shield_Piece_Black");
                        break;

                    case PlaceableType.Winged:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Wing_Piece_Black");
                        break;

                    case PlaceableType.Grapple:
                        newSprite = Resources.Load<Sprite>("Sprites/Placeables/Grapple_Piece_Black");
                        break;
                }
                break;
        }

        body.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    //since a checker is a very varying placeable that can be a lot of types, it's nice to be able to reset it back to it's simplest form
    public void ResetChecker()
    {
        //a basic piece
        ChangeType(PlaceableType.Basic);

        //with no association
        ChangeColor(PlaceableColor.Neutral);
    }
}
