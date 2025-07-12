using UnityEngine;

public class JumpState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    private float maxJumpForce = 14f; // 最大跳跃力
    private float minJumpForce = 2f; // 最小跳跃力
    private float chargeTime = 0f;
    private float maxChargeTime = 2f;
    private bool isCharging = false;
    private bool hasJumped = false;

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
        Debug.Log("开始蓄力跳跃");
    }

    public override void Update()
    {
        // 空中左右微调
        float moveInput = Input.GetAxisRaw("Horizontal");
        float airMoveSpeed = 2f; // 空中漂移速度，可自行调整
        rb.linearVelocity = new Vector2(moveInput * airMoveSpeed, rb.linearVelocity.y);

        // 蓄力跳逻辑（保持不变）
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