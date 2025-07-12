using UnityEngine;

public class MoveState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;

    public MoveState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Move";

    public override void Enter()
    {
        Debug.Log("进入Move状态");
    }

    public override void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // -1(左), 0, 1(右)
        rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        // 离开移动状态时停止水平速度
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        Debug.Log("退出Move状态");
    }
} 