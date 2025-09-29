using SkateGame;
using QFramework;
using UnityEngine;

public abstract class GroundMovementState : StateBase
{
    protected bool WasGrounded => this.GetModel<IPlayerModel>().WasGrounded.Value;
    protected bool IsGrounded => this.GetModel<IPlayerModel>().IsGrounded.Value;
    protected virtual void UpdateGroundMovement(){}
    
    public sealed override void Update()
    {
        switchAirborneMovement();
        UpdateGroundMovement();
    }
    private void switchAirborneMovement()
    {
        if (inputModel.JumpStart.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Jump");
        }
        else
        {
            CheckFall();
        }
    }
    private void CheckFall()
    {   
        if(WasGrounded && !IsGrounded)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Air");
        }
    }
}
