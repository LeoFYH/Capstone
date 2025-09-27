using UnityEngine;
using SkateGame;
using QFramework;

public class IdleState : GroundMovementState
{
    public IdleState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Idle";

    public override void Enter()
    {
        player.animator.SetBool("isGrounded", true);
        player.animator.SetBool("isPlayingLand", false);
        // Debug.Log("进入Idle状态");
        ///
        /// 
        /// 
        ///                 //MMF
        if (player.IdleEffect != null)
        {
            player.IdleEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("idleEffect为null，无法播放效果");
        } 
    }

    protected override void UpdateGroundMovement()
    {
        float moveInput = inputModel.Move.Value.x;

        /* 状态切换 */
        // 移动 
        if (rb.linearVelocity.x != 0)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Move");
        }
    }

    public override void Exit()
    {
        // Debug.Log("退出Idle状态");
        if (player.IdleEffect != null)
        {
            player.IdleEffect.StopFeedbacks();
        }
        else
        {
            Debug.LogWarning("idleEffect为null，无法停止效果");
        } 
    }
} 