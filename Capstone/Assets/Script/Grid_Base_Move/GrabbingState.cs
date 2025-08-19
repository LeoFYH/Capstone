using UnityEngine;
using SkateGame;

public class GrabbingState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;

    public GrabbingState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grab";

    public override void Enter()
    {
        // Debug.Log("进入Grabbing状态");
        //can be damage
    }

    public override void Update()
    {
        // Debug.Log("Keep Grabbing");

        if (player.isEHeld == false)
        {
            player.stateMachine.SwitchState("Idle");
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
    }
}
