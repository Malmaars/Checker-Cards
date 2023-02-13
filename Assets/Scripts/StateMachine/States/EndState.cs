using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : Gamestate
{
    //since this state will end the game, it won't need transitions to other states

    //ends the game
    public override void Enter()
    {
        //Just end the game whatever
        Application.Quit();

        //this code is unreachable haha I CANNOT DIE AND WILL LIVE FOREVER
        //except if you're in the editor. In which case:
        Debug.Log("Hihi you found me");
    }
}
