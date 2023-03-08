using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GridSystem
{

    //the size of the board, horizontally and vertically
    public static int xSize { get; private set; }
    public static int ySize { get; private set; }

    //this will show the winning color type once one wins
    public static PlaceableColor winner { get; private set; }

    //an objectpool of checkers, so we won't be garbage collecting too much
    //the pool is shared for white and black pieces, and can be changed when you pull one out of the pool.
    //It's better than having more pools for each color, especially with the option of adding more colors in the future
    private static ObjectPool<Checker> checkerPool = new ObjectPool<Checker>();

    //these are for general quality of life, object pool for visualisation of movement and attack options
    private static ObjectPool<MoveVisual> moveVisualPool = new ObjectPool<MoveVisual>();
    private static ObjectPool<AttackVisual> attackVisualPool = new ObjectPool<AttackVisual>();
    
    //this doesn't need to be a multidimensional array, because we don't need to track the location of each component;
    private static List<MoveVisual> activeMoveVisuals = new List<MoveVisual>();
    private static List<AttackVisual> activeAttackVisuals = new List<AttackVisual>();

    //An array of all the placeables on the board
    private static Checker[,] checkerGrid;

    //array of the physical instances of the board tiles
    private static GameObject[,] tiles;

    //this determines how many rows of checkers each player gets at the start
    private static int rowsOfCheckers = 3;

    private static Vector2 gridStartPosition;

    //Initializes the grid, by input size
    public static void SetGridSize(int _xSize, int _ySize)
    {
        checkerGrid = new Checker[_xSize, _ySize];;
        //selectAnimations = new SelectAnimation[_xSize, _ySize];
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

                //TODO: Scale everything if the board size gets too big?
            }
        }
    }

    public static bool CheckIfGridposIsInsideGrid(GridPos _pos)
    {
        if (_pos.x < 0 || _pos.y < 0 || _pos.x > xSize - 1|| _pos.y > ySize - 1)
        {
            return false;
        }
        return true;
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

        checkerGrid[_gridPos.x, _gridPos.y].ResetChecker();
        checkerPool.ReturnObjectToPool(checkerGrid[_gridPos.x, _gridPos.y]);

        checkerGrid[_gridPos.x, _gridPos.y] = null;
    }

    //function that can add a checker to a specific position on the board
    public static void AddChecker(PlaceableType _placeAbleType, GridPos _gridPos, PlaceableColor _color)
    {
        if (checkerGrid[_gridPos.x, _gridPos.y] != null)
            return;


        checkerGrid[_gridPos.x, _gridPos.y] = checkerPool.RequestItem();

        checkerGrid[_gridPos.x, _gridPos.y].InitializePlaceable(_gridPos, _placeAbleType, _color);
        checkerGrid[_gridPos.x, _gridPos.y].UpdatePos(_gridPos);
        checkerGrid[_gridPos.x, _gridPos.y].UpdateVisual(_gridPos);
    }

    public static void ChangePlaceableType(GridPos _pos, PlaceableType newType)
    {
        checkerGrid[_pos.x, _pos.y].ChangeType(newType);
    }

    //check if a specific placeable can attack others in range
    public static bool CheckForPossibleAttacks(GridPos _pos)
    {
        if (!CheckIfGridposIsInsideGrid(_pos) || checkerGrid[_pos.x, _pos.y] == null)
        {
            return false;
        }

        //check what the checkers possible attacks are, and if there are enemy pieces that can be attacked in that range
        GridPos[] attacks = checkerGrid[_pos.x, _pos.y].GetPossibleAttacks();

        for (int i = 0; i < attacks.Length; i++)
        {
            if(!CheckIfGridposIsInsideGrid(new GridPos(_pos.x + attacks[i].x, _pos.y + attacks[i].y)) 
                || !CheckIfGridposIsInsideGrid(new GridPos(_pos.x + attacks[i].x + attacks[i].x, _pos.y + attacks[i].y + attacks[i].y)))
            {
                continue;
            }
                if (checkerGrid[_pos.x + attacks[i].x, _pos.y + attacks[i].y] != null && checkerGrid[_pos.x + attacks[i].x, _pos.y + attacks[i].y].color != checkerGrid[_pos.x, _pos.y].color
                && checkerGrid[_pos.x + attacks[i].x + attacks[i].x, _pos.y + attacks[i].y + attacks[i].y] == null)
            {
                //there is a placeable on the attackable square, and the square behind it is free. Attack is possible
                return true;
            }
        }

        return false;
    }

    //move a checker, from a chosen position to a new position. Will only work if it's a legal move
    public static bool MoveChecker(GridPos _oldPos, GridPos _newPos)
    {
        if(!CheckIfGridposIsInsideGrid(_newPos))
        {
            Debug.Log("new position is outside of grid");
            return false;
        }

        if (checkerGrid[_oldPos.x, _oldPos.y] != null && checkerGrid[_newPos.x, _newPos.y] == null)
        {
            GridPos offset = new GridPos(_newPos.x - _oldPos.x, _newPos.y - _oldPos.y);
            GridPos[] moves = checkerGrid[_oldPos.x, _oldPos.y].GetPossibleMoves();

            for(int i = 0; i < moves.Length; i++)
            {
                if(moves[i].x == offset.x && moves[i].y == offset.y)
                {
                    checkerGrid[_newPos.x, _newPos.y] = checkerGrid[_oldPos.x, _oldPos.y];
                    checkerGrid[_newPos.x, _newPos.y].UpdatePos(_newPos);
                    checkerGrid[_newPos.x, _newPos.y].UpdateVisual(_newPos);
                    checkerGrid[_oldPos.x, _oldPos.y] = null;
                    return true;
                }
            }

            //no possible moves overlap
            return false;
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

        if (!CheckIfGridposIsInsideGrid(_newPos))
        {
            Debug.Log("new position is outside of grid");
            return false;
        }

        if (checkerGrid[_oldPos.x, _oldPos.y] != null && checkerGrid[_newPos.x, _newPos.y] != null)
        {
            GridPos offset = new GridPos(_newPos.x - _oldPos.x, _newPos.y - _oldPos.y);
            GridPos[] attacks = checkerGrid[_oldPos.x, _oldPos.y].GetPossibleAttacks();

            for (int i = 0; i < attacks.Length; i++)
            {
                Debug.Log(attacks[i].x + " & " + offset.x + ", " + attacks[i].y + " & " + offset.y);
                if (attacks[i].x == offset.x && attacks[i].y == offset.y && checkerGrid[_oldPos.x, _oldPos.y].color != checkerGrid[_newPos.x, _newPos.y].color
                    && checkerGrid[_oldPos.x + attacks[i].x + attacks[i].x, _oldPos.y + attacks[i].y + attacks[i].y] == null)
                {
                    PotionManager.UpdateWallet(checkerGrid[_newPos.x, _newPos.y].color, 1);
                    RemoveIplacable(_newPos);

                    int xDirection = _newPos.x - _oldPos.x;
                    int yDirection = _newPos.y - _oldPos.y;
                    GridPos landPos = new GridPos(_newPos.x + xDirection, _newPos.y + yDirection);

                    checkerGrid[landPos.x, landPos.y] = checkerGrid[_oldPos.x, _oldPos.y];
                    checkerGrid[_oldPos.x, _oldPos.y] = null;

                    checkerGrid[landPos.x, landPos.y].UpdatePos(landPos);
                    checkerGrid[landPos.x, landPos.y].UpdateVisual(landPos);
                    return true;
                }
            }
        }
        Debug.Log("Can't do that");
        return false;
    }

    public static void AddBorderToBoard()
    {
        //add board border
        GameObject side = Resources.Load("Prefabs/WoodSide") as GameObject;

        for (int i = 0; i < xSize; i++)
        {
            GameObject temp = UnityEngine.Object.Instantiate(side, new Vector3(gridStartPosition.x + i, gridStartPosition.y - 0.56f, 0.1f), new Quaternion(0, 0, 0, 0));
            temp.transform.rotation = Quaternion.Euler(0, 0, 90);

            GameObject temp2 = UnityEngine.Object.Instantiate(side, new Vector3(gridStartPosition.x + i, gridStartPosition.y + (ySize - 1) + 0.56f, 0.1f), new Quaternion(0, 0, 0, 0));
            temp2.transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        for (int i = 0; i < ySize; i++)
        {
            UnityEngine.Object.Instantiate(side, new Vector3(gridStartPosition.x - 0.56f, gridStartPosition.y + i, 0.1f), new Quaternion(0, 0, 0, 0));

            GameObject temp = UnityEngine.Object.Instantiate(side, new Vector3(gridStartPosition.x + (xSize - 1) + 0.56f, gridStartPosition.y + i, 0.1f), new Quaternion(0, 0, 0, 0));
            temp.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
    
    //public static void SpawnAllLocationAnimations()
    //{
    //    if(tiles == null)
    //    {
    //        Debug.LogError("Can't spawn animations on the grid, since there is no grid");
    //        return;
    //    }

    //    for (int i = 0; i < ySize; i++)
    //    {
    //        for (int j = 0; j < xSize; j++)
    //        {
    //            selectAnimations[i, j] = new SelectAnimation();
    //            selectAnimations[i, j].InitializeAnimation(new GridPos(i, j));
    //        }
    //    }
    //}

    //provides visual for possible moves and attacks
    public static void ShowAllMovementOptionsForPlaceable(GridPos _pos)
    {
        GridPos[] moves = checkerGrid[_pos.x, _pos.y].GetPossibleMoves();
        for (int i = 0; i < moves.Length; i++)
        {
            if (!CheckIfGridposIsInsideGrid(new GridPos(_pos.x + moves[i].x, _pos.y + moves[i].y)))
                continue;

            if(checkerGrid[_pos.x + moves[i].x, _pos.y + moves[i].y] == null)
            {
                //there is an open tile we can move to
                MoveVisual newVisual = moveVisualPool.RequestItem();
                newVisual.CreateBody();
                newVisual.UpdatePos(_pos + moves[i]);
                newVisual.UpdateVisual(_pos + moves[i]);
                activeMoveVisuals.Add(newVisual);
            }
        }

    }

    //shows all options of attack for the specified checker
    public static void ShowAllAttackOptionsForPlaceable(GridPos _pos)
    {
        GridPos[] attacks = checkerGrid[_pos.x, _pos.y].GetPossibleAttacks();

        for (int i = 0; i < attacks.Length; i++)
        {

            if (!CheckIfGridposIsInsideGrid(new GridPos(_pos.x + attacks[i].x, _pos.y + attacks[i].y)) || !CheckIfGridposIsInsideGrid(new GridPos(_pos.x + attacks[i].x + attacks[i].x, _pos.y + attacks[i].y + attacks[i].y)))
                continue;

            if (checkerGrid[_pos.x + attacks[i].x, _pos.y + attacks[i].y] != null && checkerGrid[_pos.x + attacks[i].x, _pos.y + attacks[i].y].color != checkerGrid[_pos.x, _pos.y].color
                && checkerGrid[_pos.x + attacks[i].x + attacks[i].x, _pos.y + attacks[i].y + attacks[i].y] == null)
            {
                //there is an open piece we can attack
                AttackVisual newVisual = attackVisualPool.RequestItem();
                newVisual.CreateBody();
                newVisual.UpdatePos(_pos + attacks[i]);
                newVisual.UpdateVisual(_pos + attacks[i]);
                activeAttackVisuals.Add(newVisual);
            }
        }
    }

    //clears all visuals
    public static void ClearOptions()
    {
        foreach(MoveVisual vis in activeMoveVisuals)
        {
            moveVisualPool.ReturnObjectToPool(vis);
        }
        activeMoveVisuals.Clear();

        foreach (AttackVisual vis in activeAttackVisuals)
        {
            attackVisualPool.ReturnObjectToPool(vis);
        }
        activeAttackVisuals.Clear();

    }

    public static void SpawnAllCheckers()
    {
        //I spawn checkers based on the gridSize
        PlaceableColor pieceColor = PlaceableColor.Neutral;
        for (int i = 0; i < ySize; i++)
        {
            if (i < rowsOfCheckers)
                pieceColor = PlaceableColor.White;

            if (i >= ySize - rowsOfCheckers)
                pieceColor = PlaceableColor.Black;

            if (i >= rowsOfCheckers && i < ySize - rowsOfCheckers)
                continue;

            //the starter checkers are all basic placeableTypes
            for (int j = 0; j < xSize; j++)
            {
                if ((i + j) % 2 == 0)
                    SpawnChecker(new GridPos(j, i), PlaceableType.Basic, pieceColor);
            }
        }
    }


    //spawns a checker of the given type
    public static void SpawnChecker(GridPos _initPos, PlaceableType _placeableType, PlaceableColor _color)
    {
        AddChecker(_placeableType, _initPos, _color);
    }

    //function to remove all remaining checkers from the board
    public static void ClearAllCheckers()
    {
        for(int i = 0; i < checkerGrid.GetLength(0); i++)
        {
            for (int k = 0; k < checkerGrid.GetLength(1); k++)
            {
                if(checkerGrid[i,k] != null)
                {
                    checkerGrid[i, k].ResetChecker();
                    checkerPool.ReturnObjectToPool(checkerGrid[i, k]);
                    checkerGrid[i, k] = null;
                }
            }
        }
    }

    public static bool CheckForVictory(PlaceableColor _color)
    {
        foreach(Checker checker in checkerGrid)
        {
            if(checker != null && checker.color != _color)
            {
                //the requested color isn't the only one on the board, victory hasn't been acquired yet
                return false;
            }
        }

        //all the checkers are of the requested color, it won
        return true;
    }

    public static void SetWinner(PlaceableColor _color)
    {
        winner = _color;
    }

    //fetch what the checker is on a given location
    public static Checker CheckGridPosition(GridPos _gridPos)
    {
        if(!CheckIfGridposIsInsideGrid(_gridPos))
        {
            //the point falls outside of the grid
            return null;
        }

        //Debug.Log(_gridPos.x + ", " + _gridPos.y);

        return checkerGrid[_gridPos.x, _gridPos.y];
    }

    //if you do need the vector2 position from 
    public static Vector2 FetchVector2FromGridpos(GridPos _gridPos)
    {
        return gridStartPosition + new Vector2(_gridPos.x, _gridPos.y);
    }
}
