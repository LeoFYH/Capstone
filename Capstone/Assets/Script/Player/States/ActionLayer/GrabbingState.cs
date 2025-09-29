using SkateGame;
using UnityEngine;

public class GrabbingState : ActionStateBase
{

    public GrabbingState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
    }

    public override string GetStateName() => "Grab";

    protected override void EnterActionState()
    {
        //MMF
        if (player.GrabEffect != null)
        {
            player.GrabEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateActionState()
    {

        //WallRide状态下的逻辑
        //add combo point
        if (inputModel.Grind.Value == false)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "None");

            //根据玩家当前状态决定Movement层的切换
            if (!playerModel.IsGrounded.Value)
            {
                //在空中，切换到Air状态以继续空中控制
                player.stateMachine.SwitchState(StateLayer.Movement, "Air");
            }
            else
            {
                //已落地，切换到Idle状态
                player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
            }
        }




    }

    protected override void ExitActionState()
    {
        //MMF
        if (player.GrabEffect != null)
        {
            player.GrabEffect.StopFeedbacks();
        }
    }
}
