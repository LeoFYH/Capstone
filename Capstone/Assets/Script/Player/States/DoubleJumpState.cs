using UnityEngine;
using SkateGame;
using QFramework;

public class DoubleJumpState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;
    
    public DoubleJumpState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "DoubleJump";

    public override void Enter()
    {
        // 获取当前水平速度（与JumpState保持一致）
        float currentHorizontalVelocity = rb.linearVelocity.x;
        
        // 执行二段跳（与JumpState相同的逻辑）
        float jumpForce = player.doubleJumpForce;
        rb.linearVelocity = new Vector2(currentHorizontalVelocity, jumpForce);
        
        Debug.Log($"DoubleJumpState执行二段跳 - 使用跳跃力: {jumpForce}, 水平速度: {currentHorizontalVelocity}");
        
        // 发送技巧执行事件给系统，更新UIController
        Debug.Log("DoubleJumpState: 发送TrickPerformedEvent事件");
        player.SendEvent<TrickPerformedEvent>(new TrickPerformedEvent { TrickName = "doublejump" });
        
        // 立即切换到Air状态，让AirState处理空中移动和落地检测
        player.stateMachine.SwitchState("Air");
    }

    public override void Update()
    {
        // DoubleJump状态会立即切换到Air状态，所以这里不需要处理移动和落地检测
        // 空中移动和落地检测都由AirState处理
    }

    public override void Exit()
    {
        // Debug.Log("退出DoubleJump状态");
    }
} 