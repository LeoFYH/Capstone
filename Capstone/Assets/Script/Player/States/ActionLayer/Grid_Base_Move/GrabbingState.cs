using UnityEngine;
using SkateGame;

public class GrabbingState : StateBase
{

    public GrabbingState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grab";

    public override void Enter()
    {
        Debug.Log("进入Grabbing状态");
        //can be damage


                //MMF
        if (player.GrabEffect != null)
        {
            player.GrabEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("GrabEffect为null，无法播放效果");
        } 
    }

    public override void Update()
    {
        // Debug.Log("Keep Grabbing");

        if (inputModel.Grind.Value == false)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "None");
            player.stateMachine.SwitchState(StateLayer.Movement, "Idle");
        }
        //else if (player.isEHeld == true && player.DetectNearbyPole() != null)
        //{
        //    player.currentTrack = player.DetectNearbyPole();
        //    player.stateMachine.SwitchState("Grind");
            
        //}


        // WallRide状态下的逻辑
        // add combo point
    }

    public override void Exit()
    {
        // Debug.Log("退出Grabbing状态");
        //exit can be damage

        //MMF
        if (player.GrabEffect != null)
        {
            player.GrabEffect.StopFeedbacks();
        }
        else
        {
            Debug.LogWarning("GrabEffect为null，无法播放效果");
        } 
    }
}
