using UnityEngine;
using System.Collections;
using SkateGame;
using QFramework;

public class TrickAState : TrickState
{
    public TrickAState(InputController player, Rigidbody2D rb) : base(player, rb)
    {
        this.trickName = "TrickA";
    }

    public override string GetStateName() => "TrickA";

}