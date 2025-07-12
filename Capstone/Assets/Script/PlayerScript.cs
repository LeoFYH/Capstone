using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public E stateMachine;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new E();
        stateMachine.AddState("Idle", new IdleState());
        stateMachine.AddState("Jump", new JumpState(this, rb));
        stateMachine.AddState("Move", new MoveState(this, rb));
        stateMachine.SwitchState("Idle");
    }

    void Update()
    {
        stateMachine.UpdateCurrentState();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            stateMachine.SwitchState("Jump");
            return;
        }

        string currentState = stateMachine.GetCurrentStateName();
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Idle时有输入才切Move
        if (currentState == "Idle" && Mathf.Abs(moveInput) > 0.01f && IsGrounded())
        {
            stateMachine.SwitchState("Move");
        }
        // Move时没输入才切Idle
        else if (currentState == "Move" && Mathf.Abs(moveInput) <= 0.01f && IsGrounded())
        {
            stateMachine.SwitchState("Idle");
        }
    }

    // 判断是否在地面上
    public bool IsGrounded()
    {
        // 这里简单示例，实际项目建议用射线检测
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }
}