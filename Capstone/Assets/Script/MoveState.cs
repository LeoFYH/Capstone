using UnityEngine;

public class MoveState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    
    // 加速度相关参数
    private float currentVelocityX = 0f;
    private float acceleration = 15f; // 加速度
    private float deceleration = 20f; // 减速度
    private float maxSpeed = 5f; // 最大速度

    public MoveState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Move";

    public override void Enter()
    {
        // Debug.Log("进入Move状态");
        currentVelocityX = rb.linearVelocity.x; // 保持当前水平速度
    }

    public override void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // -1(左), 0, 1(右)
        
        // 根据输入计算目标速度
        float targetVelocityX = moveInput * maxSpeed;

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            // 是否是转向：当前速度方向和输入方向相反
            bool isTurning = Mathf.Sign(currentVelocityX) != Mathf.Sign(moveInput) && Mathf.Abs(currentVelocityX) > 0.1f;

            if (isTurning)
            {
                // 转向时使用更大的减速
                currentVelocityX -= Mathf.Sign(currentVelocityX) * deceleration * Time.deltaTime;

                // 如果已经减速到0了，再开始按新方向加速
                if (Mathf.Sign(currentVelocityX) != Mathf.Sign(rb.linearVelocity.x))
                {
                    currentVelocityX = 0f;
                }
            }
            else
            {
                // 正常加速
                currentVelocityX += moveInput * acceleration * Time.deltaTime;
            }

            // 限制最大速度
            currentVelocityX = Mathf.Clamp(currentVelocityX, -maxSpeed, maxSpeed);
        }
        else
        {
            // 无输入时减速至0
            if (Mathf.Abs(currentVelocityX) > 0.1f)
            {
                currentVelocityX -= Mathf.Sign(currentVelocityX) * deceleration * Time.deltaTime;

                // 防止反向滑出
                if (Mathf.Sign(currentVelocityX) != Mathf.Sign(rb.linearVelocity.x))
                {
                    currentVelocityX = 0f;
                }
            }
            else
            {
                currentVelocityX = 0f;
            }
        }

        // 应用计算出的速度
        rb.linearVelocity = new Vector2(currentVelocityX, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        // 离开移动状态时停止水平速度
        //rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //currentVelocityX = 0f;
        // Debug.Log("退出Move状态");
    }
} 