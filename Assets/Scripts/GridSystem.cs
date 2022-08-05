using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GridSystem
{
    //this determines how many rows of checkers each player gets at the start
    static int rowsOfCheckers = 3;

    static Vector2 gridStartPosition;
    //the size of the board, horizontally and vertically
    public static int xSize { get; private set; }
    public static int ySize { get; private set; }

    //an array that tracks where the checkers are
    private static ObjectPool<PlaceAble> blackPieces = new ObjectPool<PlaceAble>();
    private static ObjectPool<PlaceAble> whitePieces = new ObjectPool<PlaceAble>();

    private static PlaceAble[,] checkerGrid;
    private static GameObject[,] tiles;

    //Initializes the grid, by input size
    public static void SetGridSize(int _xSize, int _ySize)
    {
        checkerGrid = new PlaceAble[_xSize, _ySize];;
        xSize = _xSize;
        ySize = _ySize;
        
    }

    public static void InitializeGrid(Vector2 _gridStartPosition)
    {
        gridStartPosition = _gridStartPosition;
        tiles = new GameObject[xSize, ySize];

        //Tells me how many tiles there are in total
        Debug.Log(xSize * ySize + " is the amount of tiles availible");

        //let's spawn the tiles
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                GameObject squareColor;

                //check if it's even, all even tiles are black, all odd tiles are white. Quick math
                if ((i + j) % 2 == 0)
                {
                    squareColor = Resources.Load("Prefabs/SquareBlack") as GameObject;
                }
                else
                {
                    squareColor = Resources.Load("Prefabs/SquareWhite") as GameObject;
                }
                tiles[i, j] = squareColor;

                //the board will always spawn in the center of the screen. Each tile will spawn individually
                UnityEngine.Object.Instantiate(tiles[i, j], new Vector3(gridStartPosition.x + i, gridStartPosition.y + j, 0.1f), new Quaternion(0, 0, 0, 0));
            }
        }

    }

    //returns the gridposition of the location you click in game
    public static GridPos ClickOnTiles()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new GridPos((int)Mathf.Round(worldPosition.x - gridStartPosition.x), (int)Mathf.Round(worldPosition.y - gridStartPosition.y));
    }

    //Return a placable on a specific coordinate to the pool in case they get hit, or we need to remove them for any other reason
    public static void RemoveIplacable(GridPos _gridPos)
    {
        if (checkerGrid[_gridPos.x, _gridPos.y] == null)
        {
            Debug.LogError("Couldn't remove IPlaceable, because checkerGrid[" + _gridPos.x + ", " + _gridPos.y + "] is null");
            return;
        }
        
        switch(checkerGrid[_gridPos.x, _gridPos.y].placeableType)
        {
            case 0:
                blackPieces.ReturnObjectToPool(checkerGrid[_gridPos.x, _gridPos.y]);
                break;
            case 1:
                whitePieces.ReturnObjectToPool(checkerGrid[_gridPos.x, _gridPos.y]);
                break;
        }
        checkerGrid[_gridPos.x, _gridPos.y] = null;
    }

    //function that can add a placeable to a specific position on the board
    public static void AddPlaceable(int _placeAbleType, GridPos _gridPos)
    {
        if (checkerGrid[_gridPos.x, _gridPos.y] != null)
            return;

        switch (_placeAbleType)
        {
            case 0:
                checkerGrid[_gridPos.x, _gridPos.y] = blackPieces.RequestItem();
                break;

            case 1:
                checkerGrid[_gridPos.x, _gridPos.y] = whitePieces.RequestItem();
                break;
        }

        checkerGrid[_gridPos.x, _gridPos.y].InitializePlaceable(_gridPos, _placeAbleType);
        checkerGrid[_gridPos.x, _gridPos.y].UpdatePos(_gridPos);
        checkerGrid[_gridPos.x, _gridPos.y].UpdateVisual(_gridPos);
    }

    //move a checker, from a chosen position to a new position. Will only work if it's a legal move
    public static bool MoveChecker(GridPos _oldPos, GridPos _newPos)
    {

        if (checkerGrid[_oldPos.x, _oldPos.y] != null && checkerGrid[_newPos.x, _newPos.y] == null
            && (_newPos.x == _oldPos.x - 1 || _newPos.x == _oldPos.x + 1) && (_newPos.y == _oldPos.y - 1 || _newPos.y == _oldPos.y + 1))
        {
            GridPos direction = _newPos - _oldPos;
            if((direction.y < 0 && checkerGrid[_oldPos.x, _oldPos.y].placeableType == 1) || (direction.y > 0 && checkerGrid[_oldPos.x, _oldPos.y].placeableType == 0))
            {
                Debug.Log("Can't go that direction");
                return false;
            }

            checkerGrid[_newPos.x, _newPos.y] = checkerGrid[_oldPos.x, _oldPos.y];
            checkerGrid[_newPos.x, _newPos.y].UpdatePos(_newPos);
            checkerGrid[_newPos.x, _newPos.y].UpdateVisual(_newPos);
            checkerGrid[_oldPos.x, _oldPos.y] = null;
            return true;
        }

        //return false when selected piece is not an option
        else
        {
            Debug.Log("Can't do that");
            return false;
        }
    }

    //Attack another piece, and remove it if it's a legal move
    public static bool AttackChecker(GridPos _oldPos, GridPos _newPos)
    {
        Debug.Log("ATTACK");
        if (checkerGrid[_oldPos.x, _oldPos.y] != null && checkerGrid[_newPos.x, _newPos.y] != null
            && (_newPos.x == _oldPos.x - 1 || _newPos.x == _oldPos.x + 1) && (_newPos.y == _oldPos.y - 1 || _newPos.y == _oldPos.y + 1)
            && checkerGrid[_oldPos.x, _oldPos.y].placeableType != checkerGrid[_newPos.x, _newPos.y].placeableType)
        {
            int xDirection = _newPos.x - _oldPos.x;
            int yDirection = _newPos.y - _oldPos.y;
            GridPos landPos = new GridPos(_newPos.x + xDirection, _newPos.y + yDirection);

            if (landPos.x >= 0 && landPos.x < xSize && landPos.y >= 0 && landPos.y < ySize && checkerGrid[landPos.x, landPos.y] == null)
            {
                RemoveIplacable(_newPos);

                checkerGrid[landPos.x, landPos.y] = checkerGrid[_oldPos.x, _oldPos.y];
                checkerGrid[_oldPos.x, _oldPos.y] = null;
                //Debug.Log(checkerGrid[landPos.x, landPos.y]);
                checkerGrid[landPos.x, landPos.y].UpdatePos(landPos);
                checkerGrid[landPos.x, landPos.y].UpdateVisual(landPos);
                return true;
            }

            else
            {
                Debug.Log("Can't do that");
                return false;
            }
        }
        else
        {
            Debug.Log("Can't do that");
            return false;
        }
    }

    public static void SpawnAllCheckers()
    {
        //I spawn checkers based on the gridSize
        int pieceColor = 2;
        for (int i = 0; i < ySize; i++)
        {
            if (i < rowsOfCheckers)
                pieceColor = 1;

            if (i >= ySize - rowsOfCheckers)
                pieceColor = 0;

            if (i >= rowsOfCheckers && i < ySize - rowsOfCheckers)
                continue;

            for (int j = 0; j < xSize; j++)
            {
                if ((i + j) % 2 == 0)
                    SpawnChecker(new GridPos(j, i), pieceColor);
            }
        }
    }


    public static void SpawnChecker(GridPos _initPos, int _placeableType)
    {
        AddPlaceable(_placeableType, _initPos);
    }

    //fetch what the checker is on a given location
    public static IPlaceable CheckGridPosition(GridPos _gridPos)
    {
        return checkerGrid[_gridPos .x, _gridPos.y];
    }

    //if you do need the vector2 position from 
    public static Vector2 FetchVector2FromGridpos(GridPos _gridPos)
    {
        return gridStartPosition + new Vector2(_gridPos.x, _gridPos.y);
    }
}

//a struct for positions on the grid
public struct GridPos
{
    public int x;
    public int y;

    public GridPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static GridPos operator +(GridPos lhs, GridPos rhs)
    {
        return new GridPos(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    public static GridPos operator -(GridPos lhs, GridPos rhs)
    {
        return new GridPos(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    public static GridPos operator -(GridPos v)
    {
        return new GridPos(-v.x, -v.y);
    }
}