using SkateGame;
using UnityEngine;
public abstract class AirborneMovementState : StateBase
{
    protected virtual void UpdateAirMovement(){}
    
    public sealed override void Update()
    {
        switchGroundMovement();
        UpdateAirMovement();
    }
    private void switchGroundMovement()
    {
        if (playerModel.IsGrounded.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Movement, "Land");
        }
    }
}


