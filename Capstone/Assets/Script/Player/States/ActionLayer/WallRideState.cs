using UnityEngine;
using SkateGame;
using QFramework;

public class WallRideState : ActionStateBase
{
    private float normalGravity;
    private float onWallGravity = 0.1f;
    
    public WallRideState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
        isLoop = playerModel.Config.Value.isLoopWallRide;
        ignoreMovementLayerDuration = playerModel.Config.Value.ignoreMovementLayerDurationWallRide;
    }

    public override string GetStateName() => "WallRide";

    protected override void EnterActionState()
    {
        player.animator.Play("oPlayer@WallRide", 1);

        normalGravity = rb.gravityScale;
        rb.gravityScale = onWallGravity;

        if (player.WallRideEffect != null)
        {
            player.WallRideEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateActionState()
    {
        rb.gravityScale = Mathf.Lerp(onWallGravity, normalGravity, stateTimer / playerModel.Config.Value.wallrideDuration);
        if (playerModel.CurrentWall.Value == null || !inputModel.Grind.Value)
        {   
            rb.gravityScale = normalGravity;
            stateTimer = 0f;
            player.stateMachine.SwitchState(StateLayer.Action, "None");
            player.stateMachine.SwitchState(StateLayer.Movement, "Jump");
        }
    }

    protected override void ExitActionState()
    {   
        rb.gravityScale = normalGravity;
        playerModel.WallRideCooldownTimer.Value = playerModel.Config.Value.wallRideCooldown;
        if (player.WallRideEffect != null)
        {
            player.WallRideEffect.StopFeedbacks();
        }
    }
}