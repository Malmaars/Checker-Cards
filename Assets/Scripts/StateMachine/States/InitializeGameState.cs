using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGameState : Gamestate
{
    bool initialized;

    //in case we want to not have the grid in the center of the screen, we can put in an offset
    Vector2 gridOffset = new Vector2(-4, 0);
    Vector2 gridStartPosition;
    public InitializeGameState() : base()
    {
        transitions.Add(new StateTransition(typeof(WhiteTurn), () => initialized == true));
    }

    public override void Enter()
    {
        base.Enter();

        PotionManager.ResetWallets();
        PotionManager.RemoveAllPotions();

        for (int i = 0; i < 3; i++)
        {
            PotionManager.MakePotionAvailable(PotionManager.CreateRandomPotion());
        }
        PotionManager.ArrangePotions();

        //originally wanted to do this in the initializers of the turns, but that would make them not appear at the start of the game, but at the start of their turn
        PotionManager.AddWallet(PlaceableColor.White);
        PotionManager.AddWallet(PlaceableColor.Black);

        GridSystem.SetGridSize(8, 8);

        //grid is at the center of the screen, so the start position will be taken from there
        gridStartPosition = new Vector2(Camera.main.transform.position.x - GridSystem.xSize / 2 + 0.5f, Camera.main.transform.position.y - GridSystem.ySize / 2 + 0.5f);

        //add the given offset to the start position
        gridStartPosition += gridOffset;

        GridSystem.InitializeGrid(gridStartPosition);
        GridSystem.AddBorderToBoard();
        GridSystem.SpawnAllCheckers();

        initialized = true;
    }

    public override void Exit()
    {
        base.Exit();
        initialized = false;
    }
}
