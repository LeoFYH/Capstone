using UnityEngine;

public class JumpState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    private float maxJumpForce = 14f;
    private float minJumpForce = 2f;
    private float chargeTime = 0f;
    private float maxChargeTime = 2f;
    private bool isCharging = false;
    private bool hasJumped = false;
    private bool canDoubleJump = true; // 是否可以二段跳
    private float doubleJumpForce = 8f; // 二段跳力度

    public JumpState(PlayerScript player, Rigidbody2D rb)
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
        canDoubleJump = true; // 每次进入Jump状态都重置二段跳
        Debug.Log("开始蓄力跳跃");
    }

    public override void Update()
    {
        // 空中左右微调
        float moveInput = Input.GetAxisRaw("Horizontal");
        float airMoveSpeed = 2f;
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            rb.linearVelocity = new Vector2(moveInput * airMoveSpeed, rb.linearVelocity.y);
        }

        // 蓄力跳逻辑
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > maxChargeTime)
                chargeTime = maxChargeTime;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                float t = Mathf.Clamp01(chargeTime / maxChargeTime);
                float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, t);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isCharging = false;
                hasJumped = true;
            }
        }
        // 二段跳逻辑
        else if (hasJumped && canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
            canDoubleJump = false; // 只能二段跳一次
            Debug.Log("二段跳！");
        }
        // 跳出后检测落地，落地后切回Idle
        else if (hasJumped && player.IsGrounded() && rb.linearVelocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState("Idle");
        }
    }

    public override void Exit()
    {
        Debug.Log("退出Jump状态");
    }
} 