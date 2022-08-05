using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : Gamestate
{
    System.Type previousState;

    public PauseState()
    {
        transitions = new List<StateTransition>();
    }

    //This should be called whenever you switch to the pause state, the previous state will be put through, and recalled when you unpause
    public void GetPreviousState(System.Type _previousState)
    {
        transitions = new List<StateTransition>();
        previousState = _previousState;
        Debug.Log(previousState);
        transitions.Add(new StateTransition(previousState, () => Input.GetKeyDown(KeyCode.Escape)));
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("pausing game");
        Time.timeScale = 0;
    }

    public override void Exit()
    {
        Debug.Log("unpausing game");
        base.Exit();
        Time.timeScale = 1;
    }
}
