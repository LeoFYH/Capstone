using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("状态机")]
    public E stateMachine;
    private Rigidbody2D rb;

    [Header("跳跃设置")]
    public float maxJumpForce = 14f;
    public float minJumpForce = 2f;
    public float doubleJumpForce = 8f;
    public float maxChargeTime = 2f;

    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float airMoveSpeed = 2f;
    
    [Header("轨道设置")]
    public Track currentTrack; // 当前接触的轨道

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new E();
        stateMachine.AddState("Idle", new IdleState());
        stateMachine.AddState("Jump", new JumpState(this, rb));
        stateMachine.AddState("Move", new MoveState(this, rb));
        stateMachine.AddState("DoubleJump", new DoubleJumpState(this, rb));
        stateMachine.AddState("Grind", new GrindState(this, rb));
        stateMachine.SwitchState("Idle");
    }

    // 检测玩家周围isTrigger并且带有Track脚本的Collider
    public void CheckTriggerAndTrack()
    {
        // 使用正方形检测，大小为1x1
        Vector2 boxSize = new Vector2(1f, 1f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                var track = col.GetComponent<Track>();
                if (track != null)
                {
                    Debug.Log($"检测到Trigger并且有Track脚本: {col.name}");
                    currentTrack = track; // 设置当前轨道
                    // 自动进入滑轨状态
                    stateMachine.SwitchState("Grind");
                    return;
                }
            }
        }
        currentTrack = null; // 清除当前轨道
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

        CheckTriggerAndTrack();
        
    }

    // 判断是否在地面上
    public bool IsGrounded()
    {
        // 这里简单示例，实际项目建议用射线检测
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }
}