using UnityEngine;
using SkateGame;
using QFramework;

public class JumpState : AirborneMovementState
{
    private float chargeTime;
    private bool isCharging;
    private float jumpTimer;
    public JumpState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        chargeTime = playerModel.ChargeTime.Value;
        isCharging = playerModel.IsCharging.Value;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        // player.animator.Play("oPlayer@Ollie", 0);
        playerModel.GrindJumpTimer.Value = playerModel.Config.Value.grindJumpIgnoreTime;
        isCharging = true;
        chargeTime = 0f;
        jumpTimer = 0f;
        // 立即发送跳跃执行事件
        player.SendEvent<JumpExecuteEvent>();


       // 播放MMF效果
        if (player.JumpEffect != null)
        {
            player.JumpEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateAirMovement()
    {
        StateChange();
        UpdateGrindJumpTimer();
        UpdateJumpTimer();
        // 蓄力跳逻辑（现在只是计时，移动由移动系统处理）
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > playerModel.MaxChargeTime.Value)
                chargeTime = playerModel.MaxChargeTime.Value;
        }
        
    }

    public override void Exit()
    {
        // 播放MMF效果
        if (player.JumpEffect != null)
        {
            player.JumpEffect.StopFeedbacks();
        }
        playerModel.GrindJumpTimer.Value = 0f;
    }
    
    private void UpdateGrindJumpTimer()
    {
        // 更新轨道跳计时器
        if (playerModel.GrindJumpTimer.Value > 0f)
        {
            playerModel.GrindJumpTimer.Value -= Time.deltaTime;
        }
    }
    
    private void UpdateJumpTimer()
    {   
        if (jumpTimer < playerModel.JumpDuration.Value)
        {
            jumpTimer += Time.deltaTime;
        }
        else
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Air");
        }
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