using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CursorHand
{
    //works without monobehavior
    //get animation controller for hand
    //bind hand to mouse position.
    //play animation when clicked

    // Start is called before the first frame update
    public Animator handController;
    public GameObject hand;
    Vector3 trackedPos;

    public CursorHand(GameObject _hand,Animator _handcontroller )
    {
        handController = _handcontroller;
        hand = _hand;
        InitializeVisual();
    }
    void InitializeVisual()
    {
        handController.enabled = true;
        trackedPos = Vector3.zero;
        Debug.Log(handController.layerCount + "Hand Positions Availible");
    }
   public void ReceiveInput(Vector3 Pos,bool MouseLeft, bool MouseRight ) //gets the mouse position and click functions
    {
        trackedPos = new Vector3(Pos.x,Pos.y,-4f);
        if (MouseLeft)
        {
            handController.Play("Armature|Hand grab");
            //card selected
            //checker selected
            return;
        }
        if (MouseRight)
        {
            Playanimation("Armature|Hand cards");
            handController.ResetTrigger("cursormode");
            return;
        } 
   }

    public Vector3 UpdateHandVisual()
    {
    return trackedPos;
    }

    public void Playanimation(string name)
    {
        handController.Play(name);
    }
}

//public class Whitehand //returns true if white is at play
//{
//    Gamestate myTurn;
//    public bool MyTurn(Gamestate _current, WhiteTurn _myTurn)
//    {
//        myTurn = _myTurn;
//        return (_current == _myTurn);   
//    }
//}
//public class Blackhand //returns true if black is at play
//{
//    Gamestate myTurn;
//    public bool MyTurn(Gamestate _current, WhiteTurn _myTurn)
//    {
//        myTurn = _myTurn;
//        return (_current == _myTurn);
//    }
//}
