using UnityEngine;
using SkateGame;

public class WallRideState : ActionStateBase
{
    private float normalGravity;
    private float onWallGravity = 0.1f;

    private float lockedDirection = 0f; 

    public WallRideState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
    }

    public override string GetStateName() => "WallRide";

    protected override void EnterActionState()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(input) > 0.01f)
        {
            lockedDirection = Mathf.Sign(input);
        }
        else
        {
            lockedDirection = Mathf.Sign(rb.linearVelocity.x);
        }

        normalGravity = rb.gravityScale;
        rb.gravityScale = onWallGravity;

        if (player.WallRideEffect != null)
        {
            player.WallRideEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("WallRideEffect为null，无法播放效果");
        }
    }

    protected override void UpdateActionState()
    {
        rb.gravityScale = onWallGravity;
        if (playerModel.CurrentWall.Value == null || !inputModel.Grind.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "None");
            player.stateMachine.SwitchState(StateLayer.Movement, "Jump");
        }
    }

    protected override void ExitActionState()
    {
        rb.gravityScale = normalGravity;
        // Debug.Log("退出WallRide状态");
        
        if (player.WallRideEffect != null)
        {
            player.WallRideEffect.StopFeedbacks();

        }
        else
        {
            Debug.LogWarning("WallRideEffect为null，无法停止效果");
        }
    }
}
