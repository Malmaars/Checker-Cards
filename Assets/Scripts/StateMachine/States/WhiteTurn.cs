using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTurn : Turn
{
    //it's white's turn


    public WhiteTurn() : base()
    {
        turnsPlaceableColor = PlaceableColor.White;
        PotionManager.AddWallet(turnsPlaceableColor);

        //Transition to black's turn when you make a move
        transitions.Add(new StateTransition(typeof(BlackTurn), () => turnFinished == true));
    }
}
