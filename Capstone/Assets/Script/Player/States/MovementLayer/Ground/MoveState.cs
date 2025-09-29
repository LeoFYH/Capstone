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
        //Debug.Log("进入Move状态");
        //currentVelocityX = rb.linearVelocity.x; // 保持当前水平速度
        if (player.moveEffect != null)
        {
            player.moveEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("moveEffectPlayer为null，无法播放效果");
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
        // 离开移动状态时停止水平速度
        //rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //currentVelocityX = 0f;
        // Debug.Log("退出Move状态");
        if (player.moveEffect != null)
        {
            player.moveEffect.StopFeedbacks();
        }
        else
        {
            Debug.LogWarning("moveEffectPlayer为null，无法播放效果");
        }
    }
} 