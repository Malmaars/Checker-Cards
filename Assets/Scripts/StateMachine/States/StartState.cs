using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : Gamestate
{
    GameObject StartVisual, visualPrefab;

    public StartState() : base()
    {
        visualPrefab = Resources.Load("Interface/StartText") as GameObject;
        StartVisual = Object.Instantiate(visualPrefab);
        transitions.Add(new StateTransition(typeof(InitializeGameState), () => Input.GetMouseButtonDown(0) == true));
    }

    public override void Enter()
    {
        base.Enter();
        StartVisual.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        StartVisual.SetActive(false);
    }
}
