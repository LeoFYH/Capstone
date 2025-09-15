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
        //Debug.Log("进入Move状态");
        //currentVelocityX = rb.linearVelocity.x; // 保持当前水平速度
    }

    public override void Update()
    {
        // Move状态下直接处理地面移动
        float moveInput = Input.GetAxis("Horizontal");
        
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            // 检测方向改变（输入方向与当前速度方向相反）
            bool isDirectionChange = Mathf.Sign(moveInput) != Mathf.Sign(currentVelocityX) && Mathf.Abs(currentVelocityX) > 0.1f;
            
            if (isDirectionChange)
            {
                // 方向改变时，先快速减速，然后反向加速
                float turnDeceleration = deceleration * 3f; // 3倍减速
                if (currentVelocityX > 0)
                {
                    currentVelocityX -= turnDeceleration * Time.deltaTime;
                    if (currentVelocityX < 0) currentVelocityX = 0;
                }
                else if (currentVelocityX < 0)
                {
                    currentVelocityX += turnDeceleration * Time.deltaTime;
                    if (currentVelocityX > 0) currentVelocityX = 0;
                }
                
                // 如果速度已经接近0，开始反向加速
                if (Mathf.Abs(currentVelocityX) < 0.1f)
                {
                    currentVelocityX += moveInput * acceleration * 2f * Time.deltaTime; // 2倍加速
                }
                
                Debug.Log($"检测到方向改变: 输入={moveInput}, 当前速度={currentVelocityX}");
            }
            else
            {
                // 正常加速
                currentVelocityX += moveInput * acceleration * Time.deltaTime;
            }
            
            currentVelocityX = Mathf.Clamp(currentVelocityX, -maxSpeed, maxSpeed);
        }
        else
        {
            // 减速
            if (currentVelocityX > 0)
            {
                currentVelocityX -= deceleration * Time.deltaTime;
                if (currentVelocityX < 0) currentVelocityX = 0;
            }
            else if (currentVelocityX < 0)
            {
                currentVelocityX += deceleration * Time.deltaTime;
                if (currentVelocityX > 0) currentVelocityX = 0;
            }
        }
        
        // 应用水平速度
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