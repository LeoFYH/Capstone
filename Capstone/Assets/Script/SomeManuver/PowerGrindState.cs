using UnityEngine;
using SkateGame;

public class PowerGrindState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float deceleration = 1f; // 每秒速度减少的量
    private float direction;

    public PowerGrindState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "PowerGrind";

    public override void Enter()
    {
        player.isPowerGrinding = true;

        direction = Mathf.Sign(rb.linearVelocity.x);
        if (direction == 0) direction = 1f;
    }

    public override void Update()
    {
        float vx = rb.linearVelocity.x;

        // 逐渐减少的速度，保持方向不变
        float newVx = vx - direction * deceleration * Time.deltaTime;

        // 防止越过零点
        if (Mathf.Sign(newVx) != direction || Mathf.Abs(newVx) < 0.01f)
        {
            newVx = 0;
        }

        rb.linearVelocity = new Vector2(newVx, rb.linearVelocity.y);


        if (!player.isWHeld || Mathf.Abs(rb.linearVelocity.x) <= 0.5f)
        {
            player.stateMachine.SwitchState("Idle");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.SwitchState("Jump");
        }

    }

    public override void Exit()
    {
        player.isPowerGrinding = false;
    }
}
