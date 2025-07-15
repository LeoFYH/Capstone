using UnityEngine;

public class WallRideState : StateBase
{
    
    private Rigidbody2D rb;
    private PlayerScript player;
    private float normalGravity;
    private float onWallGravity;



    public WallRideState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "WallRide";


    public override void Enter()
    {
        Debug.Log("进入WallRide状态");
        normalGravity = rb.gravityScale;
        rb.gravityScale = onWallGravity;
    }

    public override void Update()
    {
        // WallRide状态下的逻辑
    }

    public override void Exit()
    {
        rb.gravityScale = normalGravity;
        Debug.Log("退出WallRide状态");
    }
}
