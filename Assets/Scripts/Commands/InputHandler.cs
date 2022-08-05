using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inputhandler ended up not being used, but I'll leave it in the files to show we tried.
//we chose not to use it because we only use the mouse, and assigning commands over and over on one button while the player is playing seemed inefficient
public class InputHandler
{
    private List<KeyCommand> keyCommands = new List<KeyCommand>();

    //checks for input and executes input when necessary
    public void HandleInput()
    {
        foreach(KeyCommand keyCommand in keyCommands)
        {
            if (Input.GetKeyDown(keyCommand.key))
            {
                keyCommand.command.Execute();
            }
        }
    }

    //binds keycodes to specific commands
    public void BindInput(KeyCode _keyCode, ICommand _command)
    {
        //first unbind, because we only want one command per key
        UnBindInput(_keyCode);

        //then add the new input
        keyCommands.Add(new KeyCommand() { key = _keyCode, command = _command });
    }

    
    //unbinds a key
    public void UnBindInput(KeyCode _keyCode)
    {
        var items = keyCommands.FindAll(x => x.key == _keyCode);
        items.ForEach(x => keyCommands.Remove(x));
    }
    public class KeyCommand
    {
        public KeyCode key;
        public ICommand command;
    }
}
