using UnityEngine;
using SkateGame;
using QFramework;

public class JumpState : AirborneMovementState
{
    private float chargeTime;
    private bool isCharging;
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
        player.animator.SetBool("isOllie", true);
        player.animator.SetBool("isGrounded", false);
        // Debug.Log("JumpState.Enter() - 开始跳跃");
        playerModel.GrindJumpTimer.Value = playerModel.Config.Value.grindJumpIgnoreTime;
        isCharging = true;
        chargeTime = 0f;
        // Debug.Log($"初始水平速度: {initialHorizontalVelocity}");
        
        // 立即发送跳跃执行事件
        player.SendEvent<JumpExecuteEvent>();


       // 播放MMF效果
        if (player.JumpEffect != null)
        {
            player.JumpEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("JumpEffecttPlayer为null，无法播放效果");
        } 
    }

    protected override void UpdateAirMovement()
    {
        UpdateGrindJumpTimer();
        
        // 蓄力跳逻辑（现在只是计时，移动由移动系统处理）
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > playerModel.maxChargeTime.Value)
                chargeTime = playerModel.maxChargeTime.Value;
        }
        
        if (playerModel.StateTimer.Value > 0f)
        {
            playerModel.StateTimer.Value -= Time.deltaTime;
        }
        else
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Air");
        }
        playerModel.GrindJumpTimer.Value -= Time.deltaTime;
    }

    public override void Exit()
    {
        // Debug.Log("退出Jump状态");
        // 播放MMF效果
        if (player.JumpEffect != null)
        {
            player.JumpEffect.StopFeedbacks();
        }
        else
        {
            Debug.LogWarning("JumpEffecttPlayer为null，无法播放效果");
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
} 