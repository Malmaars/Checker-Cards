using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTurn : Turn
{
    //it's black's turn


    public BlackTurn() : base()
    {
        turnsPlaceableColor = PlaceableColor.Black;
        PotionManager.AddWallet(turnsPlaceableColor);

        //Transition to white's turn when you make a move
        transitions.Add(new StateTransition(typeof(WhiteTurn), () => turnFinished == true));
    }
}
