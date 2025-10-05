using UnityEngine;
using SkateGame;

public class WallRideState : ActionStateBase
{
    private float normalGravity;
    private float onWallGravity = 0.1f;
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
        if (playerModel.CurrentWall.Value == null || !inputModel.Grind.Value)
        {
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
