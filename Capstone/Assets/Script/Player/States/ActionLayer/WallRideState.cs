using UnityEngine;
using SkateGame;

public class WallRideState : ActionStateBase
{
    private float normalGravity;
    private float onWallGravity = 0.1f;
    private float jumpCooldownTimer = 0f;  // 跳跃冷却计时器
    private float jumpCooldownDuration = 1.5f;  // 跳跃冷却时间1.5秒
    
    public WallRideState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
        isLoop = playerModel.Config.Value.isLoopWallRide;
    }

    public override string GetStateName() => "WallRide";

    protected override void EnterActionState()
    {
        normalGravity = rb.gravityScale;
        rb.gravityScale = onWallGravity;

        if (player.WallRideEffect != null)
        {
            player.WallRideEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateActionState()
    {
        // 更新跳跃冷却计时器
        if (jumpCooldownTimer > 0f)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }

        // 检查是否可以跳跃（冷却时间结束且松开Grind键）
        if (playerModel.CurrentWall.Value == null || !inputModel.Grind.Value)
        {   
            // 如果冷却时间还没结束，不允许跳跃
            if (jumpCooldownTimer > 0f)
            {
                return;
            }
            
            // 重置冷却计时器
            jumpCooldownTimer = jumpCooldownDuration;
            
            player.stateMachine.SwitchState(StateLayer.Action, "None");
            player.stateMachine.SwitchState(StateLayer.Movement, "Jump");
        }
    }

    protected override void ExitActionState()
    {   
        rb.gravityScale = normalGravity;
        if (player.WallRideEffect != null)
        {
            player.WallRideEffect.StopFeedbacks();
        }
    }
}