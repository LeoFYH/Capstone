using UnityEngine;
using SkateGame;
using QFramework;

public class LandState : GroundMovementState
{
    private float landTimer;

    public LandState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Land";

    public override void Enter()
    {
        playerModel.CanDoubleJump.Value = true;
        landTimer = 0f;
    }

    protected override void UpdateGroundMovement()
    {
        UpdateLandTimer();
    }

    public override void Exit()
    {
    }

    private void UpdateLandTimer()
    {
        if(landTimer < playerModel.Config.Value.landDuration)
        {
            landTimer += Time.deltaTime;
        }
        else
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
} 