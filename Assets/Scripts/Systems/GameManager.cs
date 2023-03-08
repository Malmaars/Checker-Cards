using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{    
    //the size of the board
    public int boardX, boardY;

    //in case we want to not have the grid in the center of the screen, we can put in an offset
    public Vector2 gridOffset;

    private GameStateMachine stateMachine;

    void Start()
    {
        stateMachine = new GameStateMachine(typeof(StartState));
        PotionManager.Initialize();
    }

    void Update()
    {
        stateMachine.StateUpdate();
        PotionManager.LogicUpdate();        
    }
}
