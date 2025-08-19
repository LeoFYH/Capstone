using UnityEngine;
using SkateGame;

public class AirState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float airControlForce = 10f; // 空中控制力
    private float maxAirHorizontalSpeed = 8f; // 最大空中水平速度
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

        // 检测技巧输入并切换到 TrickState
        if (Input.GetKeyDown(KeyCode.J))
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

        // 二段跳检测
        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            // 直接执行二段跳
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.doubleJumpForce);//没用doublejump状态机
            canDoubleJump = false; // 禁用二段跳
            Debug.Log("二段跳！");
            
            // 添加二段跳分数 - 暂时注释掉
            //ScoreManager.Instance.AddTrickScore(1);
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
            
            // 正常落地时立即打印技巧列表并开始5秒倒计时清零
            TrickScore.Instance.OnPlayerLanded();
            
            player.stateMachine.SwitchState("Idle");
        }
    }

    public override void Exit()
    {
        player.isInAir = false;
        Debug.Log("退出空中状态");
    }
} 