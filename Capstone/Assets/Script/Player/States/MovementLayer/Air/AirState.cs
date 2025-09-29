using UnityEngine;
using SkateGame;
using QFramework;

public class AirState : AirborneMovementState
{ 

    public AirState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Air";

    public override void Enter()
    {
    }

    protected override void UpdateAirMovement()
    {
        StateChange();
        // 检测落地
        if (playerModel.IsGrounded.Value)
        {
            // 处理瞄准时间奖励（如果执行了trick）
            player.HandleLandingAimTimeBonus();
            
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
        }
    }

    public override void Exit()
    {
    }
    
    // state change
    private void StateChange()
    {
        if (playerModel.CanDoubleJump.Value && inputModel.JumpStart.Value)
        {
            playerModel.CanDoubleJump.Value = false;
            player.stateMachine.SwitchState(StateLayer.Movement, "DoubleJump");
        }
    }
} 