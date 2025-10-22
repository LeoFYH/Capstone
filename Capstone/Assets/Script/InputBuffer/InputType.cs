using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum InputType
{
    W,
    E
}

public abstract class TimerRelated
{
    public float timeToCount { get; protected set; }

}

public class BaseInput : TimerRelated
{
    private InputType type;
    private bool ifDebug;

    //need a timer
    public BaseInput(InputType t, bool ifLog = false)
    {
        type = t;
        ifDebug = ifLog;

    }

    public InputType GetInput() => type;

    public void TakeOut()
    {
        if (ifDebug)
            Debug.Log(type.ToString() + " is taken");

    }

}


public class InputTag
{


}
