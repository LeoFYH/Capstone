using UnityEngine;

public class AirState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    private float airControlForce = 10f; // 空中控制力
    private float maxAirHorizontalSpeed = 8f; // 最大空中水平速度
    private bool canDoubleJump = true; // 是否可以二段跳

    public AirState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Air";

    public override void Enter()
    {
        player.isInAir = true;
        player.airTime = 0f;
        player.airCombo = 0;
        canDoubleJump = true; // 重置二段跳
        Debug.Log("进入空中状态");
    }

    public override void Update()
    {
        // 更新空中时间
        player.airTime += Time.deltaTime;
        
        // 空中移动控制
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            // 使用力来移动，而不是直接设置速度
            rb.AddForce(Vector2.right * moveInput * airControlForce, ForceMode2D.Force);
            
            // 限制最大水平速度
            float currentVelocityX = rb.linearVelocity.x;
            if (Mathf.Abs(currentVelocityX) > maxAirHorizontalSpeed)
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(currentVelocityX) * maxAirHorizontalSpeed, rb.linearVelocity.y);
            }
        }

        // 二段跳检测
        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            PerformDoubleJump();
        }
        // 空中技巧输入检测
        else if (Input.GetKeyDown(KeyCode.J))
        {
            // 基础技巧
            PerformBasicTrick();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // 抓板技巧
            PerformGrabTrick();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            // 翻转技巧
            PerformFlipTrick();
        }

        // 检测落地
        if (player.IsGrounded())
        {
            // 落地时根据连击数给予奖励
            if (player.airCombo > 0)
            {
                Debug.Log($"空中连击完成！连击数: {player.airCombo}");
                // 这里可以添加连击奖励逻辑
            }
            
            player.stateMachine.SwitchState("Idle");
        }
    }

    public override void Exit()
    {
        player.isInAir = false;
        Debug.Log("退出空中状态");
    }

    // 执行基础技巧
    private void PerformBasicTrick()
    {
        player.airCombo++;
        Debug.Log($"执行基础技巧！连击数: {player.airCombo}");
        
        // 添加技巧分数
        ScoreManager.Instance.AddTrickScore(1);
        
        // 可以在这里添加视觉效果或音效
        // 例如：旋转玩家、播放动画等
    }

    // 执行抓板技巧
    private void PerformGrabTrick()
    {
        player.airCombo++;
        Debug.Log($"执行抓板技巧！连击数: {player.airCombo}");
        
        // 添加技巧分数
        ScoreManager.Instance.AddTrickScore(1);
        
        // 切换到抓板技巧状态
        player.stateMachine.SwitchState("GrabTrick");
    }

    // 执行翻转技巧
    private void PerformFlipTrick()
    {
        player.airCombo++;
        Debug.Log($"执行翻转技巧！连击数: {player.airCombo}");
        
        // 添加技巧分数
        ScoreManager.Instance.AddTrickScore(1);
        
        // 切换到翻转技巧状态
        player.stateMachine.SwitchState("Trick");
    }

    // 执行二段跳
    private void PerformDoubleJump()
    {
        canDoubleJump = false; // 禁用二段跳
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.doubleJumpForce);
        Debug.Log("二段跳！");
        
        // 添加二段跳分数
        ScoreManager.Instance.AddTrickScore(1);
    }
} 