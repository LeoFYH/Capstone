using UnityEngine;
using SkateGame;

public class GJumpState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;
    private bool hasJumped = false;
    public bool canDoubleJump = true;

    public GJumpState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Jump";

    public override void Enter()
    {
        // Debug.Log("GJ");
        hasJumped = true;
        canDoubleJump = true;

        rb.gravityScale = 1f;

        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(currentVelocity.x, player.maxJumpForce);
    }

    public override void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");


        if (Mathf.Abs(moveInput) > 0.01f)
        {
            rb.AddForce(Vector2.right * moveInput * player.airControlForce, ForceMode2D.Force);


            float max = player.maxAirHorizontalSpeed;
                            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -max, max), rb.velocity.y);
        }


        if (hasJumped && canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            canDoubleJump = false;
            player.stateMachine.SwitchState("DoubleJump");
        }

        if (player.IsGrounded() && rb.velocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState("Idle");
        }
    }

    public override void Exit()
    {

    }
}
