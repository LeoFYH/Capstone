using UnityEngine;
using SkateGame;
using QFramework;

public class AirState : AirborneMovementState
{ 
    private float airControlForce;
    private float maxAirHorizontalSpeed;
    private bool canDoubleJump;

    public AirState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        airControlForce = playerModel.AirControlForce.Value/2;
        maxAirHorizontalSpeed = playerModel.MaxAirHorizontalSpeed.Value/2;
        canDoubleJump = playerModel.CanDoubleJump.Value;
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

    protected override void UpdateAirMovement()
    {
        // 二段跳检测
        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            // 切换到DoubleJump状态，让DoubleJumpState处理二段跳逻辑和事件发送
            Debug.Log("AirState: 检测到二段跳输入，切换到DoubleJump状态");
            canDoubleJump = false; // 禁用二段跳
            player.stateMachine.SwitchState(StateLayer.Movement, "DoubleJump");
        }

        // 检测落地
        if (player.IsGrounded() && player.isInAir)
        {
            player.isInAir = false;
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
            
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
            Debug.Log("2222:" + player.stateMachine.GetMovementStateName());
        }
    }

    public override void Exit()
    {
        player.isInAir = false;
        Debug.Log("退出空中状态");
    }
} 