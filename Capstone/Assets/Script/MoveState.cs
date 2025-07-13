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
        Debug.Log("进入Move状态");
        currentVelocityX = rb.linearVelocity.x; // 保持当前水平速度
    }

    public override void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // -1(左), 0, 1(右)
        
        // 根据输入计算目标速度
        float targetVelocityX = moveInput * maxSpeed;
        
        // 使用加速度/减速度平滑过渡到目标速度
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            // 有输入时加速
            if (currentVelocityX < targetVelocityX)
            {
                currentVelocityX += acceleration * Time.deltaTime;
                if (currentVelocityX > targetVelocityX)
                    currentVelocityX = targetVelocityX;
            }
            else if (currentVelocityX > targetVelocityX)
            {
                currentVelocityX -= acceleration * Time.deltaTime;
                if (currentVelocityX < targetVelocityX)
                    currentVelocityX = targetVelocityX;
            }
        }
        else
        {
            // 无输入时减速
            if (currentVelocityX > 0)
            {
                currentVelocityX -= deceleration * Time.deltaTime;
                if (currentVelocityX < 0)
                    currentVelocityX = 0;
            }
            else if (currentVelocityX < 0)
            {
                currentVelocityX += deceleration * Time.deltaTime;
                if (currentVelocityX > 0)
                    currentVelocityX = 0;
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
        Debug.Log("退出Move状态");
    }
} 