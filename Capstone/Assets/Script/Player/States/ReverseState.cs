using UnityEngine;
using SkateGame;

public class ReverseState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;

    public ReverseState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Reverse";

    public override void Enter()
    {
        Debug.Log("ReverseState!!!!!");
        Vector2 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(-velocity.x, velocity.y);
        player.stateMachine.SwitchState("Move");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
