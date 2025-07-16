using UnityEngine;

public class GrindState : StateBase
{
    private Rigidbody2D rb;
    private PlayerScript player;
    private float grindTime = 0f; // 滑轨时间

    public GrindState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Grind";

    public override void Enter()
    {
        grindTime = 0f;
    }

    public override void Update()
    {
        // 检查是否还能检测到滑轨
        if (!IsNearTrack())
        {
            player.stateMachine.SwitchState("Idle");
            return;
        }

        // 滑轨时间增加
        grindTime += Time.deltaTime;
        
        // 如果滑轨时间超过3秒，退出滑轨状态
        // if (grindTime >= 3f)
        // {
        //     player.stateMachine.SwitchState("Idle");
        //     return;
        // }

        // 玩家自动向前移动，使用moveSpeed但速度与正常移动一致
        Vector3 newPosition = player.transform.position + Vector3.right * (player.moveSpeed * 0.5f) * Time.deltaTime;
        player.transform.position = newPosition;
        
        // 滑轨时禁用重力
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    // 检查是否在轨道附近
    private bool IsNearTrack()
    {
        // 使用正方形检测，大小为1x1
        Vector2 boxSize = new Vector2(1f, 1f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(player.transform.position, boxSize, 0f);
        foreach (var col in colliders)
        {
            if (col.isTrigger)
            {
                var track = col.GetComponent<Track>();
                if (track != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void Exit()
    {
        // 恢复重力
        rb.gravityScale = 1f;
        
        // 给玩家一个向前的速度，创造滑出去的感觉
        rb.linearVelocity = new Vector2(8f, rb.linearVelocity.y);
        
        Debug.Log("退出滑轨状态");
    }
} 