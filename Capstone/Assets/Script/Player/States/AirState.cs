using UnityEngine;
using SkateGame;
using QFramework;

public class AirState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float airControlForce;
    private float maxAirHorizontalSpeed;
    private bool canDoubleJump;

    public AirState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        airControlForce = playerModel.AirControlForce.Value;
        maxAirHorizontalSpeed = playerModel.MaxAirHorizontalSpeed.Value;
        canDoubleJump = playerModel.CanDoubleJump.Value;
    }

    public override string GetStateName() => "Air";

    public override void Enter()
    {
         Debug.Log("进入空中状态");
    }

    public override void Update()
    {
        
        // Air状态下发送移动事件
        float moveInput = Input.GetAxisRaw("Horizontal");
        player.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });

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

        // 二段跳检测
        if (Input.GetKeyDown(KeyCode.Space) && this.player.GetModel<IPlayerModel>().AllowDoubleJump.Value)
        {
            // 切换到DoubleJump状态，让DoubleJumpState处理二段跳逻辑和事件发送
            Debug.Log("AirState: 检测到二段跳输入，切换到DoubleJump状态");
            player.stateMachine.SwitchState("DoubleJump");

            
        }

        // 检测落地
        if (playerModel.IsGrounded.Value)
        {
            // 落地时根据连击数给予奖励
            if (playerModel.AirCombo.Value > 0)
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
        Debug.Log("退出空中状态");
    }
} 