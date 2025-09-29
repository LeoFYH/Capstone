using UnityEngine;
using SkateGame;
using QFramework;

public class IdleState : GroundMovementState
{
    public IdleState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Idle";

    public override void Enter()
    {
        player.animator.SetBool("isOllie", false);
        player.animator.SetBool("isGrounded", true);
        player.animator.SetBool("isPlayingLand", false);
        //MMF
        if (player.IdleEffect != null)
        {
            player.IdleEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateGroundMovement()
    {
        float moveInput = inputModel.Move.Value.x;

        /* 状态切换 */
        // 移动 
        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Move");
        }
    }

    public override void Exit()
    {
        if (player.IdleEffect != null)
        {
            player.IdleEffect.StopFeedbacks();
        }
    }
} 