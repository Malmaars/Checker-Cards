using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeConvention
{
    //Order Variables/Properties/Functions (Constructor, Monobehaviour, Custom) -> Winner
    //option 1 Grouping (public, private, protected)

    //Variables: only small letters, order of variables, based on grouping
    public float someVariableIsCool = 1f;
    private readonly float someVariableIsCool2 = 1f;

    //Properties with small letters
    public float someFloat2
    {
        get;
        set;
    }

    //Functions:
    //use underscores for arguments
    public void DoSomething(float _someVal, int _arg2)
    {
        someVariableIsCool = _someVal;
    }

    //Brackets:
    //Brackets always on next line, use brackets also for one line statements
    void SomeFunc()
    {
        if (true)
        {
            SomeFunc(); //next line
        }
        if (true)
        {
            for (int i = 0; i < 3; i++)
            {

            }
        }
    }
}

//Comments
//Option 1: boven als het over meerdere regels gaat
//Functies alleen commenten als je een complexe functie schrijft
//Interfaces start with an I
public interface IStartInterfaceWIthI
{

}