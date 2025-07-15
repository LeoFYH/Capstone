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

    public override string GetStateName() => "Grabbing";


    public override void Enter()
    {
        Debug.Log("进入Grabbing状态");
        //can be damage
    }

    public override void Update()
    {
        // WallRide状态下的逻辑
        // add combo point
    }

    public override void Exit()
    {
        Debug.Log("退出Grabbing状态");
        //exit can be damage
    }
}
