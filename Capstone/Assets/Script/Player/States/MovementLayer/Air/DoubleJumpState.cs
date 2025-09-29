using UnityEngine;
using SkateGame;
using QFramework;

public class DoubleJumpState : AirborneMovementState
{
    
    public DoubleJumpState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "DoubleJump";

    public override void Enter()
    {
        Debug.Log("DoubleJumpState: Enter");
        playerModel.CanDoubleJump.Value = false;
        player.animator.SetTrigger("kickFlip");
        // 直接跳起来
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerModel.Config.Value.maxJumpForce);
        
        // 发送技巧执行事件给系统，更新UIController
        player.SendEvent<TrickPerformedEvent>(new TrickPerformedEvent { TrickName = "doublejump" });
        //MMF
        if (player.DoubleJumpEffect != null)
        {
            player.DoubleJumpEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateAirMovement()
    {
        // 空中左右微调
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        // 检测落地，落地后切回Idle
        if (playerModel.IsGrounded.Value && rb.linearVelocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
        }
    }

    public override void Exit()
    {
        //MMF
        if (player.DoubleJumpEffect != null)
        {
            player.DoubleJumpEffect.StopFeedbacks();
        }
    }
} 