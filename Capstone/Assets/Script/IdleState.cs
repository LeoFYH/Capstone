using UnityEngine;
using SkateGame;
using QFramework;

public class IdleState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;

    public IdleState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Idle";

    public override void Enter()
    {
        // Debug.Log("进入Idle状态");
        ///
        /// 
    }

    public override void Update()
    {
        // Idle状态下发送移动事件（即使没有移动输入）
        float moveInput = Input.GetAxisRaw("Horizontal");
        player.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });
    }

    public override void Exit()
    {
        // Debug.Log("退出Idle状态");
    }
} 