using UnityEngine;
using SkateGame;
using QFramework;

public class PowerGrindState : StateBase
{
    private Rigidbody2D rb;
    private InputController player;
    private float deceleration;
    private float direction;


    public PowerGrindState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
        deceleration = playerModel.PowerGrindDeceleration.Value;
        direction = playerModel.PowerGrindDirection.Value;
    }

    public override string GetStateName() => "PowerGrind";

    public override void Enter()
    {
        player.animator.SetTrigger("noseGrind");
        direction = Mathf.Sign(rb.linearVelocity.x);
        if (direction == 0) direction = 1f;
        Debug.Log("PowerGrindState!!!!!");
        
        // 播放MMF效果
        if (player.powerGrindEffect != null)
        {
            player.powerGrindEffect.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning("powerGrindEffectPlayer为null，无法播放效果");
        }
    }

    public override void Update()
    {
        float vx = rb.linearVelocity.x;

        // 逐渐减少的速度，保持方向不变
        float newVx = vx - direction * deceleration * Time.deltaTime;

        // 防止越过零点
        if (Mathf.Sign(newVx) != direction || Mathf.Abs(newVx) < 0.01f)
        {
            newVx = 0;
        }

        rb.linearVelocity = new Vector2(newVx, rb.linearVelocity.y);


        if (!inputModel.Trick.Value || Mathf.Abs(rb.linearVelocity.x) <= 0.5f)
        {
            player.stateMachine.SwitchState("Idle");
            Debug.Log("2222:" + player.stateMachine.GetCurrentStateName());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SendEvent<JumpExecuteEvent>();
        }
        // 检测反向输入
        CheckReverse();
    }

    public override void Exit()
    {   
        // 停止MMF效果
        if (player.powerGrindEffect != null)
        {
            player.powerGrindEffect.StopFeedbacks();

        }
        else
        {
            Debug.LogWarning("powerGrindEffectPlayer为null，无法停止效果");
        }
    }

    private void CheckReverse()
    {
        if (playerModel.IsCheckingReverseWindow.Value)
        {
            float currentVelocityX = rb.linearVelocity.x;

            // 如果当前有水平速度且输入方向与速度方向相反
            if (Mathf.Abs(currentVelocityX) > 1f && Mathf.Abs(inputModel.Move.Value.x) > 0.01f)
            {
                if (Mathf.Sign(inputModel.Move.Value.x) != Mathf.Sign(currentVelocityX))
                {
                    Debug.Log($"PowerGrind状态下检测到反向输入: 当前速度={currentVelocityX}, 输入={inputModel.Move.Value.x}");
                    player.SendEvent<ReverseInputEvent>();
                    playerModel.IsCheckingReverseWindow.Value = false;
                    return; // 进入反向状态后直接返回，不处理其他逻辑
                }
            }
        }
    }
}
