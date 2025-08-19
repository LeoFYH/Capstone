using UnityEngine;
using SkateGame;

public class JumpState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool hasJumped = false;
    private float initialHorizontalVelocity;
    public JumpState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        isCharging = true;
        chargeTime = 0f;
        hasJumped = false;
        initialHorizontalVelocity = rb.linearVelocity.x;
        // Debug.Log("开始蓄力跳跃");
    }

    public override void Update()
    {
        // 空中左右微调
        float moveInput = Input.GetAxisRaw("Horizontal");
        //if (Mathf.Abs(moveInput) > 0.01f)
        //{
        //    // 使用airMoveSpeed而不是直接操作刚体
        //    float targetVelocityX = moveInput * player.airMoveSpeed;
        //    rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
        //}

        // 蓄力跳逻辑
        if (isCharging)
        {
            if (Mathf.Abs(moveInput) > 0.01f)
            {
                float targetVelocityX = moveInput * player.moveSpeed;
                rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
            }

            chargeTime += Time.deltaTime;
            if (chargeTime > player.maxChargeTime)
                chargeTime = player.maxChargeTime;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                float t = Mathf.Clamp01(chargeTime / player.maxChargeTime);
                float jumpForce = Mathf.Lerp(player.minJumpForce, player.maxJumpForce, t);
                rb.linearVelocity = new Vector2(initialHorizontalVelocity, jumpForce); // 完整继承初始速度
                isCharging = false;
                hasJumped = true;
            }
        }
        // 跳出后切换到空中状态
        else if (hasJumped && !player.IsGrounded())
        {
            player.stateMachine.SwitchState("Air");
        }
        // 落地后切回Idle
        else if (hasJumped && player.IsGrounded() && rb.linearVelocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState("Idle");
        }
        else if (hasJumped)
        {
            float vx = rb.linearVelocity.x;

            // 判断是否与当前运动方向相反
            if (Mathf.Abs(moveInput) > 0.01f && Mathf.Sign(moveInput) != Mathf.Sign(vx))
            {
                rb.AddForce(Vector2.right * moveInput * player.airControlForce, ForceMode2D.Force);

                // 控制最大水平速度
                float max = player.maxAirHorizontalSpeed;
                rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -max, max), rb.linearVelocity.y);
            }
        }
    }

    public override void Exit()
    {
        // Debug.Log("退出Jump状态");
    }
} 