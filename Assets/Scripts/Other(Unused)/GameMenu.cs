using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu
{
    InputField xInput, yInput;
    Button enterGame;

    public GameMenu(InputField xInputField, InputField yInputField, Button enterButton)
    {
        xInput = xInputField;
        yInput = yInputField;
        enterGame = enterButton;
    }

    //return the x value the player put in
    public int ReadXValue()
    {
        int.TryParse(xInput.text, out int result);
        return result;
    }

    //return the y value the player put in
    public int ReadYValue()
    {
        int.TryParse(yInput.text, out int result);
        return result;
    }
}
