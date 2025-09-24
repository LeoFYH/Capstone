using UnityEngine;
using System.Collections;
using SkateGame;
using QFramework;

public class TrickState : StateBase
{
    private InputController player;
    private Rigidbody2D rb;
    private float trickTimer;
    private string currentTrickName;
    private TrickBase currentTrick;
    private bool isPerformingTrick = false;

    public TrickState(InputController player, Rigidbody2D rb)
    {
        this.player = player;
        this.rb = rb;
    }

    public override string GetStateName() => "Trick";

    public override void Enter()
    {
        Debug.Log($"TrickState: 进入技巧状态，技巧名称: {currentTrickName}");
        
        // 如果没有设置技巧名称，检测输入决定技巧
        if (string.IsNullOrEmpty(currentTrickName))
        {
            Debug.Log("TrickState: 没有预设技巧名称，检测输入");
            DetectTrickInput();
        }

        if (!string.IsNullOrEmpty(currentTrickName))
        {
            Debug.Log($"TrickState: 执行技巧: {currentTrickName}");
            // 创建技巧实例并执行
            CreateAndPerformTrick();
        }
        else
        {
            Debug.LogWarning("TrickState: 没有设置技巧名称，退出技巧状态");
            player.stateMachine.SwitchState("Air");
        }
    }

    // 设置要执行的技巧名称
    public void SetTrickName(string trickName)
    {
        currentTrickName = trickName;
        // Debug.Log($"设置技巧名称: {trickName}");
        
    }

    private void DetectTrickInput()
    {
        // 检测输入决定技巧
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E))
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
        Debug.Log($"TrickState: 创建技巧: {currentTrickName}");
        
        // 发送技巧执行事件给系统层处理
        Debug.Log($"TrickState: 发送TrickPerformedEvent: {currentTrickName}");
        player.SendEvent<TrickPerformedEvent>(new TrickPerformedEvent { TrickName = currentTrickName });
        
        // 根据技巧名称创建对应的技巧实例（仅用于状态机内部逻辑）
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
                Debug.LogWarning($"TrickState: 未知的技巧: {currentTrickName}");
                return;
        }

        Debug.Log($"TrickState: 执行技巧动画和特效: {currentTrickName}");
        // 执行技巧（仅动画和特效，不处理分数）
        currentTrick.PerformTrick(player);
        
        // 标记已执行trick，用于落地奖励
        player.MarkTrickPerformed();
        
        // 设置计时器
        trickTimer = currentTrick.duration;
        isPerformingTrick = true;
        
        Debug.Log($"TrickState: 技巧设置完成，持续时间: {trickTimer}");
        
        // 清除技巧名称，准备接收下一个技巧
        currentTrickName = null;
    }

    public override void Update()
    {
        // 检测落地，如果落地则立即退出技巧状态
        if (playerModel.IsGrounded.Value)
        {
            // 处理瞄准时间奖励（如果执行了trick）
            player.HandleLandingAimTimeBonus();
            
            // 发送玩家落地事件，让系统处理
            player.SendEvent<PlayerLandedEvent>(new PlayerLandedEvent());
            
            player.stateMachine.SwitchState("Idle");
            Debug.Log("2222:" + player.stateMachine.GetCurrentStateName());
            return;
        }

        // 检测新的技巧输入
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.E))
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
        
        // 确保颜色恢复为白色
        player.ResetPlayerColor();
        
        // 清除技巧名称和实例
        currentTrickName = null;
        currentTrick = null;
        isPerformingTrick = false;
    }
} 