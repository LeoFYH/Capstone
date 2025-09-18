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

        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(currentVelocity.x, playerModel.maxJumpForce.Value);
    }

    public override void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");


        if (Mathf.Abs(moveInput) > 0.01f)
        {
            rb.AddForce(Vector2.right * moveInput * playerModel.airControlForce.Value, ForceMode2D.Force);


            float max = playerModel.maxAirHorizontalSpeed.Value;
                            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -max, max), rb.linearVelocity.y);
        }


        if (hasJumped && canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            canDoubleJump = false;
            player.stateMachine.SwitchState("DoubleJump");
            Debug.Log("DoubleJump88888888888!");
        }

        if (player.IsGrounded() && rb.linearVelocity.y <= 0.01f)
        {
            player.stateMachine.SwitchState("Idle");
            Debug.Log("2222:" + player.stateMachine.GetCurrentStateName());
        }
    }

    public override void Exit()
    {

    }
}
