using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCommand : ICommand
{
    public void Execute()
    {
        PauseTheGame();
    }

    void PauseTheGame()
    {
        //make it so the state is switched to pausestate, but not as hardcoded?
    }
}
