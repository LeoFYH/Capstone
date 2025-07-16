using UnityEngine;

public class GrindState : StateBase
{
    private PlayerScript player;
    private Rigidbody2D rb;
    private float speed;
    private Vector2 direction;
    private bool isJumping = false;
    public GrindState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grind";

    public override void Enter()
    {
        Debug.Log("E Grind");
        Vector2 velocity = rb.linearVelocity;
        speed = velocity.magnitude;

        if (speed < 0.1f)
        {
            Vector2 trackDir = player.currentTrack.GetTrackDirection();
            direction = new Vector2(trackDir.x, 0).normalized; 
            speed = player.moveSpeed;
        }
        else
        {
            direction = new Vector2(velocity.x, 0).normalized;
        }

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(direction.x * speed, 0);
        SnapPlayerToTrack();
    }

    public override void Update()
    {
        if (isJumping)
            return;

        Vector2 moveDelta = direction * speed * Time.deltaTime;
        Vector3 pos = player.transform.position;
        pos.x += moveDelta.x;
        pos.y = player.currentTrack.GetTrackPosition().y;
        player.transform.position = pos;

        rb.linearVelocity = new Vector2(direction.x * speed, 0);

        if (!IsOnTrack() || !player.isEHeld)
        {
            player.stateMachine.SwitchState("Jump");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("G Jump");
            isJumping = true;
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(direction.x * speed, 10f);
            player.grindJumpTimer = player.grindJumpIgnoreTime;
            player.StartCoroutine(player.SwitchToStateDelayed("GJump"));

            //if (player.isEHeld)
            //    player.StartCoroutine(player.SwitchToStateDelayed("GrabBoard"));
        }
    }

    private void SnapPlayerToTrack()
    {
        Vector3 trackPos = player.currentTrack.GetTrackPosition();
        Vector3 playerPos = player.transform.position;
        playerPos.y = trackPos.y;
        player.transform.position = playerPos;
    }

    private bool IsOnTrack()
    {
        Vector2 boxSize = new Vector2(1f, 1f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(player.transform.position, boxSize, 0f);
        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                var track = col.GetComponent<Track>();
                if (track != null && track == player.currentTrack)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void Exit()
    {
        rb.gravityScale = 1f;
        isJumping = false;
        //rb.linearVelocity = direction * speed;
        Debug.Log("Exit");
    }
} 