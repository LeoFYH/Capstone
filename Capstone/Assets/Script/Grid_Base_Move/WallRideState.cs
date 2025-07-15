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
        Debug.Log("����WallRide״̬");
        normalGravity = rb.gravityScale;
        rb.gravityScale = onWallGravity;
    }

    public override void Update()
    {
        // WallRide״̬�µ��߼�
    }

    public override void Exit()
    {
        rb.gravityScale = normalGravity;
        Debug.Log("�˳�WallRide״̬");
    }
}
