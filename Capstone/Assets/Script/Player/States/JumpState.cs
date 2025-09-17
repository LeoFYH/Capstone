using UnityEngine;
using SkateGame;
using QFramework;

public class JumpState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private IPlayerModel playerModel;
    private float chargeTime;
    private bool isCharging;
    private bool hasJumped;
    private float initialHorizontalVelocity;
    public JumpState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        playerModel = this.GetModel<IPlayerModel>();
        chargeTime = playerModel.ChargeTime.Value;
        isCharging = playerModel.IsCharging.Value;
        hasJumped = playerModel.HasJumped.Value;
        initialHorizontalVelocity = playerModel.InitialHorizontalVelocity.Value;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        // Debug.Log("JumpState.Enter() - 开始跳跃");
        isCharging = true;
        chargeTime = 0f;
        hasJumped = false;
        initialHorizontalVelocity = rb.linearVelocity.x;
        // Debug.Log($"初始水平速度: {initialHorizontalVelocity}");
        
        // 直接执行跳跃逻辑
        
        Debug.Log("JumpState: 执行跳跃");
        if (player != null)
        {
            // 获取当前水平速度
            float currentHorizontalVelocity = rb.linearVelocity.x;
            
            // 执行跳跃
            float jumpForce = player.maxJumpForce;
            rb.linearVelocity = new Vector2(currentHorizontalVelocity, jumpForce);
            
            Debug.Log($"JumpState执行跳跃 - 使用跳跃力: {jumpForce}, 水平速度: {currentHorizontalVelocity}");
            
            // 立即切换到Air状态
            player.stateMachine.SwitchState("Air");
        }
        else
        {
            Debug.LogError("player为空！");
        }
    }

    public override void Update()
    {
        // 蓄力跳逻辑（现在只是计时，移动由移动系统处理）
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > player.maxChargeTime)
                chargeTime = player.maxChargeTime;
        }
        
        // Jump状态下的空中移动控制（在切换到Air状态之前）
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            // 计算空中移动力
            float moveForce = moveInput * player.airControlForce;
            
            // 获取当前水平速度
            float currentHorizontalVelocity = rb.linearVelocity.x;
            
            // 限制最大空中水平速度
            if (Mathf.Abs(currentHorizontalVelocity) < player.maxAirHorizontalSpeed)
            {
                // 应用空中移动力
                rb.linearVelocity = new Vector2(currentHorizontalVelocity + moveForce * Time.deltaTime, rb.linearVelocity.y);
            }
            else
            {
                // 如果已经达到最大速度，只允许减速
                if (Mathf.Sign(moveInput) != Mathf.Sign(currentHorizontalVelocity))
                {
                    rb.linearVelocity = new Vector2(currentHorizontalVelocity + moveForce * Time.deltaTime, rb.linearVelocity.y);
                }
            }
        }
    }
   

    public override void Exit()
    {
        // Debug.Log("退出Jump状态");
    }
} 