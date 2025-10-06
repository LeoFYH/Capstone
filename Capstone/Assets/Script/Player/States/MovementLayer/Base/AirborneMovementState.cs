using SkateGame;
using QFramework;
using UnityEngine;
public abstract class AirborneMovementState : StateBase
{
    protected virtual void UpdateAirMovement(){}
    
    public sealed override void Update()
    {
        /* 移动由OnMoveInput事件处理 */
        float moveInput = inputModel.Move.Value.x;
        player.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });
        switchGroundMovement();
        UpdateAirMovement();
    }
    private void switchGroundMovement()
    {
        Debug.Log("WasGrounded: " + playerModel.WasGrounded.Value);
        Debug.Log("IsGrounded: " + playerModel.IsGrounded.Value);
        if (!playerModel.WasGrounded.Value && playerModel.IsGrounded.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Land");
        }
    }
}


