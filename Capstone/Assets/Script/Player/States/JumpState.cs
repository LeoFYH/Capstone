using UnityEngine;
using SkateGame;
using QFramework;

public class JumpState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float chargeTime;
    private bool isCharging;
    private bool hasJumped;
    private float initialHorizontalVelocity;
    public JumpState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
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
        
        // 立即发送跳跃执行事件
        player.SendEvent<JumpExecuteEvent>();
    }

    public override void Update()
    {
        // 蓄力跳逻辑（现在只是计时，移动由移动系统处理）
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > playerModel.maxChargeTime.Value)
                chargeTime = playerModel.maxChargeTime.Value;
        }
        
        // Jump状态下发送移动事件
        float moveInput = Input.GetAxisRaw("Horizontal");
        player.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });
    }
    ///
    /// //
    
    // 公共方法，供InputController调用执行跳跃
    //错了 应该写进系统逻辑 state只管发信号给系统
    // public void ExecuteJump()
    // {
    //     Debug.Log($"执行跳跃 - 使用固定跳跃力: {player.maxJumpForce}");
    //     float jumpForce = player.maxJumpForce;
    //     rb.velocity = new Vector2(initialHorizontalVelocity, jumpForce); // 完整继承初始速度
    //     Debug.Log($"设置速度: ({initialHorizontalVelocity}, {jumpForce})");
    //     isCharging = false;
    //     hasJumped = true;
    //     Debug.Log("跳跃执行完成");
        
    //     // 立即切换到Air状态，这样空中移动就能正常工作
    //     player.stateMachine.SwitchState("Air");
    // }

    public override void Exit()
    {
        // Debug.Log("退出Jump状态");
    }
} 