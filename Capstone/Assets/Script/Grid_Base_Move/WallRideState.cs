using UnityEngine;

public class WallRideState : StateBase
{
    
    private Rigidbody2D rb;
    private PlayerScript player;
    private float normalGravity;
    private float onWallGravity = 0.1f;

    private float lockedDirection = 0f; 


    public WallRideState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "WallRide";


    public override void Enter()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(input) > 0.01f)
        {
            lockedDirection = Mathf.Sign(input);
        }
        else
        {
            lockedDirection = Mathf.Sign(rb.linearVelocity.x);
        }

        normalGravity = rb.gravityScale;
        rb.gravityScale = onWallGravity;

    }

    public override void Update()
    {

        if (player.currentWall == null || !player.isEHeld)
        {
            player.stateMachine.SwitchState("Jump");
            return;
        }
    }

    public override void Exit()
    {
        rb.gravityScale = normalGravity;
        Debug.Log("ÍË³öWallRide×´Ì¬");
    }
}
