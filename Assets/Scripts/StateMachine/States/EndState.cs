using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndState : Gamestate
{
    GameObject endVisual, visualPrefab;
    //since this state will end the game, it won't need transitions to other states

    public EndState() : base()
    {
        visualPrefab = Resources.Load("Interface/EndText") as GameObject;
        endVisual = Object.Instantiate(visualPrefab);
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
                endVisual.GetComponent<TextMeshPro>().text = "White won!";
                break;

            case PlaceableColor.Black:
                endVisual.GetComponent<TextMeshPro>().text = "Black won!";
                break;
        }

        endVisual.SetActive(true);
        Time.timeScale = 0;

        //TODO: show visual of the game being paused
    }

    public override void Exit()
    {
        Debug.Log("unending game..(?)");
        base.Exit();
        GridSystem.ClearAllCheckers();
        endVisual.SetActive(false);
        Time.timeScale = 1;
    }
}
