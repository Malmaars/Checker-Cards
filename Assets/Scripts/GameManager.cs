using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameStateMachine stateMachine;
    //the size of the board
    public int boardX, boardY;
    Hand hand;

    private Vector2 gridStartPosition;
    [SerializeField]
    protected InventoryObject player1;
    DisplayInventory HandUIDisplay;
    
    void Start()
    {
        stateMachine = new GameStateMachine(typeof(WhiteTurn));
        stateMachine.GetState(typeof(Whitecardstate));
        hand = new Hand();
        hand.Initialize();

        StartGame();
    }

    private void Update()
    {
       hand.Ticklocal();
        stateMachine.StateUpdate();
    }

    void StartGame()
    {
        GridSystem.SetGridSize(8,8);

        //grid is at the center of the screen, so the start position will be taken from there
        gridStartPosition = new Vector2(Camera.main.transform.position.x - GridSystem.xSize / 2 + 0.5f, Camera.main.transform.position.y - GridSystem.ySize / 2 + 0.5f);

        GridSystem.InitializeGrid(gridStartPosition);

        GridSystem.SpawnAllCheckers();

        player1.database = Resources.Load("ItemDatabase") as ItemDatabase;
        HandUIDisplay = new DisplayInventory(-4, -5, 1, 4, 1, player1);
        HandUIDisplay.CreateDisplay();
        HandUIDisplay.UpdateDisplay();
    }
}

//failed attempt at making fetching components more efficient
/*

//maybe give this its own file

//The componentFetcher is static so that it can be referenced by any class wherever.
static class ComponentFetcher
{
    //when referenced, we can search for a specific component at the specific name of a GameObject
    //must enter the componentType as: typeof(nameofcomponentyouwant)
    public static Component fetchComponent(string ObjectName, System.Type componentType)
    {
        GameObject foundGameobject = GameObject.Find(ObjectName);

        //If the name can't be found, we return null
        if (foundGameobject = null)
        {
            Debug.Log("Couldn't find GameObject with the name: " + ObjectName);
            return null;
        }

        Component componentTemp = foundGameobject.GetComponent(componentType);

        //if the name does exist, and it has the component, we return that component
        if (componentTemp != null)
        {
            Debug.Log("Component of correct type found");
            return componentTemp;
        }

        //if the object doesn't have the component, we will search in its children
        Debug.Log("Couldn't find component in GameObject. Searching for component in Children");

        Component[] childrencomponents = foundGameobject.GetComponentsInChildren(componentType);
        if (childrencomponents.Length > 1)
        {
            Debug.Log("There's more than one component in its children. returning null");
            return null;
        }

        //only when in its children there is one component, we will return it, otherwise we won't have enough information to specify which component we need
        if (childrencomponents.Length == 1)
        {
            Debug.Log("Found one component in GameObject's children, returning component");
            return childrencomponents[0];
        }

        else
        {
            Debug.Log("No components found");
            return null;
        }
    }
}

*/
