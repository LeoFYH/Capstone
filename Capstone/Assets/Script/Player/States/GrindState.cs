using UnityEngine;
using SkateGame;

public class GrindState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;
    private float speed;
    private Vector2 direction;
    private bool isJumping = false;
    private float normalG;

    public GrindState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grind";

    public override void Enter()
    {
        // Debug.Log("E Grind");
        
        // 检查currentTrack是否为null
        if (player.currentTrack == null)
        {
            Debug.Log("GrindState.Enter: currentTrack为null，无法进入滑轨状态，保持当前状态");
            // 不切换状态，直接退出Enter方法
            return;
        }
        
        Vector2 velocity = rb.linearVelocity;
        speed = velocity.magnitude;
        normalG = rb.gravityScale;
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
        
        // 开始滑轨计分
        //ScoreManager.Instance.StartGrindScoring();
    }

    public override void Update()
    {
        // 首先检查E键是否按住，如果没有按住立即退出
        if (!player.isEHeld)
        {
            Debug.Log("GrindState: E键未按住，退出滑轨状态");
            // 根据当前状态决定切换到哪个状态
            if (player.IsGrounded())
            {
                player.stateMachine.SwitchState("Idle");
            }
            else
            {
                player.stateMachine.SwitchState("Air");
            }
            return;
        }
        
        // 然后检查currentTrack是否为null
        if (player.currentTrack == null)
        {
            Debug.Log("GrindState: currentTrack为null，退出滑轨状态");
            // 根据当前状态决定切换到哪个状态
            if (player.IsGrounded())
            {
                player.stateMachine.SwitchState("Idle");
            }
            else
            {
                player.stateMachine.SwitchState("Air");
            }
            return;
        }

        if (isJumping) return;

        // 再次检查currentTrack，确保安全
        if (player.currentTrack != null)
        {
            Vector2 moveDelta = direction * speed * Time.deltaTime;
            Vector3 pos = player.transform.position;
            pos.x += moveDelta.x;
            pos.y = player.currentTrack.GetTrackPosition().y;
            player.transform.position = pos;

            rb.linearVelocity = new Vector2(direction.x * speed, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("G Jump");
            isJumping = true;
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(direction.x * speed, 10f);
            player.grindJumpTimer = player.grindJumpIgnoreTime;

            if (player.isEHeld)
            {
                player.StartCoroutine(player.SwitchToStateDelayed("Grab"));
            }
            else
            {
                player.StartCoroutine(player.SwitchToStateDelayed("GJump"));
            }
        }
    }

    private void SnapPlayerToTrack()
    {
        if (player.currentTrack != null)
        {
            Vector3 trackPos = player.currentTrack.GetTrackPosition();
            Vector3 playerPos = player.transform.position;
            playerPos.y = trackPos.y;
            player.transform.position = playerPos;
        }
        else
        {
            Debug.LogWarning("SnapPlayerToTrack: currentTrack为null，无法吸附到滑轨");
        }
    }

    public override void Exit()
    {
        rb.gravityScale = normalG;
        isJumping = false;
        // Debug.Log("Exit Grind");
        
        // 结束滑轨计分
        //ScoreManager.Instance.EndGrindScoring();
    }
}