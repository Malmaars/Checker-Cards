using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class PotionManager
{
    //this class will manage all the potions. whether the player can buy them, and which ones are bought
   
    static List<Potion> currentlyAvaiablePotions;
    static System.Type[] possiblePotions = new System.Type[] {typeof(PotionFrog), typeof(PotionShield), typeof(PotionWings)};

    public static Potion currentlySelectedPotion;
    public static void Initialize()
    {
        currentlyAvaiablePotions = new List<Potion>();
    }

    public static void LogicUpdate()
    {
        DoSomethingWithPotion();
    }

    public static void MakePotionAvailable(Potion newPotion)
    {
        currentlyAvaiablePotions.Add(newPotion);
    }

    //arrange the position of the potions on screen
    public static void ArrangePotions()
    {
        for(int i = 0; i < currentlyAvaiablePotions.Count; i++)
        {
            Vector2 newPosition = new Vector2(4, currentlyAvaiablePotions.Count - i * currentlyAvaiablePotions.Count);
            currentlyAvaiablePotions[i].SetPosition(newPosition);
        }
    }

    static void DoSomethingWithPotion()
    {
        if(currentlySelectedPotion == null)
        {
            currentlySelectedPotion = CheckForClick();
            return;
        }
    }

    public static void DeselectPotion()
    {
        currentlySelectedPotion = null;
    }

    //check if player clicks on available potions
    static Potion CheckForClick()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            //there has been no input, so it makes no sense to check whether the player has clicked something
            return null;
        }

        //check if the mouse is in their range, and return potion when it is clicked 
        foreach (Potion potion in currentlyAvaiablePotions)
        {
            //compare the position and radius of the potion with the position of the mouse

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(Vector2.Distance(mousePosition, potion.position) <= potion.radius)
            {
                //the player is over this potion while clicking, return it
                Debug.Log(potion.GetType());
                return potion;
            }

        }

        //if nothing has been clicked, return null
        return null;
    }

    public static void UseEffect(GridPos pos)
    {
        //check if the tile is viable for the current potion
        if (currentlySelectedPotion.AffectableTile(pos))
        {
            //if it is viable, use its effect on the given tile
            currentlySelectedPotion.Effect(pos);
            DeselectPotion();
        }

        else
        {
            //the effect can't be used, and the potion will be deselected for the player's convenience
            DeselectPotion();
        }
    }

    //function to create a random potion from the possible potions, which is a type array in this class
    public static Potion CreateRandomPotion()
    {
        int randomIndex = UnityEngine.Random.Range(0, possiblePotions.Length);
        return (Potion)Activator.CreateInstance(possiblePotions[randomIndex]);
        //create a random potion from the possible potions
    }
}
