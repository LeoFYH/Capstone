using UnityEngine;
using SkateGame;
using QFramework;

public class AirState : AirborneMovementState
{ 
    private float airControlForce;
    private float maxAirHorizontalSpeed;
    private bool canDoubleJump;

    public AirState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        airControlForce = playerModel.Config.Value.airControlForceConfig / 2f;
        maxAirHorizontalSpeed = playerModel.Config.Value.maxAirHorizontalSpeedConfig / 2f;
        canDoubleJump = playerModel.CanDoubleJump.Value;
    }

    public override string GetStateName() => "Air";

    public override void Enter()
    {
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
        if (player.IsGrounded())
        {
            // 处理瞄准时间奖励（如果执行了trick）
            player.HandleLandingAimTimeBonus();
            
            // 发送玩家落地事件，让系统处理
            //player.SendEvent<PlayerLandedEvent>(new PlayerLandedEvent());
            
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
            Debug.Log("2222:" + player.stateMachine.GetMovementStateName());
        }
    }

    public override void Exit()
    {
        Debug.Log("退出空中状态");
    }
} 