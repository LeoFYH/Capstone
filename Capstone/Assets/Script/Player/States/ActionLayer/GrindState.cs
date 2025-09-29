using UnityEngine;
using SkateGame;
using QFramework;

public class GrindState : ActionStateBase
{    private float speed;
    private Vector2 direction;
    private float normalG;
  
    public GrindState(PlayerController player, Rigidbody2D rb) : base(player, rb)
    {
        speed = playerModel.Speed.Value;
        direction = playerModel.GrindDirection.Value;
        normalG = playerModel.NormalG.Value;
        isLoop = true;
        isIgnoringMovementLayer = true;
        isRecovering = false;
    }

    public override string GetStateName() => "Grind";

    protected override void EnterActionState()
    {
        player.animator.SetBool("isNoseGrinding", true);

        // 检查currentTrack是否为null
        if (playerModel.CurrentTrack.Value == null)
        {
            Debug.LogError("GrindState.Enter: currentTrack为null，无法进入滑轨状态");
            player.stateMachine.SwitchState(StateLayer.Action, "Jump");
            return;
        }
        
        Vector2 velocity = rb.linearVelocity;
        speed = velocity.magnitude;
        normalG = rb.gravityScale;
        if (speed < 0.1f)
        {
            Vector2 trackDir = playerModel.CurrentTrack.Value.GetTrackDirection();
            direction = new Vector2(trackDir.x, 0).normalized;
            speed = playerModel.Config.Value.moveSpeed;
        }
        else
        {
            direction = new Vector2(velocity.x, 0).normalized;
        }

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(direction.x * speed, 0);
        SnapPlayerToTrack();
        
        if (player.GrindEffect != null)
        {
            player.GrindEffect.PlayFeedbacks();
        }
    }

    protected override void UpdateActionState()
    {
        // 首先检查E键是否按住，如果没有按住立即退出
        if (!inputModel.Grind.Value)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "None");
            player.stateMachine.SwitchState(StateLayer.Movement, "Jump");
            return;
        }
        
        // 然后检查currentTrack是否为null
        if (playerModel.CurrentTrack.Value == null)
        {
            player.stateMachine.SwitchState(StateLayer.Action, "None");
            player.stateMachine.SwitchState(StateLayer.Movement, "Jump");
            return;
        }

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

        if (inputModel.JumpStart.Value)
        {
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
    }

    protected override void ExitActionState()
    {
        player.animator.SetBool("isNoseGrinding", false);
        rb.gravityScale = normalG;

        if (player.GrindEffect != null)
        {
            player.GrindEffect.StopFeedbacks();

        }
    }
}