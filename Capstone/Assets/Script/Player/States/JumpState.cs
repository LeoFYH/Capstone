using UnityEngine;
using SkateGame;
using QFramework;

public class JumpState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float chargeTime;
    private bool isCharging;
    private bool hasJumped;
    private float initialHorizontalVelocity;
    public JumpState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        chargeTime = playerModel.ChargeTime.Value;
        isCharging = playerModel.IsCharging.Value;
        hasJumped = playerModel.HasJumped.Value;
        initialHorizontalVelocity = playerModel.InitialHorizontalVelocity.Value;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        player.animator.SetTrigger("ollie");
        player.animator.SetBool("isGrounded", false);

        // Debug.Log("JumpState.Enter() - 开始跳跃");
        playerModel.GrindJumpTimer.Value = player.grindJumpIgnoreTime;
        isCharging = true;
        chargeTime = 0f;
        hasJumped = false;
        initialHorizontalVelocity = rb.linearVelocity.x;
        // Debug.Log($"初始水平速度: {initialHorizontalVelocity}");

        // Reset local jump timer
        playerModel.StateTimer.Value = player.jumpTime;
        
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

    public override void Update()
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
            player.stateMachine.SwitchState("Air");
        }
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