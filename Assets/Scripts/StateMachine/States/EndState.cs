using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndState : Gamestate
{
    GameObject EndVisual, visualPrefab;
    //since this state will end the game, it won't need transitions to other states

    public EndState() : base()
    {
        visualPrefab = Resources.Load("Interface/EndText") as GameObject;
        EndVisual = Object.Instantiate(visualPrefab);
        transitions.Add(new StateTransition(typeof(StartState), () => Input.GetMouseButtonDown(0) == true));
    }

    //ends the game
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Game's at an end");

        //check who won, and change text accordingly
        switch (GridSystem.winner)
        {
            case PlaceableColor.White:
                EndVisual.GetComponent<TextMeshPro>().text = "White won!";
                break;

            case PlaceableColor.Black:
                EndVisual.GetComponent<TextMeshPro>().text = "Black won!";
                break;
        }

        EndVisual.SetActive(true);
        Time.timeScale = 0;

        //TODO: show visual of the game being paused
    }

    public override void Exit()
    {
        Debug.Log("unending game..(?)");
        base.Exit();
        GridSystem.ClearAllCheckers();
        EndVisual.SetActive(false);
        Time.timeScale = 1;
    }
}
