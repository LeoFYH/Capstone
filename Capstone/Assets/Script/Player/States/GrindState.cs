using UnityEngine;
using SkateGame;
using QFramework;

public class GrindState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;
    private float speed;
    private Vector2 direction;
    private float normalG;
    private bool isJumping;
  
    public GrindState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        speed = playerModel.Speed.Value;
        direction = playerModel.GrindDirection.Value;
        normalG = playerModel.NormalG.Value;
        isJumping = playerModel.IsJumping.Value;
    }

    public override string GetStateName() => "Grind";

    public override void Enter()
    {
        // Debug.Log("E Grind");
        
        // 检查currentTrack是否为null
        if (playerModel.CurrentTrack.Value == null)
        {
            Debug.LogError("GrindState.Enter: currentTrack为null，无法进入滑轨状态");
            player.stateMachine.SwitchState("Jump");
            return;
        }
        
        Vector2 velocity = rb.linearVelocity;
        speed = velocity.magnitude;
        normalG = rb.gravityScale;
        if (speed < 0.1f)
        {
            Vector2 trackDir = playerModel.CurrentTrack.Value.GetTrackDirection();
            direction = new Vector2(trackDir.x, 0).normalized;
            speed = playerModel.moveSpeed.Value;
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

        if (player.GrindEffect != null)
        {
            player.GrindEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("GrindEffect为null，无法播放效果");
        }
    }

    public override void Update()
    {
        // 首先检查E键是否按住，如果没有按住立即退出
        if (!inputModel.Grind.Value)
        {
            Debug.Log("GrindState: E键未按住，切换到Jump状态");
            player.stateMachine.SwitchState("Jump");
            return;
        }
        
        // 然后检查currentTrack是否为null
        if (playerModel.CurrentTrack.Value == null)
        {
            Debug.Log("GrindState: currentTrack为null，切换到Jump状态");
            player.stateMachine.SwitchState("Jump");
            return;
        }

        if (isJumping) return;

        // 再次检查currentTrack，确保安全
        if (playerModel.CurrentTrack.Value != null)
        {
            Vector2 moveDelta = direction * speed * Time.deltaTime;
            Vector3 pos = player.transform.position;
            pos.x += moveDelta.x;
            pos.y = playerModel.CurrentTrack.Value.GetTrackPosition().y+0.2f;
            player.transform.position = pos;

            rb.linearVelocity = new Vector2(direction.x * speed, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(direction.x * speed, 10f);

            if (inputModel.Grind.Value)
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
        if (playerModel.CurrentTrack.Value != null)
        {
            Vector3 trackPos = playerModel.CurrentTrack.Value.GetTrackPosition();
            Vector3 playerPos = player.transform.position;
            playerPos.y = trackPos.y+0.2f;
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

        if (player.GrindEffect != null)
        {
            player.GrindEffect.StopFeedbacks();

        }
        else
        {
            Debug.LogWarning("pGrindEffect为null，无法停止效果");
        }
    }

}