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
        // 直接跳起来
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerModel.doubleJumpForce.Value);
        Debug.Log("DoubleJumpState: 执行二段跳！");
        
        // 发送技巧执行事件给系统，更新UIController
        Debug.Log("DoubleJumpState: 发送TrickPerformedEvent事件");
        player.SendEvent<TrickPerformedEvent>(new TrickPerformedEvent { TrickName = "doublejump" });
        
    }

    public override void Update()
    {
        // 空中左右微调
        float moveInput = Input.GetAxisRaw("Horizontal");
        //if (Mathf.Abs(moveInput) > 0.01f)
        //{
        //    rb.linearVelocity = new Vector2(moveInput * player.airMoveSpeed, rb.linearVelocity.y);
        //}
        
        // 检测落地，落地后切回Idle
        if (player.IsGrounded() && rb.linearVelocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState("Idle");
            Debug.Log("2222:" + player.stateMachine.GetCurrentStateName());
        }
        this.player.GetModel<IPlayerModel>().AllowDoubleJump.Value = false;
    }

    public override void Exit()
    {
        // Debug.Log("退出DoubleJump状态");
        this.player.GetModel<IPlayerModel>().AllowDoubleJump.Value = false;
    }
} 