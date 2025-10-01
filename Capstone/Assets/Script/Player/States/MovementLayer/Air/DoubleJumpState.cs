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
        playerModel.CanDoubleJump.Value = false;
        player.animator.SetTrigger("kickFlip");
        // 直接跳起来
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerModel.Config.Value.maxJumpForce);
        
        // 发送技巧执行事件给系统，更新UIController
        player.SendEvent<JumpExecuteEvent>();
        //MMF
        if (player.DoubleJumpEffect != null)
        {
            player.DoubleJumpEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateAirMovement()
    {   
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