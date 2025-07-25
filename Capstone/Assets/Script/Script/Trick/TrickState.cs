using UnityEngine;
using System.Collections;

public class TrickState : StateBase
{
    private PlayerScript player;
    private Rigidbody2D rb;
    private float trickTimer;
    private string currentTrickName;
    private TrickBase currentTrick;

    public TrickState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Trick";

    public override void Enter()
    {
        // 如果没有设置技巧名称，检测输入决定技巧
        if (string.IsNullOrEmpty(currentTrickName))
        {
            DetectTrickInput();
        }

        if (!string.IsNullOrEmpty(currentTrickName))
        {
            // 创建技巧实例并执行
            CreateAndPerformTrick();
            
            // 开始协程，技巧结束后返回空中状态
            player.StartCoroutine(ReturnToAirAfterTrick());
        }
    }

    // 设置要执行的技巧名称
    public void SetTrickName(string trickName)
    {
        currentTrickName = trickName;
    }

    private void DetectTrickInput()
    {
        // 检测输入决定技巧
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentTrickName = "TrickA";
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            currentTrickName = "TrickB";
        }
    }

    private void CreateAndPerformTrick()
    {
        // 根据技巧名称创建对应的技巧实例
        switch (currentTrickName)
        {
            case "TrickA":
                currentTrick = new TrickA();
                break;
            case "TrickB":
                currentTrick = new TrickB();
                break;
            default:
                Debug.LogWarning($"未知的技巧: {currentTrickName}");
                return;
        }

        // 执行技巧
        currentTrick.PerformTrick(player);
        
        // 设置计时器
        trickTimer = currentTrick.duration;
    }

    public override void Update()
    {
        // 检测落地，如果落地则立即退出技巧状态
        if (player.IsGrounded())
        {
            Debug.Log("技巧执行中落地，强制退出技巧状态");
            player.stateMachine.SwitchState("Idle");
            return;
        }

        // 处理计时器
        if (trickTimer > 0)
        {
            trickTimer -= Time.deltaTime;
        }
    }

    private IEnumerator ReturnToAirAfterTrick()
    {
        yield return new WaitForSeconds(currentTrick.duration);
        
        // 检查是否已经落地，如果落地则不切换到空中状态
        if (!player.IsGrounded())
        {
            player.stateMachine.SwitchState("Air");
        }
        else
        {
            player.stateMachine.SwitchState("Idle");
        }
    }

    public override void Exit()
    {
        // 调用当前技巧的 Exit 方法
        if (currentTrick != null)
        {
            currentTrick.Exit(player);
        }
        
        // 清除技巧名称和实例
        currentTrickName = null;
        currentTrick = null;
    }
} 