using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTurn : Gamestate
{
    //it's black's turn


    private IPlaceable selectedPlaceable;
    bool turnFinished = false;

    public BlackTurn()
    {
        transitions = new List<StateTransition>();

        //Transition to white's turn when you make a move
        transitions.Add(new StateTransition(typeof(WhiteTurn), () => turnFinished == true));
        //Transition to pause state when you press the escape button
        transitions.Add(new StateTransition(typeof(PauseState), () => Input.GetKeyDown(KeyCode.Escape)));
    }
    public override void Enter()
    {
        base.Enter();
        turnFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        MovePieces();
    }

    void MovePieces()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IPlaceable clickedTile = GridSystem.CheckGridPosition(GridSystem.ClickOnTiles());
            GridPos clickedPos = GridSystem.ClickOnTiles();

            if (selectedPlaceable != null)
            {
                //if you click on a tile that's not empty, attack if possible
                if (clickedTile != null)
                    turnFinished = GridSystem.AttackChecker(selectedPlaceable.myPos, clickedPos);

                //if you click on an empty tile, move if possible
                else if (clickedTile == null)
                    turnFinished = GridSystem.MoveChecker(selectedPlaceable.myPos, clickedPos);

                selectedPlaceable = null;
            }

            //if there's not already a placeable selected, assign a new one
            else if (selectedPlaceable == null)
            {
                if (clickedTile != null && clickedTile.placeableType == 0)
                    selectedPlaceable = clickedTile;
            }
        }
    }
}
