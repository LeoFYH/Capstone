using UnityEngine;

public class DoubleJumpState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    
    public DoubleJumpState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "DoubleJump";

    public override void Enter()
    {
        // 直接跳起来
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.doubleJumpForce);
        // Debug.Log("二段跳！");
    }

    public override void Update()
    {
        // 空中左右微调
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            rb.linearVelocity = new Vector2(moveInput * player.airMoveSpeed, rb.linearVelocity.y);
        }
        
        // 检测落地，落地后切回Idle
        if (player.IsGrounded() && rb.linearVelocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState("Idle");
        }
    }

    public override void Exit()
    {
        // Debug.Log("退出DoubleJump状态");
    }
} 