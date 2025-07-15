using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;

public class GrindState : StateBase
{
    private PlayerScript player;
    private Rigidbody2D rb;
    private GrindSplinePath grindTrack;
    private float normalGravity;


    private float t = 0f;
    private float speed = 0.5f;

    private List<Vector3> sampledPoints;

    public GrindState(PlayerScript player, Rigidbody2D rb, GrindSplinePath track)
    {
        this.player = player;
        this.rb = rb;
        this.grindTrack = track;
    }


    public override string GetStateName() => "Grind";


    public override void Enter()
    {
        normalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        t = 0f;
        player.transform.position = grindTrack.GetPositionOnTrack(t);
        

    }

    public override void Update()
    {
        t += speed * Time.deltaTime;


        player.transform.position = grindTrack.GetPositionOnTrack(t);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.SwitchState("Jump");
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("ÍË³ö GrindSpline ×´Ì¬");
        rb.gravityScale = normalGravity;

    }
}
