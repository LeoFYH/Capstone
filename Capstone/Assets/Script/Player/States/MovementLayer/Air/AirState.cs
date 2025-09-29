using UnityEngine;
using SkateGame;
using QFramework;

public class AirState : AirborneMovementState
{ 
    private bool canDoubleJump;

    public AirState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        canDoubleJump = playerModel.CanDoubleJump.Value;
    }

    public override string GetStateName() => "Air";

    public override void Enter()
    {
        canDoubleJump = true; // 重置二段跳
         Debug.Log("进入空中状态");
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
            Debug.Log("2222:" + player.stateMachine.GetMovementStateName());
        }
    }

    public override void Exit()
    {
        Debug.Log("退出空中状态");
    }
    
    // state change
    private void StateChange()
    {
        if (canDoubleJump && inputModel.Jump.Value)
        {
            canDoubleJump = false;
            player.stateMachine.SwitchState(StateLayer.Movement, "DoubleJump");
        }
    }
} 