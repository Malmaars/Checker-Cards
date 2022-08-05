using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitecardstate : Gamestate
{

    Deck deck = new Deck();

    public Whitecardstate()
    {
        transitions = new List<StateTransition>();
        transitions.Add(new StateTransition(typeof(WhiteTurn), ()=> Input.GetMouseButtonDown(0)));
        transitions.Add(new StateTransition(typeof(PauseState), () => Input.GetKeyDown(KeyCode.Escape)));

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("enter card");
    }
    public override void LogicUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("click card to select");
            deck.CheckCardPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

          //  RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            //if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.TransformDirection(Vector3.forward) * 30f, out hit, Mathf.Infinity, 8))
            //{
            //    Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //    Debug.Log("Did Hit");
            //}
            //else
            //{
            //    Debug.DrawRay(  Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.TransformDirection(Vector3.forward) * 30f, Color.yellow);
            //}
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
public class Blackcardstate : Gamestate
{
    public Blackcardstate()
    {
        transitions = new List<StateTransition>();
        transitions.Add(new StateTransition(typeof(BlackTurn), () => Input.GetMouseButtonDown(0)));
        transitions.Add(new StateTransition(typeof(PauseState), () => Input.GetKeyDown(KeyCode.Escape)));
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void LogicUpdate()
    {
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        //check if we are over a card DONE
    //        //move the ui component Kinda DONE
    //        // avtivate effect NOT YET
    //        Debug.Log("Check");
    //        RaycastHit2D hit = Physics2D.Raycast(handvisual.transform.position, transform.TransformDirection(Vector3.forward));
    //        Debug.DrawRay(handvisual.transform.position, transform.TransformDirection(Vector3.forward), Color.white, 1f, true);
    //        if (hit.collider != null)
    //        {
    //            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
    //            foreach (Transform child in HandUI.transform)
    //            {
    //                if (hit.transform.transform == child)
    //                {
    //                    child.gameObject.transform.position = handvisual.transform.position;
    //                }
    //            }
    //        }
    //    }

    }
    public override void Exit()
    {
        base.Exit();
    }

}
