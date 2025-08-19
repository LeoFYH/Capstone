using UnityEngine;
using SkateGame;
using QFramework;

public class MoveState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    
    // 加速度相关参数
    private float currentVelocityX = 0f;
    private float acceleration = 15f; // 加速度
    private float deceleration = 20f; // 减速度
    private float maxSpeed = 5f; // 最大速度

    public MoveState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Move";

    public override void Enter()
    {
        // Debug.Log("进入Move状态");
        currentVelocityX = rb.velocity.x; // 保持当前水平速度
    }

    public override void Update()
    {
        // Move状态下发送移动事件
        float moveInput = Input.GetAxisRaw("Horizontal");
        player.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });
    }

    public override void Exit()
    {
        // 离开移动状态时停止水平速度
        //rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //currentVelocityX = 0f;
        // Debug.Log("退出Move状态");
    }
} 