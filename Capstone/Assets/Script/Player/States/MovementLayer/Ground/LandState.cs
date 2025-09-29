using UnityEngine;
using SkateGame;
using QFramework;

public class LandState : GroundMovementState
{
    private float landTimer = 0.5f;

    public LandState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Land";

    public override void Enter()
    {
    }

    protected override void UpdateGroundMovement()
    {
        landTimer -= Time.deltaTime;
        if(landTimer <= 0)
        {
            if(rb.linearVelocity.x == 0)
            {
                player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
            }
            else
            {
                player.stateMachine.SwitchState(StateLayer.Movement, "Move");
            }
        }
    }

    public override void Exit()
    {
    }
} 