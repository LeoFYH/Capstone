using UnityEngine;
using SkateGame;

public class GJumpState : JumpState
{
    public bool canDoubleJump = true;

    public GJumpState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        Debug.Log("GJ");
        
        playerModel.GrindJumpTimer.Value = playerModel.Config.Value.grindJumpIgnoreTime;
        canDoubleJump = true;

        rb.gravityScale = 1f;

        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(currentVelocity.x, playerModel.Config.Value.maxJumpForce);

               // 播放MMF效果
        if (player.GJumpEffect != null)
        {
            player.GJumpEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("JumpEffecttPlayer为null，无法播放效果");
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
        else
        {
            Debug.LogWarning("JumpEffecttPlayer为null，无法播放效果");
        } 
    }

    // state change
    private void StateChange()
    {       
         if (canDoubleJump && inputModel.Jump.Value)
        {
            canDoubleJump = false;
            player.stateMachine.SwitchState(StateLayer.Movement, "DoubleJump");
        }
    }
}
