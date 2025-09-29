using UnityEngine;
using SkateGame;
using QFramework;

public class LandState : GroundMovementState
{
    public LandState(PlayerController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Land";

    public override void Enter()
    {
    }

    protected override void UpdateGroundMovement()
    {
        float moveInput = inputModel.Move.Value.x;

        /* 状态切换 */
        // 移动 
        if (rb.linearVelocity.x == 0)player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
        else player.stateMachine.SwitchState(StateLayer.Movement, "Move");
    }

    public override void Exit()
    {
    }
} 