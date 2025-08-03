using UnityEngine;
using System.Collections;

public class TrickState : StateBase
{
    private PlayerScript player;
    private Rigidbody2D rb;
    private float trickTimer;
    private string currentTrickName;
    private TrickBase currentTrick;
    private bool isPerformingTrick = false;

    public TrickState(PlayerScript player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Trick";

    public override void Enter()
    {
        Debug.Log($"进入技巧状态，技巧名称: {currentTrickName}");
        
        // 如果没有设置技巧名称，检测输入决定技巧
        if (string.IsNullOrEmpty(currentTrickName))
        {
            DetectTrickInput();
        }

        if (!string.IsNullOrEmpty(currentTrickName))
        {
            // 创建技巧实例并执行
            CreateAndPerformTrick();
        }
        else
        {
            Debug.LogWarning("没有设置技巧名称，退出技巧状态");
            player.stateMachine.SwitchState("Air");
        }
    }

    // 设置要执行的技巧名称
    public void SetTrickName(string trickName)
    {
        currentTrickName = trickName;
        Debug.Log($"设置技巧名称: {trickName}");
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
        Debug.Log($"创建技巧: {currentTrickName}");
        
        // 根据技巧名称创建对应的技巧实例
        switch (currentTrickName)
        {
            case "TrickA":
                currentTrick = new TrickA();
                break;
            case "TrickB":
                currentTrick = new TrickB();
                break;
            case "TrickC":
                currentTrick = new TrickC();
                break;
            default:
                Debug.LogWarning($"未知的技巧: {currentTrickName}");
                return;
        }

        // 执行技巧
        currentTrick.PerformTrick(player);
        
        // 设置计时器
        trickTimer = currentTrick.duration;
        isPerformingTrick = true;
        
        // 清除技巧名称，准备接收下一个技巧
        currentTrickName = null;
    }

    public override void Update()
    {
        // 检测落地，如果落地则立即退出技巧状态
        if (player.IsGrounded())
        {
            // 立即清空技巧列表和分数
            TrickScore.Instance.ResetTrickScore();
            Debug.Log("技巧执行中落地，立即清空技巧列表！");
            
            
            
            player.stateMachine.SwitchState("Idle");
            return;
        }

        // 检测新的技巧输入
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 立即执行新技巧
            currentTrickName = "TrickA";
            CreateAndPerformTrick();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // 立即执行新技巧
            currentTrickName = "TrickB";
            CreateAndPerformTrick();
        }

        // 处理计时器
        if (trickTimer > 0)
        {
            trickTimer -= Time.deltaTime;
        }
        else if (isPerformingTrick)
        {
            // 技巧完成，退出技巧状态
            isPerformingTrick = false;
            player.stateMachine.SwitchState("Air");
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
        isPerformingTrick = false;
    }
} 