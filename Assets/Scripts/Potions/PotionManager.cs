using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

static class PotionManager
{
    //this class will manage all the potions. whether the player can buy them, and which ones are bought
   
    static List<Potion> currentlyAvaiablePotions;
    static System.Type[] possiblePotions = new System.Type[] {typeof(PotionFrog), typeof(PotionShield), typeof(PotionWings)};
    private static Dictionary<PlaceableColor, int> wallets;

    private static Dictionary<PlaceableColor, GameObject> walletVisuals;

    public static Potion currentlySelectedPotion;
    public static void Initialize()
    {
        currentlyAvaiablePotions = new List<Potion>();
        wallets = new Dictionary<PlaceableColor, int>();
        walletVisuals = new Dictionary<PlaceableColor, GameObject>();
    }

    public static void LogicUpdate()
    {
        DoSomethingWithPotion();
    }

    public static void AddWallet(PlaceableColor _color)
    {
        if (wallets.ContainsKey(_color))
        {
            Debug.LogError("There already is a wallet with this color");
            return;
        }

        wallets.Add(_color, 0);

        //create a visual

        GameObject temp = null;
        Debug.Log(_color);
        switch (_color)
        {
            case PlaceableColor.White:
                temp = UnityEngine.Object.Instantiate(Resources.Load("Interface/Wallets/WhiteWallet") as GameObject);
                break;

            case PlaceableColor.Black:
                temp = UnityEngine.Object.Instantiate(Resources.Load("Interface/Wallets/BlackWallet") as GameObject);
                break;
        }



        walletVisuals.Add(_color, temp);
        ArrangeWallets();
    }

    public static void ResetWallets()
    {
        foreach(KeyValuePair<PlaceableColor, int> wallet in wallets)
        {
            wallets[wallet.Key] = 0;
            UpdateWallet(wallet.Key, 0);
        }
    }

    //add or subtract from a wallet
    public static void UpdateWallet(PlaceableColor _color, int _addition)
    {
        wallets[_color] += _addition;

        walletVisuals[_color].GetComponentInChildren<TextMeshPro>().text = wallets[_color].ToString();
    }

    //check the value of a wallet
    public static int CheckWallet(PlaceableColor _color)
    {
        return wallets[_color];
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

    static void ArrangeWallets()
    {
        int index = 0;
        Vector2 newPosition = Vector2.zero;
        foreach (KeyValuePair<PlaceableColor, GameObject> keyValues in walletVisuals)
        {
            newPosition = new Vector2(walletVisuals.Count - index * walletVisuals.Count + 5, 4.4f);
            walletVisuals[keyValues.Key].transform.position = newPosition;
            index++;
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

    public static void UseEffect(GridPos pos, PlaceableColor _colorUsingIt)
    {
        //check if the tile is viable for the current potion
        if (currentlySelectedPotion.AffectableTile(pos) && wallets[_colorUsingIt] >= 3)
        {
            UpdateWallet(_colorUsingIt, -3);
            //if it is viable, use its effect on the given tile
            currentlySelectedPotion.Effect(pos);

            currentlySelectedPotion.RemovePotion();
            currentlyAvaiablePotions.Remove(currentlySelectedPotion);
            MakePotionAvailable(CreateRandomPotion());
            ArrangePotions();

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
