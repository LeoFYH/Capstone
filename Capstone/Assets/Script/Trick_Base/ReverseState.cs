using UnityEngine;

public class ReverseState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;

    public ReverseState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Reverse";

    public override void Enter()
    {

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
