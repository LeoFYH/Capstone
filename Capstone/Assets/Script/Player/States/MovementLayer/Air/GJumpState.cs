using UnityEngine;
using SkateGame;

public class GJumpState : JumpState
{
    public GJumpState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        playerModel.GrindJumpTimer.Value = playerModel.Config.Value.grindJumpIgnoreTime;
        playerModel.CanDoubleJump.Value = true;

        rb.gravityScale = 1f;

        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(currentVelocity.x, playerModel.Config.Value.maxJumpForce);

               // 播放MMF效果
        if (player.GJumpEffect != null)
        {
            player.GJumpEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateAirMovement()
    {
        StateChange();

        float moveInput = inputModel.Move.Value.x;
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            rb.AddForce(Vector2.right * moveInput * playerModel.Config.Value.airControlForce, ForceMode2D.Force);


            float max = playerModel.Config.Value.maxAirHorizontalSpeed;
                            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -max, max), rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
         // 播放MMF效果
        if (player.GJumpEffect != null)
        {
            player.GJumpEffect.StopFeedbacks();
        }
    }

    // state change
    private void StateChange()
    {       
         if (playerModel.CanDoubleJump.Value && inputModel.JumpStart.Value)
        {
            playerModel.CanDoubleJump.Value = false;
            player.stateMachine.SwitchState(StateLayer.Movement, "DoubleJump");
        }
    }
}
