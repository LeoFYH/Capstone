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
        Debug.Log("AirState: Enter");
    }

    protected override void UpdateAirMovement()
    {
        // 检测落地
        if (player.IsGrounded())
        {
            // 处理瞄准时间奖励（如果执行了trick）
            player.HandleLandingAimTimeBonus();
            
            // 发送玩家落地事件，让系统处理
            //player.SendEvent<PlayerLandedEvent>(new PlayerLandedEvent());
            
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
        }
    }

    public override void Exit()
    {
    }
    
    // state change
    private void StateChange()
    {
        if (playerModel.CanDoubleJump.Value && inputModel.Jump.Value)
        {
            playerModel.CanDoubleJump.Value = false;
            player.stateMachine.SwitchState(StateLayer.Movement, "DoubleJump");
        }
    }
} 