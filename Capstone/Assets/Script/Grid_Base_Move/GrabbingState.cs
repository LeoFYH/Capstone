using UnityEngine;

public class GrabbingState : StateBase
{

    private Rigidbody2D rb;
    private PlayerScript player;


    public GrabbingState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grab";


    public override void Enter()
    {
        Debug.Log("����Grabbing״̬");
        //can be damage
    }

    public override void Update()
    {
        Debug.Log("Keep Grabbing");

        if (player.isEHeld == false)
        {
            player.stateMachine.SwitchState("Idle");
            return;
        }
        //else if (player.isEHeld == true && player.DetectNearbyPole() != null)
        //{
        //    player.currentTrack = player.DetectNearbyPole();
        //    player.stateMachine.SwitchState("Grind");
           
        //}


        // WallRide״̬�µ��߼�
        // add combo point
    }

    public override void Exit()
    {
        Debug.Log("�˳�Grabbing״̬");
        //exit can be damage
    }
}
