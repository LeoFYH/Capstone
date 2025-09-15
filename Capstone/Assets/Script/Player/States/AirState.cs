using UnityEngine;
using SkateGame;
using QFramework;

public class AirState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    // 使用InputController的参数，不在这里重复定义
    private bool canDoubleJump = true; // 是否可以二段跳

    public AirState(InputController player, Rigidbody2D rb)
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
        
        // 空中移动控制（统一阻力感，AddForce方式）
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            // 获取当前水平速度
            float currentHorizontalVelocity = rb.linearVelocity.x;
            
            // 计算目标速度（使用InputController的参数）
            float targetVelocity = moveInput * player.maxAirHorizontalSpeed;
            
            // 计算速度差值
            float velocityDifference = targetVelocity - currentHorizontalVelocity;
            
            // 应用空中控制力（基于速度差值，产生阻力感）
            // 增加控制力倍数，让转向更容易
            float moveForce = velocityDifference * player.airControlForce * 1.5f;
            
            // 限制力的大小，避免过度加速
            moveForce = Mathf.Clamp(moveForce, -player.airControlForce * 3f, player.airControlForce * 3f);
            
            // 应用力到刚体（AddForce方式产生阻力感）
            rb.AddForce(new Vector2(moveForce, 0), ForceMode2D.Force);
        }

        // 检测技巧输入并切换到 TrickState
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E))
        {
            //新的trickstate注册方法，带动作名
            player.stateMachine.SwitchState("Trick", "TrickA");
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            //新的trickstate注册方法，带动作名，自动在StateMachine里调用trickstate的settrickname方法来传入特技名称
            player.stateMachine.SwitchState("Trick", "TrickB");
            return;
        }

        // 二段跳检测已移至InputController统一处理

        // 检测落地
        if (player.IsGrounded() && player.isInAir)
        {
            player.isInAir = false;
            
            // 着陆时平滑处理水平速度，避免突然停止
            float currentHorizontalVelocity = rb.linearVelocity.x;
            if (Mathf.Abs(currentHorizontalVelocity) > 0.1f)
            {
                // 将水平速度减少到合理范围，避免着陆后继续高速移动
                float dampedVelocity = currentHorizontalVelocity * 0.3f; // 减少到30%
                rb.linearVelocity = new Vector2(dampedVelocity, rb.linearVelocity.y);
            }
            
            // 落地时根据连击数给予奖励
            if (player.airCombo > 0)
            {
                // Debug.Log($"空中连击完成！连击数: {player.airCombo}");
                // 这里可以添加连击奖励逻辑
            }
            
            // 处理瞄准时间奖励（如果执行了trick）
            player.HandleLandingAimTimeBonus();
            
            // 确保颜色恢复为白色
            player.ResetPlayerColor();
            
            // 发送玩家落地事件，让系统处理
            //player.SendEvent<PlayerLandedEvent>(new PlayerLandedEvent());
            
            player.stateMachine.SwitchState("Idle");
            Debug.Log("2222:" + player.stateMachine.GetCurrentStateName());
        }
    }

    public override void Exit()
    {
        player.isInAir = false;
        Debug.Log("退出空中状态");
    }
} 