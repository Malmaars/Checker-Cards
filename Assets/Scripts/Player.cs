using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public PlaceableColor team;
    int amountOfCheckersLeft;

    //this int will track how many "points" or "currency" a player has to buy potions
    int wallet;

    public Player(PlaceableColor _whichTeam)
    {
        team = _whichTeam;
    }
}
