using UnityEngine;
using SkateGame;
using QFramework;

public class MoveState : GroundMovementState
{
    // 加速度相关参数
    private float currentVelocityX;
    private float acceleration;
    private float deceleration;
    private float maxSpeed;

    public MoveState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        currentVelocityX = playerModel.CurrentVelocityX.Value;
        acceleration = playerModel.Acceleration.Value;
        deceleration = playerModel.MoveDeceleration.Value;
        maxSpeed = playerModel.MaxSpeed.Value;
    }

    public override string GetStateName() => "Move";

    public override void Enter()
    {
        if (player.moveEffect != null)
        {
            player.moveEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateGroundMovement()
    {
        /* 状态切换 */
        // 停止移动
        if (rb.linearVelocity.x == 0)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
        }
        // PowerGrind
        if (inputModel.TrickStart.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "PowerGrind");
        }
    }

    public override void Exit()
    {
        if (player.moveEffect != null)
        {
            player.moveEffect.StopFeedbacks();
        }
    }
} 